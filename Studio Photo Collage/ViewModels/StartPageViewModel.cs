using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        #region Commands
        private ICommand _imageClickCommand;
        public ICommand ImageClickCommand
        {
            get
            {
                if (_imageClickCommand == null)
                    _imageClickCommand = new RelayCommand(() => _navigationService.NavigateTo("TemplatesPage"));
                return _imageClickCommand;
            }
        }

        private ICommand recentCollCloseCommand;
        public ICommand RecentCollCloseCommand
        {
            get
            {
                if (recentCollCloseCommand == null)
                    recentCollCloseCommand = new RelayCommand<object>(async (parametr) =>
                    {
                        var dialog = new ConfirmDialog();
                        var result = await dialog.ShowAsync();
                        if (result.ToString() == "Primary") //yes
                        {
                            RecentProjects.Clear();
                            JsonHelper.WriteToFile("projects.json", string.Empty);
                        }
                    });
                return recentCollCloseCommand;
            }
        }

        private ICommand settingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                if (settingsCommand == null)
                    settingsCommand = new RelayCommand(async () =>
                    {
                        var dialog = new SettingsDialog();
                        await dialog.ShowAsync();
                    });
                return settingsCommand;
            }
        }

        private ICommand templateClickCommand;
        public ICommand TemplateClickCommand
        {
            get
            {
                if (templateClickCommand == null)
                    templateClickCommand = new RelayCommand<Project>((parameter) =>
                    {
                        _navigationService.NavigateTo("MainPage");
                        Messenger.Default.Send(parameter);
                    });
                return templateClickCommand;
            }
        }
        #endregion

        private bool _isRecentCollagesOpen;
        public bool IsRecentCollagesOpen { get => _isRecentCollagesOpen; set => Set(ref _isRecentCollagesOpen, value); }


        private Visibility _isGreetingTextVisible;
        public Visibility IsGreetingTextVisible { get => _isGreetingTextVisible; set => Set(ref _isGreetingTextVisible, value); }


        private ObservableCollection<Tuple<Project>> _recentProjects;
        public ObservableCollection<Tuple<Project>> RecentProjects { get => _recentProjects; set => Set(ref _recentProjects, value); }

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            RecentProjects = new ObservableCollection<Tuple<Project>>();
            DesserializeProjects();
            Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown += (sender, arg) =>
            {
                if (arg.VirtualKey == Windows.System.VirtualKey.Space ||
                  arg.VirtualKey == Windows.System.VirtualKey.Enter)
                {
                    IsRecentCollagesOpen = true;
                    IsGreetingTextVisible = Visibility.Collapsed;
                }
            };
        }

        private async void DesserializeProjects()
        {
            var jsonStr = await JsonHelper.DeserializeFileAsync("projects.json");

            ObservableCollection<Project> projects = new ObservableCollection<Project>();
            if (!String.IsNullOrEmpty(jsonStr))
                projects = await JsonHelper.ToObjectAsync<ObservableCollection<Project>>(jsonStr);

            foreach (var proj in projects)
            {
                RecentProjects.Add(new Tuple<Project>(proj));
            }
        }
    }
}
