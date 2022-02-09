using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Studio_Photo_Collage.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class TemplatesSidePanelPageViewModel : ViewModelBase
    {
        public ICommand TemplateClickCommand { get; private set; }
        public ObservableCollection<GroupedTemplates> TemplateCollection { get; set; } = new ObservableCollection<GroupedTemplates>();


        public TemplatesSidePanelPageViewModel()
        {
            TemplateClickCommand = new RelayCommand<Project>((parameter) =>
            {
                Messenger.Default.Send(parameter);
            });

            TemplateCollection = GroupedTemplates.FillByGroupedTemplate();
        }

    }
}
