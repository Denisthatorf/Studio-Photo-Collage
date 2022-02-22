using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Studio_Photo_Collage.Models;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class ProjectSavedMessage : ValueChangedMessage<Project>
    {
        public ProjectSavedMessage(Project value) : base(value)
        {
        }
    }
}
