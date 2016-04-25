using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Commerce.Model
{
    public class SeoInfo : AuditableEntity, ILanguageSupport, ICloneable
    {
        public SeoInfo()
        {
            IsActive = true;
        }
        public string Name { get; set; }
        /// <summary>
        /// Slug
        /// </summary>
        public string SemanticUrl { get; set; }
        /// <summary>
        /// head title tag content
        /// </summary>
        public string PageTitle { get; set; }
        /// <summary>
        /// <meta name="description" />
        /// </summary>
        public string MetaDescription { get; set; }
        public string ImageAltDescription { get; set; }
        /// <summary>
        /// <meta name="keywords" />
        /// </summary>
        public string MetaKeywords { get; set; }
        /// <summary>
        /// Tenant StoreId which SEO defined
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// SEO related object id
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// SEO related object type name
        /// </summary>
        public string ObjectType { get; set; }
        /// <summary>
        /// Active/Inactive
        /// </summary>
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

            retVal.StoreId = StoreId;
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
