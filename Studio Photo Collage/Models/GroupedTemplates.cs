using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Studio_Photo_Collage.Models
{
    public class GroupedTemplates
    {
        public GroupedTemplates()
        {
        }

        public ObservableCollection<Project> Projects { get; set; } = new ObservableCollection<Project>();

        public int CountOfPhotos { get; set; }


        public static ObservableCollection<GroupedTemplates> FillByGroupedTemplate()
        {
            var collection = new ObservableCollection<GroupedTemplates>();
            var newgroup = new GroupedTemplates();

            newgroup.Projects.Add(new Project(new byte[,] { { 1, 1, 1, 1 }, { 3, 2, 4, 5} }));
            newgroup.Projects.Add(new Project(new byte[,] { { 3 }, { 4 } }));
            newgroup.CountOfPhotos = 1;

            AddProjects(ref newgroup, newgroup.Projects[0], 4);
            collection.Add(newgroup);

            return collection;
        }

        private static void AddProjects(ref GroupedTemplates groupedTemplates, Project project, int countOfRotation)
        {
            for (int i = 0; i < countOfRotation; i++)
            {
                Project newproj = project.GetRotatedProject(i);
                groupedTemplates.Projects.Add(newproj);
            }
        }
    }

}
