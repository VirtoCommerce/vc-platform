﻿using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class TierPriceConverter
    {
        public static TierPrice ToWebModel(this VirtoCommerceQuoteModuleWebModelTierPrice serviceModel, Currency currency)
        {
            var webModel = new TierPrice();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.Price = new Money(serviceModel.Price ?? 0, currency);

            return webModel;
        }
    }
}