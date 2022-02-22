
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class ImageHelper
    {
        private static readonly Random rnd = new Random();
        public static async Task<StorageFile> OpenFilePicker()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            return await picker.PickSingleFileAsync();
        }

        public static async Task<string> SaveToStringBase64Async(ImageSource imageSource)
        {
            byte[] imageBuffer;
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync($"temp{rnd.Next()}.jpg", CreationCollisionOption.ReplaceExisting);
            using (var ras = await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                var bitmap = imageSource as WriteableBitmap;
                var stream = bitmap.PixelBuffer.AsStream();
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ras);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, buffer);
                await encoder.FlushAsync();

                var imageStream = ras.AsStream();
                imageStream.Seek(0, SeekOrigin.Begin);
                imageBuffer = new byte[imageStream.Length];
                var re = await imageStream.ReadAsync(imageBuffer, 0, imageBuffer.Length);
            }

            await file.DeleteAsync(StorageDeleteOption.Default);
            string output = Convert.ToBase64String(imageBuffer);
            return output;
        }

        public static async Task<byte[]> ImageToBytes(IRandomAccessStream sourceStream)
        {
            byte[] imageArray;
            var decoder = await BitmapDecoder.CreateAsync(sourceStream);

            var transform = new BitmapTransform { ScaledWidth = decoder.PixelWidth, ScaledHeight = decoder.PixelHeight };
            var pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Rgba8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);

            using (var destinationStream = new InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, destinationStream);
                encoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied, decoder.PixelWidth,
                    decoder.PixelHeight, 96, 96, pixelData.DetachPixelData());
                await encoder.FlushAsync();

                var outputDecoder = await BitmapDecoder.CreateAsync(destinationStream);
                await destinationStream.FlushAsync();
                imageArray = (await outputDecoder.GetPixelDataAsync()).DetachPixelData();
            }

            return imageArray;
        }

        public static async Task<ImageSource> FromBase64(string base64)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            var image = bytes.AsBuffer().AsStream().AsRandomAccessStream();

            var decoder = await BitmapDecoder.CreateAsync(image);
            image.Seek(0);

            var output = new WriteableBitmap((int)decoder.PixelHeight, (int)decoder.PixelWidth);
            await output.SetSourceAsync(image);
            return output;
        }

        public static async void SetImgSourceFromBase64Async(Image img, string base64)
        {
            if (!string.IsNullOrEmpty(base64))
            {
                var source = await ImageHelper.FromBase64(base64);
                img.Source = source;
            }
        }

        public static async void SetImgSourceFromBase64Async(ImageBrush img, string base64)
        {
            if (!string.IsNullOrEmpty(base64))
            {
                var source = await ImageHelper.FromBase64(base64);
                img.ImageSource = source;
            }
        }
    }
}

