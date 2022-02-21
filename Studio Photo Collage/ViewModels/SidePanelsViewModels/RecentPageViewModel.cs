using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using Windows.Storage;

namespace Studio_Photo_Collage.ViewModels.SidePanelsViewModels
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
                        var dialogResult = await Messenger.Send(new SaveProjectRequestMessage());
                        if (dialogResult != Windows.UI.Xaml.Controls.ContentDialogResult.None)
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
                    removeAllCollagesCommand = new RelayCommand(async () =>
                    {
                        if (Projects != null)
                        {
                            var dialog = new ConfirmDialog();
                            var result = await dialog.ShowAsync();

                            if (result.ToString() == "Primary") //yes
                            {
                                Projects.Clear();
                                await ApplicationData.Current.LocalFolder.SaveAsync<ObservableCollection<Project>>("projects.json", null);
                            }
                        }
                    });
                }

                return removeAllCollagesCommand;
            }
        }

        public RecentPageViewModel()
        {
            DesserializeProjectsAsync();

            Messenger.Register<ProjectSavedMessage>(this, (r, m) =>
            {
                if (!Projects.Contains(m.Value))
                {
                    Projects.Add(m.Value);
                }
            });
        }

        private async void DesserializeProjectsAsync()
        {
            Projects = await ApplicationData.Current.LocalFolder.ReadAsync<ObservableCollection<Project>>("projects");
        }
    }
}
