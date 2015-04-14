using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class DynamicContentPlaceConverter
	{
		public static webModel.DynamicContentPlace ToWebModel(this coreModel.DynamicContentPlace place)
		{
			var retVal = new webModel.DynamicContentPlace();
			retVal.InjectFrom(place);
			if (place.Folder != null)
			{
				retVal.Outline = place.Folder.Outline;
				retVal.Path = place.Folder.Path;
			}
			return retVal;
		}

		public static coreModel.DynamicContentPlace ToCoreModel(this webModel.DynamicContentPlace place)
		{
			var retVal = new coreModel.DynamicContentPlace();
			retVal.InjectFrom(place);
			return retVal;
		}
	
	}
}
