using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class BrushSizeMessage : ValueChangedMessage<int>
    {
        public BrushSizeMessage(int value) : base(value)
        {
        }
    }
}
