using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ProjectToCollage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value as Project != null)
            {
                if (parameter == null)
                    throw new ArgumentNullException(nameof(parameter));

                var proj = value as Project;
                var arr = proj.PhotoArray;
                var collageGrid = new Grid();
                var mainGrid = FromArrToGridConverter.GetGridWith<Grid>(arr);
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

                backgroundGrid.Background = BrushGenerator.GetBrushFromHexOrStrImgBase64(proj.BorderColor);
                backgroundGrid.Opacity = proj.BorderOpacity;

                collageGrid.Children.Add(backgroundGrid);
                collageGrid.Children.Add(mainGrid);
                return collageGrid;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
