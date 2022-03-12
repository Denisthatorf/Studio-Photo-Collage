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

namespace Studio_Photo_Collage.Models
{
    public class Collage
    {
        public UIElement BackgroundGrid => (CollageGrid as Grid).Children[0];
        public UIElement MainGrid => (CollageGrid as Grid).Children[1];

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
        public Image SelectedImage
        {
            get => SelectedToggleBtn?.Content as Image;
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
        public async void SetImgByFilePickerToSelectedBtn()
        {
            if (SelectedToggleBtn != null)
            {
                var selectedTBtn = this.SelectedToggleBtn;
                var numberInList = this.SelectedImageNumberInList;
                var img = new Image();
                img.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;

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
                            selectedTBtn.Content = img;
                            Project.ImageArr[numberInList] = await ImageHelper.SaveToStringBase64Async(source);
                            img.Source = source;
                            
                        }
                        catch 
                        {
                            selectedTBtn.Content = GetPlusSignIcon();
                            var messageDialog = new MessageDialog("Image has not right format or it is to big");
                            await messageDialog.ShowAsync();
                        }
                    }
                }
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

            if(Project.Background.Length < 10)
            {
                backgroundgrid.Background = new SolidColorBrush(ColorHelper.ToColor(Project.Background));
            }
            else
            {
                backgroundgrid.Background = ColorGenerator.GetImageBrushFromString64(Project.Background);
            }

            backgroundgrid.Opacity = this.Project.BorderOpacity;

            collageGrid.Children.Add(backgroundgrid);
            collageGrid.Children.Add(maingird);

            return collageGrid;
        }

        private ToggleButton GetToggleBtnWithImg(int numberInList)
        {
            var toggleBtn = new ToggleButton();
            toggleBtn.Style = Application.Current.Resources["TemplatesToggleButton"] as Style;

            RestoreBtnContentAsync(toggleBtn, numberInList);
            toggleBtn.CommandParameter = numberInList; // number in PhotoArray
            toggleBtn.Checked += (o, e) =>
            {
                var Tbtn = o as ToggleButton;
                var comPar = (int)Tbtn.CommandParameter;
                UnCheckedAnothersBtns(comPar);
                SetImgByFilePickerToSelectedBtn();
            };
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
        private async void RestoreBtnContentAsync(ToggleButton toggleBtn, int imgNumberInList)
        {
            toggleBtn.Content = GetLoadingRing();

            var img = new Image();
            img.Stretch = Stretch.Fill;

            await ImageHelper.SetImgSourceFromBase64Async(img, Project.ImageArr?[imgNumberInList]);

            if (img.Source != null)
            {
                toggleBtn.Content = img;
            }
            else
            {
                toggleBtn.Content = GetPlusSignIcon();
            }
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
        #endregion
    }
}
