using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
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
		public static coreModel.CategoryLink ToCoreModel(this dataModel.CategoryItemRelation categoryItemRelation)
		{
			if (categoryItemRelation == null)
				throw new ArgumentNullException("categoryItemRelation");

			var retVal = new coreModel.CategoryLink
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
		public static coreModel.CategoryLink ToCoreModel(this dataModel.CategoryRelation linkedCategory, coreModel.Category category)
		{
			if (linkedCategory == null)
				throw new ArgumentNullException("linkedCategory");

			var retVal = new coreModel.CategoryLink();
		
			retVal.CategoryId = linkedCategory.TargetCategoryId;
			retVal.CatalogId = linkedCategory.TargetCatalogId;
		
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static dataModel.CategoryItemRelation ToDataModel(this coreModel.CategoryLink categoryLink, coreModel.CatalogProduct product)
		{
			var retVal = new dataModel.CategoryItemRelation
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
		public static dataModel.CategoryRelation ToDataModel(this coreModel.CategoryLink categoryLink, coreModel.Category category)
		{
			var retVal = new dataModel.CategoryRelation
			{
				SourceCategoryId = category.Id,
				TargetCategoryId = categoryLink.CategoryId,
				//Only one relation (category or catalog)
				TargetCatalogId = String.IsNullOrEmpty(categoryLink.CategoryId) ?  categoryLink.CatalogId : null
			};
			return retVal;
		}

		/// <summary>
		/// Patch LinkedCategory type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.CategoryRelation source, dataModel.CategoryRelation target)
		{
			//Nothing todo. Because we not support change  link
		}

		/// <summary>
		/// Patch LinkedCategory type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.CategoryItemRelation source, dataModel.CategoryItemRelation target)
		{
			//Nothing todo. Because we not support change link
		}

	}

	public class CategoryItemRelationComparer : IEqualityComparer<dataModel.CategoryItemRelation>
	{
		#region IEqualityComparer<CategoryItemRelation> Members

		public bool Equals(dataModel.CategoryItemRelation x, dataModel.CategoryItemRelation y)
		{
			var retVal = x.CatalogId == y.CatalogId;
			
			if (retVal && x.CategoryId != null)
			{
				retVal = x.CategoryId == y.CategoryId;
			}

			return retVal;
		}

		public int GetHashCode(dataModel.CategoryItemRelation obj)
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

	public class LinkedCategoryComparer : IEqualityComparer<dataModel.CategoryRelation>
	{
		#region IEqualityComparer<LinkedCategory> Members

		public bool Equals(dataModel.CategoryRelation x, dataModel.CategoryRelation y)
		{
			var retVal = x.TargetCatalogId == y.TargetCatalogId;
			if(retVal && x.TargetCategoryId != null)
			{
				retVal = x.TargetCategoryId == y.TargetCategoryId;
			}

			return retVal;
		}

		public int GetHashCode(dataModel.CategoryRelation obj)
		{
			var retVal = obj.SourceCategoryId.GetHashCode();
			if(obj.TargetCategoryId != null)
			{
				retVal = obj.TargetCategoryId.GetHashCode();
			}
			else if (obj.TargetCatalogId != null)
			{
				retVal = obj.TargetCatalogId.GetHashCode();
			}
			return retVal;
		}

		#endregion
	}

}
