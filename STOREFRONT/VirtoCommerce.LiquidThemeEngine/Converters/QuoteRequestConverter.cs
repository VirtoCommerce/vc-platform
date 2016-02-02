using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.LiquidThemeEngine.Objects;
using System.Collections.Generic;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class QuoteRequestConverter
    {
        public static QuoteRequest ToShopifyModel(this Storefront.Model.Quote.QuoteRequest storefrontModel)
        {
            var shopifyModel = new QuoteRequest();

            shopifyModel.InjectFrom<NullableAndEnumValueInjecter>(storefrontModel);

            shopifyModel.Addresses = new List<Address>();
            foreach (var address in storefrontModel.Addresses)
            {
                shopifyModel.Addresses.Add(address.ToShopifyModel());
            }

            shopifyModel.Attachments = new List<Attachment>();
            foreach (var attachment in storefrontModel.Attachments)
            {
                shopifyModel.Attachments.Add(attachment.ToShopifyModel());
            }

            if (storefrontModel.Coupon != null)
            {
                shopifyModel.Coupon = storefrontModel.Coupon.Code;
            }

            shopifyModel.Currency = storefrontModel.Currency.ToShopifyModel();

            shopifyModel.Items = new List<QuoteItem>();
            foreach (var quoteItem in storefrontModel.Items)
            {
                shopifyModel.Items.Add(quoteItem.ToShopifyModel());
            }

            shopifyModel.Language = storefrontModel.Language.ToShopifyModel();
            shopifyModel.ManualRelDiscountAmount = storefrontModel.ManualRelDiscountAmount.Amount;
            shopifyModel.ManualShippingTotal = storefrontModel.ManualShippingTotal.Amount;
            shopifyModel.ManualSubTotal = storefrontModel.ManualSubTotal.Amount;

            if (storefrontModel.ShipmentMethod != null)
            {
                shopifyModel.ShipmentMethod = storefrontModel.ShipmentMethod.ToShopifyModel();
            }

            shopifyModel.TaxDetails = new List<TaxLine>();
            foreach (var taxDetail in storefrontModel.TaxDetails)
            {
                shopifyModel.TaxDetails.Add(taxDetail.ToShopifyModel());
            }

            if (storefrontModel.Totals != null)
            {
                shopifyModel.Totals = storefrontModel.Totals.ToShopifyModel();
            }

            return shopifyModel;
        }

        public static QuoteItem ToShopifyModel(this Storefront.Model.Quote.QuoteItem storefrontModel)
        {
            var shopifyModel = new QuoteItem();

            shopifyModel.InjectFrom<NullableAndEnumValueInjecter>(storefrontModel);

            shopifyModel.Currency = storefrontModel.Currency.ToShopifyModel();
            shopifyModel.ListPrice = storefrontModel.ListPrice.Amount * 100;

            shopifyModel.ProposalPrices = new List<TierPrice>();
            foreach (var proposalPrice in storefrontModel.ProposalPrices)
            {
                shopifyModel.ProposalPrices.Add(proposalPrice.ToShopifyModel());
            }

            shopifyModel.SalePrice = storefrontModel.SalePrice.Amount * 100;

            if (storefrontModel.SelectedTierPrice != null)
            {
                shopifyModel.SelectedTierPrice = storefrontModel.SelectedTierPrice.ToShopifyModel();
            }

            return shopifyModel;
        }

        public static QuoteRequestTotals ToShopifyModel(this Storefront.Model.Quote.QuoteRequestTotals storefrontModel)
        {
            var shopifyModel = new QuoteRequestTotals();

            shopifyModel.AdjustmentQuoteExlTax = storefrontModel.AdjustmentQuoteExlTax.Amount * 100;
            shopifyModel.DiscountTotal = storefrontModel.DiscountTotal.Amount * 100;
            shopifyModel.GrandTotalExlTax = storefrontModel.GrandTotalExlTax.Amount * 100;
            shopifyModel.GrandTotalInclTax = storefrontModel.GrandTotalInclTax.Amount * 100;
            shopifyModel.OriginalSubTotalExlTax = storefrontModel.OriginalSubTotalExlTax.Amount * 100;
            shopifyModel.ShippingTotal = storefrontModel.ShippingTotal.Amount * 100;
            shopifyModel.SubTotalExlTax = storefrontModel.SubTotalExlTax.Amount * 100;
            shopifyModel.TaxTotal = storefrontModel.TaxTotal.Amount * 100;

            return shopifyModel;
        }
    }
}