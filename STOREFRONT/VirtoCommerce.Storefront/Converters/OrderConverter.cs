using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class OrderConverter
    {
        public static CustomerOrder ToWebModel(this VirtoCommerceOrderModuleWebModelCustomerOrder order)
        {
            var webModel = new CustomerOrder();

            var currency = new Currency(EnumUtility.SafeParse(order.Currency, CurrencyCodes.USD));

            webModel.InjectFrom(order);

            if (order.Addresses != null)
            {
                webModel.Addresses = order.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            if (order.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = order.ChildrenOperations.Select(co => co.ToWebModel()).ToList();
            }

            webModel.Currency = currency;

            webModel.DiscountAmount = new Money(order.DiscountAmount ?? 0, currency.Code);

            if (order.DynamicProperties != null)
            {
                webModel.DynamicProperties = order.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            if (order.InPayments != null)
            {
                webModel.InPayments = order.InPayments.Select(p => p.ToWebModel()).ToList();
            }

            if (order.Items != null)
            {
                webModel.Items = order.Items.Select(i => i.ToWebModel()).ToList();
            }

            if (order.Shipments != null)
            {
                webModel.Shipments = order.Shipments.Select(s => s.ToWebModel()).ToList();
            }

            webModel.Sum = new Money(order.Sum ?? 0, currency.Code);
            webModel.Tax = new Money(order.Tax ?? 0, currency.Code);

            if (order.TaxDetails != null)
            {
                webModel.TaxDetails = order.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return webModel;
        }
    }
}