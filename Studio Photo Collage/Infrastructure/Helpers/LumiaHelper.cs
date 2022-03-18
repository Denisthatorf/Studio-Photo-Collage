using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumia.Imaging;
using Lumia.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class LumiaHelper
    {
        public static async Task<WriteableBitmap> SetEffectToWritableBitmap(WriteableBitmap writeableBitmap, IImageConsumer effect)
        {
            var bitmap = new WriteableBitmap(writeableBitmap.PixelWidth, writeableBitmap.PixelHeight);
            var source = new BitmapImageSource(writeableBitmap.AsBitmap());
            effect.Source = source;

            using (var renderer = new WriteableBitmapRenderer(effect as IImageProvider, bitmap))
            {
                var result = await renderer.RenderAsync();
                return result;
            }
        }
    }
}
