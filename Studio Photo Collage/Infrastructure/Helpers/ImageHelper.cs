using Studio_Photo_Collage.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class ImageHelper
    {

        public static async Task<string> SaveCollageUIAsImage(Collage collage)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(collage.CollageGrid);

            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();

            var path = $"{collage.Project.ProjectName}.{collage.Project.SaveFormat}";
            var f = await ApplicationData.Current.LocalFolder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
            using (var stream = await f.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder;
                var format = collage.Project.SaveFormat;
                switch (format)
                {
                    case "png":
                        encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                        break;
                    default:
                        throw new NotImplementedException();
                        break;

                }
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, displayInformation.RawDpiX, displayInformation.RawDpiY, pixels);
                await encoder.FlushAsync();

            }
            return $"ms-appdata:///local/{path}";
        }

        public static async Task<StorageFile> OpenFilePicker()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            return await picker.PickSingleFileAsync();
        }


        private static Random rnd = new Random();
        public static async Task<String> SaveToStringBase64Async(ImageSource imageSource)
        {
            byte[] imageBuffer;
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync($"temp{rnd.Next()}.jpg", CreationCollisionOption.ReplaceExisting);
            using (var ras = await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                WriteableBitmap bitmap = imageSource as WriteableBitmap;
                var stream = bitmap.PixelBuffer.AsStream();
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ras);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, buffer);
                await encoder.FlushAsync();

                var imageStream = ras.AsStream();
                imageStream.Seek(0, SeekOrigin.Begin);
                imageBuffer = new byte[imageStream.Length];
                var re = await imageStream.ReadAsync(imageBuffer, 0, imageBuffer.Length);
            }
            await file.DeleteAsync(StorageDeleteOption.Default);
            return Convert.ToBase64String(imageBuffer);
        }

        public static async Task<byte[]> ImageToBytes(IRandomAccessStream sourceStream)
        {
            byte[] imageArray;
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(sourceStream);

            var transform = new BitmapTransform { ScaledWidth = decoder.PixelWidth, ScaledHeight = decoder.PixelHeight };
            PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Rgba8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);

            using (var destinationStream = new InMemoryRandomAccessStream())
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, destinationStream);
                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, decoder.PixelWidth,
                    decoder.PixelHeight, 96, 96, pixelData.DetachPixelData());
                await encoder.FlushAsync();

                BitmapDecoder outputDecoder = await BitmapDecoder.CreateAsync(destinationStream);
                await destinationStream.FlushAsync();
                imageArray = (await outputDecoder.GetPixelDataAsync()).DetachPixelData();
            }
            return imageArray;
        }

        public static async Task<ImageSource> FromBase64(string base64)
        {
            // read stream
            var bytes = Convert.FromBase64String(base64);
            var image = bytes.AsBuffer().AsStream().AsRandomAccessStream();

            // decode image
            var decoder = await BitmapDecoder.CreateAsync(image);
            image.Seek(0);

            // create bitmap
            var output = new WriteableBitmap((int)decoder.PixelHeight, (int)decoder.PixelWidth);
            await output.SetSourceAsync(image);
            return output;
        }

    }
}
