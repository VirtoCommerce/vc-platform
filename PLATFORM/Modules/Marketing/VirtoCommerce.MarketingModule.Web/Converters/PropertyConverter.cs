using Omu.ValueInjecter;
using System.Linq;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
	public static class PropertyConverter
	{
		public static webModel.Property ToWebModel(this coreModel.Property property)
		{
			var retVal = new webModel.Property();
			retVal.InjectFrom(property);
			return retVal;
		}

		public static coreModel.Property ToCoreModel(this webModel.Property property)
		{
			var retVal = new coreModel.Property();
			retVal.InjectFrom(property);
			return retVal;
		}
	
	}
}
