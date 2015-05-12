#region

using System;
using System.Web.Hosting;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Views.Contents;
using VirtoCommerce.Web.Views.Engines.Liquid;

#endregion

namespace VirtoCommerce.Web.Services
{
    public class PagesService
    {
        /*
        #region Public Methods and Operators
        public async Task<Page> GetPageAsync(SiteContext context, string handle)
        {
            var client = ClientContext.Clients.CreatePageClient();
            var page = await client.GetPageAsync(context.StoreId, context.Language, handle);

            if (page == null)
                return null;

            return new Page
            {
                Author = "",
                Content = page.Content,
                Handle = handle,
                Id = handle,
                Url = "/pages/"+handle,
                PublishedAt = page.ModifiedDate,
                Title = page.Name
            };
        }
        #endregion
         * */

        #region Public Methods and Operators
        public Page GetPage(SiteContext context, string handle)
        {
            var filesPath = HostingEnvironment.MapPath(String.Format("~/App_Data/Pages/{0}/{1}", context.StoreId, context.Language));
            var service = new PublishingService(filesPath, new[] { new LiquidTemplateEngine(filesPath) });

            if (!handle.StartsWith("/"))
                handle = "/" + handle;

            var item = service.GetContentItem(handle);

            if (item == null)
            {
                return null;
            }

            return new Page
            {
                Author = item.Author,
                Content = item.Content,
                Handle = handle,
                Id = handle,
                Url = item.Url,
                PublishedAt = item.Date,
                Title = item.Title,
                Layout = item.Layout
            };
        }
        #endregion
    }
}