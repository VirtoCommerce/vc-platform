using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webModel = VirtoCommerce.PackagingModule.Web.Model;
using moduleModel = VirtoCommerce.PackagingModule.Model;

namespace VirtoCommerce.PackagingModule.Web.Converters
{
	public static class ProgressMessageConverter
	{
		public static webModel.ProgressMessage ToWebModel(this moduleModel.ProgressMessage message)
		{
			webModel.ProgressMessage retVal = new webModel.ProgressMessage();
			retVal.Message = message.Message;
			retVal.Level = message.Level.ToString();
			return retVal;
		}
	}
}
