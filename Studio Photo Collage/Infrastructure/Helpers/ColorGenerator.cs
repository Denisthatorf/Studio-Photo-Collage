using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.Helpers;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

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
                new SolidColorBrush(ColorHelper.ToColor("#FFBA00")),
                new SolidColorBrush(ColorHelper.ToColor("#F76304")),
                new SolidColorBrush(ColorHelper.ToColor("#DB3800")),
                new SolidColorBrush(ColorHelper.ToColor("#D23135")),
                new SolidColorBrush(ColorHelper.ToColor("#E9091E")),
                new SolidColorBrush(ColorHelper.ToColor("#C40051")),
                new SolidColorBrush(ColorHelper.ToColor("#E4008D")),
                new SolidColorBrush(ColorHelper.ToColor("#C336B5")),
                new SolidColorBrush(ColorHelper.ToColor("#891099")),
                new SolidColorBrush(ColorHelper.ToColor("#754CAB")),
                new SolidColorBrush(ColorHelper.ToColor("#8F8DD9")),
                new SolidColorBrush(ColorHelper.ToColor("#6B69D7")),
                new SolidColorBrush(ColorHelper.ToColor("#0063B3")),
                new SolidColorBrush(ColorHelper.ToColor("#0079D8")),
                new SolidColorBrush(ColorHelper.ToColor("#009ABD")),
                new SolidColorBrush(ColorHelper.ToColor("#00B8C4")),
                new SolidColorBrush(ColorHelper.ToColor("#00B395")),
                new SolidColorBrush(ColorHelper.ToColor("#008675")),
                new SolidColorBrush(ColorHelper.ToColor("#078A3C")),
                new SolidColorBrush(ColorHelper.ToColor("#505C6B")),
                new SolidColorBrush(ColorHelper.ToColor("#7F745F"))
            };

            return Colors;
        }
    }
}
