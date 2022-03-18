using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class ChangeSelectImageMessage : ValueChangedMessage<Image>
    {
        public ChangeSelectImageMessage(Image value) : base(value)
        {
        }
    }
}
