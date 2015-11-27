using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartLineItemConverter
    {
        public static LineItem ToLineItem(this Product product, int quantity)
        {
            var lineItemWebModel = new LineItem();

            lineItemWebModel.CatalogId = product.CatalogId;
            lineItemWebModel.CategoryId = product.CategoryId;

            if (product.Price != null)
            {
                lineItemWebModel.Currency = product.Price.Currency;
                lineItemWebModel.ListPrice = product.Price.ListPrice;
                lineItemWebModel.SalePrice = product.Price.SalePrice;
            }

            if (product.PrimaryImage != null)
            {
                lineItemWebModel.ImageUrl = product.PrimaryImage.Url;
                lineItemWebModel.ThumbnailImageUrl = product.PrimaryImage.Url;
            }

            lineItemWebModel.Height = product.Height;
            lineItemWebModel.ImageUrl = product.PrimaryImage != null ?
                product.PrimaryImage.Url : null;
            lineItemWebModel.Length = product.Length;
            lineItemWebModel.MeasureUnit = product.MeasureUnit;
            lineItemWebModel.Name = product.Name;
            lineItemWebModel.Product = product;
            lineItemWebModel.ProductId = product.Id;
            lineItemWebModel.Quantity = quantity;
            lineItemWebModel.Sku = product.Sku;
            lineItemWebModel.Weight = product.Weight;
            lineItemWebModel.WeightUnit = product.WeightUnit;
            lineItemWebModel.Width = product.Width;

            return lineItemWebModel;
        }

        public static LineItem ToWebModel(this VirtoCommerceCartModuleWebModelLineItem lineItem)
        {
            var lineItemWebModel = new LineItem();

            var currency = new Currency(EnumUtility.SafeParse(lineItem.Currency, CurrencyCodes.USD));

            lineItemWebModel.InjectFrom(lineItem);
            lineItemWebModel.Currency = currency;
            lineItemWebModel.Quantity = lineItem.Quantity.HasValue ? lineItem.Quantity.Value : 0;
            lineItemWebModel.ListPrice = new Money(lineItem.ListPrice ?? 0, currency.Code);
            lineItemWebModel.SalePrice = new Money(lineItem.SalePrice ?? 0, currency.Code);

            if (lineItem.Discounts != null)
            {
                lineItemWebModel.Discounts = lineItem.Discounts.Select(d => d.ToWebModel()).ToList();
            }

            if (lineItem.TaxDetails != null)
            {
                lineItemWebModel.TaxDetails = lineItem.TaxDetails.Select(td => td.ToWebModel()).ToList();
            }

            return lineItemWebModel;
        }

        public static VirtoCommerceCartModuleWebModelLineItem ToServiceModel(this LineItem lineItem)
        {
            var lineItemServiceModel = new VirtoCommerceCartModuleWebModelLineItem();

            lineItemServiceModel.InjectFrom(lineItem);
            lineItemServiceModel.Currency = lineItem.Currency.CurrencyCode.ToString();
            lineItemServiceModel.Quantity = lineItem.Quantity;
            lineItemServiceModel.ExtendedPrice = (double)lineItem.ExtendedPrice.Amount;
            lineItemServiceModel.ListPrice = (double)lineItem.ListPrice.Amount;
            lineItemServiceModel.PlacedPrice = (double)lineItem.PlacedPrice.Amount;
            lineItemServiceModel.SalePrice = (double)lineItem.SalePrice.Amount;
            lineItemServiceModel.DiscountTotal = (double)lineItem.DiscountTotal.Amount;
            lineItemServiceModel.TaxTotal = (double)lineItem.TaxTotal.Amount;

            if (lineItem.Discounts != null)
            {
                lineItemServiceModel.Discounts = lineItem.Discounts.Select(d => d.ToServiceModel()).ToList();
            }

            if (lineItem.TaxDetails != null)
            {
                lineItemServiceModel.TaxDetails = lineItem.TaxDetails.Select(td => td.ToServiceModel()).ToList();
            }

            return lineItemServiceModel;
        }
    }
}