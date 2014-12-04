using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web2.Model;

namespace VirtoCommerce.MerchandisingModule.Web2.Converters
{
	public static class ResponseGroupConverter
	{
		public static moduleModel.ItemResponseGroup ToModuleModel(this webModel.ItemResponseGroups respGroup)
		{
			moduleModel.ItemResponseGroup retVal = moduleModel.ItemResponseGroup.ItemInfo;
			if ((respGroup & webModel.ItemResponseGroups.ItemAssets) == webModel.ItemResponseGroups.ItemAssets)
			{
				retVal |= moduleModel.ItemResponseGroup.ItemAssets;
			}
			if ((respGroup & webModel.ItemResponseGroups.ItemAssociations) == webModel.ItemResponseGroups.ItemAssociations)
			{
				retVal |= moduleModel.ItemResponseGroup.ItemAssociations;
			}
			if ((respGroup & webModel.ItemResponseGroups.ItemEditorialReviews) == webModel.ItemResponseGroups.ItemEditorialReviews)
			{
				retVal |= moduleModel.ItemResponseGroup.ItemEditorialReviews;
			}
			return retVal;
		}
	}
}
