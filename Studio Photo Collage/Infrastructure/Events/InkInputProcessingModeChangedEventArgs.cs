using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace Studio_Photo_Collage.Infrastructure.Events
{
    public class InkInputProcessingModeChangedEventArgs : EventArgs
    {
        public InkInputProcessingModeChangedEventArgs(InkInputProcessingMode mode)
        {
            Mode = mode;
        }

        public InkInputProcessingMode Mode { get; }
    }
}
