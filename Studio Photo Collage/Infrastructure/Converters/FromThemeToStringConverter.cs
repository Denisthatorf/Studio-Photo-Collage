using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ThemeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var theme = (ElementTheme)value;

            switch (theme)
            {
                case ElementTheme.Light:
                    return "Light theme";
                case ElementTheme.Dark:
                    return "Dark theme";
                case ElementTheme.Default:
                    return "Custom theme";
                default:
                    return "Custom theme";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var str = (string)value;

            switch (str)
            {
                case "Light theme":
                    return ElementTheme.Light;
                case "Dark theme":
                    return ElementTheme.Dark;
                case "Custom theme":
                    return ElementTheme.Default;
                default:
                    return ElementTheme.Default;
            }
        }
    }
}
