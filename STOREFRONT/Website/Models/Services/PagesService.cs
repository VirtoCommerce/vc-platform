#region

using System;
using System.Web.Hosting;
using VirtoCommerce.Web.Views.Contents;
using VirtoCommerce.Web.Views.Engines.Liquid;

#endregion

namespace VirtoCommerce.Web.Models.Services
{
    public class PagesService
    {
        #region Fields
        private Page[] AllPages =
        {
            new Page
            {
                Author = "Unknown",
                Content = "Some sample content",
                Handle = "about-us",
                Id = "About Us",
                Url = "/pages/aboutus",
                PublishedAt = DateTime.Now,
                Title = "About Us"
            }
        };
        #endregion

        #region Public Methods and Operators
        public Page GetPage(string handle)
        {
            var filesPath = HostingEnvironment.MapPath("~/App_Data/vc-contents");
            var service = new PublishingService(filesPath, new[] { new LiquidTemplateEngine(filesPath) });

            var item = service.GetContentItem(handle.Contains("/") ? handle : String.Format("/pages/{0}", handle));

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