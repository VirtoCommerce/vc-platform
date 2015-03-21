using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.PackagingModule.Model;
using webModel = VirtoCommerce.PackagingModule.Web.Model;

namespace VirtoCommerce.PackagingModule.Web.Converters
{
	public static class ModuleDecriptorConverter
	{
		public static webModel.ModuleDescriptor ToWebModel(this moduleModel.ModuleDescriptor descriptor)
		{
			webModel.ModuleDescriptor retVal = new webModel.ModuleDescriptor();
			retVal.InjectFrom(descriptor);
			return retVal;
		}

		public static moduleModel.ModuleDescriptor ToModuleModel(this webModel.ModuleDescriptor descriptor)
		{
			var retVal = new moduleModel.ModuleDescriptor();
			return retVal;
		}


	}
}
