using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml.Controls;
using Studio_Photo_Collage.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Windows.UI.Xaml;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Infrastructure.Services;

namespace Studio_Photo_Collage.ViewModels
{
    public class TemplatePageViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        private ICommand templateClickCommand;
        public ICommand TemplateClickCommand
        {
            get
            {
                if (templateClickCommand == null)
                {
                    templateClickCommand = new RelayCommand<Project>((parameter) =>
                    {
                        navigationService.Navigate(typeof(MainPage));
                        WeakReferenceMessenger.Default.Send(parameter);
                    });
                }

                return templateClickCommand;
            }
        }

        public ObservableCollection<GroupedTemplates> TemplateCollection { get; set; }

        public TemplatePageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            TemplateCollection = new ObservableCollection<GroupedTemplates>();
            TemplateCollection = GroupedTemplates.FillByGroupedTemplate();
        }
    }
}
