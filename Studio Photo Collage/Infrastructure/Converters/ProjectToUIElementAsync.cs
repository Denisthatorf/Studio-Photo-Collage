using System;
using System.Threading.Tasks;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public static class ProjectToUIElementAsync
    {
        public static async Task<Grid> Convert(object value, object parameter)
        {
            Grid collage = null;
            if (value as Project != null && parameter != null)
            {

                var proj = value as Project;
                var arr = proj.PhotoArray;

                collage = new Grid();
                var mainGrid = CollageGenerator.GetGridWith<Grid>(arr);
                var backgroundGrid = new Grid();

                for (int i = 0; i < mainGrid.Children.Count; i++)
                {
                    var gridInGrid = mainGrid.Children[i] as Grid;

                    var img = new Image();
                    img.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
                    await ImageHelper.SetImgSourceFromBase64Async(img, proj.ImageInfo[i].ImageBase64);

                    gridInGrid.Children.Add(img);
                    gridInGrid.BorderThickness = new Windows.UI.Xaml.Thickness(proj.BorderThickness * (int.Parse(parameter.ToString()) / 480.0));
                }

                if (proj.Background.Length < 10)
                {
                    backgroundGrid.Background = new SolidColorBrush(ColorHelper.ToColor(proj.Background));
                }
                else
                {
                    var brush = new ImageBrush();
                    var source = await ImageHelper.FromBase64(proj.Background);
                    brush.ImageSource = source;
                    backgroundGrid.Background = brush;
                }
                backgroundGrid.Opacity = proj.BorderOpacity;

                collage.Children.Add(backgroundGrid);
                collage.Children.Add(mainGrid);
                return collage;
            }

            return collage;
        }
    }
}
