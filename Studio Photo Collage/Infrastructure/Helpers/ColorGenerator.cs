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

        public static Brush GetBrushFromHexOrStrImgBase64(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                return null;
            }

            Brush brush;
            if (color.Length < 10)
            {
                var c = GetColorFromString(color);
                brush = new SolidColorBrush(c);
            }
            else
            {
                brush = new ImageBrush();
                ImageHelper.SetImgSourceFromBase64Async((ImageBrush)brush, color);
            }

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
        public static List<SolidColorBrush> FillSettingByBrush()
        {
            var Colors = new List<SolidColorBrush>
            {
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("FFBA00"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("F76304"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("DB3800"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("D23135"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("E9091E"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("C40051"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("E4008D"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("C336B5"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("891099"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("754CAB"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("8F8DD9"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("6B69D7"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("0063B3"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("0079D8"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("009ABD"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("00B8C4"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("00B395"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("008675"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("078A3C"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("505C6B"),
                (SolidColorBrush)ColorGenerator.GetBrushFromHexOrStrImgBase64("7F745F")
            };

            return Colors;
        }
    }
}
