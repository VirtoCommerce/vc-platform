using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class ResponseGroupConverter
    {
        #region Public Methods and Operators

        public static moduleModel.ItemResponseGroup ToModuleModel(this webModel.ItemResponseGroups respGroup)
        {
            var retVal = moduleModel.ItemResponseGroup.ItemInfo;
            if ((respGroup & webModel.ItemResponseGroups.ItemAssets) == webModel.ItemResponseGroups.ItemAssets)
            {
                retVal |= moduleModel.ItemResponseGroup.ItemAssets;
            }
            if ((respGroup & webModel.ItemResponseGroups.ItemAssociations)
                == webModel.ItemResponseGroups.ItemAssociations)
            {
                retVal |= moduleModel.ItemResponseGroup.ItemAssociations;
            }
            if ((respGroup & webModel.ItemResponseGroups.ItemEditorialReviews)
                == webModel.ItemResponseGroups.ItemEditorialReviews)
            {
                retVal |= moduleModel.ItemResponseGroup.ItemEditorialReviews;
            }
            return retVal;
        }

        #endregion
    }
}
