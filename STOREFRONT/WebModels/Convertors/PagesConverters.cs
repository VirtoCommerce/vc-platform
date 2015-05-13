using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Views.Contents;

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
                Layout = item.Layout
            };

            return ret;
        }
    }
}
