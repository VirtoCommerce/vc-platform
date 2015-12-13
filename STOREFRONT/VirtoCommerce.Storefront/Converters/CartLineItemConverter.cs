using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartLineItemConverter
    {
        public static LineItem ToLineItem(this Product product, Language language, int quantity)
        {
            var lineItemWebModel = new LineItem(product.Price.Currency, language);

            lineItemWebModel.InjectFrom(product);

            var currency = product.Price.Currency;

            lineItemWebModel.ExtendedPrice = product.Price.SalePrice * quantity;
            lineItemWebModel.ImageUrl = product.PrimaryImage.Url;
            lineItemWebModel.ListPrice = product.Price.ListPrice;
            lineItemWebModel.PlacedPrice = product.Price.SalePrice;
            lineItemWebModel.ProductId = product.Id;
            lineItemWebModel.Quantity = quantity;
            lineItemWebModel.SalePrice = product.Price.SalePrice;
            lineItemWebModel.TaxTotal = new Money(0, currency.Code);
            lineItemWebModel.ThumbnailImageUrl = product.PrimaryImage.Url;

            return lineItemWebModel;
        }

        public static LineItem ToWebModel(this VirtoCommerceCartModuleWebModelLineItem serviceModel, Currency currency, Language language)
        {
            var webModel = new LineItem(currency, language);

            webModel.InjectFrom(serviceModel);

            if (serviceModel.Discounts != null)
            {
                webModel.Discounts = serviceModel.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            if (serviceModel.TaxDetails != null)
            {
                webModel.TaxDetails = serviceModel.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            if (serviceModel.DynamicProperties != null)
            {
                webModel.DynamicProperties = serviceModel.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            webModel.DiscountTotal = new Money(serviceModel.DiscountTotal ?? 0, currency.Code);
            webModel.ExtendedPrice = new Money(serviceModel.ExtendedPrice ?? 0, currency.Code);
            webModel.IsGift = (bool)serviceModel.IsGift;
            webModel.IsReccuring = (bool)serviceModel.IsReccuring;
            webModel.Length = (decimal)(serviceModel.Length ?? 0);
            webModel.ListPrice = new Money(serviceModel.ListPrice ?? 0, currency.Code);
            webModel.PlacedPrice = new Money(serviceModel.PlacedPrice ?? 0, currency.Code);
            webModel.Quantity = serviceModel.Quantity ?? 0;
            webModel.RequiredShipping = (bool)serviceModel.RequiredShipping;
            webModel.SalePrice = new Money(serviceModel.SalePrice ?? 0, currency.Code);
            webModel.TaxIncluded = (bool)serviceModel.TaxIncluded;
            webModel.Weight = (decimal)(serviceModel.Weight ?? 0);
            webModel.Width = (decimal)(serviceModel.Width ?? 0);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelLineItem ToServiceModel(this LineItem webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelLineItem();

            serviceModel.InjectFrom(webModel);

            serviceModel.Currency = webModel.Currency.Code;
            serviceModel.Discounts = webModel.Discounts.Select(d => d.ToServiceModel()).ToList();
            serviceModel.DiscountTotal = (double)webModel.DiscountTotal.Amount;
            serviceModel.ExtendedPrice = (double)webModel.ExtendedPrice.Amount;
            serviceModel.Height = (double)webModel.Height;
            serviceModel.Length = (double)webModel.Length;
            serviceModel.ListPrice = (double)webModel.ListPrice.Amount;
            serviceModel.PlacedPrice = (double)webModel.PlacedPrice.Amount;
            serviceModel.Quantity = webModel.Quantity;
            serviceModel.SalePrice = (double)webModel.SalePrice.Amount;
            serviceModel.TaxDetails = webModel.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            serviceModel.DynamicProperties = webModel.DynamicProperties.Select(dp => dp.ToServiceModel()).ToList();
            serviceModel.TaxTotal = (double)webModel.TaxTotal.Amount;
            serviceModel.VolumetricWeight = (double)(webModel.VolumetricWeight ?? 0);
            serviceModel.Weight = (double)webModel.Weight;
            serviceModel.Width = (double)webModel.Width;

            return serviceModel;
        }

        public static VirtoCommerceDomainMarketingModelProductPromoEntry ToPromotionItem(this LineItem lineItem)
        {
            var promoItem = new VirtoCommerceDomainMarketingModelProductPromoEntry();

            promoItem.InjectFrom(lineItem);

            promoItem.Discount = (double)lineItem.DiscountTotal.Amount;
            promoItem.Price = (double)lineItem.PlacedPrice.Amount;
            promoItem.Variations = null; // TODO

            return promoItem;
        }
    }
}