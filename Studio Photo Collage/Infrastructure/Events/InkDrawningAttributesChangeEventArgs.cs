using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Input.Inking;

namespace Studio_Photo_Collage.Infrastructure.Events
{
    public class InkDrawningAttributesChangeEventArgs : EventArgs
    {
        public InkDrawningAttributesChangeEventArgs(InkDrawingAttributes attributes)
        {
            Attributes = attributes;
        }

        public InkDrawingAttributes Attributes { get; }
    }
}
