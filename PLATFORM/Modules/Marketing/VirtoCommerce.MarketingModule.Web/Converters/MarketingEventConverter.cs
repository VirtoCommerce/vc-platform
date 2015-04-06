using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class MarketingEventConverter
	{
		public static coreModel.MarketingEvent ToCoreModel(this webModel.MarketingEvent marketingEvent)
		{
			var retVal = new coreModel.MarketingEvent();
			retVal.InjectFrom(marketingEvent);
		
			return retVal;
		}

	}
}
