using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

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
			var patchInjectionPolicy = new PatchInjection<foundation.AssociationGroup>(x => x.Name, x => x.Description);
			target.InjectFrom(patchInjectionPolicy, source);

			if (!source.Associations.IsNullCollection())
			{
				var associationComparer = AnonymousComparer.Create((foundation.Association x) => x.ItemId);
				source.Associations.Patch(target.Associations, associationComparer,
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
			var patchInjectionPolicy = new PatchInjection<foundation.Association>(x => x.Priority);
			target.InjectFrom(patchInjectionPolicy, source);
		}
	}

	
}
