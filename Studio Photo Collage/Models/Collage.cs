using System;
using System.Threading.Tasks;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Controls;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;
using Windows.UI.Popups;
using Windows.Storage;
using System.Collections.Generic;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;
using Studio_Photo_Collage.Infrastructure.Events;
using Lumia.Imaging;

namespace Studio_Photo_Collage.Models
{
    public class Collage
    {
        private Project project;

        public event EventHandler<SelectedImageChangedEventArg> SelectedImageChanged;

        public bool IsSaved { get; set; }
        public UIElement BackgroundGrid => (CollageGrid as Grid).Children[0];
        public UIElement MainGrid => (CollageGrid as Grid).Children[1];
        public Canvas FrameCanv => (CollageGrid as Grid).Children[2] as Canvas;

        public ToggleButton SelectedToggleBtn
        {
            get
            {
                var grid = MainGrid as Grid;
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    var gridInGrid = grid.Children[i] as Grid;
                    var ToggleBtn = gridInGrid.Children[0] as ToggleButton;
                    if ((bool)ToggleBtn.IsChecked)
                    {
                        return ToggleBtn;
                    }
                }
                return null;
            }
        }
        public ScrollViewer SelectedScrollViewer => SelectedToggleBtn?.Content as ScrollViewer;
        public Image SelectedImage => (SelectedScrollViewer?.Content as Grid)?.Children[0] as Image;

        public int SelectedImageNumberInList => SelectedToggleBtn != null ? (int)SelectedToggleBtn.CommandParameter : -1;

        public Project Project
        {
            get
            {
                IsSaved = false;
                return project;
            }
        }
        public UIElement CollageGrid { get; }

        public Collage(Project proj)
        {
            project = proj;
            CollageGrid = CreateCollage();
        }
        public Collage() { }

        public List<ToggleButton> GetListOfBtns()
        {
            var result = new List<ToggleButton>();
            var grid = MainGrid as Grid;
            for (int i = 0; i < grid.Children.Count; i++)
            {
                var gridInGrid = grid.Children[i] as Grid;
                var troggleBtn = gridInGrid.Children[0] as ToggleButton;
                result.Add(troggleBtn);
            }

            return result;
        }
        public List<InkCanvas> GetListInkCanvases()
        {
            var listInk = new List<InkCanvas>();

            var tbtnList = GetListOfBtns();
            foreach (var tbtn in tbtnList)
            {
                var scrollVewer = tbtn.Content as ScrollViewer;
                var scrollViwerContent = scrollVewer?.Content as Grid;
                var ink = scrollViwerContent?.Children[1] as InkCanvas;
                if (ink != null)
                {
                    listInk.Add(ink);
                }
            }
            return listInk;
        }

        public void UpdateUIAsync()
        {
            var grid = this.MainGrid as Grid;
            for (int i = 0; i < grid.Children.Count; i++)
            {
                var gridInsideOfGrid = grid.Children[i] as Grid;
                gridInsideOfGrid.BorderThickness = new Thickness(Project.BorderThickness);
            }

            var background = this.BackgroundGrid as Grid;
            background.Opacity = this.Project.BorderOpacity;
        }
        public async void UpdateProjectInfoAsync()
        {
            if (SelectedImage?.Source != null)
            {
                Project.ImageInfo[SelectedImageNumberInList].ImageBase64
                    = await ImageHelper.SaveToStringBase64Async(SelectedImage.Source);
            }

            var background = this.BackgroundGrid as Grid;
            var brush = background.Background;
            if (brush is ImageBrush imageBrush)
            {
                Project.Background = await ImageHelper.SaveToStringBase64Async(imageBrush.ImageSource);
            }
            else if (brush is SolidColorBrush solidColor)
            {
                Project.Background = solidColor.Color.ToString();
            }
        }

        public void DeleteSelectedImgFromBtn()
        {
            if(SelectedToggleBtn != null)
            {
                SelectedToggleBtn.Content = GetPlusSignIcon();
            }
        }
        public async Task SetImgByFilePickerToSelectedBtn()
        {
            if (SelectedToggleBtn != null)
            {
                var selectedTBtn = this.SelectedToggleBtn;
                var file = await ImageHelper.OpenFilePicker();
                if (file != null)
                {
                    await SetBtnContentAsync(selectedTBtn, file);
                }
            }
        }
        public void SetFrame(string pathData)
        {
            var pathFromCode = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{pathData}</Path.Data></Path>") as Path;
            pathFromCode.Fill = (SolidColorBrush)Application.Current.Resources["CustomBrush"];
            pathFromCode.VerticalAlignment = VerticalAlignment.Stretch;
            pathFromCode.HorizontalAlignment = HorizontalAlignment.Stretch;
            pathFromCode.Stretch = Stretch.Fill;
            pathFromCode.Width = FrameCanv.ActualWidth;
            pathFromCode.Height = FrameCanv.ActualHeight;

            Canvas.SetLeft(pathFromCode, 0);
            Canvas.SetTop(pathFromCode, 0);
            FrameCanv.Children.Clear();
            FrameCanv.Children.Add(pathFromCode);
        }
        public async void ApplyEffectToSelectedImage(List<Type> effectTypes)
        {
            if(SelectedImage?.Source != null)
            {
                var selectedImage = SelectedImage;
                var imgNumberInList = SelectedImageNumberInList;
                var result = (WriteableBitmap)await ImageHelper.FromBase64(Project.ImageInfo[imgNumberInList].ImageBase64Clear);

                foreach (var effectType in effectTypes)
                {
                    Project.ImageInfo[imgNumberInList].EffectsTypes.Add(effectType);
                    var effeсt = (IImageConsumer)Activator.CreateInstance(effectType);
                    result = await LumiaHelper.SetEffectToWritableBitmap(result, effeсt);
                }

                selectedImage.Source = result;

                UpdateProjectInfoAsync();
            }
        }

