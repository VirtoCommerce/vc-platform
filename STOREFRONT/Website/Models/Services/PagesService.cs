#region

using System;
using System.Threading.Tasks;
using System.Web.Hosting;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Views.Contents;
using VirtoCommerce.Web.Views.Engines.Liquid;

#endregion

namespace VirtoCommerce.Web.Models.Services
{
    public class PagesService
    {
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
    }
}