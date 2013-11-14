using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Models;
using Xunit;

namespace VirtoCommerce.ConfigurationUtility.Tests
{
    public class ProjectsScenarios : IDisposable
    {
        private readonly string _filePath = String.Empty;
        private readonly string _fileName = "";

        public ProjectsScenarios()
        {
            _fileName = "projects.test.xml";

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _filePath = String.Format(@"{0}\VirtoCommerce\{1}", tempPath, _fileName);
        }

        public void Dispose()
        {
            File.Delete(_filePath);
        }

        [Fact]
        public void Can_save_project()
        {
            var project = new Project()
            {
                Name = "Sample Project",
                Id = Guid.NewGuid(),
                BrowseUrl = "Click to resolve",
                Created = DateTime.Now,
                Version = "1.0.0.0"
            };

            project.Location = new ProjectLocation() { LocalPath = @"E:\Program Files (x86)\VirtoCommerce 1.0\Projects\Default2", Type = LocationType.FileSystem, Url = @"E:\Program Files (x86)\VirtoCommerce 1.0\Projects\Default2" };

            // serialize
            var xml = XmlSerializationService<Project>.Serialize(project);


            var repository = new ProjectsRepository();
            repository.Add(project);
            repository.UnitOfWork.Commit();

            repository = new ProjectsRepository();
            Assert.True(repository.Projects.Count() == 1);


            var project2 = new Project()
            {
                Name = "Sample Project 2",
                Id = Guid.NewGuid(),
                BrowseUrl = "Click to resolve",
                Created = DateTime.Now,
                Version = "1.0.0.0"
            };

            project2.Location = new ProjectLocation() { LocalPath = @"E:\Program Files (x86)\VirtoCommerce 1.0\Projects\Default2", Type = LocationType.FileSystem, Url = @"E:\Program Files (x86)\VirtoCommerce 1.0\Projects\Default2" };

            repository.Add(project2);
            repository.UnitOfWork.Commit();

            repository = new ProjectsRepository();
            Assert.True(repository.Projects.Count() == 2);

            repository.Remove(repository.Projects.ElementAt(0));
            repository.UnitOfWork.Commit();

            repository = new ProjectsRepository();
            Assert.True(repository.Projects.Count() == 1);
        }
    }
}
