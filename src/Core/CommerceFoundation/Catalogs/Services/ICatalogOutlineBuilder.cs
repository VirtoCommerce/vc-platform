namespace VirtoCommerce.Foundation.Catalogs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.Foundation.Catalogs.Model;

    public interface ICatalogOutlineBuilder
    {
        CatalogOutlines BuildCategoryOutline(string catalogId, Item item, bool useCache = true);
        CatalogOutline BuildCategoryOutline(string catalogId, CategoryBase category, bool useCache = true);
    }

    public class CatalogOutlines
    {
        public List<CatalogOutline> Outlines { get; private set; }

        public CatalogOutlines()
        {
            Outlines = new List<CatalogOutline>();
        }

        public override string ToString()
        {
            return ToString(";");
        }

        public string ToString(string separator, string field = "CategoryId")
        {
            return String.Join(separator, Outlines.Select(m => m.ToString(separator, field)));
        }
    }

    public class CatalogOutline
    {
        public string CatalogId { get; set; }
        public List<CategoryBase> Categories { get; private set; }

        public CatalogOutline()
        {
            Categories = new List<CategoryBase>();
        }

        public override string ToString()
        {
            return ToString("/");
        }

        public string ToString(string separator, string field = "CategoryId")
        {
            var ret = this.CatalogId;
            return this.Categories.Aggregate(ret, (current, category) => current + String.Format("{0}{1}", separator, category.GetType().GetProperty(field).GetValue(category) ));
        }
    }
}
