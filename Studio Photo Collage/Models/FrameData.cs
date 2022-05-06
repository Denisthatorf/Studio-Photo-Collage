using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Photo_Collage.Models
{
    public class FrameData
    {
        public string PathData { get; set; }
        public int AdditionalSize { set; get; }
        public string Color { get; set; }
        public FrameData()
        {
            PathData = string.Empty;
            Color = "#000000";
            AdditionalSize = 0;
        }
    }
}
