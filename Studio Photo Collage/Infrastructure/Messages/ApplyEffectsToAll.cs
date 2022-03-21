using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class ApplyEffectsToAllMessage : ValueChangedMessage<List<Type>>
    {
        public ApplyEffectsToAllMessage(List<Type> value) : base(value)
        {
        }
    }
}
