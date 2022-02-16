using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private ICommand saveImageCommand;
        private ICommand saveProjectCommand;
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
                    saveImageCommand = new RelayCommand<object>(async (parametr) =>
                    {
                        var dialog = new SaveImageDialog();
                        dialog.NameOfImg = CurrentCollage.Project.ProjectName;
                        var result = await dialog.ShowAsync();

                        var name = dialog.NameOfImg;
                        var format = dialog.Format;
                        var quality = dialog.Quality;
                    });
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
                    saveProjectCommand = new RelayCommand<object>(async (parametr) =>
                    {
                        var dialog = new SaveProjectDialog();
                        dialog.ProjectName = CurrentCollage.Project.ProjectName;
                        var result = await dialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                        {
                            CurrentCollage.Project.ProjectName = dialog.ProjectName;
                        }

                        _ = SaveProjectInJsonAsync();
                    });
                }

                return saveProjectCommand;
            }
        }

        public MainPageViewModel()
        {
            checkBoxesEnum = null;
            MessengersRegistration();
        }

        /*  public async void GoBack()
        {
            var result = await SaveProjectAsync();

            if (result == ContentDialogResult.Primary)
                NavigationService.NavigateTo("TemplatesPage");
            else if (result == ContentDialogResult.Secondary)
                NavigationService.NavigateTo("TemplatesPage");
        }*/

        public async Task<ContentDialogResult> SaveProjectAsync()
        {
            var dialog = new SaveDialog();
            dialog.ProjectName = CurrentCollage.Project.ProjectName;
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary) // Yes
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
                await SaveProjectInJsonAsync();
            }

            return result;
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
            }
        }

        private void MessengersRegistration()
        {

            WeakReferenceMessenger.Default.Register<Project>(this, (r, m) => CurrentCollage = new Collage(m));

            //WeakReferenceMessenger.Default.Register<Thickness>(this, (r, m) =>
            //{
            //    CurrentCollage.Project.BorderThickness = m.Top; //everywhere is one parameter
            //    CurrentCollage.UpdateUIAsync();
            //});

            //WeakReferenceMessenger.Default.Register<double>(this, (r, m) =>
            //{
            //    CurrentCollage.Project.BorderOpacity = m;
            //    CurrentCollage.UpdateUIAsync();
            //});

            WeakReferenceMessenger.Default.Register<ImageBrush>(this, async (r, m) =>
            {
                var backgroundGrid = CurrentCollage.BackgroundGrid as Grid;
                backgroundGrid.Background = m;

                var project = CurrentCollage.Project;
                project.BackgroundColor = await ImageHelper.SaveToStringBase64Async(m.ImageSource);
            });

            WeakReferenceMessenger.Default.Register<SolidColorBrush>(this, (r, m) =>
             {
                 var backgroundGrid = CurrentCollage.BackgroundGrid as Grid;
                 backgroundGrid.Background = m;

                 var project = CurrentCollage.Project;
                 project.BackgroundColor = m.Color.ToString();
             });

            /*WeakReferenceMessenger.Default.Register<Image>(this, async (r, m) =>
            {
                var image = CurrentCollage.SelectedImage;
                if (image?.Source != null)
                {
                    messageAct.Execute(image);
                }

                var selectedimg = CurrentCollage.SelectedImage;

                if (selectedimg?.Source != null)
                {
                    CurrentCollage.Project.ImageArr[CurrentCollage.SelectedImageNumberInList]
                        = await ImageHelper.SaveToStringBase64Async(selectedimg.Source);
                }
            });*/
        }

        private async Task SaveProjectInJsonAsync()
        {
            var str = await JsonHelper.DeserializeFileAsync("projects.json");
            var projects = new List<Project>();

            if (!string.IsNullOrEmpty(str))
            {
                projects = await JsonHelper.ToObjectAsync<List<Project>>(str);
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

            string projectsAsList = await JsonHelper.StringifyAsync(projects);
            await JsonHelper.WriteToFile("projects.json", projectsAsList);
        }

        private async void TakePthoto()
        {
            if (CurrentCollage.SelectedImage != null)
            {
                var captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
                // Open the Camera to capture the Image
                var photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
                // If the capture gets cancelled by user, do nothing
                if (photo == null)
                {
                    // User cancelled photo capture
                    return;
                }
                // Else, display the captured Image in the Placeholder
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
