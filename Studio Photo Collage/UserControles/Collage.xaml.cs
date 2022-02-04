using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Studio_Photo_Collage.UserControles
{
    public sealed partial class Collage : UserControl
    {
        public Project MyProject
        {
            get { return (Project)GetValue(MyProjectProperty);
                InitializeDataAsync(); 
                UpdateAll(); }
            set { SetValue(MyProjectProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyProjectProperty =
            DependencyProperty.Register("MyProject", typeof(Project), typeof(Collage), new PropertyMetadata(null));

        private Grid _mainGrid;
        public Grid MainGrid
        {
            get { return _mainGrid; }
            set { _mainGrid = value; }
        }

        private void UpdateAll()
        {
            for (int i = 0; i < MainGrid.Children.Count; i++)
            {
                var gridInsideOfGrid = MainGrid.Children[i] as Grid;
                gridInsideOfGrid.BorderThickness = new Thickness(MyProject.BorderThickness);
            }

            Background = ColorGenerator.GetSolidColorBrush(MyProject.BorderColor);
        }

        public async Task<UIElement> CreateCollage()
        {
            var proj = MyProject;
            var arr = proj.PhotoArray;
            var maingird = FromArrToGridConverter.GetGridWith<Grid>(arr);

            for (int i = 0; i < maingird.Children.Count; i++)
            {
                var gridInsideOfGrid = maingird.Children[i] as Grid;
                gridInsideOfGrid.BorderThickness = new Thickness(MyProject.BorderThickness);
                gridInsideOfGrid.Background = new SolidColorBrush(Colors.Transparent);

                var btn = await GetToggleBtnWithImg(i);
                gridInsideOfGrid.Children.Add(btn); ;
            }
            return maingird;
        }

        private async Task<ToggleButton> GetToggleBtnWithImg(int numberInList)
        {
            var ToggleBtn = new ToggleButton();

            var img = new Image();
            img.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
            if (MyProject.ImageArr[numberInList] != null)
                img.Source = await ImageHelper.FromBase64(MyProject.ImageArr[numberInList]);
            //img.Source = await ImageHelper.ByteArrayToImageAsync(this.ImagesInBytes[numberInList], new InMemoryRandomAccessStream());

            ToggleBtn.Content = img;
            ToggleBtn.Style = Application.Current.Resources["ToggleButtonProjStyle"] as Style;

            ToggleBtn.CommandParameter = numberInList; // number in PhotoArray
            ToggleBtn.Checked += async (o, e) =>
            {
                var Tbtn = o as ToggleButton;
                if (Tbtn.Content != null)
                {
                    var comPar = Tbtn.CommandParameter;
                    //UnCheckedAnothersBtns((int)comPar);
                    await LoadMedia((int)comPar, Tbtn.Content);
                }
            };
            return ToggleBtn;
        }
        public async Task LoadMedia(int numberInList, object content)
        {
            /* var objGrid = (Collage as Grid).Children[numberInList];
            var objBtn = (objGrid as Grid).Children[0];
            var Tbtn = objBtn as ToggleButton;*/
            var img = content as Image;

            StorageFile file = await ImageHelper.OpenFilePicker();

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    WriteableBitmap source = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                    await source.SetSourceAsync(fileStream);
                    img.Source = source;
                    MyProject.ImageArr[numberInList] = await ImageHelper.SaveToBytesAsync(source);
                }
            }
        }
        private void UnCheckedAnothersBtns(int numberInList)
        {
            var gridd = MainGrid as Grid;
            var grid = gridd.Children[1] as Grid;
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

        public Collage()
        {
            this.InitializeComponent();
           // var ignored = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow
           //      .Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => InitializeComponent());
        }
        public async  void InitializeDataAsync()
        {
            MainGrid = await CreateCollage() as Grid;
        }
    }
}
