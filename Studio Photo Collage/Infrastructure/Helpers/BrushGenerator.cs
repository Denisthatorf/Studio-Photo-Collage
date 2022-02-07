using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public static Brush GetSolidColorBrush(string colour)
        {
            if (string.IsNullOrEmpty(colour))
                return null;

            colour = colour.Replace("#", string.Empty).ToLower();

            if (colour.Length == 6)
            {
                var c = Color.FromArgb(255,
                        Convert.ToByte(colour.Substring(0, 2), 16),
                        Convert.ToByte(colour.Substring(2, 2), 16),
                        Convert.ToByte(colour.Substring(4, 2), 16));
                SolidColorBrush myBrush = new SolidColorBrush(c);
                return myBrush;
            }

            else if (colour.Length == 8)
            {
                byte a = (byte)(Convert.ToUInt32(colour.Substring(0, 2), 16));
                byte r = (byte)(Convert.ToUInt32(colour.Substring(2, 2), 16));
                byte g = (byte)(Convert.ToUInt32(colour.Substring(4, 2), 16));
                byte b = (byte)(Convert.ToUInt32(colour.Substring(6, 2), 16));
                SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
                return myBrush;
            }
            else
               throw new NotImplementedException();
        }
        public static async Task<Brush> GetBrushForCollageAcync( string colourOrImage)
        {
            if (string.IsNullOrEmpty(colourOrImage))
                return null;

            colourOrImage = colourOrImage.Replace("#", string.Empty).ToLower();

            if (colourOrImage.Length == 6)
            {
                var c = Color.FromArgb(255,
                        Convert.ToByte(colourOrImage.Substring(0, 2), 16),
                        Convert.ToByte(colourOrImage.Substring(2, 2), 16),
                        Convert.ToByte(colourOrImage.Substring(4, 2), 16));
                SolidColorBrush myBrush = new SolidColorBrush(c);
                return myBrush;
            }

            else if (colourOrImage.Length == 8)
            {
                byte a = (byte)(Convert.ToUInt32(colourOrImage.Substring(0, 2), 16));
                byte r = (byte)(Convert.ToUInt32(colourOrImage.Substring(2, 2), 16));
                byte g = (byte)(Convert.ToUInt32(colourOrImage.Substring(4, 2), 16));
                byte b = (byte)(Convert.ToUInt32(colourOrImage.Substring(6, 2), 16));
                SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
                return myBrush;
            }

            else
            {
                var ImageBrush = new ImageBrush();
                var source = await ImageHelper.FromBase64(colourOrImage);
                ImageBrush.ImageSource = source;
                return ImageBrush;
            }
        }
    }
}
