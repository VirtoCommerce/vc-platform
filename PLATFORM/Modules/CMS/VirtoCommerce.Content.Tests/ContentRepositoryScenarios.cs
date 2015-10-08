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
                    token: Environment.GetEnvironmentVariable("GithubToken"),
                    productHeaderValue: "VirtoCommerce",
                    ownerName: "VirtoCommerce",
                    repositoryName: "test-repository",
                    mainPath: _githubMainPath);

                return repository;
            }
            else if (repositoryType == "Database")
            {
                EnsureDatabaseInitialized(() => new DatabaseContentRepositoryImpl(DatabaseName));
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

        [Theory,
            InlineData("GitHub")
        ]   
        [Trait("Category", "CI")]
        public void Can_save_and_delete_pages(string repositoryType)
        {
            var repository = GetRepository(repositoryType);

            //create pages
            repository.SavePage("Content_Test_Store/en-US/test_pages/test_page_1.html", new ContentPage
            {
                ByteContent = Encoding.UTF8.GetBytes("<a></a>"),
                ContentType = "text/html",
                CreatedDate = DateTime.UtcNow,
                Language = "en-US",
                Name = "test_page_1.html",
                Path = "Content_Test_Store/en-US/test_pages/test_page_1.html",
                Id = "Content_Test_Store/en-US/test_pages/test_page_1.html"
            });

            repository.SavePage("Content_Test_Store/en-US/test_pages/test_page_2.html", new ContentPage
            {
                ByteContent = Encoding.UTF8.GetBytes("<div></div>"),
                ContentType = "text/html",
                CreatedDate = DateTime.UtcNow,
                Language = "en-US",
                Name = "test_page_2.html",
                Path = "Content_Test_Store/en-US/test_pages/test_page_2.html",
                Id = "Content_Test_Store/en-US/test_pages/test_page_2.html"
            });

            //get page
            var page = repository.GetPage("Content_Test_Store/en-US/test_pages/test_page_1.html");
            Assert.NotNull(page);
            Assert.Equal(Encoding.UTF8.GetBytes("<a></a>"), page.ByteContent);
            Assert.Equal("test_page_1", page.Name);

            // TODO: make this work
            //get pages 
            //var pages = repository.GetPages("Content_Test_Store/", null);
            //Assert.NotNull(pages);
            //Assert.Equal(2, pages.Count());
            //Assert.True(pages.First().Path.StartsWith("Content_Test_Store/en-US/test_pages/test_page_"));

            //delete pages
            repository.DeletePage("Content_Test_Store/en-US/test_pages/test_page_1.html");
            repository.DeletePage("Content_Test_Store/en-US/test_pages/test_page_2.html");
            //pages = repository.GetPages("Content_Test_Store/", null);
            //Assert.Equal(0, pages.ToArray().Length);
        }

        [Theory, InlineData("GitHub")]
        [Trait("Category", "CI")]
        public void Can_create_and_query_pages(string repositoryType)
        {
            var repository = GetRepository(repositoryType);

            //create pages
            repository.SavePage("Content_Test_Store/en-US/test_pages/test_page_1.html", new ContentPage
            {
                ByteContent = Encoding.UTF8.GetBytes("<a></a>"),
                ContentType = "text/html",
                CreatedDate = DateTime.UtcNow,
                Language = "en-US",
                Name = "test_page_1.html",
                Path = "Content_Test_Store/en-US/test_pages/test_page_1.html",
                Id = "Content_Test_Store/en-US/test_pages/test_page_1.html"
            });

            var pages = repository.GetPages("Content_Test_Store/", null);
            Assert.NotNull(pages);
            Assert.Equal(1, pages.Count());

            repository.DeletePage("Content_Test_Store/en-US/test_pages/test_page_1.html");
        }

        public override void Dispose()
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
