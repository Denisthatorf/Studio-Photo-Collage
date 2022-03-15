using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Messages;
using Studio_Photo_Collage.Models;
using Studio_Photo_Collage.Views.PopUps;
using Windows.Storage;
using Windows.UI.Input.Inking;

namespace Studio_Photo_Collage.Infrastructure.Helpers
{
    public static class ProjectHelper
    {
        public static async void DeleteAllProjectsFromJson()
        {
            await ApplicationData.Current.LocalFolder.SaveAsync<object>("projects", null);
            WeakReferenceMessenger.Default.Send(new DeleteAllProjectMessage());
        }

        public static async void DeleteProject(Project project)
        {
            var projects = await GetProjectsFromFile();
            if (projects != null && projects.Contains(project))
            {
                projects.Remove(project);
                await ApplicationData.Current.LocalFolder.SaveAsync("projects", projects);
            }
            var removedProject = projects.Where(x => x == project).FirstOrDefault();

            projects.Remove(removedProject);
            await InkCanvasHelper.DeleteStrokeFileByUid(project.uid.ToString());

            WeakReferenceMessenger.Default.Send(new DeleteProjectMessage(removedProject));
        }

        public static async void SaveProject(Project project, InkPresenter inkPresenter)
        {
            var projects = await GetProjectsFromFile();
            if (projects == null)
            {
                projects = new List<Project>();
            }

            int index = projects.IndexOf(project);
            if (index == -1)
            {
                projects.Add(project);
            }
            else
            {
                projects[index] = project;
            }

            await ApplicationData.Current.LocalFolder.SaveAsync("projects", projects);
            await InkCanvasHelper.SaveStrokesAsync(inkPresenter, project.uid.ToString());
            WeakReferenceMessenger.Default.Send(new ProjectSavedMessage(project));
        }

        private static async Task<List<Project>> GetProjectsFromFile()
        {
            return await ApplicationData.Current.LocalFolder.ReadAsync<List<Project>>("projects");
        }
    }
}
