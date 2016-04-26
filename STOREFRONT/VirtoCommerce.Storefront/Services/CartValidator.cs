using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Cart.ValidationErrors;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Services
{

    public class CartValidator : ICartValidator
    {
        private readonly Func<WorkContext> _workContextFactory;
        private readonly IShoppingCartModuleApi _cartApi;
        private readonly ICatalogSearchService _catalogService;
        private readonly ILocalCacheManager _cacheManager;

        public CartValidator(Func<WorkContext> workContextFaxtory, IShoppingCartModuleApi cartApi, ICatalogSearchService catalogService, ILocalCacheManager cacheManager)
        {
            _workContextFactory = workContextFaxtory;
            _cartApi = cartApi;
            _catalogService = catalogService;
            _cacheManager = cacheManager;
        }

        public async Task ValidateAsync(ShoppingCart cart)
        {
            if (cart.IsTransient())
            {
                return;
            }

            await Task.WhenAll(ValidateItemsAsync(cart), ValidateShipmentsAsync(cart));
        }

        private async Task ValidateItemsAsync(ShoppingCart cart)
        {
            var workContext = _workContextFactory();
            var productIds = cart.Items.Select(i => i.ProductId).ToArray();
            var cacheKey = "CartValidator.ValidateItemsAsync-" + workContext.CurrentCurrency.Code + ":" + workContext.CurrentLanguage + ":" + string.Join(":", productIds);
            var products = await _cacheManager.GetAsync(cacheKey, "ApiRegion", async () => { return await _catalogService.GetProductsAsync(productIds, ItemResponseGroup.ItemWithPrices | ItemResponseGroup.ItemWithDiscounts | ItemResponseGroup.Inventory); });
       
            foreach (var lineItem in cart.Items.ToList())
            {
                lineItem.ValidationErrors.Clear();

                var product = products.FirstOrDefault(p => p.Id == lineItem.ProductId);
                if (product == null || (product != null && (!product.IsActive || !product.IsBuyable)))
                {
                    lineItem.ValidationErrors.Add(new ProductUnavailableError());
                }
                else if (product != null)
                {
                    if (product.TrackInventory && product.Inventory != null &&
                        (lineItem.ValidationType == ValidationType.PriceAndQuantity || lineItem.ValidationType == ValidationType.Quantity))
                    {
                        var availableQuantity = product.Inventory.InStockQuantity;
                        if (product.Inventory.ReservedQuantity.HasValue)
                        {
                            availableQuantity -= product.Inventory.ReservedQuantity.Value;
                        }
                        if (availableQuantity.HasValue && lineItem.Quantity > availableQuantity.Value)
                        {
                            lineItem.ValidationErrors.Add(new ProductQuantityError(availableQuantity.Value));
                        }
                    }
                    if (lineItem.ValidationType == ValidationType.PriceAndQuantity || lineItem.ValidationType == ValidationType.Price)
                    {
                        var tierPrice = product.Price.GetTierPrice(lineItem.Quantity);
                        if (tierPrice.ActualPrice != lineItem.PlacedPrice)
                        {
                            lineItem.ValidationWarnings.Add(new ProductPriceError(lineItem.PlacedPrice));
                            lineItem.SalePrice = tierPrice.Price;
                        }
                    }
                }
            }
        }

        private async Task ValidateShipmentsAsync(ShoppingCart cart)
        {
            var workContext = _workContextFactory();
            foreach (var shipment in cart.Shipments)
            {
                shipment.ValidationErrors.Clear();
                var availableShippingMethods = await _cacheManager.GetAsync("CartValidator.ValidateShipmentsAsync-" + workContext.CurrentCurrency + ":" + cart.Id, "ApiRegion", async () => { return await _cartApi.CartModuleGetShipmentMethodsAsync(cart.Id); });
                if (availableShippingMethods.Count == 0)
                {
                    shipment.ValidationWarnings.Add(new ShippingUnavailableError());
                    break;
                }
                if (!string.IsNullOrEmpty(shipment.ShipmentMethodCode))
                {
                    var existingShippingMethod = availableShippingMethods.FirstOrDefault(sm => sm.ShipmentMethodCode == shipment.ShipmentMethodCode);
                    if (existingShippingMethod == null)
                    {
                        shipment.ValidationWarnings.Add(new ShippingUnavailableError());
                        break;
                    }
                    if (existingShippingMethod != null)
                    {
                        var shippingMethod = existingShippingMethod.ToWebModel(workContext.AllCurrencies, workContext.CurrentLanguage);
                        if (shippingMethod.Price != shipment.ShippingPrice &&
                            (cart.ValidationType == ValidationType.PriceAndQuantity || cart.ValidationType == ValidationType.Price))
                        {
                            shipment.ValidationWarnings.Add(new ShippingPriceError(shipment.ShippingPrice));

                            cart.Shipments.Clear();
                            cart.Shipments.Add(shippingMethod.ToShipmentModel(cart.Currency));
                        }
                    }
                }
            }
        }
    }
}