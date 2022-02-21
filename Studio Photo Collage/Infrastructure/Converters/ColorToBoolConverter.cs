using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ColorToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) 
        {
            return value.ToString() == parameter.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null && parameter != null)
            {
                return ColorGenerator.GetColorFromString(parameter.ToString());
            }

            return default(Color);
        }
    }
}
