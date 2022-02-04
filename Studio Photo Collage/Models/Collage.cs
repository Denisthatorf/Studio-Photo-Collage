using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
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
        private List<Image> ImageCollection = new List<Image>();

        private Project _myProject;
        public Project Project
        {
            get { return _myProject; }
            set { _myProject = value; }
        }

        private UIElement _collageGrid;
        public UIElement CollageGrid
        {
            get {  return _collageGrid; }
            set { _collageGrid = value; }
        }

        public UIElement BackgroundGrid => (CollageGrid as Grid).Children[0];
        public UIElement MainGrid => (CollageGrid as Grid).Children[1];
        public Image SelectedImage
        {
            get
            {
                var grid = MainGrid as Grid;
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    var gridInGrid = grid.Children[i] as Grid;
                    var ToggleBtn = gridInGrid.Children[0] as ToggleButton;
                    if ((bool)ToggleBtn.IsChecked)
                        return ToggleBtn.Content as Image;
                }
                return null;
            }
        }


        public Collage(Project _proj)
        {
            Project = _proj;
            InitializeGrid();
            //CollageGrid = CreateCollage();
        }
        public Collage() { }

        public void UpdateAll()
        {
            var grid = this.MainGrid as Grid;
            for (int i = 0; i < grid.Children.Count; i++)
            {
                var gridInsideOfGrid = grid.Children[i] as Grid;
                gridInsideOfGrid.BorderThickness = new Thickness(Project.BorderThickness);
            }

            var background = this.BackgroundGrid as Grid;
            background.Background = ColorGenerator.GetSolidColorBrush(this.Project.BorderColor);
            background.Opacity = this.Project.BorderOpacity;
        }

        private void InitializeGrid()
        {
            CollageGrid = CreateCollage();
        }

        #region UIElement creation
        private UIElement CreateCollage()
        {
            var proj = Project;
            var arr = proj.PhotoArray;

            var collageGrid = new Grid();
            var maingird = FromArrToGridConverter.GetGridWith<Grid>(arr);
            var backgroundgrid = new Grid();

            for (int i = 0; i < maingird.Children.Count; i++)
            {
                var gridInsideOfGrid = maingird.Children[i] as Grid;
                gridInsideOfGrid.BorderThickness = new Thickness(Project.BorderThickness);
                gridInsideOfGrid.Background = new SolidColorBrush(Colors.Transparent);

                var btn = GetToggleBtnWithImg(i);
                gridInsideOfGrid.Children.Add(btn); ;
            }
            backgroundgrid.Background = ColorGenerator.GetSolidColorBrush(this.Project.BorderColor);


            collageGrid.Children.Add(backgroundgrid);
            collageGrid.Children.Add(maingird);

            return collageGrid;
        }

        private ToggleButton GetToggleBtnWithImg(int numberInList)
        {
            var ToggleBtn = new ToggleButton();

            var img = new Image();
            img.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
            
            // if (Project.ImageArr[numberInList] != null)
             //  img.Source = await ImageHelper.FromBase64(Project.ImageArr[numberInList]);
            SetImageSourceAsync(img, numberInList);

            ToggleBtn.Content = img;
            ToggleBtn.Style = Application.Current.Resources["ToggleButtonProjStyle"] as Style;

            ToggleBtn.CommandParameter = numberInList; // number in PhotoArray
            ToggleBtn.Checked += async (o, e) =>
            {
                var Tbtn = o as ToggleButton;
                if (Tbtn.Content != null)
                {
                    var comPar = Tbtn.CommandParameter;
                    UnCheckedAnothersBtns((int)comPar);
                    await LoadMediaAsync((int)comPar, Tbtn.Content);
                }
            };
            return ToggleBtn;
        }

        private async Task LoadMediaAsync(int numberInList, object content)
        {
            var img = content as Image;
            StorageFile file = await ImageHelper.OpenFilePicker();

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    try
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                        WriteableBitmap source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                        await source.SetSourceAsync(fileStream);
                        img.Source = source;
                        Project.ImageArr[numberInList] = await ImageHelper.SaveToBytesAsync(source);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private async void SetImageSourceAsync(Image img, int numberInList)
        {
            if (Project.ImageArr[numberInList] != null)
                img.Source = await ImageHelper.FromBase64(Project.ImageArr[numberInList]);
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
