using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Commerce.Model
{
    public class SeoInfo : AuditableEntity, ILanguageSupport, ICloneable
    {
        public string SemanticUrl { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string ImageAltDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string ObjectId { get; set; }
        public string ObjectType { get; set; }
        public bool IsActive { get; set; }

        #region ILanguageSupport Members
        public string LanguageCode { get; set; }
        #endregion

        #region ICloneable members
        public object Clone()
        {
            var retVal = new SeoInfo();

            retVal.CreatedBy = CreatedBy;
            retVal.CreatedDate = CreatedDate;
            retVal.ModifiedBy = ModifiedBy;
            retVal.ModifiedDate = ModifiedDate;

            retVal.SemanticUrl = SemanticUrl;
            retVal.PageTitle = PageTitle;
            retVal.MetaDescription = MetaDescription;
            retVal.ImageAltDescription = ImageAltDescription;
            retVal.MetaKeywords = MetaKeywords;
            retVal.ObjectId = ObjectId;
            retVal.ObjectType = ObjectType;
            retVal.IsActive = IsActive;
            retVal.LanguageCode = LanguageCode;
            return retVal;
        }
        #endregion
    }
}
