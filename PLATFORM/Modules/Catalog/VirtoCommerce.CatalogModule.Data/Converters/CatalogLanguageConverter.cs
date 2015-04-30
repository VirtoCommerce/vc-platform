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
	public static class CatalogLanguageConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.CatalogLanguage ToCoreModel(this dataModel.CatalogLanguage dbLanguage, coreModel.Catalog catalog)
		{
			var retVal = new coreModel.CatalogLanguage();
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
		public static dataModel.CatalogLanguage ToDataModel(this coreModel.CatalogLanguage language)
		{
			var retVal = new dataModel.CatalogLanguage
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
		public static void Patch(this dataModel.CatalogLanguage source, dataModel.CatalogLanguage target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<dataModel.CatalogLanguage>(x => x.Language);
			target.InjectFrom(patchInjectionPolicy, source);

		}

	}

	
}
