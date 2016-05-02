using CacheManager.Core;
using Moq;
using System;
using System.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Quote.Events;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Services;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class QuoteRequestTests : StorefrontTestBase
    {
        [Fact]
        public void CreateAnonymousQuoteRequest()
        {
            var workContext = GetTestWorkContext();
            var quoteRequestBuilder = GetQuoteRequestBuilder();
            var customer = new CustomerInfo
            {
                Id = Guid.NewGuid().ToString(),
                IsRegisteredUser = false
            };

            var quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.True(quoteRequest.IsTransient());
        }

        [Fact]
        public void ManageProductWithQuoteRequest()
        {
            var workContext = GetTestWorkContext();
            var quoteRequestBuilder = GetQuoteRequestBuilder();
            var customer = new CustomerInfo
            {
                Id = Guid.NewGuid().ToString(),
                IsRegisteredUser = false
            };
            workContext.CurrentCustomer = customer;

            var quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.True(quoteRequest.IsTransient());

            var catalogSearchService = GetCatalogSearchService();
            var searchResult = catalogSearchService.GetProductsAsync(new[] { "217be9f3d9064075821f6785dca658b9" }, ItemResponseGroup.ItemLarge).Result;
            quoteRequestBuilder.AddItem(searchResult.First(), 1);
            Assert.True(quoteRequest.ItemsCount == 1);

            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.False(quoteRequest.IsTransient());

            var quoteItem = quoteRequestBuilder.QuoteRequest.Items.First();
            var productPrice = quoteItem.SalePrice;
            quoteItem.ProposalPrices.Add(new TierPrice(productPrice, 10 ));
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            quoteItem = quoteRequestBuilder.QuoteRequest.Items.First();
            Assert.True(quoteItem.ProposalPrices.Count == 2);

            quoteRequestBuilder.RemoveItem(quoteItem.Id);
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.True(quoteRequest.ItemsCount == 0);
        }

        [Fact]
        public void SubmitQuoteRequest()
        {
            var workContext = GetTestWorkContext();
            var quoteRequestBuilder = GetQuoteRequestBuilder();
            var customer = new CustomerInfo
            {
                Id = Guid.NewGuid().ToString(),
                IsRegisteredUser = false
            };
            workContext.CurrentCustomer = customer;

            var quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.True(quoteRequest.IsTransient());

            var catalogSearchService = GetCatalogSearchService();
            var searchResult = catalogSearchService.GetProductsAsync(new[] { "217be9f3d9064075821f6785dca658b9" }, ItemResponseGroup.ItemLarge).Result;
            quoteRequestBuilder.AddItem(searchResult.First(), 1);
            Assert.True(quoteRequest.ItemsCount == 1);

            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.False(quoteRequest.IsTransient());

            var quoteItem = quoteRequestBuilder.QuoteRequest.Items.First();
            var productPrice = quoteItem.SalePrice;
            quoteItem.ProposalPrices.Add(new TierPrice(productPrice, 10));
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            quoteItem = quoteRequestBuilder.QuoteRequest.Items.First();
            Assert.True(quoteItem.ProposalPrices.Count == 2);

            quoteRequestBuilder.Submit();
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.TakeQuoteRequest(quoteRequestBuilder.QuoteRequest).QuoteRequest;
            Assert.True(quoteRequest.Status == "Processing");
        }

        [Fact]
        public void RejectQuoteRequest()
        {
            var workContext = GetTestWorkContext();
            var quoteRequestBuilder = GetQuoteRequestBuilder();
            var customer = new CustomerInfo
            {
                Id = Guid.NewGuid().ToString(),
                IsRegisteredUser = false
            };
            workContext.CurrentCustomer = customer;

            var quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.True(quoteRequest.IsTransient());

            var catalogSearchService = GetCatalogSearchService();
            var searchResult = catalogSearchService.GetProductsAsync(new[] { "217be9f3d9064075821f6785dca658b9" }, ItemResponseGroup.ItemLarge).Result;
            quoteRequestBuilder.AddItem(searchResult.First(), 1);
            Assert.True(quoteRequest.ItemsCount == 1);

            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            Assert.False(quoteRequest.IsTransient());

            var quoteItem = quoteRequestBuilder.QuoteRequest.Items.First();
            var productPrice = quoteItem.SalePrice;
            quoteItem.ProposalPrices.Add(new TierPrice(productPrice, 10 ));
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, customer, workContext.CurrentLanguage, workContext.CurrentCurrency).Result.QuoteRequest;
            quoteItem = quoteRequestBuilder.QuoteRequest.Items.First();
            Assert.True(quoteItem.ProposalPrices.Count == 2);

            quoteRequestBuilder.Submit();
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.TakeQuoteRequest(quoteRequestBuilder.QuoteRequest).QuoteRequest;
            Assert.True(quoteRequest.Status == "Processing");

            quoteRequestBuilder.Reject();
            quoteRequestBuilder.SaveAsync().Wait();
            quoteRequest = quoteRequestBuilder.TakeQuoteRequest(quoteRequestBuilder.QuoteRequest).QuoteRequest;
            Assert.True(quoteRequest.Status == "Rejected");
        }

        [Fact]
        public void ConfirmQuoteRequest()
        {
        }

        private IQuoteRequestBuilder GetQuoteRequestBuilder()
        {
            var apiClientConfiguration = new Client.Client.Configuration(GetApiClient());
            var quoteApi = new QuoteModuleApi(apiClientConfiguration);
            var cacheManager = new Mock<ILocalCacheManager>();
            var quoteRequestEventPublisher = new Mock<IEventPublisher<QuoteRequestUpdatedEvent>>();

            return new QuoteRequestBuilder(quoteApi, cacheManager.Object, quoteRequestEventPublisher.Object);
        }

        private ICatalogSearchService GetCatalogSearchService()
        {
            var apiClientConfiguration = new Client.Client.Configuration(GetApiClient());
            var workContextFactory = new Func<WorkContext>(GetTestWorkContext);
            var catalogApi = new CatalogModuleApi(apiClientConfiguration);
            var pricingApi = new PricingModuleApi(apiClientConfiguration);
            var pricingService = new PricingServiceImpl(workContextFactory, pricingApi);
            var inventoryApi = new InventoryModuleApi(apiClientConfiguration);
            var searchApi = new SearchModuleApi(apiClientConfiguration);
            var marketingApi = new MarketingModuleApi(apiClientConfiguration);
            var promotionEvaluator = new PromotionEvaluator(marketingApi);

            return new CatalogSearchServiceImpl(workContextFactory, catalogApi, pricingService, inventoryApi, searchApi, promotionEvaluator);
        }
    }
}