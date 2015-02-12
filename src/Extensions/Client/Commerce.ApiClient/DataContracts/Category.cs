namespace VirtoCommerce.Web.Core.DataContracts
{
    public class Category
    {
        #region Public Properties

        public string Code { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public CategoryInfo[] Parents { get; set; }

        public SeoKeyword[] SeoKeywords { get; set; }

        public bool Virtual { get; set; }

        #endregion
    }
}