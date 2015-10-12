using Omu.ValueInjecter;
using webModel = VirtoCommerce.StoreModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Tax.Model;
using System.Linq;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class TaxProviderConverter
	{
		public static webModel.TaxProvider ToWebModel(this coreModel.TaxProvider method)
		{
			var retVal = new webModel.TaxProvider();
			retVal.InjectFrom(method);
			if(method.Settings != null)
				retVal.Settings = method.Settings.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.TaxProvider ToCoreModel(this webModel.TaxProvider webTaxProvider, coreModel.TaxProvider taxProvider)
		{
			var retVal = taxProvider;
			retVal.InjectFrom(webTaxProvider);
			if (webTaxProvider.Settings != null)
				retVal.Settings = webTaxProvider.Settings.Select(x => x.ToCoreModel()).ToList();
			return retVal;
		}
	}
}
