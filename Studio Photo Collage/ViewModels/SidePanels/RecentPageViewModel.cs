using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                    _projectCommand = new RelayCommand<Project>((parametr) => { });
                return _projectCommand;
            }
        }

        private ICommand _removeAllCollagesCommand;
        public ICommand RemoveCommand
        {
            get
            {
                if (_removeAllCollagesCommand == null)
                    _removeAllCollagesCommand = new RelayCommand(() => { RemoveProjects(); });
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
        private async void RemoveProjects()
        {
            var dialog = new ConfirmDialog();
            var result = await dialog.ShowAsync();

            if (result.ToString() == "Primary") //yes
            {
                Projects?.Clear();
                await JsonHelper.WriteToFile("projects.json", string.Empty);
            }
        }
    
    }
}
