using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
