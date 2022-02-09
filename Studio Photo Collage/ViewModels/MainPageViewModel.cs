using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
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
                    _saveImageCommand = new RelayCommand<object>((parametr) => { });
                return _saveImageCommand;
            }
        }

        private ICommand _saveProjectCommand;
        public ICommand SaveProjectCommand
        {
            get
            {
                if (_saveProjectCommand == null)
                    _saveProjectCommand = new RelayCommand<object>(async(parametr) => 
                    {
                        var dialog = new SaveDialog("project");
                        var result = await dialog.ShowAsync();
                        if (result != ContentDialogResult.None)
                            SaveProject();
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
                    if (value == BtnNameEnum.Settings)
                        ShowSettingDialog();
                    if (value == BtnNameEnum.Print)
                        PinCollageToSecondaryTile();
                    if (value == BtnNameEnum.Photo)
                        TakePthoto();
                }
                else
                    Set(ref _checkBoxesEnum, null);

                SidePanelFrame = StringToFrameConverter.Convert(_checkBoxesEnum);
                if (_checkBoxesEnum == BtnNameEnum.Background) { } 
            }
        }

        private Collage _currentCollage;
        public Collage CurrentCollage {
            get => _currentCollage;
            set => Set(ref _currentCollage, value); }

        public Frame PaintFrame { get; }

        private Frame _sidePanelFrame;
        public Frame SidePanelFrame { 
            get => _sidePanelFrame;
            set => Set(ref _sidePanelFrame, value); }


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
            var dialog = new SaveDialog("collage");
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary) // Yes
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
                SaveProject();
                NavigationService.NavigateTo("TemplatesPage");
            }
            else if (result == ContentDialogResult.Secondary)
            {
                CurrentCollage = null;
                NavigationService.NavigateTo("TemplatesPage");
            }
        }

        private void MessengersRegistration()
        {
            Messenger.Default.Register<Project>(this, (parameter) =>
           CurrentCollage = new Collage(parameter));

            Messenger.Default.Register<Thickness>(this, (Action<Thickness>)((parameter) => {
                CurrentCollage.Project.BorderThickness = parameter.Top; //everywhere is one parameter
                CurrentCollage.UpdateUIAsync();
            }));

            Messenger.Default.Register<double>(this, (Action<double>)((parameter) => {
                CurrentCollage.Project.BorderOpacity = parameter;
                CurrentCollage.UpdateUIAsync();
            }));

            Messenger.Default.Register<Brush>(this, (Action<Brush>)(async (parameter) => {
                if(parameter is SolidColorBrush solidBrush)
                    CurrentCollage.Project.BorderColor = solidBrush.Color.ToString();
                if (parameter is ImageBrush imageBrush)
                    CurrentCollage.Project.BorderColor = await ImageHelper.SaveToBytesAsync(imageBrush.ImageSource);
                CurrentCollage.UpdateUIAsync();
            }));

            Messenger.Default.Register<NotificationMessageAction<Image>>(this, (messageAct) => {
                var image = CurrentCollage.SelectedImage;
                if (image?.Source != null)
                    messageAct.Execute(image);
                CurrentCollage.UpdateProjectInfoAsync();
            });
        }

        private async Task SaveProject()
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

        private async void ShowSettingDialog()
        {
            var dialog = new SettingsDialog();
            await dialog.ShowAsync();
            CheckBoxesEnum = null;
        }

        private async void PinCollageToSecondaryTile()
        {
            var dialog = new SaveDialog("collage");
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary) // Yes
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
                SaveProject();

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

        private async Task UpdateSecondaryTile()
        {
            var tileId = CurrentCollage.Project.GetHashCode().ToString();
            bool isPinned = SecondaryTile.Exists(tileId);

            if (isPinned)
            {
                var path = $"{CurrentCollage.Project.ProjectName}.{CurrentCollage.Project.SaveFormat}";
                    var file =await ApplicationData.Current.LocalFolder.GetFileAsync(path);
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
         if(CurrentCollage.SelectedImage != null)
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
            CheckBoxesEnum = null;

        }
    }
}
