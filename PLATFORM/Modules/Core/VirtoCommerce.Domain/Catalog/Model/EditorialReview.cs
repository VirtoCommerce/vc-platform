
using System;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
	public class EditorialReview : AuditableEntity, ILanguageSupport, ICloneable, IInheritable
	{
		public string Content { get; set; }
		public string ReviewType { get; set; }

		#region ILanguageSupport Members
        public string LanguageCode { get; set; }
        #endregion

        #region IInheritable Members
        public bool IsInherited { get; set; }
        #endregion

        #region ICloneable members
        public object Clone()
        {
            var retVal = new EditorialReview();
            retVal.Id = Id;
            retVal.CreatedBy = CreatedBy;
            retVal.CreatedDate = CreatedDate;
            retVal.ModifiedBy = ModifiedBy;
            retVal.ModifiedDate = ModifiedDate;

            retVal.Content = Content;
            retVal.ReviewType = ReviewType;
            retVal.LanguageCode = LanguageCode;
            retVal.IsInherited = IsInherited;
            return retVal;
        }
        #endregion
    }
}
