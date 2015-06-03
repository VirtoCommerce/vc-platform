using System;
using System.Linq;
using VirtoCommerce.Content.Data;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Platform.Tests.Bases;
using Xunit;

namespace VirtoCommerce.Content.Tests
{
    public class ContentScenarios : RepositoryTestBase<DatabaseContentRepositoryImpl, SqlContentDatabaseInitializer>
    {
        [Fact]
        [Trait("Category", "CI")]
        public void Can_content_query_menu_lists()
        {
            var repository = this.GetRepository();
            var service = new MenuServiceImpl(repository);
            var lists = service.GetListsByStoreId("SampleStore");
            Assert.True(lists.Any());
        }

        [Fact]
        [Trait("Category", "CI")]
        public void Can_content_add_themes()
        {
            var repository = GetRepository();

            var theme1 = new Theme
            {
                ThemePath = "Apple/Simple",
                Name = "Simple",
                Id = "Apple/Simple"
            };

            var theme2 = new Theme
            {
                ThemePath = "Apple/Timber",
                Name = "Timber",
                Id = "Apple/Timber"
            };

            repository.Add(theme1);
            repository.Add(theme2);
            repository.UnitOfWork.Commit();
        }

        [Fact]
        [Trait("Category", "CI")]
        public void Can_content_add_items()
        {
            var fullPath = string.Format("{0}\\Themes\\", Environment.CurrentDirectory.Replace("\\bin\\Debug", string.Empty));

            var fileSystemFileRepository = new FileSystemContentRepositoryImpl(fullPath);
            var item = fileSystemFileRepository.GetContentItem("Apple/Simple/layout/theme.liquid").Result;

            var repository = GetRepository();

            var file1 = new ContentItem
            {
                Id = Guid.NewGuid().ToString(),
                Path = "Apple/Simple/layout/theme.liquid",
                Name = "theme.liquid",
                CreatedDate = DateTime.UtcNow,
                ByteContent = item.ByteContent
            };

            repository.Add(file1);

            var file2 = new ContentItem
            {
                Id = Guid.NewGuid().ToString(),
                Path = "Apple/Simple/templates/404.liquid",
                Name = "404.liquid",
                CreatedDate = DateTime.UtcNow,
                ByteContent = item.ByteContent
            };

            repository.Add(file2);

            var file3 = new ContentItem
            {
                Id = Guid.NewGuid().ToString(),
                Path = "Apple/Timber/templates/404.liquid",
                Name = "404.liquid",
                CreatedDate = DateTime.UtcNow,
                ByteContent = item.ByteContent
            };

            repository.Add(file3);
            repository.UnitOfWork.Commit();
        }
    }
}
