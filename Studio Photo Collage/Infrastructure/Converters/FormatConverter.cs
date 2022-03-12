using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class FormatConverter
    {
        public static Guid Convert(string format)
        {
            switch (format)
            {
                case "png":
                    return BitmapEncoder.PngEncoderId;
                case "jpeg":
                    return BitmapEncoder.JpegEncoderId;
                case "jpg":
                    return BitmapEncoder.JpegEncoderId;
                case "bmp":
                    return BitmapEncoder.BmpEncoderId;
                case "tiff":
                    return BitmapEncoder.TiffEncoderId;
                case "gif":
                    return BitmapEncoder.GifEncoderId;
                 default:
                    throw new ArgumentException();
            }
        }
    }
}
