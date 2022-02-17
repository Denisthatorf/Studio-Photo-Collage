using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Messages;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class RecentPageViewModel : ObservableRecipient
    {
        private ObservableCollection<Project> projects; 
        private ICommand projectCommand;
        private ICommand removeAllCollagesCommand;

        public ObservableCollection<Project> Projects
        {
            get => projects; 
            set => SetProperty(ref projects, value); 
        }

        public ICommand ProjectCommand
        {
            get
            {
                if (projectCommand == null)
                {
                    projectCommand = new RelayCommand<Project>(async (parametr) =>
                    {
                        bool permission = await Messenger.Send<SaveProjectRequestMessage>();
                        if (permission)
                        {
                            Messenger.Send(parametr);
                        }
                    });
                }

                return projectCommand;
            }
        }
        public ICommand RemoveCommand
        {
            get
            {
                if (removeAllCollagesCommand == null)
                {
                    removeAllCollagesCommand = new RelayCommand(async() => {
                        if(Projects != null)
                        {
                            var dialog = new ConfirmDialog();
                            var result = await dialog.ShowAsync();

                            if (result.ToString() == "Primary") //yes
                            {
                                Projects.Clear();
                                await JsonHelper.WriteToFile("projects.json", string.Empty);
                            }
                        }
                    });
                }

                return removeAllCollagesCommand;
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
