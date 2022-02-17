using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class BrushGenerator
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

        public static Brush GetBrushFromHexOrStrImgBase64(string colour)
        {
            if (string.IsNullOrEmpty(colour))
            {
                return null;
            }

            colour = colour.Replace("#", string.Empty);
            Brush brush;
            if (colour.Length == 6)
            {
                colour = colour.ToLower();
                var c = Color.FromArgb(255,
                        Convert.ToByte(colour.Substring(0, 2), 16),
                        Convert.ToByte(colour.Substring(2, 2), 16),
                        Convert.ToByte(colour.Substring(4, 2), 16));
                brush = new SolidColorBrush(c);
            }

            else if (colour.Length == 8)
            {
                colour = colour.ToLower();
                byte a = (byte)(Convert.ToUInt32(colour.Substring(0, 2), 16));
                byte r = (byte)(Convert.ToUInt32(colour.Substring(2, 2), 16));
                byte g = (byte)(Convert.ToUInt32(colour.Substring(4, 2), 16));
                byte b = (byte)(Convert.ToUInt32(colour.Substring(6, 2), 16));
                brush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            }
            else
            {
                brush = new ImageBrush();
                ImageHelper.SetImgSourceFromBase64Async((ImageBrush)brush, colour);
            }

            return brush;
        }

        public static List<Brush> FillSettingByBrush()
        {
            var Colors = new List<Brush>
            {
                BrushGenerator.GetBrushFromHexOrStrImgBase64("FFBA00"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("F76304"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("DB3800"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("D23135"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("E9091E"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("C40051"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("E4008D"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("C336B5"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("891099"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("754CAB"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("8F8DD9"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("6B69D7"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("0063B3"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("0079D8"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("009ABD"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("00B8C4"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("00B395"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("008675"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("078A3C"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("505C6B"),
                BrushGenerator.GetBrushFromHexOrStrImgBase64("7F745F")
            };

            return Colors;
        }
    }
}
