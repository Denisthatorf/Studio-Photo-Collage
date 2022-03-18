using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class ApplyEffectsMessage : ValueChangedMessage<List<Type>>
    {
        public ApplyEffectsMessage(List<Type> value) : base(value)
        {
        }
    }
}
