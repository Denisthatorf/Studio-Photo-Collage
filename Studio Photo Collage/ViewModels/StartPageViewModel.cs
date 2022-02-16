
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.PopUps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ObservableObject
    {
        private ObservableCollection<Tuple<Project>> recentProjects;

        private bool isGreetingTextVisible;
        private bool isRecentCollagesOpen;

        private ICommand imageClickCommand;
        private ICommand recentCollCloseCommand;
        private ICommand settingsCommand;
        private ICommand templateClickCommand;

        public ICommand ImageClickCommand
        {
            get
            {
                if (imageClickCommand == null)
                {
                    imageClickCommand = new RelayCommand(() => 
                    {
                        var rootFrame = Window.Current.Content as Frame;
                        rootFrame.Navigate(typeof(TemplatePage));
                        WeakReferenceMessenger.Default.Send(new Project(new byte[,] { {1} }));
                    });
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
                            _ = JsonHelper.WriteToFile("projects.json", string.Empty);
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
                        //_navigationService.NavigateTo("MainPage");
                        // Messenger.Default.Send(parameter);
                    });
                }

                return templateClickCommand;
            }
        }

        public ObservableCollection<Tuple<Project>> RecentProjects
        {
            get => recentProjects;
            set => SetProperty(ref recentProjects, value);
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

        public StartPageViewModel()
        {
            RecentProjects = new ObservableCollection<Tuple<Project>>();
            isGreetingTextVisible = true;

            DesserializeProjects();

            Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown += (sender, arg) =>
            {
                if (arg.VirtualKey == Windows.System.VirtualKey.Space || arg.VirtualKey == Windows.System.VirtualKey.Enter)
                {
                    IsRecentCollagesOpen = true;
                }
            };
        }

        private async void DesserializeProjects()
        {
            var jsonStr = await JsonHelper.DeserializeFileAsync("projects.json");

            var projects = new ObservableCollection<Project>();
            if (!string.IsNullOrEmpty(jsonStr))
            {
                projects = await JsonHelper.ToObjectAsync<ObservableCollection<Project>>(jsonStr);
            }

            foreach (var proj in projects)
            {
                RecentProjects.Add(new Tuple<Project>(proj));
            }
        }
    }
}
