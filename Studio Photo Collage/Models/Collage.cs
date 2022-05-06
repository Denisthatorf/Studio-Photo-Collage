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
using System.Linq;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Studio_Photo_Collage.Models
{
    public class Collage
    {
        private const float MAIN_SIZE = 480;
        private readonly Project project;
        private float r => (float)((CollageGrid as Grid).ActualWidth / MAIN_SIZE);

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
        public List<Image> GetListOfImagesWithNullIfNoImage()
        {
            var imageList = new List<Image>();

            var tbtnList = GetListOfBtns();
            foreach (var tbtn in tbtnList)
            {
                var scrollVewer = tbtn.Content as ScrollViewer;
                var scrollViwerContent = scrollVewer?.Content as Grid;
                var image = scrollViwerContent?.Children[0] as Image;
                imageList.Add(image);
            }
            return imageList;
        }

        public void UpdateUIAsync()
        {
            var grid = MainGrid as Grid;
            for (int i = 0; i < grid.Children.Count; i++)
            {
                var gridInsideOfGrid = grid.Children[i] as Grid;
                gridInsideOfGrid.BorderThickness = new Thickness(Project.BorderThickness * r);
            }

            var background = BackgroundGrid as Grid;
            background.Opacity = Project.BorderOpacity;
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

        public async Task DeleteSelectedImgFromBtn()
        {
            if(SelectedToggleBtn != null)
            {
                await InkCanvasHelper.DeleteStrokeFileByUid(Project.uid, SelectedImageNumberInList);
                SelectedToggleBtn.Content = GetPlusSignIcon();
            }
        }
        public async Task SetImgByFilePickerToSelectedBtnAsync()
        {
            if (SelectedToggleBtn != null)
            {
                var selectedTBtn = this.SelectedToggleBtn;
                var numberInList = (int)selectedTBtn.CommandParameter;
                var file = await ImageHelper.OpenFilePicker();
                if (file != null)
                {
                    selectedTBtn.Content = GetLoadingRing();

                    using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        try
                        {
                            var decoder = await BitmapDecoder.CreateAsync(fileStream);
                            var source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);

                            await source.SetSourceAsync(fileStream);
                            await SetImageToSelectedBtnAsync(source);
                        }
                        catch
                        {
                            selectedTBtn.Content = GetPlusSignIcon();
                            var messageDialog = new MessageDialog("Image has not right format or it's too big");
                            await messageDialog.ShowAsync();
                        }
                    }
                }

                var clearImageInfo = Project.ImageInfo[numberInList]?.ImageBase64Clear;
                if (clearImageInfo != null) 
                {
                    var clearImageSource = await ImageHelper.FromBase64(clearImageInfo);
                    SelectedImageChanged?.Invoke(this, new SelectedImageChangedEventArg(clearImageSource, Project.ImageInfo[numberInList]));
                }
            }
        }
        public async Task SetImageToSelectedBtnAsync(ImageSource source)
        {
            await DeleteSelectedImgFromBtn();

            var selectedTBtn = SelectedToggleBtn;
            var numberInList = (int)selectedTBtn.CommandParameter;
            Project.ImageInfo[numberInList].ImageBase64Clear = await ImageHelper.SaveToStringBase64Async(source);

            var img = new Image();
            img.Stretch = Stretch.UniformToFill;
            img.Source = source;

            if(Project.IsFilltersUsedToAllImages == true)
            {
                var effects = new List<Type>();
                effects.AddRange(Project.ImageInfo[numberInList].EffectsTypes);
                await ApplyEffectToImage(effects, img, numberInList);
            }
            else
            {
                Project.ImageInfo[numberInList].EffectsTypes.Clear();
            }

            var scrollViewer = await GetScrollViewer(img);
            selectedTBtn.Content = scrollViewer;

            //if (Project.ImageInfo[numberInList]?.ImageBase64Clear != null)
            //{
            //    var clearImageSource = await ImageHelper.FromBase64(Project.ImageInfo[numberInList]?.ImageBase64Clear);
            //    SelectedImageChanged?.Invoke(this, new SelectedImageChangedEventArg(clearImageSource, Project.ImageInfo[numberInList]));
            //}
        }

        public void SetFrame(string pathData)
        {
            FrameCanv.Children.Clear();
            if (!string.IsNullOrEmpty(pathData))
            {
                var pathFromCode = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{pathData}</Path.Data></Path>") as Path;
                pathFromCode.Fill = new SolidColorBrush(Project.Frame.Color.ToColor());
                pathFromCode.VerticalAlignment = VerticalAlignment.Stretch;
                pathFromCode.HorizontalAlignment = HorizontalAlignment.Stretch;
                pathFromCode.Stretch = Stretch.Fill;
                //pathFromCode.Width = FrameCanv.ActualWidth;
                //pathFromCode.Height = FrameCanv.ActualHeight;

                Canvas.SetLeft(pathFromCode, 0);
                Canvas.SetTop(pathFromCode, 0);
                FrameCanv.Children.Add(pathFromCode);

                Project.Frame.PathData = pathData;
                SetFrameAdditionalSize(Project.Frame.AdditionalSize);
                SetFrameColor(new SolidColorBrush(Project.Frame.Color.ToColor()));
            }
        }
        public void SetFrameAdditionalSize(int size)
        {
            var canv = FrameCanv;
            var path = canv.Children[0] as Path;
            path.Width = canv.ActualWidth + size;
            path.Height = canv.ActualHeight + size;
            Canvas.SetLeft(path, -size / 2);
            Canvas.SetTop(path, -size / 2);

            Project.Frame.AdditionalSize = size;
        }
        public void SetFrameColor(SolidColorBrush color)
        {
            var path = (FrameCanv.Children[0] as Path);
            if(path != null)
            {
                path.Fill = color;
                Project.Frame.Color = color.Color.ToString();
            }
        }

        public async Task ApplyEffectToImage(List<Type> effectTypes, Image image, int imgNumberInList)
        {
            var effects = Project.ImageInfo[imgNumberInList].EffectsTypes;
            effects.Clear();
            effects.AddRange(effectTypes);

            if (image?.Source != null)
            {
                var result = (WriteableBitmap)await ImageHelper.FromBase64(Project.ImageInfo[imgNumberInList].ImageBase64Clear);

                foreach (var effectType in effectTypes)
                {
                    var effeсt = (IImageConsumer)Activator.CreateInstance(effectType);
                    result = await LumiaHelper.SetEffectToWritableBitmap(result, effeсt);
                }

                image.Source = result;

                Project.ImageInfo[imgNumberInList].ImageBase64 = await ImageHelper.SaveToStringBase64Async(result);
            }
        }
        public async void ApplyEffectToAllImageImage(List<Type> effectTypes)
        {
            var images = GetListOfImagesWithNullIfNoImage();
            for (int i = 0; i < images.Count; i++)
            {
                await ApplyEffectToImage(effectTypes, images[i], i);
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

            if (Project.Background.Length < 10)
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
                if (canv.Children.Count != 0 && canv.Children[0] is Path path)
                {
                    path = canv.Children[0] as Path;
                    SetFrameAdditionalSize(Project.Frame.AdditionalSize);
                }
            };

            frameCanvas.IsHitTestVisible = false;
            frameCanvas.IsTapEnabled = false;

            void SetFrameFirstTime(object o, SizeChangedEventArgs e) {
                SetFrame(Project.Frame.PathData);
                frameCanvas.SizeChanged -= SetFrameFirstTime;
            };
            frameCanvas.SizeChanged += SetFrameFirstTime;

            collageGrid.Children.Add(backgroundgrid);
            collageGrid.Children.Add(maingird);
            collageGrid.Children.Add(frameCanvas);

            collageGrid.SizeChanged += CollageGridSizeChanged; 
            return collageGrid;
        }

        private void CollageGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var r = (float)((CollageGrid as Grid).ActualWidth / 480);
            var btns = GetListOfBtns();
            var scrolls = btns.Select(x => x.Content as ScrollViewer).ToList();

            for (int i = 0; i < scrolls.Count; i++)
            {
                var scroll = scrolls[i];
                if(scroll != null)
                {
                    var imageInfo = Project.ImageInfo[i];
                    var horizontalOffsetForSaving = Project.ImageInfo[i].ZoomInfo.HorizontalOffset;
                    var verticallOffsetForSaving = Project.ImageInfo[i].ZoomInfo.VerticalOffset;
                    var zoomFactorForSaving = Project.ImageInfo[i].ZoomInfo.ZoomFactor;

                    scroll.ChangeView(
                        horizontalOffsetForSaving * r,
                        verticallOffsetForSaving * r, 
                        zoomFactorForSaving * r, 
                        false);
                }
            }

            UpdateUIAsync();
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

                if(Tbtn.Content is ScrollViewer scroll)
                {
                    var clearImageInfo = Project.ImageInfo[numberInList]?.ImageBase64Clear;
                    if (clearImageInfo != null)
                    {
                        var clearImageSource = await ImageHelper.FromBase64(clearImageInfo);
                        SelectedImageChanged?.Invoke(this, new SelectedImageChangedEventArg(clearImageSource, Project.ImageInfo[numberInList]));
                    }
                }
                else
                {
                    await SetImgByFilePickerToSelectedBtnAsync();
                }
            };
            toggleBtn.Unchecked += (o, e) =>
            {
                var tBtns = GetListOfBtns();
                if (!tBtns.Any(x => x.IsChecked == true))
                {
                    SelectedImageChanged?.Invoke(this, new SelectedImageChangedEventArg(null, null));
                }
            };

            return toggleBtn;
        }
        private void UnCheckedAnothersBtns(int numberInList)
        {
            var tBtns = GetListOfBtns();
            var grid = MainGrid as Grid;
            foreach (var tBtn in tBtns)
            {
                if ((int)tBtn.CommandParameter != numberInList)
                {
                    tBtn.IsChecked = false;
                }
            }
        }

        private async void RestoreBtnContentAsync(ToggleButton toggleBtn, int numberInList)
        {
            toggleBtn.Content = GetLoadingRing();

            var img = new Image();
            img.Stretch = Stretch.Uniform;

            await ImageHelper.SetImgSourceFromBase64Async(img, Project.ImageInfo[numberInList].ImageBase64);

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

        private async Task<ScrollViewer> GetScrollViewer(Image img, int numberInList = -1)
        {
            img.Stretch = Stretch.Uniform;
            var scrollViewer = new ScrollViewer();

            scrollViewer.ZoomMode = ZoomMode.Enabled;

            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;

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
                SetScrollViewMinZoomFactor(scrollV, bitmap);

                scrollV.RegisterPropertyChangedCallback(ScrollViewer.ZoomFactorProperty, (o, e) =>
                {
                    var scroll = o as ScrollViewer;
                    var btn = scroll.Parent as ToggleButton;
                    var nInList = (int)btn.CommandParameter;
                    Project.ImageInfo[nInList].ZoomInfo.ZoomFactor = scroll.ZoomFactor / r;
                    IsSaved = false;
                });
                scrollV.RegisterPropertyChangedCallback(ScrollViewer.HorizontalOffsetProperty, (o, e) =>
                {
                    var scroll = o as ScrollViewer;
                    var btn = scroll.Parent as ToggleButton;
                    var nInList = (int)btn.CommandParameter;
                    Project.ImageInfo[nInList].ZoomInfo.HorizontalOffset = scroll.HorizontalOffset / r;
                    IsSaved = false;
                });
                scrollV.RegisterPropertyChangedCallback(ScrollViewer.VerticalOffsetProperty, (o, e) =>
                {
                    var scroll = o as ScrollViewer;
                    var btn = scroll.Parent as ToggleButton;
                    var nInList = (int)btn.CommandParameter;
                    Project.ImageInfo[nInList].ZoomInfo.VerticalOffset = scroll.VerticalOffset / r;
                    IsSaved = false;
                });
                scrollV.SizeChanged += (o, e) => SetScrollViewMinZoomFactor(scrollV, bitmap);
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
                    scrollViewer.ChangeView(zoom.HorizontalOffset * r, zoom.VerticalOffset * r, zoom.ZoomFactor * r, false);
                };
            }

            return scrollViewer;
        }
        private void SetScrollViewMinZoomFactor(ScrollViewer scrollV, WriteableBitmap bitmap)
        {
            /*if (scrollRatio < 2)
            {
                scrollV.MinZoomFactor = (float)(1 / bitmapRatio / scrollRatio);

            }
            else if (scrollRatio > 2)
            {
                scrollV.MinZoomFactor = scrollV.ActualWidth > scrollV.ActualHeight ?
                    (float)(scrollV.ActualWidth / bitmap.PixelWidth) :
                    (float)(scrollV.ActualHeight / bitmap.PixelHeight);
            }*/

            if (bitmap != null)
            {
                var bitmapRatio = (float)(bitmap.PixelWidth * 1.0 / bitmap.PixelHeight);
                var scrollRatio = (float)(scrollV.ActualWidth * 1.0 / scrollV.ActualHeight);

                if (bitmapRatio < 1f)
                {
                    if (scrollRatio < 1f)
                    {
                        scrollRatio = (float)(scrollV.ActualHeight * 1.0 / scrollV.ActualWidth);
                        scrollV.MinZoomFactor = bitmapRatio * scrollRatio;
                    }
                    else if (scrollRatio > 1f)
                    {
                        scrollV.MinZoomFactor = bitmapRatio * scrollRatio;
                    }
                }
                else if (bitmapRatio > 1f)
                {
                   if(scrollRatio < 1f)
                   {
                        scrollV.MinZoomFactor = bitmapRatio * scrollRatio;
                   }
                   else if (scrollRatio > 1f)
                   {
                        bitmapRatio = (float)(bitmap.PixelHeight * 1.0 / bitmap.PixelWidth);
                        scrollRatio = (float)(scrollV.ActualHeight * 1.0 / scrollV.ActualWidth);
                        scrollV.MinZoomFactor = bitmapRatio * scrollRatio;
                   }
                }
            }
        }

        private async Task<InkCanvas> GetInkCanvas(int numberInList)
        {
            var inkCanv = new InkCanvas();
            if (numberInList != -1)
            {
                await InkCanvasHelper.RestoreStrokesAsync(inkCanv.InkPresenter, Project.uid, numberInList);
            }
            return inkCanv;
        }
        private static FontIcon GetPlusSignIcon()
        {
            var icon = new FontIcon();
            icon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            icon.Glyph = "\xE710";
            return icon;
        }
        private static Loading GetLoadingRing()
        {
            var load = new Loading()
            {
                IsLoading = true,
                Content = new ProgressRing() { IsActive = true },
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            return load;
        }
        #endregion
    }
}
