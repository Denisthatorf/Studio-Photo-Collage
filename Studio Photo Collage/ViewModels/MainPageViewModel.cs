using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        #region Save Commands
        private ICommand _saveImageCommand;
        public ICommand SaveImageCommand
        {
            get
            {
                if (_saveImageCommand == null)
                    _saveImageCommand = new RelayCommand<object>( async(parametr) => 
                    { 
                        var dialog = new SaveImageDialog();
                        dialog.NameOfImg = CurrentCollage.Project.ProjectName;
                        var result = await dialog.ShowAsync();

                        var name = dialog.NameOfImg;
                        var format = dialog.Format;
                        var quality = dialog.Quality; 
                    });
                return _saveImageCommand;
            }
        }

        private ICommand _saveProjectCommand;
        public ICommand SaveProjectCommand
        {
            get
            {
                if (_saveProjectCommand == null)
                    _saveProjectCommand = new RelayCommand<object>(async (parametr) =>
                    {
                        var dialog = new SaveProjectDialog();
                        dialog.ProjectName = CurrentCollage.Project.ProjectName;
                        var result = await dialog.ShowAsync();
                        if (result == ContentDialogResult.Primary)
                            CurrentCollage.Project.ProjectName = dialog.ProjectName;
                            SaveProjectInJsonAsync();
                    });
                return _saveProjectCommand;
            }
        }
        #endregion

        private BtnNameEnum? _checkBoxesEnum;
        public BtnNameEnum? CheckBoxesEnum
        {
            get => _checkBoxesEnum;
            set
            {
                if (_checkBoxesEnum != value)
                {
                    Set(ref _checkBoxesEnum, value);
                    switch (value)
                    {
                        case BtnNameEnum.Settings:
                            ShowSettingDialog();
                            break;
                        case BtnNameEnum.Print:
                            PinCollageToSecondaryTile();
                            break;
                        case BtnNameEnum.Photo:
                            TakePthoto();
                            break;
                    }
                }
                else
                    Set(ref _checkBoxesEnum, null);

                var type = StringToFrameConverter.Convert(_checkBoxesEnum);
                if (type != null)
                {
                    SidePanelFrame.Visibility = Visibility.Visible;
                    SidePanelFrame.Navigate(type);
                }
                else
                    SidePanelFrame.Visibility = Visibility.Collapsed;
            }
        }

        private Collage _currentCollage;
        public Collage CurrentCollage
        {
            get => _currentCollage;
            set => Set(ref _currentCollage, value);
        }

        public Frame PaintFrame { get; }
        public Frame SidePanelFrame
        {
            get
            {
                var rootFrame = (Window.Current.Content as Frame).Content as MainPage;
                return rootFrame.SidePanelFrame;
            }
        }

        public MainPageViewModel(INavigationService _navigationService)
        {
            _checkBoxesEnum = null;

            PaintFrame = new Frame();
            PaintFrame.Navigate(typeof(PaintPopUpPage));

            NavigationService = _navigationService;
            MessengersRegistration();
        }

        public async void GoBack()
        {
            var result = await SaveProjectAsync();

            if (result == ContentDialogResult.Primary)
                NavigationService.NavigateTo("TemplatesPage");
            else if (result == ContentDialogResult.Secondary)
                NavigationService.NavigateTo("TemplatesPage");
        }

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

        private void MessengersRegistration()
        {

            Messenger.Default.Register<Project>(this, (parameter) => CurrentCollage = new Collage(parameter));

            #region Have To update Ui
            // use this CurrentCollage.UpdateUIAsync();

            Messenger.Default.Register<Thickness>(this, (Action<Thickness>)((parameter) =>
            {
                CurrentCollage.Project.BorderThickness = parameter.Top; //everywhere is one parameter
                CurrentCollage.UpdateUIAsync();
            }));

            Messenger.Default.Register<double>(this, (Action<double>)((parameter) =>
            {
                CurrentCollage.Project.BorderOpacity = parameter;
                CurrentCollage.UpdateUIAsync();
            }));
            #endregion

            #region Have to update Project information

            // use this  CurrentCollage.UpdateProjectInfoAsync();
            // for image updation 

            Messenger.Default.Register<Brush>(this, (Action<Brush>)((brush) =>
            {
                (CurrentCollage.BackgroundGrid as Grid).Background = brush;
                CurrentCollage.UpdateProjectInfoAsync();
            }));

            Messenger.Default.Register<NotificationMessageAction<Image>>(this, (messageAct) =>
            {
                var image = CurrentCollage.SelectedImage;
                if (image?.Source != null)
                    messageAct.Execute(image);
                CurrentCollage.UpdateProjectInfoAsync();
            });
            #endregion
        }

        private async Task SaveProjectInJsonAsync()
        {
            var str = await JsonHelper.DeserializeFileAsync("projects.json");
            var projects = new List<Project>();
            if (!String.IsNullOrEmpty(str))
                projects = await JsonHelper.ToObjectAsync<List<Project>>(str);
            if (projects == null)
                projects = new List<Project>();

            int index = projects.IndexOf(CurrentCollage.Project);
            if (index == -1)
                projects.Add(CurrentCollage.Project);
            else
                projects[index] = CurrentCollage.Project;

            string projectsAsList = await JsonHelper.StringifyAsync(projects);
            await JsonHelper.WriteToFile("projects.json", projectsAsList);

            await UpdateSecondaryTile();
        }

        private async Task UpdateSecondaryTile()
        {
            var tileId = CurrentCollage.Project.GetHashCode().ToString();
            bool isPinned = SecondaryTile.Exists(tileId);

            if (isPinned)
            {
                var path = $"{CurrentCollage.Project.ProjectName}.{CurrentCollage.Project.SaveFormat}";
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(path);
                await file.DeleteAsync();

                var source = await ImageHelper.SaveCollageUIAsImage(CurrentCollage);

                // Initialize a secondary tile with the same tile ID you want to update
                SecondaryTile tile = new SecondaryTile(tileId);

                // Assign ALL properties, including ones you aren't changing

                // And then update it
                await tile.UpdateAsync();
            }

        }

        private async void TakePthoto()
        {
            if (CurrentCollage.SelectedImage != null)
            {
                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);
                // Open the Camera to capture the Image
                StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
                // If the capture gets cancelled by user, do nothing
                if (photo == null)
                {
                    // User cancelled photo capture
                    return;
                }
                // Else, display the captured Image in the Placeholder
                else
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (IRandomAccessStream photoStream = await photo.OpenAsync(FileAccessMode.Read))
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

        private async void PinCollageToSecondaryTile()
        {
            var result = await SaveProjectAsync();

            if (result == ContentDialogResult.Primary) // Yes
            {
                var source = await ImageHelper.SaveCollageUIAsImage(CurrentCollage);

                var zipCode = CurrentCollage.Project.GetHashCode().ToString();
                string tileId = zipCode;
                string displayName = CurrentCollage.Project.ProjectName != null ?
                                            CurrentCollage.Project.ProjectName : "Test";
                string arguments = zipCode;
                // Initialize the tile with required arguments
                SecondaryTile tile = new SecondaryTile(
                    tileId,
                    displayName,
                    arguments,
                    new Uri(source),
                    Windows.UI.StartScreen.TileSize.Default);

                var p = await tile.RequestCreateAsync();
            }
        }
    }
}
