using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class BorderThicknessChangedMessage : ValueChangedMessage<double>
    {
        public BorderThicknessChangedMessage(double value) : base(value)
        {
        }
    }
}
