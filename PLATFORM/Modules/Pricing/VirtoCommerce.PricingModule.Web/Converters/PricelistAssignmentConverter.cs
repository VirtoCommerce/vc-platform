using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using webModel = VirtoCommerce.PricingModule.Web.Model;
using coreCatalogModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.PricingModule.Web.Converters
{
	public static class PricelistAssignmentConverter
	{
		public static webModel.PricelistAssignment ToWebModel(this coreModel.PricelistAssignment assignment, coreCatalogModel.Catalog[] catalogs = null)
		{
			var retVal = new webModel.PricelistAssignment();
			retVal.InjectFrom(assignment);
		
			if(catalogs != null)
			{
				var catalog = catalogs.FirstOrDefault(x => x.Id == assignment.CatalogId);
				if(catalog != null)
				{
					retVal.CatalogName = catalog.Name;
				}
			}
			return retVal;
		}

		public static coreModel.PricelistAssignment ToCoreModel(this webModel.PricelistAssignment assignment)
		{
			var retVal = new coreModel.PricelistAssignment();
			retVal.InjectFrom(assignment);
			return retVal;
		}


	}
}
