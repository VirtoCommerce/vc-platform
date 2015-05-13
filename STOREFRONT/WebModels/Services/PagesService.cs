#region

using System;
using System.Web.Hosting;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Views.Contents;
using VirtoCommerce.Web.Views.Engines.Liquid;

#endregion

namespace VirtoCommerce.Web.Services
{
    public class PagesService
    {
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

            return item.AsPageWebModel();
        }
        #endregion
    }
}