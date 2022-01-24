using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        
        public RelayCommand ImageClickCommand { get; private set; }
        public RelayCommand RecentCollCloseCommand { get; private set; }
        public ICommand TemplateClickCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }

        private bool _isRecentCollagesOpen;
        public bool IsRecentCollagesOpen { get => _isRecentCollagesOpen; set => Set(ref _isRecentCollagesOpen, value); }
        
        private Visibility _isGreetingTextVisible;
        public Visibility IsGreetingTextVisible { get => _isGreetingTextVisible; set => Set(ref _isGreetingTextVisible, value); }

        public ObservableCollection<Project> RecentProjects { get; set; } 

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            SettingsCommand = new RelayCommand(async()=>
            {
                var dialog = new SettingsDialog();
                //dialog.RequestedTheme = ElementTheme.Dark;
                await dialog.ShowAsync();
            });
            ImageClickCommand = new RelayCommand(() => _navigationService.NavigateTo("TemplatesPage"));
            RecentCollCloseCommand = new RelayCommand(async()=>
            {
                var dialog = new ConfirmDialog();
                //dialog.RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
                var result = await dialog.ShowAsync();
                if (result.ToString() == "Primary") //yes
                {
                    RecentProjects.Clear();
                    File.WriteAllText("projects.json", string.Empty);
                }

            });
            TemplateClickCommand = new RelayCommand<Project>((parameter)=> 
                {
                    _navigationService.NavigateTo("MainPage");
                    Messenger.Default.Send(parameter);
                });

            _ = DesserializeProjects();
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

        private async Task DesserializeProjects()
        {
            // var jsonDesStr = await Json.StringifyAsync(Projects);
            // await Json.AddTextToFile("projects.json", jsonDesStr);
            //var coll = new ObservableCollection<Project>();
            //coll.Add();
            //var jsonDesStr = await Json.StringifyAsync(RecentProjects);
            //await Json.WriteToFile("projects.json", jsonDesStr);
            var jsonStr = await Json.DeserializeFileAsync("projects.json");
            RecentProjects = await Json.ToObjectAsync<ObservableCollection<Project>>(jsonStr);
        }
    }
}