        #region UIElement creation
        private UIElement CreateCollage()
        {
            var proj = Project;
            var arr = proj.PhotoArray;

            var collageGrid = new Grid();
            var maingird = CollageGenerator.GetGridWith<Grid>(arr);
            var backgroundgrid = new Grid();
            var frameCanvas = new Canvas();

            for (int i = 0; i < maingird.Children.Count; i++)
            {
                var borderGridInGrid = maingird.Children[i] as Grid;

                borderGridInGrid.BorderThickness = new Thickness(Project.BorderThickness);
                borderGridInGrid.Background = new SolidColorBrush(Colors.Transparent);

                var btn = GetToggleBtnWithImg(i);
                borderGridInGrid.Children.Add(btn); ;
            }

            if(Project.Background.Length < 10)
            {
                backgroundgrid.Background = new SolidColorBrush(ColorHelper.ToColor(Project.Background));
            }
            else
            {
                backgroundgrid.Background = ColorGenerator.GetImageBrushFromString64(Project.Background);
            }

            backgroundgrid.Opacity = this.Project.BorderOpacity;

            frameCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            frameCanvas.VerticalAlignment = VerticalAlignment.Stretch;
            frameCanvas.SizeChanged += (o, e) =>
            {
                var canv = o as Canvas;
                if(canv.Children.Count != 0 && canv.Children[0] is Path path)
                {
                    path = (o as Canvas).Children[0] as Path;
                    path.Width = e.NewSize.Width;
                    path.Height = e.NewSize.Height;
                }
            };

            collageGrid.Children.Add(backgroundgrid);
            collageGrid.Children.Add(maingird);
            collageGrid.Children.Add(frameCanvas);

            return collageGrid;
        }

        private ToggleButton GetToggleBtnWithImg(int numberInList)
        {
            var toggleBtn = new ToggleButton();
            toggleBtn.Style = Application.Current.Resources["TemplatesToggleButton"] as Style;

            RestoreBtnContentAsync(toggleBtn, numberInList);
            toggleBtn.CommandParameter = numberInList;
            toggleBtn.Checked += async (o, e) =>
            {
                var Tbtn = o as ToggleButton;

                var comPar = (int)Tbtn.CommandParameter;
                UnCheckedAnothersBtns(comPar);
                await SetImgByFilePickerToSelectedBtn();

                var clearImageSource = await ImageHelper.FromBase64(Project.ImageInfo[numberInList].ImageBase64Clear);
                SelectedImageChanged?.Invoke(this, new SelectedImageChangedEventArg(clearImageSource, Project.ImageInfo[numberInList]));
            };
            toggleBtn.Unchecked += (o, e) =>
                SelectedImageChanged?.Invoke(this, new SelectedImageChangedEventArg(null, null));

            return toggleBtn;
        }

