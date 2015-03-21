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

			var retVal = new module.PropertyAttribute
					{
						Name = dbAttribute.PropertyAttributeName,
						Value = dbAttribute.PropertyAttributeValue,
						PropertyId = property.Id,
						Property = property
					};
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.PropertyAttribute ToFoundation(this module.PropertyAttribute attribute)
		{
			var retVal = new foundation.PropertyAttribute
			{
				PropertyAttributeName = attribute.Name,
				PropertyAttributeValue = attribute.Value,
				PropertyId = attribute.PropertyId
			};
			retVal.PropertyAttributeId = retVal.GenerateNewKey();
			if (attribute.Id != null)
				retVal.PropertyAttributeId = attribute.Id;
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

			//Simply propertie spatch
			if (source.PropertyAttributeName != null)
				target.PropertyAttributeName = source.PropertyAttributeName;
			if (source.PropertyAttributeValue != null)
				target.PropertyAttributeValue = source.PropertyAttributeValue;
		}

	}

	public class PropertyAttributeComparer : IEqualityComparer<foundation.PropertyAttribute>
	{
		#region IEqualityComparer<CatalogLanguage> Members

		public bool Equals(foundation.PropertyAttribute x, foundation.PropertyAttribute y)
		{
			return x.PropertyAttributeId == y.PropertyAttributeId;
		}

		public int GetHashCode(foundation.PropertyAttribute obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}

}
