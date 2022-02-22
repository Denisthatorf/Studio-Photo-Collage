using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class ColorGenerator
    {
        public static List<SolidColorBrush> GenerateBrushes()
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

        public static Brush GetSolidColorBrushFromString(string color)
        {
            var c = GetColorFromString(color);
            var brush = new SolidColorBrush(c);

            return brush;
        }
        public static Color GetColorFromString(string color)
        {
            Color result;
            color = color.Replace("#", string.Empty);
            color = color.ToLower();

            if (color.Length == 6)
            {
                result = Color.FromArgb(255,
                    Convert.ToByte(color.Substring(0, 2), 16),
                    Convert.ToByte(color.Substring(2, 2), 16),
                    Convert.ToByte(color.Substring(4, 2), 16));
            }
            else if (color.Length == 8)
            {
                result = Color.FromArgb(
                    Convert.ToByte(color.Substring(0, 2), 16),
                    Convert.ToByte(color.Substring(2, 2), 16),
                    Convert.ToByte(color.Substring(4, 2), 16),
                    Convert.ToByte(color.Substring(6, 2), 16));
            }
            else
            {
                result = default;
            }

            return result;
        }
        public static ImageBrush GetImageBrushFromString64(string str64)
        {
            var brush = new ImageBrush();
            ImageHelper.SetImgSourceFromBase64Async((ImageBrush)brush, str64);
            return brush;
        }
        public static List<SolidColorBrush> FillSettingByBrush()
        {
            var Colors = new List<SolidColorBrush>
            {
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("FFBA00"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("F76304"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("DB3800"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("D23135"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("E9091E"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("C40051"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("E4008D"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("C336B5"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("891099"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("754CAB"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("8F8DD9"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("6B69D7"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("0063B3"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("0079D8"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("009ABD"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("00B8C4"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("00B395"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("008675"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("078A3C"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("505C6B"),
                (SolidColorBrush)ColorGenerator.GetSolidColorBrushFromString("7F745F")
            };

            return Colors;
        }
    }
}
