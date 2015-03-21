using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class ProductAssociationConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.ProductAssociation ToModuleModel(this foundation.Association dbAssociation, module.CatalogProduct associatedProduct)
		{
			if (dbAssociation == null)
				throw new ArgumentNullException("dbAssociation");

			var retVal = new module.ProductAssociation
			{
				Name = dbAssociation.AssociationGroup.Name,
				Description = dbAssociation.AssociationGroup.Description,
				Priority = dbAssociation.Priority,
				AssociatedProductId = associatedProduct.Id,
                Type = dbAssociation.AssociationType,
				AssociatedProduct = associatedProduct
			};
			return retVal;
		}

		

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.Association ToFoundation(this module.ProductAssociation association)
		{
			if (association == null)
				throw new ArgumentNullException("association");

			var retVal = new foundation.Association
			{
				ItemId = association.AssociatedProductId,
				Priority = association.Priority,
				AssociationType = association.Type ?? "optional"
			};
			return retVal;
		}

	
		/// <summary>
		/// Patch AssociationGroup type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.AssociationGroup source, foundation.AssociationGroup target)
		{
			//Simply properties patch
			if (source.Name != null)
				target.Name = source.Name;
			if (source.Description != null)
				target.Description = source.Description;

			if (!source.Associations.IsNullCollection())
			{
				source.Associations.Patch(target.Associations, new AssociationComparer(),
										 (sourceAssociation, targetAssociation) => sourceAssociation.Patch(targetAssociation));
			}
		}
		/// <summary>
		/// Patch Association type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.Association source, foundation.Association target)
		{
			if (source.Priority != target.Priority)
				target.Priority = source.Priority;
		}
	}

	public class AssociationGroupComparer : IEqualityComparer<foundation.AssociationGroup>
	{
		#region IEqualityComparer<AssociationGroup> Members

		public bool Equals(foundation.AssociationGroup x, foundation.AssociationGroup y)
		{
			var retVal = x.Name == y.Name;
			return retVal;
		}

		public int GetHashCode(foundation.AssociationGroup obj)
		{
			var retVal = obj.Name.GetHashCode();
			return retVal;
		}

		#endregion
	}

	public class AssociationComparer : IEqualityComparer<foundation.Association>
	{
		#region IEqualityComparer<Association> Members

		public bool Equals(foundation.Association x, foundation.Association y)
		{
			var retVal = x.ItemId == y.ItemId;
		
			return retVal;
		}

		public int GetHashCode(foundation.Association obj)
		{
			var retVal = obj.ItemId.GetHashCode();
			return retVal;
		}

		#endregion
	}

}
