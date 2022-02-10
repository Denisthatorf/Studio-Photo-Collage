using CommonServiceLocator;
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
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class RecentPageViewModel : ViewModelBase
    {
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        public ObservableCollection<Project> Projects { get => _projects; set => Set(ref _projects, value); }


        private ICommand _projectCommand;
        public ICommand ProjectCommand
        {
            get
            {
                if (_projectCommand == null)
                    _projectCommand = new RelayCommand<Project>(async (parametr) =>
                    {
                        var mainVM = ServiceLocator.Current.GetInstance<MainPageViewModel>();
                        var result = await mainVM.SaveProjectAsync();

                        if (result != ContentDialogResult.None)
                             Messenger.Default.Send(parametr);
                    });
                return _projectCommand;
            }
        }

        private ICommand _removeAllCollagesCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_removeAllCollagesCommand == null)
                    _removeAllCollagesCommand = new RelayCommand(async() => {
                        var dialog = new ConfirmDialog();
                        var result = await dialog.ShowAsync();

                        if (result.ToString() == "Primary") //yes
                        {
                            Projects?.Clear();
                            await JsonHelper.WriteToFile("projects.json", string.Empty);
                        }
                    });
                return _removeAllCollagesCommand;
            }
        }


        public RecentPageViewModel()
        {
            DesserializeProjects();
        }
        private async void DesserializeProjects()
        {
            var jsonStr = await JsonHelper.DeserializeFileAsync("projects.json");
            Projects = await JsonHelper.ToObjectAsync<ObservableCollection<Project>>(jsonStr);
        }

    }
}
