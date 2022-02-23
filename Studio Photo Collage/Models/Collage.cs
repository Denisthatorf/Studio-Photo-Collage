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

namespace Studio_Photo_Collage.Models
{
    public class Collage
    {
        public UIElement BackgroundGrid => (CollageGrid as Grid).Children[0];
        public UIElement MainGrid => (CollageGrid as Grid).Children[1];

        public ScrollViewer SelectedImgZoom
        {
            get
            {
                if(SelectedImageNumberInList == -1)
                {
                    return null;
                }

                var grid = MainGrid as Grid;
                var gridInGrid = grid.Children[SelectedImageNumberInList] as Grid;
                var ToggleBtn = gridInGrid.Children[0] as ToggleButton;
                return ToggleBtn.Content as ScrollViewer;
            }
        }
        public Image SelectedImage
        {
            get
            {
               return SelectedImgZoom?.Content as Image;
            }
        }
        public int SelectedImageNumberInList
        {
            get
            {
                var result = -1;
                var grid = MainGrid as Grid;
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    var gridInGrid = grid.Children[i] as Grid;
                    var ToggleBtn = gridInGrid.Children[0] as ToggleButton;
                    if ((bool)ToggleBtn.IsChecked)
                    {
                        result = i;
                    }
                }

                return result;
            }
        }

        public Project Project { get; set; }
        public UIElement CollageGrid { get; }

        public Collage(Project _proj)
        {
            Project = _proj;
            CollageGrid = CreateCollage();
        }
        public Collage() { }

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
                Project.ImageArr[SelectedImageNumberInList] = await ImageHelper.SaveToStringBase64Async(SelectedImage.Source);
            }

            var background = this.BackgroundGrid as Grid;
            var brush = background.Background;
            if (brush is ImageBrush imageBrush)
            {
                Project.BackgroundColor = await ImageHelper.SaveToStringBase64Async(imageBrush.ImageSource);
            }
            else if (brush is SolidColorBrush solidColor)
            {
                Project.BackgroundColor = solidColor.Color.ToString();
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

            for (int i = 0; i < maingird.Children.Count; i++)
            {
                var borderGridInGrid = maingird.Children[i] as Grid;

                borderGridInGrid.BorderThickness = new Thickness(Project.BorderThickness);
                borderGridInGrid.Background = new SolidColorBrush(Colors.Transparent);

                var btn = GetToggleBtnWithImg(i);
                borderGridInGrid.Children.Add(btn); ;
            }

            if(Project.BackgroundColor.Length < 10)
            {
                backgroundgrid.Background = BrushGenerator.GetSolidColorBrushFromString(Project.BackgroundColor);
            }
            else
            {
                backgroundgrid.Background = BrushGenerator.GetImageBrushFromString64(Project.BackgroundColor);
            }
            backgroundgrid.Opacity = this.Project.BorderOpacity;

            collageGrid.Children.Add(backgroundgrid);
            collageGrid.Children.Add(maingird);

            return collageGrid;
        }

        private ToggleButton GetToggleBtnWithImg(int numberInList)
        {
            var ToggleBtn = new ToggleButton();
            var scrollViewer = new ScrollViewer();
            var img = new Image();

            img.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
            img.HorizontalAlignment = HorizontalAlignment.Stretch;

            ImageHelper.SetImgSourceFromBase64Async(img, Project.ImageArr?[numberInList]);

            #region Zoom settings
            scrollViewer.ZoomMode = ZoomMode.Enabled;
            scrollViewer.IsVerticalScrollChainingEnabled = false;

            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            scrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;

            scrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
            scrollViewer.VerticalScrollMode = ScrollMode.Enabled;

            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
           
            #endregion

            scrollViewer.Content = img;

            ToggleBtn.Content = scrollViewer;

            ToggleBtn.Style = Application.Current.Resources["TemplatesToggleButton"] as Style;

            ToggleBtn.CommandParameter = numberInList; // number in PhotoArray
            ToggleBtn.Checked += (o, e) =>
            {
                var Tbtn = o as ToggleButton;
                var scroll = Tbtn.Content as ScrollViewer;
                var imgInBtn = scroll.Content as Image;
                if (imgInBtn.Source == null)
                {
                    var comPar = Tbtn.CommandParameter;
                    UnCheckedAnothersBtns((int)comPar);
                    LoadMediaAsync((int)comPar, scroll.Content);
                }
            };
            ToggleBtn.Unchecked += (o, e) =>
            {
                var Tbtn = o as ToggleButton;
                var scroll = Tbtn.Content as ScrollViewer;
                var imgInBtn = scroll.Content as Image;

                var comPar = Tbtn.CommandParameter;
                LoadMediaAsync((int)comPar, scroll.Content);
            };
            return ToggleBtn;
        }

        private async void LoadMediaAsync(int numberInList, object content)
        {
            var img = content as Image;
            var file = await ImageHelper.OpenFilePicker();

            if (file != null)
            {
                using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {

                    try
                    {
                        var decoder = await BitmapDecoder.CreateAsync(fileStream);
                        var source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                        await source.SetSourceAsync(fileStream);
                        img.Source = source;
                        Project.ImageArr[numberInList] = await ImageHelper.SaveToStringBase64Async(source);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
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
        #endregion
    }
}
