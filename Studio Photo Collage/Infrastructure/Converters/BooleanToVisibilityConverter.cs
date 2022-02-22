using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = Visibility.Collapsed;
            if (value != null)
            {
                bool b = (bool)value;
                result = b == true ? Visibility.Visible : Visibility.Collapsed;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var result = false;
            if (value != null)
            {
                var visibility = (Visibility)value;
                result = visibility == Visibility.Visible;
            }

            return result;
        }
    }
}
