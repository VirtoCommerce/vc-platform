using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.StaticContent;
using VirtoCommerce.Storefront.Services;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class StaticContentTests
    {
        [Fact]
        public void LoadPageForLanguage_PageLoadedFromFolder()
        {
            var language = new Model.Language("en-US");
            var service = GetStaticContentService();
            var result = service.LoadContentItemsByUrl("/about_us", new Model.Store { Id = "TestStore" }, language, () => new ContentPage());

            var page = result.Single();
            Assert.IsType<ContentPage>(page);
            Assert.Equal(page.Language, language);
            Assert.NotEmpty(page.Content);
            Assert.Equal(page.Url, "folder1/about_us");
            Assert.Equal(Path.GetFileName(page.LocalPath), "about_us.en-US.md");
        }


        [Fact]
        public void LoadPageForMissedLanguage_PageLoadedWithDefaultLanguage()
        {
            var language = new Model.Language("es-ES");
            var service = GetStaticContentService();
            var result = service.LoadContentItemsByUrl("/about_us", new Model.Store { Id = "TestStore" }, language, () => new ContentPage());

            var page = result.Single();
            Assert.IsType<ContentPage>(page);
            Assert.Equal(page.Language, language);
            Assert.NotEmpty(page.Content);
            Assert.Equal(page.Url, "about_us");
            Assert.Equal(Path.GetFileName(page.LocalPath), "about_us.md");
        }

        private IStaticContentService GetStaticContentService()
        {
            var cacheManager = new Moq.Mock<ICacheManager<object>>();
            var urlBuilder = new Moq.Mock<IStorefrontUrlBuilder>();
            var liquidEngine = new Moq.Mock<ILiquidThemeEngine>();
            var markdown = new Moq.Mock<MarkdownDeep.Markdown>();
            var path = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "Pages");
            var retVal = new StaticContentServiceImpl(path, markdown.Object, liquidEngine.Object, cacheManager.Object, ()=> null, ()=> urlBuilder.Object);
            return retVal;
        }
    }
}
