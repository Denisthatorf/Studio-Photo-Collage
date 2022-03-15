using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Photo_Collage.Models
{
    public class Zoom
    {
        public Zoom()
        {
            ZoomFactor = 0.1f;
        }

        public float ZoomFactor { get; set; }
        public double HorizontalOffset { get; set; }
        public double VerticalOffset { get; set; }
    }
}
