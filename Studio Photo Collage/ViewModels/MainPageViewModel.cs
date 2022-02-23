using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Infrastructure.Services;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ObservableRecipient
    {
        private readonly INavigationService navigationService;

        private ICommand saveImageCommand;
        private ICommand saveProjectCommand;
        private ICommand goBackCommand;
        private BtnNameEnum? checkBoxesEnum;
        private Collage currentCollage;

        public Collage CurrentCollage
        {
            get => currentCollage;
            set => SetProperty(ref currentCollage, value);
        }
        public BtnNameEnum? CheckBoxesEnum
        {
            get => checkBoxesEnum;
            set
            {
                if (checkBoxesEnum != value)
                {
                    SetProperty(ref checkBoxesEnum, value);
                    CheckBoxesEnumValueChange();
                }
                else
                {
                    SetProperty(ref checkBoxesEnum, null);
                }
            }
        }

        public ICommand SaveImageCommand
        {
            get
            {
                if (saveImageCommand == null)
                {
                    saveImageCommand = new RelayCommand(() => SaveCollageAsImageAsync());
                }

                return saveImageCommand;
            }
        }
        public ICommand SaveProjectCommand
        {
            get
            {
                if (saveProjectCommand == null)
                {
                    saveProjectCommand = new RelayCommand(() => SaveProjectBySaveProjectDialogAsync());
                }

                return saveProjectCommand;
            }
        }
        public ICommand GoBackCommand
        {
            get
            {
                if (goBackCommand == null)
                {
                    goBackCommand = new RelayCommand(() => GoBack());
                }
                return goBackCommand;
            }
        }

        private Frame SidePanel
        {
            get
            {
                var frame = (Frame)Window.Current.Content;
                var page = frame.Content as MainPage;
                var result = page?.RootFrame;
                return result;
            }
        }

        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            checkBoxesEnum = null;
            MessengersRegistration();
        }



        private async Task SaveProjectInJsonAsync()
        {
            var projects = await ApplicationData.Current.LocalFolder.ReadAsync<List<Project>>("projects");
            if (projects == null)
            {
                projects = new List<Project>();
            }

            int index = projects.IndexOf(CurrentCollage.Project);
            if (index == -1)
            {
                projects.Add(CurrentCollage.Project);
            }
            else
            {
                projects[index] = CurrentCollage.Project;
            }

            await ApplicationData.Current.LocalFolder.SaveAsync<List<Project>>("projects", projects);

            Messenger.Send(new ProjectSavedMessage(CurrentCollage.Project));
        }

        public async Task<ContentDialogResult> SaveProjectBySaveDialogAsync()
        {
            var dialog = new SaveDialog();
            dialog.ProjectName = CurrentCollage.Project.ProjectName;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
                await SaveProjectInJsonAsync();
            }

            return result;
        }

        public async void GoBack()
        {
            var result = await SaveProjectBySaveDialogAsync();

            if (result != ContentDialogResult.None)
            {
                CheckBoxesEnum = null;
                CurrentCollage = null;
                navigationService.Navigate(typeof(TemplatePage));
            }
        }

        private async void SaveCollageAsImageAsync()
        {
            var dialog = new SaveImageDialog();
            dialog.NameOfImg = CurrentCollage.Project.ProjectName;
            var result = await dialog.ShowAsync();

            var name = dialog.NameOfImg;
            var format = dialog.Format;
            var quality = dialog.Quality;
        }

        private async void SaveProjectBySaveProjectDialogAsync()
        {
            var dialog = new SaveProjectDialog();
            dialog.ProjectName = CurrentCollage.Project.ProjectName;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
                _ = SaveProjectInJsonAsync();
            }
        }

        private void CheckBoxesEnumValueChange()
        {
            switch (CheckBoxesEnum)
            {
                case BtnNameEnum.Settings:
                    ShowSettingDialog();
                    break;
                case BtnNameEnum.Photo:
                    TakePthoto();
                    break;
                case BtnNameEnum.Add:
                    _ = CurrentCollage.SetImgByFilePickerToSelectedBtn();
                    break;
                case BtnNameEnum.Delete:
                    CurrentCollage.DeleteSelectedImgFromBtn();
                    break;
            }

            var type = StringToFrameConverter.Convert(checkBoxesEnum);
            if (type != null)
            {
                SidePanel.Visibility = Visibility.Visible;
                SidePanel.Navigate(type);
            }
            else
            {
                SidePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void MessengersRegistration()
        {

            Messenger.Register<Project>(this, (r, m) =>
            CurrentCollage = new Collage(m));

            Messenger.Register<BorderThicknessChangedMessage>(this, (r, m) =>
            {
                CurrentCollage.Project.BorderThickness = m.Value;
                CurrentCollage.UpdateUIAsync();
            });

            Messenger.Register<BackgroundOpacityChangedMessage>(this, (r, m) =>
            {
                CurrentCollage.Project.BorderOpacity = m.Value;
                CurrentCollage.UpdateUIAsync();
            });

            Messenger.Register<SaveProjectRequestMessage>(this, (r, m) =>
            {
                m.Reply(SaveProjectBySaveDialogAsync());
            });

            Messenger.Register<ImageBrush>(this, async (r, m) =>
            {
                var backgroundGrid = CurrentCollage.BackgroundGrid as Grid;
                backgroundGrid.Background = m;

                var project = CurrentCollage.Project;
                project.BackgroundColor = await ImageHelper.SaveToStringBase64Async(m.ImageSource);
            });

            Messenger.Register<SolidColorBrush>(this, (r, m) =>
             {
                 var backgroundGrid = CurrentCollage.BackgroundGrid as Grid;
                 backgroundGrid.Background = m;

                 var project = CurrentCollage.Project;
                 project.BackgroundColor = m.Color.ToString();
             });

            Messenger.Register<Action<Image>>(this, async (r, m) =>
            {
                var image = CurrentCollage.SelectedImage;
                var selectedimg = CurrentCollage.SelectedImage;
                if (image?.Source != null)
                {
                    m?.Invoke(image);

                    CurrentCollage.Project.ImageArr[CurrentCollage.SelectedImageNumberInList]
                        = await ImageHelper.SaveToStringBase64Async(selectedimg.Source);
                }
            });
        }

        private async void TakePthoto()
        {
            if (CurrentCollage.SelectedImage != null)
            {
                var captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

                var photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (photo == null)
                {
                    return;
                }

                else
                {
                    var bitmapImage = new BitmapImage();
                    using (var photoStream = await photo.OpenAsync(FileAccessMode.Read))
                    {
                        bitmapImage.SetSource(photoStream);
                    }

                    CurrentCollage.SelectedImage.Source = bitmapImage;
                }
            }
        }

        private async void ShowSettingDialog()
        {
            var dialog = new SettingsDialog();
            await dialog.ShowAsync();
            CheckBoxesEnum = null;
        }
    }
    public enum BtnNameEnum
    {
        Background,
        Filters,
        Frames,
        Recent,
        Templates,
        Transform,
        Photo,
        Add,
        Resents,
        Save,
        Settings,
        AddFile,
        Paint,
        Delete,
        Print
    }
}
