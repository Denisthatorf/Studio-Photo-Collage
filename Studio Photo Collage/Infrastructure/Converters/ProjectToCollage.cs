using System;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ProjectToUICollage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            Grid collage = null;
            if (value as Project != null)
            {
                if (parameter != null)
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
                        ImageHelper.SetImgSourceFromBase64Async(img, proj.ImageArr[i]);

                        gridInGrid.Children.Add(img);
                        gridInGrid.BorderThickness = new Windows.UI.Xaml.Thickness(proj.BorderThickness * (int.Parse(parameter as string) / 480.0));
                    }

                    if (proj.BackgroundColor.Length < 10)
                    {
                        backgroundGrid.Background = BrushGenerator.GetSolidColorBrushFromString(proj.BackgroundColor);
                    }
                    else
                    {
                        backgroundGrid.Background = BrushGenerator.GetImageBrushFromString64(proj.BackgroundColor);
                    }
                    backgroundGrid.Opacity = proj.BorderOpacity;

                    collage.Children.Add(backgroundGrid);
                    collage.Children.Add(mainGrid);
                    return collage;
                }
            }

            return collage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
