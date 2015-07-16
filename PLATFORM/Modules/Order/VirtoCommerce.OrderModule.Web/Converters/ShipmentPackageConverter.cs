using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class ShipmentPackageConverter
	{
		public static webModel.ShipmentPackage ToWebModel(this coreModel.ShipmentPackage package)
		{
			var retVal = new webModel.ShipmentPackage();
			retVal.InjectFrom(package);

			if (package.Items != null)
			{
				retVal.Items = package.Items.Select(x => x.ToWebModel()).ToList();
			}
			return retVal;
		}

		public static coreModel.ShipmentPackage ToCoreModel(this webModel.ShipmentPackage package)
		{
			var retVal = new coreModel.ShipmentPackage();
			retVal.InjectFrom(package);

			if (package.Items != null)
			{
				retVal.Items = package.Items.Select(x => x.ToCoreModel()).ToList();
			}
		
			return retVal;
		}


	}
}