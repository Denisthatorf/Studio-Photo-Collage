﻿using System;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Input.Inking;
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
        private ICommand addImageCommand;
        private ICommand deleteImageCommand;
        private BtnNameEnum? checkBoxesEnum;
        private Collage currentCollage;
        private Grid renderCollage;
        private Color paintColor;

        public Color PaintColor
        {
            get => paintColor;
            set
            {
                SetProperty(ref paintColor, value);
            }
        }
        public Collage CurrentCollage
        {
            get
            {
                return currentCollage;
            }
            set => SetProperty(ref currentCollage, value);
        }
        public Grid RenderCollage
        {
            get => renderCollage;
            set => SetProperty(ref renderCollage, value);
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
                    CheckBoxesEnumValueChange();
                }
            }
        }

        public ICommand SaveImageCommand
        {
            get
            {
                if (saveImageCommand == null)
                {
                    saveImageCommand = new RelayCommand(() => SaveCollageAsImageBySaveImageDialogAsync());
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
        public ICommand DeleteImageCommmand
        {
            get
            {
                if(deleteImageCommand == null)
                {
                    deleteImageCommand = new RelayCommand(() =>
                    {
                        CurrentCollage.DeleteSelectedImgFromBtn();
                    });
                }

                return deleteImageCommand;
            }
        }
        public ICommand AddImageCommand
        {
            get
            {
                if (addImageCommand == null)
                {
                    addImageCommand = new RelayCommand(() => _ = CurrentCollage.SetImgByFilePickerToSelectedBtn());
                }

                return addImageCommand;
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

        public void ActivateInkCanvas()
        {
            var inkList = CurrentCollage.GetListInkCanvases();
            foreach (var ink in inkList)
            {
                ink.InkPresenter.InputDeviceTypes 
                   = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen;
            }
        }
        public void DeactivateInkCanvas()
        {
            var inkList = CurrentCollage.GetListInkCanvases();
            foreach (var ink in inkList)
            {
                ink.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.None;
            }
        }
        public void ChangeInkCanvasMode(InkInputProcessingMode mode)
        {
            var inkList = CurrentCollage.GetListInkCanvases();
            foreach (var ink in inkList)
            {
                ink.InkPresenter.InputProcessingConfiguration.Mode = mode;
            }
        }
        public void ChangeInkCanvasAttributes(InkDrawingAttributes attributes)
        {
            var inkList = CurrentCollage.GetListInkCanvases();
            foreach (var ink in inkList)
            {
                ink.InkPresenter.UpdateDefaultDrawingAttributes(attributes);
            }
        }

        public async void GoBack()
        {
            var result = ContentDialogResult.None;
            if (!CurrentCollage.IsSaved)
            {
                result = await SaveProjectBySaveDialogAsync();
                if (result != ContentDialogResult.None)
                {
                    CheckBoxesEnum = null;
                    CurrentCollage = null;
                    navigationService.Navigate(typeof(TemplatePage));
                }
            }
            else
            {
                navigationService.Navigate(typeof(TemplatePage));
            }         
        }

        public async Task<ContentDialogResult> SaveProjectBySaveDialogAsync()
        {
            var dialog = new SaveDialog();
            dialog.ProjectName = CurrentCollage.Project.ProjectName;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
                ProjectHelper.SaveProject(CurrentCollage);
            }

            return result;
        }

        private async void SaveCollageAsImageBySaveImageDialogAsync()
        {
            var dialog = new SaveImageDialog();
            dialog.NameOfImg = CurrentCollage.Project.ProjectName;
            var result = await dialog.ShowAsync();

            var name = dialog.NameOfImg;
            var format = dialog.Format.ToLower();
            var quality = dialog.Quality;

            if(result != ContentDialogResult.None)
            {
                SaveCollageAsImageAsync(name, format);
            }
        }

        private async void SaveCollageAsImageAsync(string name, string format)
        {
            var collage = await ProjectToUIElementAsync.Convert(CurrentCollage.Project, 1000);
            collage.Width = 1000;
            collage.Height = 1000;
            RenderCollage = collage;

            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(RenderCollage);

            var pixelBuffer = await bitmap.GetPixelsAsync();
            byte[] pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();
            var pictureFolder = KnownFolders.SavedPictures;
            var file = await pictureFolder.CreateFileAsync($"{name}.{format}", CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encformat = FormatConverter.Convert(format);
                var encoder = await BitmapEncoder.CreateAsync(encformat, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)bitmap.PixelWidth,
                    (uint)bitmap.PixelHeight,
                    displayInformation.RawDpiX,
                    displayInformation.RawDpiY,
                    pixels);
                await encoder.FlushAsync();
            }

            RenderCollage = null;
        }

        private async void SaveProjectBySaveProjectDialogAsync()
        {
            var dialog = new SaveProjectDialog();
            dialog.ProjectName = CurrentCollage.Project.ProjectName;
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                CurrentCollage.Project.ProjectName = dialog.ProjectName;
               ProjectHelper.SaveProject(CurrentCollage);
            }

            var collage = CurrentCollage;
            collage.IsSaved = true;
        }

        private void CheckBoxesEnumValueChange()
        {
            switch (CheckBoxesEnum)
            {
                case null:
                    SidePanel.Navigate(typeof(Page));
                    break;
                case BtnNameEnum.Settings:
                    ShowSettingDialog();
                    break;
                case BtnNameEnum.Photo:
                    TakePthoto();
                    break;
                case BtnNameEnum.Delete:
                    CurrentCollage.DeleteSelectedImgFromBtn();
                    break;
                case BtnNameEnum.Print:
                    Print();
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
            {
                CurrentCollage = new Collage(m);
                Messenger.Send(new NewCollageBackgroundOpacityMessage(m.BorderOpacity));
                Messenger.Send(new NewCollageBorderThicknessMessage(m.BorderThickness));
            });

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
                if (CurrentCollage.IsSaved)
                {
                    m.Reply(ContentDialogResult.Primary);   
                }
                else
                {
                    m.Reply(SaveProjectBySaveDialogAsync());
                }
            });

            Messenger.Register<ImageBrush>(this, async (r, m) =>
            {
                var backgroundGrid = CurrentCollage.BackgroundGrid as Grid;
                backgroundGrid.Background = m;

                var project = CurrentCollage.Project;
                project.Background = await ImageHelper.SaveToStringBase64Async(m.ImageSource);
            });

            Messenger.Register<SolidColorBrush>(this, (r, m) =>
             {
                 var backgroundGrid = CurrentCollage.BackgroundGrid as Grid;
                 backgroundGrid.Background = m;

                 var project = CurrentCollage.Project;
                 project.Background = m.Color.ToString();
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
            Messenger.Register<PaintColorChangedMessage>(this, (r, m) => PaintColor = m.Value);

            Messenger.Register<ZoomMessage>(this, (r, m) =>
            {
                var scroll = CurrentCollage.SelectedScrollViewer;
                var img = CurrentCollage.SelectedImage;
                if(img != null && img.Source is WriteableBitmap bitmap)
                {
                    if (m.Value == ZoomType.ZoomIn)
                    {
                        var height = scroll.ExtentHeight;
                        var width = scroll.ExtentWidth;
                        scroll.ChangeView(width / 2, height / 2, scroll.ZoomFactor + 0.5f);

                        //var vOffset = scroll.VerticalOffset;
                        //var hOffset = scroll.HorizontalOffset;

                        //var hh = (height - vOffset) / 2  + vOffset;
                        //var ww = (Width - hOffset) / 2 + hOffset;
                        //scroll.ChangeView(null, null, scroll.ZoomFactor + 0.5f);
                    }
                    else
                    {
                        var height = scroll.ExtentHeight;
                        var width = scroll.ExtentWidth;
                        scroll.ChangeView(height / 2, width / 2, scroll.ZoomFactor - 0.5f);
                    }
                }
            });

            Messenger.Register<FrameMessageChanged>(this, (r, m) =>
            {
                CurrentCollage.SetFrame(m.Value);
            });
        }

        private async void TakePthoto()
        {
            if (CurrentCollage.SelectedToggleBtn != null)
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

            CheckBoxesEnum = null;
        }

        private async void ShowSettingDialog()
        {
            var dialog = new SettingsDialog();
            await dialog.ShowAsync();
            CheckBoxesEnum = null;
        }

        private async void Print()
        {
            var printHelper = new PrintHelper();

            var collage = await ProjectToUIElementAsync.Convert(CurrentCollage.Project, "1000");
            collage.HorizontalAlignment = HorizontalAlignment.Center;
            collage.VerticalAlignment = VerticalAlignment.Center;

            printHelper.AddPrintContent(collage);
            printHelper.Print();

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