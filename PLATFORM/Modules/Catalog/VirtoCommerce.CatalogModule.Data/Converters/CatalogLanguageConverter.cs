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
	public static class CatalogLanguageConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.CatalogLanguage ToModuleModel(this foundation.CatalogLanguage dbLanguage, module.Catalog catalog)
		{
			var retVal = new module.CatalogLanguage
					{
						CatalogId = catalog.Id,
						LanguageCode = dbLanguage.Language
					};
			if (dbLanguage.CatalogLanguageId != null)
				retVal.Id = dbLanguage.CatalogLanguageId;
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="catalog"></param>
		/// <returns></returns>
		public static foundation.CatalogLanguage ToFoundation(this module.CatalogLanguage language)
		{
			var retVal = new foundation.CatalogLanguage
			{
				Language = language.LanguageCode,
			};

			return retVal;
		}

		
		/// <summary>
		/// Patch CatalogLanguage type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.CatalogLanguage source, foundation.CatalogLanguage target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			//Simply propertie spatch
			if (source.Language != null)
				target.Language = source.Language;
		}

	}

	public class CatalogLanguageComparer : IEqualityComparer<foundation.CatalogLanguage>
	{
		#region IEqualityComparer<CatalogLanguage> Members

		public bool Equals(foundation.CatalogLanguage x, foundation.CatalogLanguage y)
		{
			return x.Language == y.Language;
		}

		public int GetHashCode(foundation.CatalogLanguage obj)
		{
			return obj.GetHashCode();
		}

		#endregion
	}
}
