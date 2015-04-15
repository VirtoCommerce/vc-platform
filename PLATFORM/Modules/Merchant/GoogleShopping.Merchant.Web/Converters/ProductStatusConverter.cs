using System;
using System.Linq;
using GoogleShopping.MerchantModule.Web.Model;
using Omu.ValueInjecter;
using moduleModel = VirtoCommerce.Domain.Catalog.Model;
using googleModel = Google.Apis.ShoppingContent.v2.Data;

namespace GoogleShopping.MerchantModule.Web.Converters
{
    public static class ProductStatusConverter
    {
        public static ProductStatus ToModuleModel(this googleModel.ProductStatus input)
        {
            var retVal = new ProductStatus();
            retVal.InjectFrom(input);
            DateTime gExpirationDate;
            if (DateTime.TryParse(input.GoogleExpirationDate, out gExpirationDate))
                retVal.ExpirationDate = gExpirationDate;
            retVal.ProductId = input.ProductId.Split(':').Last();

            return retVal;
        }
    }
}