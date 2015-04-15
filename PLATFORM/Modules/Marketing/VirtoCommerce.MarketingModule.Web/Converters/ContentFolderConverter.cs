using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class ContentFolderConverter
	{
		public static webModel.DynamicContentFolder ToWebModel(this coreModel.DynamicContentFolder folder)
		{
			var retVal = new webModel.DynamicContentFolder();
			retVal.InjectFrom(folder);
			return retVal;
		}

		public static coreModel.DynamicContentFolder ToCoreModel(this webModel.DynamicContentFolder folder)
		{
			var retVal = new coreModel.DynamicContentFolder();
			retVal.InjectFrom(folder);
			return retVal;
		}

	}
}
