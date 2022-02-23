using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Infrastructure.Services;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views;

namespace Studio_Photo_Collage.ViewModels
{
    public class TemplatePageViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        private ICommand templateClickCommand;
        private ICommand templateOnMainPageClickCommand;
        private ICommand goBackCommand;
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
        public ICommand TemplateOnMainPageClickCommand 
        {
            get
            {
                if (templateOnMainPageClickCommand == null)
                {
                    templateOnMainPageClickCommand = new RelayCommand<Project>(async (parameter) =>
                    {
                        var result = await WeakReferenceMessenger.Default.Send(new SaveProjectRequestMessage());
                        if (result != Windows.UI.Xaml.Controls.ContentDialogResult.None)
                        {
                            WeakReferenceMessenger.Default.Send(parameter);
                        }
                    });
                }

                return templateOnMainPageClickCommand;
            }
        }
        public ICommand GoBackCommand 
        {
            get 
            {
                if(goBackCommand == null)
                {
                    goBackCommand = new RelayCommand(() => navigationService.Navigate(typeof(StartPage)));
                }
                return goBackCommand;
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
