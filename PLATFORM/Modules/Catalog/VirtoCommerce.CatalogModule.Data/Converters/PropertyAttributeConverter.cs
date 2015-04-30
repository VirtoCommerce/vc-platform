using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class PropertyAttributeConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.PropertyAttribute ToCoreModel(this dataModel.PropertyAttribute dbAttribute, coreModel.Property property)
		{
			if (property == null)
				throw new ArgumentNullException("dbProperty");

			var retVal = new coreModel.PropertyAttribute();
			retVal.InjectFrom(dbAttribute);

			retVal.Name = dbAttribute.PropertyAttributeName;
			retVal.Value = dbAttribute.PropertyAttributeValue;
			retVal.PropertyId = property.Id;
			retVal.Property = property;
			
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static dataModel.PropertyAttribute ToDataModel(this coreModel.PropertyAttribute attribute)
		{
			var retVal = new dataModel.PropertyAttribute();
			var id = retVal.Id;
			retVal.InjectFrom(attribute);
			if(attribute.Id == null)
			{
				retVal.Id = id;
			}

			retVal.PropertyAttributeName = attribute.Name;
			retVal.PropertyAttributeValue = attribute.Value;
			return retVal;
		}


		/// <summary>
		/// Patch CatalogLanguage type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.PropertyAttribute source, dataModel.PropertyAttribute target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<dataModel.PropertyAttribute>(x => x.PropertyAttributeName, x => x.PropertyAttributeValue);
			target.InjectFrom(patchInjectionPolicy, source);
		}

	}

}
