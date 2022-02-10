using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels
{
    public class TemplatesPageViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        private ICommand templateClickCommand;
        public ICommand TemplateClickCommand
        {
            get
            {
                if (templateClickCommand == null)
                    templateClickCommand = new RelayCommand<Project>(async (parameter) =>
                    {
                        var currentPage = ViewModelLocator.GetStringCurrentPage();
                        if (currentPage == "MainPage")
                        {
                            var mainVM = ServiceLocator.Current.GetInstance<MainPageViewModel>();
                            var result = await mainVM.SaveProjectAsync();

                            if (result != ContentDialogResult.None)
                                Messenger.Default.Send(parameter);
                        }
                        else
                        {
                            NavigationService.NavigateTo("MainPage");
                            Messenger.Default.Send(parameter);
                        }
                    });
                return templateClickCommand;
            }
        }

        public ObservableCollection<GroupedTemplates> TemplateCollection { get; set; }


        public TemplatesPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            TemplateCollection = new ObservableCollection<GroupedTemplates>();
            TemplateCollection = GroupedTemplates.FillByGroupedTemplate();
        }
    }
}
