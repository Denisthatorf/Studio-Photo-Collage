using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Studio_Photo_Collage.Models;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.Infrastructure.Messages
{
    public class DeleteProjectMessage : ValueChangedMessage<Project>
    {
        public DeleteProjectMessage(Project value) : base(value)
        {
        }
    }
}
