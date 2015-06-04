using System.Collections.Generic;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class DynamicContentItem
    {
        #region Public Properties

        public string ContentType { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public bool IsMultilingual { get; set; }
        public string Name { get; set; }

        public PropertyDictionary Properties { get; set; }

        #endregion
    }
 
}
