
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using Windows.Storage;

namespace UnitTestsUwp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task SerializeTestEmptyProj()
        {
            try
            {
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await localFolder.CreateFileAsync($"test.jpg", CreationCollisionOption.ReplaceExisting);
                await file.DeleteAsync(StorageDeleteOption.Default);

                var project = new Project();

                string projectsAsList = await JsonHelper.StringifyAsync(project);
                await JsonHelper.WriteToFile("test.json", projectsAsList);
            }
            catch
            {
                Assert.Fail("Can't serialize");
            }
        }

        [TestMethod]
        public async Task SerializeTestWithNormalConstructorUsed()
        {
            try
            {
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await localFolder.CreateFileAsync($"test.jpg", CreationCollisionOption.ReplaceExisting);
                await file.DeleteAsync(StorageDeleteOption.Default);

                var project = new Project(new byte[,] { {1}, {2} });

                string projectsAsList = await JsonHelper.StringifyAsync(project);
                await JsonHelper.WriteToFile("test.json", projectsAsList);
            }
            catch
            {
                Assert.Fail("Can't serialize");
            }
        }


        [TestMethod]
        public async Task SerializeTestWithNormalConstructorUsedAndInList()
        {
            try
            {
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var file = await localFolder.CreateFileAsync($"test.jpg", CreationCollisionOption.ReplaceExisting);
                await file.DeleteAsync(StorageDeleteOption.Default);

                var list = new List<Project>()
                {
                    new Project(new byte[,] { { 1 }, { 2 } }),
                    new Project(new byte[,] { { 1, 3 }, { 2, 2 } })
                };

                string projectsAsList = await JsonHelper.StringifyAsync(list);
                await JsonHelper.WriteToFile("test.json", projectsAsList);
            }
            catch
            {
                Assert.Fail("Can't serialize");
            }
        }

        /* [TestMethod]
         public async void TestMethod2()
         {
             var str = await JsonHelper.DeserializeFileAsync("test.json");
             List<Project> projects = null;
             if (!String.IsNullOrEmpty(str))
                 projects = await JsonHelper.ToObjectAsync<List<Project>>(str);
             if (projects == null)
                 projects = new List<Project>();

             int index = projects.IndexOf(project);
             if (index == -1)
                 projects.Add(project);
             else
                 projects[index] = project;
         }*/
    }
}
