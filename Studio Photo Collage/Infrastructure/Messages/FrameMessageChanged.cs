using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class FrameMessageChanged : ValueChangedMessage<string>
    {
        public FrameMessageChanged(string value) : base(value)
        {
        }
    }
}
