using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OrderLineItemConverter
    {
        public static LineItem ToWebModel(this VirtoCommerceOrderModuleWebModelLineItem lineItem)
        {
            var webModel = new LineItem();

            var currency = new Currency(EnumUtility.SafeParse(lineItem.Currency, CurrencyCodes.USD));

            webModel.InjectFrom(lineItem);

            webModel.Currency = currency;
            webModel.DiscountAmount = new Money(lineItem.DiscountAmount ?? 0, currency.Code);

            if (lineItem.DynamicProperties != null)
            {
                webModel.DynamicProperties = lineItem.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            webModel.Price = new Money(lineItem.Price ?? 0, currency.Code);
            webModel.Tax = new Money(lineItem.Tax ?? 0, currency.Code);

            if (lineItem.TaxDetails != null)
            {
                webModel.TaxDetails = lineItem.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return webModel;
        }
    }
}