using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using VirtoCommerce.Content.Data.Migrations;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Tests.Bases;
using Xunit;

namespace VirtoCommerce.Content.Tests
{
    public class ContentRepositoryScenarios : FunctionalTestBase
    {
        public const string DatabaseName = "ContentTests";
        private string _githubMainPath = "Themes/";

        protected IContentRepository GetRepository(string repositoryType)
        {
            if (repositoryType == "GitHub")
            {
                var repository = new GitHubContentRepositoryImpl(
                    token: " 59575aed67fa0e4a5a960cc9cc99f75b90d72d08",
                    productHeaderValue: "VirtoCommerce",
                    ownerName: "VirtoCommerce",
                    repositoryName: "test-repository",
                    mainPath: _githubMainPath);

                return repository;
            }
            else if (repositoryType == "Database")
            {
                EnsureDatabaseInitialized(() => new DatabaseContentRepositoryImpl(DatabaseName), () => Database.SetInitializer(new SetupDatabaseInitializer<DatabaseContentRepositoryImpl, Configuration>()));
                return new DatabaseContentRepositoryImpl(DatabaseName, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor());
            }
            else if (repositoryType == "FileSystem")
            {
                var repository = new FileSystemContentRepositoryImpl(TempPath);
                return repository;
            }

            throw new NullReferenceException(String.Format("{0} is not supported", repositoryType));
        }

        [Theory, 
            InlineData("GitHub"),
            InlineData("Database"),
            InlineData("FileSystem")
            ]
        [Trait("Category", "CI")]
        public void Can_content_create_new_template_file(string repositoryType)
        {
            var repository = GetRepository(repositoryType);

            var contentItem = new ContentItem();
            contentItem.Id = Guid.NewGuid().ToString();
            contentItem.ByteContent = Encoding.UTF8.GetBytes("test");;
            contentItem.ContentType = "text/html";
            contentItem.Path = "default/template/testtemplate.html";
            contentItem.Path = string.Format("{0}/{1}", "teststore", contentItem.Path);
            contentItem.CreatedDate = DateTime.UtcNow;
            contentItem.ModifiedDate = DateTime.UtcNow;

            repository.SaveContentItem(contentItem.Path, contentItem);
            repository.DeleteContentItem(contentItem.Path);
        }

        [Theory,
            InlineData("GitHub"),
            InlineData("Database"),
            InlineData("FileSystem")
            ]
        [Trait("Category", "CI")]
        public void Can_content_save_delete_repositoryitems(string repositoryType)
        {
            var repository = GetRepository(repositoryType);

            var content = new ContentItem();
            content.ByteContent = Encoding.UTF8.GetBytes("Some new stuff");
            content.Path = "new/new123.liquid";
            content.CreatedDate = DateTime.UtcNow;
            content.Id = Guid.NewGuid().ToString();
            content.Name = "new123.liquid";

            var path = "Apple/Simple/new/new123.liquid";

            repository.SaveContentItem(path, content);

            var items = repository.GetContentItems("Apple/Simple", new GetThemeAssetsCriteria() { LoadContent = true, LastUpdateDate = DateTime.UtcNow.AddDays(-5) });

            Assert.Equal(items.Count(), 1);

            var item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

            Assert.True(Encoding.UTF8.GetString(item.ByteContent).Contains("Some"));

            content = new ContentItem();
            content.ByteContent = Encoding.UTF8.GetBytes("Some new stuff. Changes");
            path = "Apple/Simple/new/new123.liquid";

            repository.SaveContentItem(path, content);

            items = repository.GetContentItems("Apple/Simple", new GetThemeAssetsCriteria() { LoadContent = true, LastUpdateDate = DateTime.UtcNow.AddDays(-5) });

            Assert.Equal(items.Count(), 1);

            item = repository.GetContentItem("Apple/Simple/new/new123.liquid");

            Assert.True(Encoding.UTF8.GetString(item.ByteContent).Contains("Some") && Encoding.UTF8.GetString(item.ByteContent).Contains("Changes"));

            path = "Apple/Simple/new/new123.liquid";

            repository.DeleteContentItem(path);

            items = repository.GetContentItems("Apple/Simple", new GetThemeAssetsCriteria() { LoadContent = true, LastUpdateDate = DateTime.UtcNow.AddDays(-5) });

            Assert.Equal(items.Count(), 0);
        }

        public void Dispose()
        {
            try
            {
                // Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
                // the temp location in which they are stored is later cleaned.
                using (var context = new DatabaseContentRepositoryImpl(DatabaseName))
                {
                    context.Database.Delete();
                }
            }
            finally
            {
            }
        }
    }
}
