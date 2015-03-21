using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class CatalogLanguageConverter
	{
		public static webModel.CatalogLanguage ToWebModel(this moduleModel.CatalogLanguage language)
		{
			var result = new webModel.CatalogLanguage();
			result.InjectFrom(language);
			return result;
		}


		public static moduleModel.CatalogLanguage ToModuleModel(this webModel.CatalogLanguage language)
		{
			var result = new moduleModel.CatalogLanguage();
			result.InjectFrom(language);
			return result;
		}

	}
}
