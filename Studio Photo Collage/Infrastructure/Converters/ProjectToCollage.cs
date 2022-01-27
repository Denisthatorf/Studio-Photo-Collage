using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ProjectToCollage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var arr = (value as Project).PhotoArray;
            var grid = FromArrToGridConverter.GetGridWith<Image>(arr);

            foreach (var gridChild in grid.Children)
            {
                //var canvas = gridChild as Canvas;
                BitmapImage source = new BitmapImage(new Uri("ms-appx:///Assets/StartPageCentral.jpg"));
                var img = gridChild as Image;
                img.Source = source;
                img.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
                //var img = new Image() { Source = source, Stretch = Windows.UI.Xaml.Media.Stretch.Fill};
                //canvas.Children.Add( new Image() { Source = source, Stretch = Windows.UI.Xaml.Media.Stretch.Uniform});
            }

            return grid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
