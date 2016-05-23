using System;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Services;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class ShoppingCartTests : StorefrontTestBase
    {
        [Fact]
        public void CreateNewCart_AnonymousUser_CheckThatNewCartCreated()
        {
            var cartBuilder = GetCartBuilder();
            var workContext = GetTestWorkContext();
            var anonymousCustomer = new CustomerInfo
            {
                Id = Guid.NewGuid().ToString(),
                IsRegisteredUser = false
            };
            cartBuilder = cartBuilder.GetOrCreateNewTransientCartAsync(workContext.CurrentStore, anonymousCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result;
            Assert.True(cartBuilder.Cart.IsTransient());

            cartBuilder.SaveAsync().Wait();
            var cart = cartBuilder.Cart;
            Assert.False(cart.IsTransient());

            cartBuilder = cartBuilder.GetOrCreateNewTransientCartAsync(workContext.CurrentStore, anonymousCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result;
            Assert.Equal(cart.Id, cartBuilder.Cart.Id);
        }

        [Fact]
        public void ManageItemsInCart_AnonymousUser_CheckThatItemsAndTotalsChanged()
        {
        }

        [Fact]
        public void MergeAnonymousCart_RegisteredUser_CheckThatAllMerged()
        {
        }

        [Fact]
        public void SingleShipmentAndPaymentWithCouponCheckout_RegisteredUser_CheckOrderCreated()
        {
        }

        [Fact]
        public void MultipleShipmentAndPartialPaymentWithCouponCheckout_RegisteredUser_CheckOrderCreated()
        {
        }

        private ICartBuilder GetCartBuilder()
        {
            var apiClient = GetApiClient();
            var marketingApi = new MarketingModuleApi(apiClient);
            var cartApi = new ShoppingCartModuleApi(apiClient);
            var cacheManager = new Moq.Mock<ILocalCacheManager>();
            var workContextFactory = new Func<WorkContext>(GetTestWorkContext);
            var promotionEvaluator = new PromotionEvaluator(marketingApi);
            var catalogModuleApi = new CatalogModuleApi(apiClient);
            var pricingApi = new PricingModuleApi(apiClient);
            var commerceApi = new CommerceCoreModuleApi(apiClient);
            var pricingService = new PricingServiceImpl(workContextFactory, pricingApi, commerceApi);
            var inventoryApi = new InventoryModuleApi(apiClient);
            var searchApi = new SearchModuleApi(apiClient);
            var catalogSearchService = new CatalogSearchServiceImpl(workContextFactory, catalogModuleApi, pricingService, inventoryApi, searchApi, promotionEvaluator);
            var retVal = new CartBuilder(cartApi, promotionEvaluator, catalogSearchService, commerceApi, cacheManager.Object);
            return retVal;
        }
    }
}
