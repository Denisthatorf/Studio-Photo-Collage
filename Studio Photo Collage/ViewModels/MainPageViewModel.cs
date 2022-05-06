using System;
using System.Linq;
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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

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
        private ICommand photoCommand;
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
                    deleteImageCommand = new RelayCommand(async() =>
                    {
                        await CurrentCollage.DeleteSelectedImgFromBtn();
                        Messenger.Send(new ChangeSelectedImageMessage(null));
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
                    addImageCommand = new RelayCommand(() => _ = CurrentCollage.SetImgByFilePickerToSelectedBtnAsync());
                }

                return addImageCommand;
            }
        }
        public ICommand PhotoCommand
        {
            get
            {
                if(photoCommand == null)
                {
                    photoCommand = new RelayCommand(TakePhoto);
                }

                return photoCommand;
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
            await dialog.ShowAsync();

            var name = dialog.NameOfImg;
            var format = dialog.Format.ToLower();
            var quality = dialog.Quality;
            var folder = dialog.Folder;

            if(dialog.Result != ContentDialogResult.None)
            {
                SaveCollageAsImageAsync(name, format, folder);
            }
        }

        private async void SaveCollageAsImageAsync(string name, string format, StorageFolder folder)
        {
            var btns = CurrentCollage.GetListOfBtns();
            var scrolles = btns.Select((x) => x.Content as ScrollViewer);
            scrolles.ToList().ForEach(x => {
               if(x != null)
               {
                    x.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    x.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
               }
            });
            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(CurrentCollage.CollageGrid);

            var pixelBuffer = await bitmap.GetPixelsAsync();
            byte[] pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();
            
            if(folder == null)
            {
                folder = KnownFolders.SavedPictures;
            }

            var file = await folder.CreateFileAsync($"{name}.{format}", CreationCollisionOption.ReplaceExisting);

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

            scrolles.ToList().ForEach(x => {
                if (x != null)
                {
                    x.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    x.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                }
            });
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

                CurrentCollage.SelectedImageChanged += (o, e) =>
                {
                    Messenger.Send(new ChangeSelectedImageMessage(e.ImageInfo));
                };
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

            Messenger.Register<Action<Image>>(this, (r, m) =>
            {
                var image = CurrentCollage.SelectedImage;
                var selectedimg = CurrentCollage.SelectedImage;
                if (image?.Source != null)
                {
                    m?.Invoke(image);

                    CurrentCollage.UpdateProjectInfoAsync();
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

            Messenger.Register<ApplyEffectsMessage>(this, (r, m) =>
            {
                _ = CurrentCollage.ApplyEffectToImage(m.Value,
                CurrentCollage.SelectedImage, CurrentCollage.SelectedImageNumberInList);

                CurrentCollage.Project.IsFilltersUsedToAllImages = false;
            });

            Messenger.Register<ApplyEffectsToAllMessage>(this, (r,m) =>{
                CurrentCollage.ApplyEffectToAllImageImage(m.Value);

                CurrentCollage.Project.IsFilltersUsedToAllImages = true;
            });

            Messenger.Register<FrameColorChangedMessege>(this, (r, m) =>
            {
                CurrentCollage.SetFrameColor(m.Value);
            });

            Messenger.Register<FrameSizeChangedMessage>(this, (r, m) =>
            {
                CurrentCollage.SetFrameAdditionalSize(m.Value);
            });
        }

        private async void TakePhoto()
        {
            if (CurrentCollage.SelectedToggleBtn != null)
            {
                var captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

                var photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (photo == null)
                {
                    return;
                }

                else
                {
                    using (var fileStream = await photo.OpenAsync(FileAccessMode.Read))
                    {
                        try
                        {
                            var decoder = await BitmapDecoder.CreateAsync(fileStream);
                            var source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);

                            await source.SetSourceAsync(fileStream);
                            await CurrentCollage.SetImageToSelectedBtnAsync(source);
                        }
                        catch
                        {
                            var messageDialog = new MessageDialog("Camera error");
                            await messageDialog.ShowAsync();
                        }
                    }
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

            var collage = await ProjectToUIElementAsync.Convert(CurrentCollage.Project, 400);
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