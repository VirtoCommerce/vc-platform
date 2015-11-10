using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class CatalogSearchResult
    {
        public CatalogSearchResult()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
            Catalogs = new List<Catalog>();
        }
        public int TotalCount { get; set; }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Catalog> Catalogs { get; set; }
    }
}