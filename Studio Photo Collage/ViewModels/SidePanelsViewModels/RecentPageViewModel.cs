using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels.SidePanelsViewModels
{
    public class RecentPageViewModel : ObservableRecipient
    {
        private ObservableCollection<Project> projects;
        private ICommand projectCommand;
        private ICommand removeAllCollagesCommand;
        private ICommand recentCollageDeleteCommand;

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
                    projectCommand = new RelayCommand<Project>(async (parameter) =>
                    {
                        var dialogResult = await Messenger.Send(new SaveProjectRequestMessage());
                        if (dialogResult != Windows.UI.Xaml.Controls.ContentDialogResult.None)
                        {
                            Messenger.Send(parameter);
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
                    removeAllCollagesCommand = new RelayCommand(async () =>
                    {
                        if (Projects != null)
                        {
                            var dialog = new ConfirmDialog("all");
                            var result = await dialog.ShowAsync();

                            if (result.ToString() == "Primary") //yes
                            {
                                Projects.Clear();
                                ProjectHelper.DeleteAllProjectsFromJson();

                            }
                        }
                    });
                }

                return removeAllCollagesCommand;
            }
        }
        public ICommand RecentCollageDeleteOneProjectCommand
        {
            get
            {
                if (recentCollageDeleteCommand == null)
                {
                    recentCollageDeleteCommand = new RelayCommand<Project>(async(parameter) =>
                    {
                        var dialog = new ConfirmDialog("this");
                        var result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            var removedProject = Projects.Where(x => x == parameter).FirstOrDefault();
                            ProjectHelper.DeleteProject(removedProject);
                            Projects.Remove(removedProject);
                        }
                    });
                }

                return recentCollageDeleteCommand;
            }
        }

        public RecentPageViewModel()
        {
            DesserializeProjectsAsync();

            Messenger.Register<ProjectSavedMessage>(this, (r, m) =>
            {
                var index = Projects.IndexOf(m.Value);
                if (index == -1)
                {
                    Projects.Add(m.Value);
                }
                else
                {
                    Projects[index] = m.Value;
                }
            });
            Messenger.Register<DeleteProjectMessage>(this, (r, m) =>
            {
                    if (Projects.Count == 0 || Projects.Contains(m.Value))
                    {
                        Projects.Remove(m.Value);
                    }
                    else
                    {
                        var index = Projects.IndexOf(Projects.Where((x) => x == m.Value).FirstOrDefault());
                        if(index != -1)
                        {
                        Projects[index] = m.Value;
                        }
                    }
            });

            Messenger.Register<DeleteAllProjectMessage>(this,
                (r, m) => 
                {
                    Projects.Clear();
                });
        }

        private async void DesserializeProjectsAsync()
        {
            Projects = await ApplicationData.Current.LocalFolder.ReadAsync<ObservableCollection<Project>>("projects");
            if(Projects == null)
            {
                Projects = new ObservableCollection<Project>();
            }
        }
    }
}
