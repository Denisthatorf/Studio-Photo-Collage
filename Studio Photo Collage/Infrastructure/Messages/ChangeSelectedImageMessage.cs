using System;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Studio_Photo_Collage.Models;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class ChangeSelectedImageMessage : ValueChangedMessage<ImageInfo>
    {
        public ChangeSelectedImageMessage(ImageInfo value) : base(value)
        {
        }
    }
}
