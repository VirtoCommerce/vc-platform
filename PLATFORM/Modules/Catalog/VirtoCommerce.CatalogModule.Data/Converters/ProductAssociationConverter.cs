using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
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
		public static coreModel.ProductAssociation ToCoreModel(this dataModel.Association dbAssociation, coreModel.CatalogProduct associatedProduct)
		{
			if (dbAssociation == null)
				throw new ArgumentNullException("dbAssociation");

			var retVal = new coreModel.ProductAssociation
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
		public static dataModel.Association ToDataModel(this coreModel.ProductAssociation association)
		{
			if (association == null)
				throw new ArgumentNullException("association");

			var retVal = new dataModel.Association
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
		public static void Patch(this dataModel.AssociationGroup source, dataModel.AssociationGroup target)
		{
			var patchInjectionPolicy = new PatchInjection<dataModel.AssociationGroup>(x => x.Name, x => x.Description);
			target.InjectFrom(patchInjectionPolicy, source);

			if (!source.Associations.IsNullCollection())
			{
				var associationComparer = AnonymousComparer.Create((dataModel.Association x) => x.ItemId);
				source.Associations.Patch(target.Associations, associationComparer,
										 (sourceAssociation, targetAssociation) => sourceAssociation.Patch(targetAssociation));
			}
		}
		/// <summary>
		/// Patch Association type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.Association source, dataModel.Association target)
		{
			var patchInjectionPolicy = new PatchInjection<dataModel.Association>(x => x.Priority);
			target.InjectFrom(patchInjectionPolicy, source);
		}
	}

	
}
