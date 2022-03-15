using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Windows.UI;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class PaintColorChangedMessage : ValueChangedMessage<Color>
    {
        public PaintColorChangedMessage(Color value) : base(value)
        {
        }
    }
}
