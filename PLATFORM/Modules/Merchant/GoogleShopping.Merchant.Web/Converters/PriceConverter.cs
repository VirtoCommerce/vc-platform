﻿using System;
using System.Globalization;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using googleModel = Google.Apis.ShoppingContent.v2.Data;

namespace GoogleShopping.MerchantModule.Web.Converters
{
    public static class PriceConverter
    {
        public static googleModel.Price ToGoogleModel(this coreModel.Price price)
        {
            var retVal = new googleModel.Price();
            retVal.InjectFrom(price);
            retVal.Value = price.List.ToString(CultureInfo.InvariantCulture);
            retVal.Currency = price.Currency.ToString();
            return retVal;
        }

        public static coreModel.Price ToCoreModel(this googleModel.Price price)
        {
            var retVal = new coreModel.Price();
            retVal.InjectFrom(price);
            retVal.Currency = price.Currency;
            return retVal;
        }
    }
}