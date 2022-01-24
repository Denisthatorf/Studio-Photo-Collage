using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class ColorGenerator
    {
        public static List<SolidColorBrush> GenerateColors()
        {
            int H = 359;
            int S;
            int L;
            //int i = 0;
            var colors = new List<SolidColorBrush>();
            while (H > 0)
            {
                S = 84;
                L = 82;
                while (S <= 100 && L >= 50)
                {
                    var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.FromHsl(H, S / 100.0, L / 100.0);
                    var brush = new SolidColorBrush(color);
                    colors.Add(brush);
                    S += 4;
                    L -= 8;
                }

                H -= 30;
                //i++;
            }
            return colors;
        }

        public static SolidColorBrush GetSolidColorBrush(string colour)
        {
                var c = Color.FromArgb(255,
                        Convert.ToByte(colour.Substring(0, 2), 16),
                        Convert.ToByte(colour.Substring(2, 2), 16),
                        Convert.ToByte(colour.Substring(4, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(c);
            return myBrush;
        }
    }
}
