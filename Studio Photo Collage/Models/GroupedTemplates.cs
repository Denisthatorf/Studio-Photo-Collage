using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Studio_Photo_Collage.Models
{
    public class GroupedTemplates
    {
        public GroupedTemplates()
        {
            Projects = new List<Project>();
        }

        public List<Project> Projects { get; }

        public int CountOfPhotos { get; set; }


        public static ObservableCollection<GroupedTemplates> FillByGroupedTemplate()
        {
            var collection = new ObservableCollection<GroupedTemplates>();

            #region 1 Group
            var group1 = new GroupedTemplates();
            group1.CountOfPhotos = 1;
            AddProjects(ref group1, new Project(new byte[,] { { 1 } }), 1);
            collection.Add(group1);
            #endregion

            #region 2 Group
            var group2 = new GroupedTemplates();
            group2.CountOfPhotos = 2;

            AddProjects(ref group2, new Project(new byte[,] { { 1 }, { 2 } }), 2);

            AddProjects(ref group2, new Project(new byte[,] { { 1, 2, 2 } }), 4);
            collection.Add(group2);
            #endregion

            #region 3 Group
            var group3 = new GroupedTemplates();
            group3.CountOfPhotos = 3;

            AddProjects(ref group3, new Project(new byte[,] { { 1,2 },
                                                              { 1,3 }}), 4);

            AddProjects(ref group3, new Project(new byte[,] { { 1, 2, 3 } }), 2);

            AddProjects(ref group3, new Project(new byte[,] { { 1, 2, 2, 3 } }), 2);

            AddProjects(ref group3, new Project(new byte[,] { { 1,1 },
                                                              { 2,3 },
                                                              { 2,3 } }), 4);

            AddProjects(ref group3, new Project(new byte[,] { { 1,1 },
                                                              { 1,1 },
                                                              { 2,3 } }), 4);
            collection.Add(group3);
            #endregion

            //newgroup.Projects.Add(new Project(new byte[,] { { 1, 1, 1, 1 }, { 3, 2, 4, 5} }));
            return collection;
        }

        private static void AddProjects(ref GroupedTemplates groupedTemplates, Project project, int countOfRotation)
        {
            for (int i = 0; i < countOfRotation; i++)
            {
                Project newproj = GetRotatedProject(project.PhotoArray, i);
                groupedTemplates.Projects.Add(newproj);
            }
        }
        public static Project GetRotatedProject(byte[,] arr, int countOfRotation)
        {
            var width = arr.GetUpperBound(0) + 1;
            var height = arr.GetUpperBound(1) + 1;
            //byte[,] arr = new byte[width, height];

            //Array.Copy(_photoArray, arr, _photoArray.Length);
            for (int i = 0; i < countOfRotation; i++)
            {
                arr = RotateRight(arr);
            }
            return new Project(arr);
        }
        public static byte[,] RotateRight(byte[,] arr)
        {
            int width;
            int height;
            byte[,] dst;

            width = arr.GetUpperBound(0) + 1;
            height = arr.GetUpperBound(1) + 1;

            //var src = new byte[width, height];
            // Array.Copy(arr, src, src.Length);

            dst = new byte[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = height - (row + 1);

                    dst[newCol, newRow] = arr[col, row];
                }
            }

            return dst;
        }
    }

}
