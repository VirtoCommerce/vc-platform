using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Common;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class PromotionConverter
	{
		public static webModel.Promotion ToWebModel(this coreModel.Promotion promotion)
		{
			var retVal = new webModel.Promotion();
			retVal.InjectFrom(promotion);
			return retVal;
		
		}

	
	}
}
