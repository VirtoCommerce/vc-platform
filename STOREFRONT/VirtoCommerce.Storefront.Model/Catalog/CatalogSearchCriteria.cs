using System;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchCriteria
    {
        public CatalogSearchCriteria()
        {
            ResponseGroup = CatalogSearchResponseGroup.WithProducts;
            PageNumber = 1;
            PageSize = 20;
        }

        public CatalogSearchResponseGroup ResponseGroup { get; set; }
        public string CatalogId { get; set; }
        public string CategoryId { get; set; }
        public Currency Currency { get; set; }
        public Language Language { get; set; }
        public string Keyword { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Term[] Terms { get; set; }
        public string SortBy { get; set; }

        public static CatalogSearchCriteria Parse(NameValueCollection queryString)
        {
            var retVal = new CatalogSearchCriteria();
            retVal.Keyword = queryString.Get("q");
            retVal.PageNumber = Convert.ToInt32(queryString.Get("page") ?? "1");
            //TODO move this code to Parse or Converter method
            // tags=name1:value1,value2,value3;name2:value1,value2,value3
            retVal.SortBy = queryString.Get("sort_by");
            retVal.Terms = (queryString.GetValues("terms") ?? new string[0])
                .SelectMany(s => s.Split(';'))
                .Select(s => s.Split(':'))
                .Where(a => a.Length == 2)
                .SelectMany(a => a[1].Split(',').Select(v => new Term { Name = a[0], Value = v }))
                .ToArray();

            return retVal;
        }
    }
}
