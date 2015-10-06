using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
{
    public static class CollectionConverters
    {
        public static Collection AsWebModel(this Data.Category category)
        {
            var collection = new Collection();

            var urlTemplate = VirtualPathUtility.ToAbsolute("~/collections/{0}");

            collection.AllTypes = null; // TODO
            collection.AllVendors = null; // TODO
            collection.CurrentType = null; // TODO
            collection.CurrentVendor = null; // TODO
            collection.DefaultSortBy = "manual";
            collection.Description = null; // TODO
            collection.Handle = category.Code;
            collection.Id = category.Id;

            if (category.Image != null)
            {
                collection.Image = category.Image.AsWebModel(category.Image.Name, category.Id);
            }

            collection.Keywords = category.Seo?.Select(k => k.AsWebModel());
            collection.NextProduct = null; // TODO
            collection.Parents = category.Parents?.Select(p => p.AsWebModel());
            collection.PreviousProduct = null; // TODO
            collection.TemplateSuffix = null; // TODO
            collection.Title = category.Name;
            collection.Url = string.Format(urlTemplate, category.Code);

            // specify SEO based url
            var outline = collection.BuildOutline(Thread.CurrentThread.CurrentUICulture.Name).Select(x => x.Value);
            if (outline.Any())
            {
                var urlHelper = UrlHelperExtensions.GetUrlHelper();
                collection.Outline = string.Join("/", outline);
                collection.Url = urlHelper.CategoryUrl(collection.Outline);
            }

            return collection;
        }
    }
}