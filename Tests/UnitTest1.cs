using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class ProjectSerializationTests
    {
        [Fact]
        public async void Test1()
        {
            try
            {
                var str = await JsonHelper.DeserializeFileAsync("projects.json");
                var projects = await JsonHelper.ToObjectAsync<List<Project>>(str);
                if (projects == null)
                    projects = new List<Project>();

                var project = new Project();
                int index = projects.IndexOf(project);
                if (index == -1)
                    projects.Add(project);
                else
                    projects[index] = project;

                // string projec = await JsonHelper.StringifyAsync(CurrentCollage.Project);
                string projectsAsList = await JsonHelper.StringifyAsync(projects);
                await JsonHelper.WriteToFile("test.json", projectsAsList);
            }
            catch 
            {
                Assert.True(false);
            }

        }
    }
}