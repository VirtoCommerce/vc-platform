using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Services;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	public sealed class DynamicContentEvaluationContext : BaseEvaluationContext, IDynamicContentEvaluationContext
	{
		#region IDynamicContentEvaluationContext Members

		public string CustomerId { get; set; }
		public DateTime CurrentDate { get; set; }
		public string ContentPlace { get; set; }

		#endregion

		public bool IsShopperInCategory(string categoryId)
		{
            var id = GetStringValue(ContextFieldConstants.CategoryId);
            if (!String.IsNullOrEmpty(id))
            {
                return string.Equals(id, categoryId, StringComparison.Ordinal);
            }

			return false;
		}

		public bool IsShopperInStore(string storeId)
		{
            var id = GetStringValue(ContextFieldConstants.StoreId);
            if (!String.IsNullOrEmpty(id))
            {
                return string.Equals(id, storeId, StringComparison.Ordinal);
            }

			return false;
		}

        public bool IsShopperInLanguage(string languageId)
        {
            var id = GetStringValue(ContextFieldConstants.Language);
            if (!String.IsNullOrEmpty(id))
            {
                return string.Equals(id, languageId, StringComparison.Ordinal);
            }

            return false;
        }

		public bool IsShopperInCategoryOrSubcategories(string categoryId)
		{
		    if (IsShopperInCategory(categoryId))
		    {
		        return true;
		    }

            //Check for sub categories
            var outline = GetStringValue(ContextFieldConstants.CategoryOutline);
            if (!String.IsNullOrEmpty(outline))
		    {
                return CatalogOutlineBuilder.GetCategoriesHierarchy(outline).Any(x => categoryId.Equals(x, StringComparison.OrdinalIgnoreCase));
		    }

		    return false;
		}

		public decimal CartTotal
		{
			get
			{
                var total = GetDecimalValue(ContextFieldConstants.CartTotal);

                if (total.HasValue)
                {
                    return total.Value;
                }

				return 0;
			}
		}
	}
}
