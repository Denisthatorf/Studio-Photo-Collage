﻿using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.ViewModels.PopUps;
using System;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class EnumValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
                return false;
            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
                return null;
            var rtnValue = parameter.ToString();
            try
            {
                object returnEnum = Enum.Parse(typeof(BtnNameEnum), rtnValue);
                return returnEnum;
            }
            catch (ArgumentException) { }
            try
            {
                object returnEnum = Enum.Parse(typeof(PaintClearEnum), rtnValue);
                return returnEnum;
            }
            catch (ArgumentException) { }

            return null;
        }
    }
}
