namespace VirtoCommerce.ApiClient.DataContracts
{
    public class Category
    {
        #region Public Properties

        public string Code { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }

        public Category[] Parents { get; set; }

        public SeoKeyword[] Seo { get; set; }

        public bool Virtual { get; set; }

        #endregion
    }
}
