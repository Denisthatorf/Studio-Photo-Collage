using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class BackgroundOpacityChangedMessage : ValueChangedMessage<double>
    {
        public BackgroundOpacityChangedMessage(double value) : base(value)
        {
        }
    }
}
