using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class FrameColorChangedMessege : ValueChangedMessage<SolidColorBrush>
    {
        public FrameColorChangedMessege(SolidColorBrush value) : base(value)
        {
        }
    }
}
