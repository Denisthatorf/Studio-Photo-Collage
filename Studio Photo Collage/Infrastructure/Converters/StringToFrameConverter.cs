using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public static class StringToFrameConverter
    {
        public static Frame Convert(object value)
        {
            if (value != null)
            {
                var strEnum = value.ToString();
                var frame = new Frame();
                switch (strEnum)
                {
                    case "Filters":
                        frame.Navigate(typeof(FiltersPage));
                        return frame;
                    case "Templates":
                        frame.Navigate(typeof(TemplatesSidePage));
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
                    case "Resents":
                        frame.Navigate(typeof(ResentsPage));
                        return frame;
                }
            }
            return null;
        }
    }
}
