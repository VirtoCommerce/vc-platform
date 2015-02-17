namespace VirtoCommerce.Web.Core.DataContracts
{
    public class CategoryInfo
    {
        #region Public Properties
        public string Id { get; set; }

        public string Name { get; set; }

        public SeoKeyword[] SeoKeywords { get; set; }
        #endregion
    }
}