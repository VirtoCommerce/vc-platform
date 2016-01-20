﻿using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartLineItemConverter
    {
        public static LineItem ToLineItem(this Product product, Language language, int quantity)
        {
            var lineItemWebModel = new LineItem(product.Price.Currency, language);

            lineItemWebModel.InjectFrom<NullableAndEnumValueInjecter>(product);

            var currency = product.Price.Currency;

            lineItemWebModel.ImageUrl = product.PrimaryImage.Url;
            lineItemWebModel.ListPrice = product.Price.ListPrice;
            lineItemWebModel.SalePrice = product.Price.SalePrice;
            lineItemWebModel.ProductId = product.Id;
            lineItemWebModel.Quantity = quantity;

            lineItemWebModel.ThumbnailImageUrl = product.PrimaryImage.Url;

            return lineItemWebModel;
        }

        public static LineItem ToWebModel(this VirtoCommerceCartModuleWebModelLineItem serviceModel, Currency currency, Language language)
        {
            var webModel = new LineItem(currency, language);

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            if (serviceModel.TaxDetails != null)
            {
                webModel.TaxDetails = serviceModel.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            if (serviceModel.DynamicProperties != null)
            {
                webModel.DynamicProperties = serviceModel.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }


            webModel.IsGift = (bool)serviceModel.IsGift;
            webModel.IsReccuring = (bool)serviceModel.IsReccuring;
            webModel.Length = (decimal)(serviceModel.Length ?? 0);
            webModel.ListPrice = new Money(serviceModel.ListPrice ?? 0, currency);

            webModel.RequiredShipping = (bool)serviceModel.RequiredShipping;
            webModel.SalePrice = new Money(serviceModel.SalePrice ?? 0, currency);
            webModel.TaxIncluded = (bool)serviceModel.TaxIncluded;
            webModel.TaxTotal = new Money(serviceModel.TaxTotal ?? 0, currency);
            webModel.Weight = (decimal)(serviceModel.Weight ?? 0);
            webModel.Width = (decimal)(serviceModel.Width ?? 0);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelLineItem ToServiceModel(this LineItem webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelLineItem();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.Currency = webModel.Currency.Code;
            serviceModel.Discounts = webModel.Discounts.Select(d => d.ToServiceModel()).ToList();
            serviceModel.DiscountTotal = (double)webModel.DiscountTotal.Amount;
            serviceModel.ExtendedPrice = (double)webModel.ExtendedPrice.Amount;
            serviceModel.Height = (double)webModel.Height;
            serviceModel.Length = (double)webModel.Length;
            serviceModel.ListPrice = (double)webModel.ListPrice.Amount;
            serviceModel.PlacedPrice = (double)webModel.PlacedPrice.Amount;
            serviceModel.SalePrice = (double)webModel.SalePrice.Amount;
            serviceModel.TaxDetails = webModel.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            serviceModel.DynamicProperties = webModel.DynamicProperties.Select(dp => dp.ToServiceModel()).ToList();
            serviceModel.TaxTotal = (double)webModel.TaxTotal.Amount;
            serviceModel.VolumetricWeight = (double)(webModel.VolumetricWeight ?? 0);
            serviceModel.Weight = (double)webModel.Weight;
            serviceModel.Width = (double)webModel.Width;

            return serviceModel;
        }

        public static PromotionProductEntry ToPromotionItem(this LineItem lineItem)
        {
            var promoItem = new PromotionProductEntry();

            promoItem.InjectFrom(lineItem);

            promoItem.Discount = new Money(lineItem.DiscountTotal.Amount, lineItem.DiscountTotal.Currency);
            promoItem.Price = new Money(lineItem.PlacedPrice.Amount, lineItem.PlacedPrice.Currency);
            promoItem.Quantity = lineItem.Quantity;
            promoItem.Variations = null; // TODO

            return promoItem;
        }
    }
}