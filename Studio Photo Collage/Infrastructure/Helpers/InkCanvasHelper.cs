using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class InkCanvasHelper
    {
        public static async Task SaveStrokesAsync(InkPresenter inkPresenter, string uid)
        {
            var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync($"{uid}.gif", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            Windows.Storage.CachedFileManager.DeferUpdates(file);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            using (var outputStream = stream.GetOutputStreamAt(0))
            {
                await inkPresenter.StrokeContainer.SaveAsync(outputStream);
                await outputStream.FlushAsync();
            }
            stream.Dispose();

            var status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

            if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            {
                // File saved
            }
            else
            {
                // File NOT saved
            }
        }
        public static async Task RestoreStrokesAsync(InkPresenter inkPresenter, string uid)
        {
            var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var isExist = File.Exists(Path.Combine(storageFolder.Path, $"{uid}.gif"));
            if (isExist)
            {
                var file = await storageFolder.GetFileAsync($"{uid}.gif");

                if (file != null)
                {
                    // Open a file stream for reading.
                    var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    // Read from file.
                    using (var inputStream = stream.GetInputStreamAt(0))
                    {
                        await inkPresenter.StrokeContainer.LoadAsync(inputStream);
                    }
                    stream.Dispose();
                }
                // User selects Cancel and picker returns null.
                else
                {
                    // Operation cancelled.
                }
            }
        }

        public static async Task DeleteStrokeFileByUid(string uid)
        {
            var storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var isExist = File.Exists(Path.Combine(storageFolder.Path, $"{uid}.gif"));
            if (isExist)
            {
                var file = await storageFolder.GetFileAsync($"{uid}.gif");
                await file.DeleteAsync();
            }
        }
    }
}
