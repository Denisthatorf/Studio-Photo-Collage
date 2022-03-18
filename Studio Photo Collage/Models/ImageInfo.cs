using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Studio_Photo_Collage.Models
{
    [JsonObject(MemberSerialization.Fields)]
    public class ImageInfo
    {
        private string imageBase64Clear;
        private string imageBase64;

        public ImageInfo()
        {
            ZoomInfo = new Zoom();
            EffectsTypes = new List<Type>();
        }

        [JsonIgnore]
        public string ImageBase64 
        {
            get => imageBase64; 
            set => imageBase64 = value; 
        }
        [JsonIgnore]
        public string ImageBase64Clear
        {
            get => imageBase64Clear;
            set
            {
                imageBase64Clear = value;
                ImageBase64 = value;
            }
        }
        public Zoom ZoomInfo { get; set; }
        public List<Type> EffectsTypes { get; set; }
    }
}
