using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class FiltersHelper
    {
        public static async void ToGrayScale(Image image)
        {
            var srcBitmap = image.Source as WriteableBitmap;
            byte[] srcPixels = new byte[4 * srcBitmap.PixelWidth * srcBitmap.PixelHeight];

            using (var pixelStream = srcBitmap.PixelBuffer.AsStream())
            {
                await pixelStream.ReadAsync(srcPixels, 0, srcPixels.Length);
            }
            // Create a destination bitmap and pixels array
            var dstBitmap =
                    new WriteableBitmap(srcBitmap.PixelWidth, srcBitmap.PixelHeight);
            byte[] dstPixels = new byte[4 * dstBitmap.PixelWidth * dstBitmap.PixelHeight];

            for (int i = 0; i < srcPixels.Length; i += 4)
            {
                double b = (double)srcPixels[i] / 255.0;
                double g = (double)srcPixels[i + 1] / 255.0;
                double r = (double)srcPixels[i + 2] / 255.0;

                byte a = srcPixels[i + 3];

                double e = (0.21 * r + 0.71 * g + 0.07 * b) * 255;
                byte f = Convert.ToByte(e);

                dstPixels[i] = f;
                dstPixels[i + 1] = f;
                dstPixels[i + 2] = f;
                dstPixels[i + 3] = a;

            }

            // Move the pixels into the destination bitmap
            using (var pixelStream = dstBitmap.PixelBuffer.AsStream())
            {
                await pixelStream.WriteAsync(dstPixels, 0, dstPixels.Length);
            }

            dstBitmap.Invalidate();

            // Display the new bitmap
            image.Source = dstBitmap;
        }    
    }
}
