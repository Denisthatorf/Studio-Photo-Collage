using Studio_Photo_Collage.Views.PopUps.Settings;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class StringToTypeOfPageHelper
    {
        public static Type Convert(string str)
        {
            switch (str)
            {
                case "Filters":
                    //frame.Navigate(typeof(FiltersPage));
                    // return frame;
                    return typeof(FiltersPage);
                case "Templates":
                    return typeof(TemplatesSidePage);
                case "Background":
                    return typeof(BackgroundPage);
                case "Transform":
                    return typeof(TransformPage);
                case "Frames":
                    return typeof(FramesPage);
                case "Recent":
                    return typeof(RecentPage);
            }
            return null;
        }
    }
}
