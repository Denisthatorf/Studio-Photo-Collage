﻿using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.ComponentModel;
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
    public class Collage : INotifyPropertyChanged
    {
        //private List<Image> ImageCollection = new List<Image>();

        private Project _myProject;
        public Project Project
        {
            get { return _myProject; }
            set { _myProject = value; }
        }

        private UIElement _collageGrid;
        public UIElement CollageGrid
        {
            get { return _collageGrid; }
            set { _collageGrid = value; NotifyPropertyChanged("CollageGrid"); }
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
        public int SelectedImageNumberInList
        {
            get
            {
                var grid = MainGrid as Grid;
                for (int i = 0; i < grid.Children.Count; i++)
                {
                    var gridInGrid = grid.Children[i] as Grid;
                    var ToggleBtn = gridInGrid.Children[0] as ToggleButton;
                    if ((bool)ToggleBtn.IsChecked)
                        return i;
                }
                return -1;
            }
        }


        public Collage(Project _proj)
        {
            Project = _proj;
            CollageGrid = CreateCollage();
        }
        public Collage() { }

        public async void UpdateUIAsync()
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
                Project.ImageArr[SelectedImageNumberInList] = await ImageHelper.SaveToStringBase64Async(SelectedImage.Source);

            var background = this.BackgroundGrid as Grid;
            var brush = background.Background;
            if (brush is ImageBrush imageBrush)
                Project.BackgroundColor = await ImageHelper.SaveToStringBase64Async(imageBrush.ImageSource);
            else if (brush is SolidColorBrush solidColor)
                Project.BackgroundColor = solidColor.Color.ToString();
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
                var borderMarginGridInGrid = maingird.Children[i] as Grid;

                borderMarginGridInGrid.BorderThickness = new Thickness(Project.BorderThickness);
                borderMarginGridInGrid.Background = new SolidColorBrush(Colors.Transparent);

                var btn = GetToggleBtnWithImg(i);
                borderMarginGridInGrid.Children.Add(btn); 
            }

            backgroundgrid.Background = BrushGenerator.GetBrushFromHexOrStrImgBase64(this.Project.BackgroundColor);
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
            //scrollViewer.MinZoomFactor = 0.9f;
            #endregion

            scrollViewer.Content = img;

            ToggleBtn.Content = scrollViewer;
            ToggleBtn.Style = Application.Current.Resources["ToggleButtonProjStyle"] as Style;

            ToggleBtn.CommandParameter = numberInList; // number in PhotoArray
            ToggleBtn.Checked += (o, e) =>
            {
                var Tbtn = o as ToggleButton;
                var scroll = Tbtn.Content as ScrollViewer;
                var imgInBtn = scroll.Content as Image;
                if(imgInBtn.Source == null)
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


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
