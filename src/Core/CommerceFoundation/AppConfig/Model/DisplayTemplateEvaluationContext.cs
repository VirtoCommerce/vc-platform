using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Catalogs;

namespace VirtoCommerce.Foundation.AppConfig.Model
{
	public static class DisplayTemplateEvaluationContextExtension
	{
		public static bool InCategory(this TargetEntry target, string categoryId)
		{
			return target.InCategories(new string[] { categoryId });
		}

		public static bool InCategories(this TargetEntry target, string[] categoryIds)
		{
			categoryIds = categoryIds.Where(x => x != null).ToArray();
			
			if (categoryIds.Count() > 0)
			{
				return IsTargetInCategories(target, categoryIds);
			}

			return true;
		}

		public static bool ExcludeCategories(this TargetEntry target, string[] categoryIds)
		{
			return !IsTargetInCategories(target, categoryIds);
		}
		
		private static bool IsTargetInCategories(TargetEntry target, string[] categoryIds)
		{
			var retVal = CatalogOutlineBuilder.GetCategoriesHierarchy(target.Outline).Any(x => categoryIds.Contains(x, StringComparer.OrdinalIgnoreCase));
			return retVal;
		}
	}

	[DataContract]
	public sealed class DisplayTemplateEvaluationContext : IDisplayTemplateEvaluationContext
	{
		#region IDisplayTemplateEvaluationContext members
		public TargetTypes TargetType { get; set; }
		#endregion

		public object ContextObject { get; set; }

		public bool IsCurrentItem(string itemId)
		{
			var id = GetStringValue(ContextFieldConstants.ItemId);
			if (!String.IsNullOrEmpty(id))
			{
				return string.Equals(id, itemId, StringComparison.Ordinal);
			}

			return false;
		}

		public bool IsCurrentCategory(string categoryId, string[] excludingCategoryIds, string[] excludingItemIds)
		{
			var id = GetStringValue(ContextFieldConstants.CategoryId);
			if (!String.IsNullOrEmpty(id))
			{
				return string.Equals(id, categoryId, StringComparison.Ordinal);
			}

			return false;
		}

		public TargetEntry TargetEntry
		{
			get
			{
				return this.Context[ContextFieldConstants.TargetEntry] as TargetEntry;
			}
		}

		public bool IsCategorySubcategoryOf(string categoryId, string[] excludingCategoryIds)
		{
			var retVal = TargetEntry.InCategories(new string[] { categoryId });
			return retVal && TargetEntry.ExcludeCategories(excludingCategoryIds);
		}

		public bool ItemIsInCategory(string categoryId, string[] excludingCategoryIds, string[] excludingItemIds)
		{
			var id = GetStringValue(ContextFieldConstants.ItemId);
			var retVal = false;
			if (!String.IsNullOrEmpty(id) && excludingItemIds.Count() > 0)
			{
				retVal = !excludingItemIds.Any(item => string.Equals(id, item, StringComparison.Ordinal));
			}

			if (retVal)
			{
				retVal = TargetEntry.InCategories(new string[] { categoryId });
				return retVal && TargetEntry.ExcludeCategories(excludingCategoryIds);
			}

			return retVal;
		}

		public bool ItemIsInStore(string storeId)
		{
			var id = GetStringValue(ContextFieldConstants.StoreId);
			if (!String.IsNullOrEmpty(id))
			{
				return string.Equals(id, storeId, StringComparison.Ordinal);
			}

			return false;
		}

		public bool CategoryIsInStore(string storeId)
		{
			var id = GetStringValue(ContextFieldConstants.StoreId);
			if (!String.IsNullOrEmpty(id))
			{
				return string.Equals(id, storeId, StringComparison.Ordinal);
			}

			return false;
		}

		public string ItemType
		{
			get
			{
				return GetStringValue(ContextFieldConstants.ItemType);
			}
		}

		public bool ItemPropertyValueIs(string propertyName, string propertyValue)
		{
			return string.Equals(GetPropertyValue(propertyName), propertyValue, StringComparison.Ordinal);
		}

		public string CategoryProperty
		{
			get
			{
				return GetStringValue(ContextFieldConstants.CategoryProperty);
			}
		}

		private IDictionary<string, object> Context
		{
			get
			{
				return ContextObject as IDictionary<string, object>;
			}
		}

		private string GetStringValue(string name)
        {
            if (Context != null && Context.Count > 0 && Context.ContainsKey(name))
            {
                return Convert.ToString(Context[name]);
            }

            return String.Empty;
        }

		private string GetPropertyValue(string name)
		{
			if (TargetEntry != null && TargetEntry.Storage.Count > 0 && TargetEntry.Storage.ContainsKey(name))
			{
				return Convert.ToString(TargetEntry.Storage[name]);
			}

			return String.Empty;
		}     
	}
}
