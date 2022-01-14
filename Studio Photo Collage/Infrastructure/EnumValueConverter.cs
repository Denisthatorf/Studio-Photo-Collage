using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure
{
	class EnumValueConverter : IValueConverter
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
			catch
			{
				return null;
			}
		}
    }
}
