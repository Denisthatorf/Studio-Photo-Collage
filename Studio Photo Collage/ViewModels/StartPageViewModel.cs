using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Infrastructure.Services;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ObservableRecipient
    {
        private readonly INavigationService navigationService;

        private ObservableCollection<Tuple<Project>> recentProjects;

        private bool isGreetingTextVisible;
        private bool isRecentCollagesOpen;

        private ICommand imageClickCommand;
        private ICommand recentCollCloseCommand;
        private ICommand settingsCommand;
        private ICommand templateClickCommand;
        private ICommand recentCollageDeleteCommand;

        public ICommand RecentCollageDeleteOneProjectCommand
        {
            get
            {
                if (recentCollageDeleteCommand == null)
                {
                    recentCollageDeleteCommand = new RelayCommand<Project>((parameter) =>
                    {
                        var removedProject = RecentProjects.Where(x => x.Item1 == parameter).FirstOrDefault();
                        ProjectHelper.DeleteProject(removedProject?.Item1);
                        RecentProjects.Remove(removedProject);
                    });
                }

                return recentCollageDeleteCommand;
            }
        }
        public ICommand ImageClickCommand
        {
            get
            {
                if (imageClickCommand == null)
                {
                    imageClickCommand = new RelayCommand(() => navigationService.Navigate(typeof(TemplatePage)));
                }

                return imageClickCommand;
            }
        }
        public ICommand RecentCollCloseCommand
        {
            get
            {
                if (recentCollCloseCommand == null)
                {
                    recentCollCloseCommand = new RelayCommand<object>(async (parametr) =>
                    {
                        var dialog = new ConfirmDialog();
                        var result = await dialog.ShowAsync();
                        if (result.ToString() == "Primary") //yes
                        {
                            RecentProjects.Clear();
                            IsRecentCollagesOpen = false;
                            ProjectHelper.DeleteAllProjectsFromJson();
                        }
                    });
                }

                return recentCollCloseCommand;
            }
        }
        public ICommand SettingsCommand
        {
            get
            {
                if (settingsCommand == null)
                {
                    settingsCommand = new RelayCommand(async () =>
                    {
                        var dialog = new SettingsDialog();
                        await dialog.ShowAsync();
                    });
                }

                return settingsCommand;
            }
        }
        public ICommand TemplateClickCommand
        {
            get
            {
                if (templateClickCommand == null)
                {
                    templateClickCommand = new RelayCommand<Project>((parameter) =>
                    {
                        navigationService.Navigate(typeof(MainPage));
                        Messenger.Send(parameter);
                    });
                }

                return templateClickCommand;
            }
        }

        public ObservableCollection<Tuple<Project>> RecentProjects
        {
            get => recentProjects;
            set
            {
                SetProperty(ref recentProjects, value);
                if(RecentProjects.Count == 0)
                {
                    IsRecentCollagesOpen = false;
                }
            } 
        }

        public bool IsRecentCollagesOpen
        {
            get => isRecentCollagesOpen;
            set
            {
                if (!(RecentProjects.Count == 0 && value == true))
                {
                    SetProperty(ref isRecentCollagesOpen, value);
                    IsGreetingTextVisible = !value;
                }
            }
        }
        public bool IsGreetingTextVisible
        {
            get => isGreetingTextVisible;
            set => SetProperty(ref isGreetingTextVisible, value);
        }

        public StartPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            RecentProjects = new ObservableCollection<Tuple<Project>>();

            InitializeAsync();
            Messenger.Register<ProjectSavedMessage>(this, (r, m) =>
            {
                bool isContains = RecentProjects.Any((x) => x.Item1 == m.Value);
                if (RecentProjects.Count == 0)
                {
                    RecentProjects.Add(new Tuple<Project>(m.Value));
                    IsRecentCollagesOpen = true;
                }
                else if (!isContains)
                {
                    RecentProjects.Add(new Tuple<Project>(m.Value));
                }
                else
                {
                    var index = RecentProjects.IndexOf(RecentProjects.Where((x) => x.Item1 == m.Value).FirstOrDefault());
                    RecentProjects[index] = new Tuple<Project>(m.Value);
                }
            });
            Messenger.Register<DeleteAllProjectMessage>(this,
                (r, m) =>{
                    RecentProjects.Clear();
                    IsRecentCollagesOpen = false;
                });
        }

        private async void InitializeAsync()
        {
            await DesserializeProjectsAsync();
            if(RecentProjects.Count > 0)
            {
                IsRecentCollagesOpen = true;
            }
        }
        private async Task DesserializeProjectsAsync()
        {
            var projects = await ApplicationData.Current.LocalFolder.ReadAsync<List<Project>>("projects");

            if (projects != null)
            {
                foreach (var proj in projects)
                {
                    RecentProjects.Add(new Tuple<Project>(proj));
                }
            }
        }
    }
}
