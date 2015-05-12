#region
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class SeoKeyword : Drop
    {
        #region Public Properties
        public string ImageAltDescription { get; set; }

        public string Keyword { get; set; }

        public string Language { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public string Title { get; set; }
        #endregion
    }
}