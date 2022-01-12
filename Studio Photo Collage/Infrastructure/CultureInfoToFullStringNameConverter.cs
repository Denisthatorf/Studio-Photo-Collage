﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure
{
    public class CultureInfoToFullStringNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var culture = parameter as CultureInfo;
            if (culture != null)
            {
                var strCulture = culture.ToString();

                switch (strCulture)
                {
                    case "en-US":
                        return "English";
                    case "ru-RU":
                        return "Russian";
                    default:
                        return "English";
                }
            }
            return "English";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var culture = value.ToString();
                var strCulture = culture.ToString();

                switch (strCulture)
                {
                    case "English":
                        return new CultureInfo("en-US");
                    case "Russian":
                        return new CultureInfo("ru-RU");
                    default:
                        return new CultureInfo("en-US");
                }
        }
    }
}