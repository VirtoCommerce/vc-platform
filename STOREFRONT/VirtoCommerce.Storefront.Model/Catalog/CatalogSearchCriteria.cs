using System;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchCriteria : PagedSearchCriteria
    {
        public CatalogSearchCriteria()
            :this(new NameValueCollection())
        {
        }
        public CatalogSearchCriteria(NameValueCollection queryString)
            : base(queryString)
        {
            ResponseGroup = CatalogSearchResponseGroup.WithProducts;
            SearchInChildren = true;

            Parse(queryString);
        }

        public CatalogSearchResponseGroup ResponseGroup { get; set; }
        public string CatalogId { get; set; }
        public string CategoryId { get; set; }
        public Currency Currency { get; set; }
        public Language Language { get; set; }
        public string Keyword { get; set; }
      
        public Term[] Terms { get; set; }
        public string SortBy { get; set; }
        public bool SearchInChildren { get; set; }

        private void Parse(NameValueCollection queryString)
        {
            Keyword = queryString.Get("q");
            //TODO move this code to Parse or Converter method
            // tags=name1:value1,value2,value3;name2:value1,value2,value3
            SortBy = queryString.Get("sort_by");
            Terms = (queryString.GetValues("terms") ?? new string[0])
                .SelectMany(s => s.Split(';'))
                .Select(s => s.Split(':'))
                .Where(a => a.Length == 2)
                .SelectMany(a => a[1].Split(',').Select(v => new Term { Name = a[0], Value = v }))
                .ToArray();
        }
    }
}
