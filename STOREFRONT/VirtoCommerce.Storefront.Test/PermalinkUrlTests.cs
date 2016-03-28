using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.StaticContent;
using VirtoCommerce.Storefront.Services;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    [Trait("Category", "CI")]
    public class PermalinkUrlTests
    {
  
        [Fact]
        public void GetUrl_returns_folder_and_original_value_when_no_timestamp()
        {
            var contentItem = new ContentPage
            {
                StoragePath = @"/temp/foobar_baz.md",
            };
            contentItem.Permalink = ":folder/:title";
            contentItem.LoadContent(string.Empty, null, null);
            Assert.Equal("temp/foobar_baz", contentItem.Url);
        }

        [Fact]
        public void GetUrl_returns_file_name_when_no_folder()
        {
            var contentItem = new ContentPage
            {
                StoragePath = @"/foobar_baz.en-us.md",
            };
            contentItem.Permalink = ":title";
            contentItem.LoadContent(string.Empty, null, null);
            Assert.Equal("foobar_baz", contentItem.Url);
        }


        [Fact]
        public void GetUrl_returns_strips_timestamp()
        {
            var contentItem = new ContentPage
            {
                StoragePath = @"/temp/2012-01-03-foobar_baz.md",
            };
            contentItem.Permalink = ":title";
            contentItem.LoadContent(string.Empty, null, null);
            Assert.Equal("foobar_baz", contentItem.Url);
        }

        [Fact]
        public void GetUrl_preserves_dash_separated_values_that_arent_timestamps()
        {
            var contentItem = new ContentPage
            {
                StoragePath = @"/temp/foo-bar-baz-qak-foobar_baz.md",
            };
            contentItem.Permalink = ":title";
            contentItem.LoadContent(string.Empty, null, null);
            Assert.Equal("foo-bar-baz-qak-foobar_baz", contentItem.Url);
        }

        [InlineData("date", "temp/cat1/cat2/2015/03/09/foobar-baz", "cat1,cat2")]
        [InlineData("date", "temp/2015/03/09/foobar-baz", "")]
        [InlineData("/:dashcategories/:year/:month/:day/:title.html", "/cat1-cat2/2015/03/09/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:dashcategories/:year/:month/:day/:title.html", "/2015/03/09/foobar-baz.html", "")]
        [InlineData("pretty", "temp/cat1/cat2/2015/03/09/foobar-baz/", "cat1,cat2")]
        [InlineData("ordinal", "temp/cat1/cat2/2015/068/foobar-baz", "cat1,cat2")]
        [InlineData("none", "temp/cat1/cat2/foobar-baz", "cat1,cat2")]
        [InlineData("/:categories/:short_year/:i_month/:i_day/:title.html", "/cat1/cat2/15/3/9/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:category/:title.html", "/cat1/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:category/:title.html", "/foobar-baz.html", "")]
        [InlineData("/:category1/:title.html", "/cat1/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:category2/:title.html", "/cat2/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:category3/:title.html", "/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:categories/:title/", "/cat1/cat2/foobar-baz/", "cat1,cat2")]
        [InlineData("/:categories/:title", "/cat1/cat2/foobar-baz", "cat1,cat2")]
        [Theory]
        public void EvaluatePermalink_url_is_well_formatted(string permalink, string expectedUrl, string categories)
        {
            var page = new ContentPage
            {
                Categories = categories == null ? new List<string>() : categories.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                PublishedDate = new DateTime(2015, 03, 09),
                StoragePath = @"/temp/2015-03-09-foobar-baz.md",
                Permalink = permalink
            };
            page.LoadContent(string.Empty, null, null);
            Assert.Equal(expectedUrl, page.Url);
        }
    }
}
