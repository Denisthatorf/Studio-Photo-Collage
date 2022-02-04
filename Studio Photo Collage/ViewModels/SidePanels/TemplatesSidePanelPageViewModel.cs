using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class TemplatesSidePanelPageViewModel : ViewModelBase
    {
        public ICommand TemplateClickCommand { get; private set; }
        public ObservableCollection<GroupedTemplates> TemplateCollection { get; set; } = new ObservableCollection<GroupedTemplates>();


        public TemplatesSidePanelPageViewModel()
        {
            TemplateClickCommand = new RelayCommand<Project>((parameter) => {
                Messenger.Default.Send(parameter);
            });

            TemplateCollection = GroupedTemplates.FillByGroupedTemplate();
        }
        
    }
}
