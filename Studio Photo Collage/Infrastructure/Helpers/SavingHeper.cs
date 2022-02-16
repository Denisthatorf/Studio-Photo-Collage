using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Studio_Photo_Collage.Models;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public class SavingHeper
    {
        public static async Task<string> SaveCollageUIAsImage(Collage collage, string saveFormat)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(collage.CollageGrid);

            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();

            var path = $"{collage.Project.ProjectName}.{saveFormat}";
            var f = await ApplicationData.Current.LocalFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
            using (var stream = await f.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder;
                var format = saveFormat;
                switch (format)
                {
                    case "png":
                        encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, displayInformation.RawDpiX, displayInformation.RawDpiY, pixels);
                await encoder.FlushAsync();

            }

            return $"ms-appdata:///local/{path}";
        }
    }
}
