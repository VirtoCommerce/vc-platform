using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
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
		public static module.PropertyAttribute ToModuleModel(this foundation.PropertyAttribute dbAttribute, module.Property property)
		{
			if (property == null)
				throw new ArgumentNullException("dbProperty");

			var retVal = new module.PropertyAttribute();
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
		public static foundation.PropertyAttribute ToFoundation(this module.PropertyAttribute attribute)
		{
			var retVal = new foundation.PropertyAttribute();
			retVal.InjectFrom(attribute);

			retVal.PropertyAttributeName = attribute.Name;
			retVal.PropertyAttributeValue = attribute.Value;
			return retVal;
		}


		/// <summary>
		/// Patch CatalogLanguage type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.PropertyAttribute source, foundation.PropertyAttribute target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<foundation.PropertyAttribute>(x => x.PropertyAttributeName, x => x.PropertyAttributeValue);
			target.InjectFrom(patchInjectionPolicy, source);
		}

	}

}
