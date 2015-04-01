using System;
using System.Linq;
using System.Web;
using Data = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class CollectionConverters
    {
        public static Collection AsWebModel(this Data.Category category)
        {
            var collection = new Collection();

            var urlTemplate = VirtualPathUtility.ToAbsolute("~/collections/{0}");

            collection.AllProductsCount = 0; // TODO
            collection.AllTags = null; // TODO
            collection.AllTypes = null; // TODO
            collection.AllVendors = null; // TODO
            collection.CurrentType = null; // TODO
            collection.CurrentVendor = null; // TODO
            collection.DefaultSortBy = "manual";
            collection.Description = null; // TODO
            collection.Handle = category.Code;
            collection.Id = category.Id;
            collection.Image = null; // TODO
            collection.Keywords = category.Seo != null ? category.Seo.Select(k => k.AsWebModel()) : null;
            collection.NextProduct = null; // TODO
            collection.Parents = category.Parents != null ? category.Parents.Select(p => p.AsWebModel()) : null;
            collection.PreviousProduct = null; // TODO
            collection.Products = null; // TODO
            collection.TemplateSuffix = null; // TODO
            collection.Title = category.Name;
            collection.Url = string.Format(urlTemplate, category.Code);

            return collection;
        }
    }
}