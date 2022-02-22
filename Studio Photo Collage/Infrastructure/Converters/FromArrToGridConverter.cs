using System;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Shapes;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class FromArrToGridConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var grid = CollageGenerator.GetGridWith<Rectangle>(value as byte[,]);
            return grid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
