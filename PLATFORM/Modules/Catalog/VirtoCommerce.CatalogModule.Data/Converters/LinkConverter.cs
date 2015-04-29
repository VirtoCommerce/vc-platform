using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class LinkConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.CategoryLink ToModuleModel(this foundation.CategoryItemRelation categoryItemRelation)
		{
			if (categoryItemRelation == null)
				throw new ArgumentNullException("categoryItemRelation");

			var retVal = new module.CategoryLink
			{
				 CategoryId = categoryItemRelation.CategoryId,
				 CatalogId = categoryItemRelation.CatalogId
			};
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static module.CategoryLink ToModuleModel(this foundation.LinkedCategory linkedCategory, module.Category category)
		{
			if (linkedCategory == null)
				throw new ArgumentNullException("linkedCategory");

			var retVal = new module.CategoryLink();
		
			retVal.CategoryId = linkedCategory.LinkedCategoryId;
			retVal.CatalogId = linkedCategory.LinkedCatalogId;
		
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.CategoryItemRelation ToFoundation(this module.CategoryLink categoryLink, module.CatalogProduct product)
		{
			var retVal = new foundation.CategoryItemRelation
			{
				 CategoryId = categoryLink.CategoryId,
				 ItemId = product.Id,
				 CatalogId = categoryLink.CatalogId,
				 Priority = 100
			};
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.LinkedCategory ToFoundation(this module.CategoryLink categoryLink, module.Category category)
		{
			var retVal = new foundation.LinkedCategory
			{
				CatalogId = category.CatalogId,
				LinkedCategoryId = categoryLink.CategoryId,
				LinkedCatalogId = categoryLink.CatalogId,
				ParentCategoryId = category.Id,
				IsActive = true,
				Code = Guid.NewGuid().ToString().Substring(0, 10),
			};
			return retVal;
		}

		/// <summary>
		/// Patch LinkedCategory type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.LinkedCategory source, foundation.LinkedCategory target)
		{
			//Nothing todo. Because we not support change  link
		}

		/// <summary>
		/// Patch LinkedCategory type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.CategoryItemRelation source, foundation.CategoryItemRelation target)
		{
			//Nothing todo. Because we not support change link
		}

	}

	public class CategoryItemRelationComparer : IEqualityComparer<foundation.CategoryItemRelation>
	{
		#region IEqualityComparer<CategoryItemRelation> Members

		public bool Equals(foundation.CategoryItemRelation x, foundation.CategoryItemRelation y)
		{
			var retVal = x.CatalogId == y.CatalogId;
			
			if (retVal && x.CategoryId != null)
			{
				retVal = x.CategoryId == y.CategoryId;
			}

			return retVal;
		}

		public int GetHashCode(foundation.CategoryItemRelation obj)
		{
			var retVal = obj.CatalogId.GetHashCode();
			if (obj.CategoryId != null)
			{
				retVal = obj.CategoryId.GetHashCode();
			}
			return retVal;
		}

		#endregion
	}

	public class LinkedCategoryComparer : IEqualityComparer<foundation.LinkedCategory>
	{
		#region IEqualityComparer<LinkedCategory> Members

		public bool Equals(foundation.LinkedCategory x, foundation.LinkedCategory y)
		{
			var retVal = x.LinkedCatalogId == y.LinkedCatalogId;;
			if(retVal && x.LinkedCategoryId != null)
			{
				retVal = x.LinkedCategoryId == y.LinkedCategoryId;
			}

			return retVal;
		}

		public int GetHashCode(foundation.LinkedCategory obj)
		{
			var retVal = obj.LinkedCatalogId.GetHashCode();
			if(obj.LinkedCategoryId != null)
			{
				retVal = obj.LinkedCategoryId.GetHashCode();
			}
			return retVal;
		}

		#endregion
	}

}
