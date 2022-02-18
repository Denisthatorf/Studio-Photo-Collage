using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace Studio_Photo_Collage.Infrastructure.Helpers
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
    }
}
