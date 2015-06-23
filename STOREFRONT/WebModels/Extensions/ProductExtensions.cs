using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Extensions
{
    public static class ProductExtensions
    {
        public static string[] GetAllVariationIds(this Data.Product product)
        {
            if (product == null) return null;

            var allIds = new List<string>();
            if (product.Variations != null)
            {
                allIds.AddRange(product.Variations.Select(v=>v.Id));
            }

            allIds.Add(product.Id);

            return allIds.ToArray();
        }

        public static string[] GetAllVariationIds(this Data.Product[] products)
        {
            var variationIds = products.Where(i => i.Variations != null).SelectMany(i => i.Variations).Select(v => v.Id);
            var productIds = products.Where(i => i.Variations == null).Select(p => p.Id);
            var allIds = new List<string>(variationIds);
            allIds.AddRange(productIds);
            allIds.AddRange(products.Where(p => p.Variations != null).Select(p => p.Id));

            return allIds.ToArray();
        }

        public static string BuildOutline(this Product product, string language, Collection collection)
        {
            var outline = String.Empty;
            if (product.Keywords != null)
            {
                var keyword = product.Keywords.SeoKeyword(Thread.CurrentThread.CurrentUICulture.Name);
                if (keyword != null)
                {
                }
            }

            return outline;
        }
    }
}