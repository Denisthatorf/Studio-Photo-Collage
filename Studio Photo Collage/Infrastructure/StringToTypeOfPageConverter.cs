using Studio_Photo_Collage.Views.PopUps.Settings;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure
{
    public class StringToTypeOfPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var frame = new Frame();
            var tag = (value as Microsoft.UI.Xaml.Controls.NavigationViewItem)?.Tag?.ToString();
            switch (tag)
            {
                case "Filters":
                    frame.Navigate(typeof(FiltersPage));
                    return frame;
                case "Background":
                    frame.Navigate(typeof(BackgroundPage));
                    return frame;
                case "Transform":
                    frame.Navigate(typeof(TransformPage));
                    return frame;
                case "Frames":
                    frame.Navigate(typeof(FramesPage));
                    return frame;
                case "Recent":
                    frame.Navigate(typeof(RecentPage));
                    return frame;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
