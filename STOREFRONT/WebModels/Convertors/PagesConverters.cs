using Omu.ValueInjecter;
using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Views.Contents;
using VirtoCommerce.Web.Extensions;

namespace VirtoCommerce.Web.Convertors
{
    public static class PagesConverters
    {
        public static Page AsPageWebModel(this ContentItem item)
        {
            var ret = new Page
            {
                Author = item.Author,
                Content = item.Content,
                Handle = item.FileName,
                Id = item.FileName,
                Url = item.Url,
                PublishedAt = item.Date,
                Title = item.Title,
                Layout = item.Layout//,
                //Properties = item.Settings
            };

            return ret;
        }

        public static Article AsArticleWebModel(this ContentItem item)
        {
            var page = item.AsPageWebModel();
            var ret = new Article();
            ret.InjectFrom(page);
            ret.Handle = item.Url.TrimStart('/').TrimStart("blogs/");
            ret.Id = item.Url.TrimStart('/');
            ret.Excerpt = item.ContentExcerpt;

            return ret;
        }
    }
}
