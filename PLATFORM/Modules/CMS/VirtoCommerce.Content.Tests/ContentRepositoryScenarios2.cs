using System;
using System.Text;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Platform.Tests.Bases;
using Xunit;

namespace VirtoCommerce.Content.Tests
{
    public class ContentRepositoryScenarios2 : FunctionalTestBase
    {
        protected IContentRepository2 GetRepository(string repositoryType)
        {
            if (repositoryType == "GitHub")
            {
                var repository = new GitHubContentRepositoryImpl2(
                    token: Environment.GetEnvironmentVariable("GithubToken"),
                    productHeaderValue: "VirtoCommerce",
                    ownerName: "VirtoCommerce",
                    repositoryName: "test-repository",
                    mainPath: "repository2/");

                return repository;
            }

            throw new NullReferenceException(string.Format("{0} is not supported", repositoryType));
        }

        [Theory, 
            InlineData("GitHub")
            ]
        [Trait("Category", "CI")]
        public async void Can_content_create_new_template_file(string repositoryType)
        {
            var repository = GetRepository(repositoryType);

            var contentItem = new ContentItem
                              {
                                  Id = Guid.NewGuid().ToString(),
                                  ByteContent = Encoding.UTF8.GetBytes("test"),
                                  ContentType = "text/html",
                                  Path = "default/template/testtemplate.html"
                              };

            contentItem.Path = string.Format("{0}/{1}", "teststore", contentItem.Path);
            contentItem.CreatedDate = DateTime.UtcNow;
            contentItem.ModifiedDate = DateTime.UtcNow;

            var changeSet = await repository.CreateFile(contentItem.Path, contentItem);
            var createdItem = await repository.GetContent(changeSet.Items[0].ContentItem.Path);

            await repository.DeleteFile(contentItem.Path);
        }
    }
}
