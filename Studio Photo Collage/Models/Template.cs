using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studio_Photo_Collage.Models
{
    public class Template
    {
        public double Rotation { get; }
        public int CountOfPhoto { get; }
        public bool IsSecondRow2Star { get; }


        public Template(int _countOfPhoto, bool _isSecondRow2Star, double _Rotation)
        {
            Rotation = _Rotation;
            CountOfPhoto = _countOfPhoto;
            IsSecondRow2Star = _isSecondRow2Star;
        }
    }
}
