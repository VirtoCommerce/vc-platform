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
	public static class CatalogLanguageConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static module.CatalogLanguage ToModuleModel(this foundation.CatalogLanguage dbLanguage, module.Catalog catalog)
		{
			var retVal = new module.CatalogLanguage();
			retVal.InjectFrom(dbLanguage);

			retVal.CatalogId = catalog.Id;
			retVal.LanguageCode = dbLanguage.Language;

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

			var patchInjectionPolicy = new PatchInjection<foundation.CatalogLanguage>(x => x.Language);
			target.InjectFrom(patchInjectionPolicy, source);

		}

	}

	
}
