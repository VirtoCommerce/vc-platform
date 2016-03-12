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
    public class LinkHelperTest
    {
        public LinkHelper LinkHelper { get; private set; }

        public LinkHelperTest()
        {
            LinkHelper = new LinkHelper();
        }

        [Fact]
        public void GetTitle_returns_original_value_when_no_timestamp()
        {
            var result = LinkHelper.GetTitle(@"C:\temp\foobar_baz.md");
            Assert.Equal("foobar_baz", result);
        }

        [Fact]
        public void GetTitle_returns_strips_timestamp()
        {
            var result = LinkHelper.GetTitle(@"C:\temp\2012-01-03-foobar_baz.md");
            Assert.Equal("foobar_baz", result);
        }

        [Fact]
        public void GetTitle_preserves_dash_separated_values_that_arent_timestamps()
        {
            var result = LinkHelper.GetTitle(@"C:\temp\foo-bar-baz-qak-foobar_baz.md");
            Assert.Equal("foo-bar-baz-qak-foobar_baz", result);
        }

        [InlineData("date", "cat1/cat2/2015/03/09/foobar-baz", "cat1,cat2")]
        [InlineData("date", "2015/03/09/foobar-baz", "")]
        [InlineData("/:dashcategories/:year/:month/:day/:title.html", "/cat1-cat2/2015/03/09/foobar-baz.html", "cat1,cat2")]
        [InlineData("/:dashcategories/:year/:month/:day/:title.html", "/2015/03/09/foobar-baz.html", "")]
        [InlineData("pretty", "cat1/cat2/2015/03/09/foobar-baz/", "cat1,cat2")]
        [InlineData("ordinal", "cat1/cat2/2015/068/foobar-baz", "cat1,cat2")]
        [InlineData("none", "cat1/cat2/foobar-baz", "cat1,cat2")]
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
                LocalPath = @"C:\temp\2015-03-09-foobar-baz.md",
                RelativePath = @"2015-03-09-foobar-baz.md"
            };

            Assert.Equal(expectedUrl, LinkHelper.EvaluatePermalink(permalink, page));
        }
    }
}
