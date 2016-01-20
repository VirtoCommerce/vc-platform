using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Converters
{
    public static class QuoteItemConverter
    {
        public static QuoteItem ToWebModel(this VirtoCommerceQuoteModuleWebModelQuoteItem serviceModel, Currency currency)
        {
            var webModel = new QuoteItem();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.Currency = currency;
            webModel.ListPrice = new Money(serviceModel.ListPrice ?? 0, currency);
            webModel.SalePrice = new Money(serviceModel.SalePrice ?? 0, currency);

            if (serviceModel.ProposalPrices != null)
            {
                webModel.ProposalPrices = serviceModel.ProposalPrices.Select(pp => pp.ToWebModel(currency)).ToList();
            }

            if (serviceModel.SelectedTierPrice != null)
            {
                webModel.SelectedTierPrice = serviceModel.SelectedTierPrice.ToWebModel(currency);
            }

            return webModel;
        }
    }
}