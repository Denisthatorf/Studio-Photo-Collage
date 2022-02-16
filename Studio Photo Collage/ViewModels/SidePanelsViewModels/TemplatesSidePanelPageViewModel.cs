using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Models;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class TemplatesSidePanelPageViewModel : ObservableObject
    {
        public ObservableCollection<GroupedTemplates> TemplateCollection { get; set; }
        public ICommand TemplateClickCommand { get; private set; }

        public TemplatesSidePanelPageViewModel()
        {
            TemplateCollection = new ObservableCollection<GroupedTemplates>();
            TemplateCollection = GroupedTemplates.FillByGroupedTemplate();

            TemplateClickCommand = new RelayCommand<Project>(
                (parameter) => WeakReferenceMessenger.Default.Send(parameter));
        }
    }
}
