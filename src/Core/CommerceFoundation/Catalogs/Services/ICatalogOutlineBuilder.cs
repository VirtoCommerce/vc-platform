namespace VirtoCommerce.Foundation.Catalogs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VirtoCommerce.Foundation.Catalogs.Model;

    public interface ICatalogOutlineBuilder
    {
        CatalogOutlines BuildCategoryOutline(string catalogId, string itemId, bool useCache = true);
        CatalogOutline BuildCategoryOutline(string catalogId, CategoryBase category, bool useCache = true);
    }

    public class CatalogOutlines : List<CatalogOutline>
    {
        public override string ToString()
        {
            return ToString(";");
        }

        public string ToString(string separator)
        {
            return String.Join(separator, this.Select(m => m.ToString(separator)));
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

        /// <summary>
        /// Only returns paths for actual categories (linked categories shouldn't be included)
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ToString(string separator)
        {

            return Categories.OfType<Category>().Aggregate(CatalogId, 
                (current, category) => current + String.Format("{0}{1}",separator, category.CategoryId));
        }
    }

    /// <summary>
    /// The CatalogOutline wrapper that is used for item browsing.
    /// It includes only Category type.
    /// </summary>
    public class BrowsingOutline
    {
        public readonly CatalogOutline FullOutline;

        public List<Category> Categories { get; private set; }

        public BrowsingOutline(CatalogOutline fullOutline)
        {
            FullOutline = fullOutline;
            Categories = fullOutline.Categories.OfType<Category>().ToList();
        }

        public override string ToString()
        {
            return ToString("/");
        }

        public string ToString(string separator, string field = "CategoryId")
        {
            return Categories.Aggregate("",
                (current, category) => current + String.Format("{0}{1}",
                    string.IsNullOrEmpty(current) ? string.Empty : separator,
                    category.GetType().GetProperty(field).GetValue(category)));
        }
    }
}
