using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class ContentItemConverter
	{
		public static webModel.DynamicContentItem ToWebModel(this coreModel.DynamicContentItem content)
		{
			var retVal = new webModel.DynamicContentItem();
			retVal.InjectFrom(content);
		
		
			return retVal;
		}

	
	
	}
}
