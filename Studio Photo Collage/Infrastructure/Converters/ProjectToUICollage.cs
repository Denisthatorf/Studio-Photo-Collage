using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ProjectToUICollage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //var collage = new Collage(value as Project);
            //return collage.CollageGrid;

            Grid collage = null;
            if (value as Project != null && parameter != null)
            {
                var project = value as Project;
                var arr = project.PhotoArray;

                collage = new Grid();
                var mainGrid = CollageGenerator.GetGridWith<Grid>(arr);
                var backgroundGrid = new Grid();
                var frameCanvas = new Canvas();
                for (int i = 0; i < mainGrid.Children.Count; i++)
                {
                    var gridInGrid = mainGrid.Children[i] as Grid;
                    gridInGrid.BorderThickness = new Windows.UI.Xaml.Thickness(project.BorderThickness * (int.Parse(parameter.ToString()) / 480.0));

                    var img = new Image();
                    img.Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill;

                    var info = project.ImageInfo[i];
                    SetImageSourceAsync(img, info);

                    var inkCanvas = new InkCanvas();
                    RestoreStrokes(inkCanvas, project.uid, i);

                    gridInGrid.Children.Add(img);
                    gridInGrid.Children.Add(inkCanvas);
                }

                if (project.Background.Length < 10)
                {
                    backgroundGrid.Background = new SolidColorBrush(ColorHelper.ToColor(project.Background));
                }
                else
                {
                    backgroundGrid.Background = ColorGenerator.GetImageBrushFromString64(project.Background);
                }

                backgroundGrid.Opacity = project.BorderOpacity;

                frameCanvas.HorizontalAlignment = HorizontalAlignment.Stretch;
                frameCanvas.VerticalAlignment = VerticalAlignment.Stretch;

                frameCanvas.SizeChanged += (o, e) => SetFrame(project, frameCanvas, int.Parse(parameter.ToString()));

                collage.Children.Add(backgroundGrid);
                collage.Children.Add(mainGrid);
                collage.Children.Add(frameCanvas);
                return collage;
            }

            return collage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static async void SetImageSourceAsync(Image img, ImageInfo info) 
        {
            await ImageHelper.SetImgSourceFromBase64Async(img, info.ImageBase64);
            var wrBitmap = img.Source as WriteableBitmap;
            if(wrBitmap != null)
            {
                var x = (int)(info.ZoomInfo.HorizontalOffset / info.ZoomInfo.ZoomFactor);
                var y = (int)(info.ZoomInfo.VerticalOffset / info.ZoomInfo.ZoomFactor);
                var width = (wrBitmap.PixelWidth - x) / info.ZoomInfo.ZoomFactor;
                var height = (wrBitmap.PixelHeight - y) / info.ZoomInfo.ZoomFactor;
                img.Source = wrBitmap.Crop(x, y,
                    (int)width, (int)height);
            }
        }
        private static void SetFrame(Project project, Canvas canv, int parameter)
        {
            if (!string.IsNullOrEmpty(project.Frame.PathData))
            {
                var path = XamlReader.Load($"<Path xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><Path.Data>{project.Frame.PathData}</Path.Data></Path>") as Path;
                path.Fill = new SolidColorBrush(project.Frame.Color.ToColor());
                path.VerticalAlignment = VerticalAlignment.Stretch;
                path.HorizontalAlignment = HorizontalAlignment.Stretch;
                path.Stretch = Stretch.Fill;

                var size = project.Frame.AdditionalSize * (int.Parse(parameter.ToString()) / 480.0);
                path.Width = canv.ActualWidth + size;
                path.Height = canv.ActualHeight + size;
                Canvas.SetLeft(path, -size / 2);
                Canvas.SetTop(path, -size / 2);

                canv.Children.Add(path);
            }
        }
        private async void RestoreStrokes(InkCanvas inkCanvas, int uid, int i)
        {
            await InkCanvasHelper.RestoreStrokesAsync(inkCanvas.InkPresenter, uid, i);
        }
    }
}
