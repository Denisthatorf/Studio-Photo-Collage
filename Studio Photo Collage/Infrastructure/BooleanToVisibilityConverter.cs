using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                bool b = (bool)value;
                if (b == true)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
