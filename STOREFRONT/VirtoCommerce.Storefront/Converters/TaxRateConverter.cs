using Omu.ValueInjecter;
using System;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class TaxRateConverter
    {
        public static TaxRate ToWebModel(this VirtoCommerceDomainTaxModelTaxRate serviceModel, Currency currency)
        {
            var retVal = new TaxRate(currency);
            retVal.Rate = new Money(serviceModel.Rate.Value, currency);
            if (serviceModel.Line != null)
            {
                retVal.Line = new TaxLine(currency);
                retVal.Line.InjectFrom(serviceModel.Line);
                retVal.Line.Amount = new Money(serviceModel.Line.Amount.Value, currency);
                retVal.Line.Price = new Money(serviceModel.Line.Price.Value, currency);
            }

            return retVal;
        }

    }
}