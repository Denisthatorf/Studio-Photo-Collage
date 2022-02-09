using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Studio_Photo_Collage.ViewModels
{
    public class TemplatesPageViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        public ICommand TemplateClickCommand { get; private set; }
        public ObservableCollection<GroupedTemplates> TemplateCollection { get; set; } = new ObservableCollection<GroupedTemplates>();


        public TemplatesPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            TemplateClickCommand = new RelayCommand<Project>((parameter) => {
                TemplateClickMethod(parameter);
            });
            TemplateCollection = GroupedTemplates.FillByGroupedTemplate();
        }

        private async void TemplateClickMethod(Project parameter)
        {
            var currentPage = ViewModelLocator.GetStringCurrentPage();
            if (currentPage == "MainPage")
            {
                var dialog = new SaveDialog("collage");
                var result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary) //yes
                    Messenger.Default.Send(parameter);
                else if(result == ContentDialogResult.Secondary)//no
                    Messenger.Default.Send(parameter);
            }
            else
            {
                NavigationService.NavigateTo("MainPage");
                Messenger.Default.Send(parameter);
            }
        }
    }
}