        private void UnCheckedAnothersBtns(int numberInList)
        {
            var grid = MainGrid as Grid;
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (i != numberInList)
                {
                    var gridInGrid = grid.Children[i] as Grid;
                    var ToggleBtn = gridInGrid.Children[0] as ToggleButton;
                    ToggleBtn.IsChecked = false;
                }
            }
        }

        private async void RestoreBtnContentAsync(ToggleButton toggleBtn, int numberInList)
        {
            toggleBtn.Content = GetLoadingRing();

            var img = new Image();
            img.Stretch = Stretch.Uniform;

            await ImageHelper.SetImgSourceFromBase64Async(img, Project.ImageInfo?[numberInList].ImageBase64);

            if (img.Source != null)
            {
                var scrollViewer = await GetScrollViewer(img, numberInList);
                toggleBtn.Content = scrollViewer;
            }
            else
            {
                toggleBtn.Content = GetPlusSignIcon();
            }
        }
        private async Task SetBtnContentAsync(ToggleButton selectedTBtn, StorageFile file)
        {
            selectedTBtn.Content = GetLoadingRing();
            var numberInList = (int)selectedTBtn.CommandParameter;

            using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                try
                {
                    var decoder = await BitmapDecoder.CreateAsync(fileStream);
                    var source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);

                    await source.SetSourceAsync(fileStream);
                    Project.ImageInfo[numberInList].ImageBase64Clear = await ImageHelper.SaveToStringBase64Async(source);

                    var img = new Image();
                    img.Stretch = Stretch.UniformToFill;
                    img.Source = source;

                    var scrollViewer = await GetScrollViewer(img);

                    selectedTBtn.Content = scrollViewer;

                }
                catch
                {
                    selectedTBtn.Content = GetPlusSignIcon();
                    var messageDialog = new MessageDialog("Image has not right format or it's too big");
                    await messageDialog.ShowAsync();
                }
            }
        }
        private async Task<ScrollViewer> GetScrollViewer(Image img, int numberInList = -1)
        {
            var scrollViewer = new ScrollViewer();

            scrollViewer.ZoomMode = ZoomMode.Enabled;

            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;

            scrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
            scrollViewer.VerticalScrollMode = ScrollMode.Enabled;

            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;

            var scrollViewerContent = new Grid();
            scrollViewerContent.Children.Add(img);
            scrollViewerContent.Children.Add(await GetInkCanvas(numberInList));
            scrollViewer.Content = scrollViewerContent;
           
            var bitmap = img.Source as WriteableBitmap;
            scrollViewer.Loaded += (ol, el) =>
            {

                var scrollV = ol as ScrollViewer;
                if (bitmap != null)
                {
                    var bitmapRatio = bitmap.PixelWidth * 1f / bitmap.PixelHeight;

                    if(bitmapRatio < 1f)
                    {
                        scrollV.MinZoomFactor = scrollV.ActualWidth < scrollV.ActualHeight ?
                        (float)(scrollV.ActualWidth / bitmap.PixelWidth) :
                        (float)(scrollV.ActualHeight / bitmap.PixelHeight);
                    }
                    else
                    {
                        scrollV.MinZoomFactor = scrollV.ActualWidth > scrollV.ActualHeight ?
                        (float)(scrollV.ActualWidth / bitmap.PixelWidth) :
                        (float)(scrollV.ActualHeight / bitmap.PixelHeight);
                    }    
                }

                scrollV.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (o, e) =>
                {
                    var scroll = o as ScrollViewer;
                    var btn = scroll.Parent as ToggleButton;
                    var nInList = (int)btn.CommandParameter;
                    Project.ImageInfo[nInList].ZoomInfo.ZoomFactor = scroll.ZoomFactor;
                    IsSaved = false;
                });
                scrollV.RegisterPropertyChangedCallback(ScrollViewer.HorizontalOffsetProperty, (o, e) =>
                {
                    var scroll = o as ScrollViewer;
                    var btn = scroll.Parent as ToggleButton;
                    var nInList = (int)btn.CommandParameter;
                    Project.ImageInfo[nInList].ZoomInfo.HorizontalOffset = scroll.HorizontalOffset;
                    IsSaved = false;
                });
                scrollV.RegisterPropertyChangedCallback(ScrollViewer.VerticalOffsetProperty, (o, e) =>
                {
                    var scroll = o as ScrollViewer;
                    var btn = scroll.Parent as ToggleButton;
                    var nInList = (int)btn.CommandParameter;
                    Project.ImageInfo[nInList].ZoomInfo.VerticalOffset = scroll.VerticalOffset;
                    IsSaved = false;
                });
            };

            if (numberInList == -1)
            {
                scrollViewer.Loaded += (o, e) =>
                {
                    (o as ScrollViewer).ChangeView(null, null, (o as ScrollViewer).MinZoomFactor);
                };
            }
            else
            {
                scrollViewer.Loaded += (o, e) =>
                {
                    var zoom = Project.ImageInfo[numberInList].ZoomInfo;
                    (o as ScrollViewer).ChangeView(zoom.HorizontalOffset, zoom.VerticalOffset, zoom.ZoomFactor, false);
                };
            }

            return scrollViewer;
        }
        private FontIcon GetPlusSignIcon()
        {
            var icon = new FontIcon();
            icon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            icon.Glyph = "\xE710";
            return icon;
        }
        private Loading GetLoadingRing()
        {
            var load = new Loading() { IsLoading = true };
            load.Content = new ProgressRing() { IsActive = true };
            load.HorizontalAlignment = HorizontalAlignment.Stretch;
            load.VerticalAlignment = VerticalAlignment.Stretch;
            return load;
        }
        private async Task<InkCanvas> GetInkCanvas(int numberInList)
        {
            var inkCanv = new InkCanvas();
            if(numberInList != -1)
            {
                await InkCanvasHelper.RestoreStrokesAsync(inkCanv.InkPresenter, string.Concat(Project.uid.ToString(), numberInList));
            }

            return inkCanv;

        }
        #endregion
    }
}
