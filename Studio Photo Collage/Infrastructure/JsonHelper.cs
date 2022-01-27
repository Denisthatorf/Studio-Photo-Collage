using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Studio_Photo_Collage.Infrastructure
{
    public static class JsonHelper
    {
        public static async Task<T> ToObjectAsync<T>(string value)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<T>(value);
            }).ConfigureAwait(true);
        }

        public static async Task<string> StringifyAsync(object value)
        {
            return await Task.Run(() =>
            {
                return JsonConvert.SerializeObject(value);
            }).ConfigureAwait(true);
        }

        public static async Task<List<string>> LoadFromJsonAsync(string JsonFile)
        {
            string JsonString = await DeserializeFileAsync(JsonFile).ConfigureAwait(true);
            if (JsonString != null)
                return (List<string>)JsonConvert.DeserializeObject(JsonString, typeof(List<string>));
            return null;
        }

        public static async Task<string> DeserializeFileAsync(string fileName)
        {
            try
            {
                StorageFile localFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(localFile);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static async Task WriteToFile(string fileNameString, string textToSave)
        {
            var appFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            var file = await appFolder.CreateFileAsync(fileNameString,
                Windows.Storage.CreationCollisionOption.OpenIfExists);
           // await Windows.Storage.FileIO.AppendTextAsync(file, textToSave + Environment.NewLine);
            await Windows.Storage.FileIO.WriteTextAsync(file, textToSave);

            // Look in Output Window of Visual Studio for path to file
            System.Diagnostics.Debug.WriteLine(String.Format("File is located at {0}", file.Path.ToString()));
        }

    }
}
