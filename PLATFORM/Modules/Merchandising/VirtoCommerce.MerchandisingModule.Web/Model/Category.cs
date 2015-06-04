using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Category
    {
        #region Public Properties

        public string Code { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Category> Parents { get; set; }

        public IEnumerable<SeoKeyword> Seo { get; set; }
        public bool Virtual { get; set; }

        #endregion
    }
}
