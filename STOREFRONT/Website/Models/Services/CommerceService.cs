#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Orders;
using VirtoCommerce.ApiClient.DataContracts.Search;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.Extensions;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Lists;
using VirtoCommerce.Web.Views.Engines.Liquid;
using VirtoCommerce.ApiClient.DataContracts.Cart;
using VirtoCommerce.ApiClient.DataContracts.Marketing;
using VirtoCommerce.ApiClient.DataContracts.Contents;

#endregion

namespace VirtoCommerce.Web.Models.Services
{
    public class CommerceService
    {
        #region Fields
        private readonly BrowseClient _browseClient;
        private readonly CartClient _cartClient;
        private readonly CartHelper _cartHelper;
        private readonly OrderClient _orderClient;
        private readonly SecurityClient _securityClient;
        private readonly StoreClient _storeClient;
        private readonly PriceClient _priceClient;
        private readonly InventoryClient _inventoryClient;
        private readonly ListClient _listClient;
        private readonly ThemeClient _themeClient;
        private readonly PageClient _pageClient;
        private readonly IViewLocator _viewLocator;
        private readonly FileStorageCacheService _pageStorageClient;
        private readonly ReviewsClient _reviewsClient;
        private readonly string _themesCacheStoragePath;
        private readonly string _pagesCacheStoragePath;
        private readonly MarketingClient _marketingClient;
        private readonly ContentClient _contentClient;

        private static readonly object _LockObject = new object();
        #endregion

        #region Constructors and Destructors
        public CommerceService()
        {
            this._listClient = ClientContext.Clients.CreateListClient();
            this._browseClient = ClientContext.Clients.CreateBrowseClient();
            this._storeClient = ClientContext.Clients.CreateStoreClient();
            this._cartClient = ClientContext.Clients.CreateCartClient();
            this._orderClient = ClientContext.Clients.CreateOrderClient();
            this._securityClient = ClientContext.Clients.CreateSecurityClient();
            this._priceClient = ClientContext.Clients.CreatePriceClient();
            this._marketingClient = ClientContext.Clients.CreateMarketingClient();
            this._inventoryClient = ClientContext.Clients.CreateInventoryClient();
            this._themeClient = ClientContext.Clients.CreateThemeClient();
            this._pageClient = ClientContext.Clients.CreatePageClient();
            this._reviewsClient = ClientContext.Clients.CreateReviewsClient();
            this._contentClient = ClientContext.Clients.CreateDefaultContentClient();

            _themesCacheStoragePath = ConfigurationManager.AppSettings["ThemeCacheFolder"];
            _pagesCacheStoragePath = ConfigurationManager.AppSettings["PageCacheFolder"];
            
            this._viewLocator = new FileThemeViewLocator(HostingEnvironment.MapPath(_themesCacheStoragePath));

            this._cartHelper = new CartHelper(this);
        }
        #endregion

        #region Public Properties
        public CartHelper Cart
        {
            get
            {
                return this._cartHelper;
            }
        }
        #endregion

        #region Public Methods and Operators
        public async Task<ICollection<Review>> GetReviewsAsync(string productId)
        {
            var webReviews = new List<Review>();

            var response = await this._reviewsClient.GetReviewsAsync(productId);

            if (response != null)
            {
                foreach (var review in response.Items)
                {
                    webReviews.Add(review.AsWebModel());
                }
            }

            return webReviews;
        }

        public async Task<Collection> GetAllCollectionAsync(string sort = "")
        {
            var collections = await this.GetCollectionsAsync(sort);
            return collections.First();
        }

        public async Task<Collection> GetCollectionAsync(string handle, string sort = "")
        {
            if (handle == "undefined")
            {
                return null;
            }

            var category =
                await
                    this._browseClient.GetCategoryByCodeAsync(
                        SiteContext.Current.StoreId,
                        SiteContext.Current.Language,
                        handle);

            return this.CreateCollection(category, sort);
        }

        public async Task<Collection> GetCollectionByKeywordAsync(string keyword, string sort = "")
        {
            var category =
                await
                    this._browseClient.GetCategoryByKeywordAsync(
                        SiteContext.Current.StoreId,
                        SiteContext.Current.Language,
                        keyword);

            return this.CreateCollection(category, sort);
        }

        public async Task<Collections> GetCollectionsAsync(string sort = "")
        {
            var categories =
                await this._browseClient.GetCategoriesAsync(SiteContext.Current.StoreId, SiteContext.Current.Language);

            var collections = new Collections(
                categories.Items.Select(category => this.CreateCollection(category, sort)));

            return collections;
        }

        public string GetCountryTags()
        {
            return "<option value=\"United States\" data-provinces=\"[&quot;California&quot;,&quot;Ohio&quot;]\">United States</option>";
        }

        public async Task<Cart> CreateCartAsync(ShoppingCart dtoCart)
        {
            await _cartClient.CreateCartAsync(dtoCart);

            dtoCart = await _cartClient.GetCartAsync(dtoCart.StoreId, dtoCart.CustomerId);

            return dtoCart != null ? dtoCart.AsWebModel() : null;
        }

        public async Task DeleteCartAsync(string cartId)
        {
            await _cartClient.DeleteCartAsync(new[] { cartId });
        }

        public async Task<Cart> GetCurrentCartAsync()
        {
            var cart = await this._cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);
            return cart != null ? cart.AsWebModel() : null;
        }

        public async Task<Checkout> GetCheckoutAsync()
        {
            var checkout = new Checkout();

            var dtoCart = await _cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);
            if (dtoCart != null)
            {
                if (dtoCart.Addresses != null)
                {
                    var billingAddress = dtoCart.Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
                    if (billingAddress != null)
                    {
                        checkout.BillingAddress = billingAddress.AsCartWebModel();
                    }

                    var shippingAddress = dtoCart.Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);
                    if (shippingAddress != null)
                    {
                        checkout.ShippingAddress = shippingAddress.AsCartWebModel();
                    }

                    checkout.BuyerAcceptsMarketing = true; // TODO
                    checkout.Currency = dtoCart.Currency;
                    checkout.CustomerId = dtoCart.CustomerId;

                    // TODO: Discounts
                    // TODO: GiftCards

                    checkout.Email = SiteContext.Current.Customer != null ? SiteContext.Current.Customer.Email : null;
                    checkout.GuestLogin = dtoCart.IsAnonymous;
                    checkout.Id = dtoCart.Id;

                    if (dtoCart.Items != null)
                    {
                        checkout.LineItems = new List<LineItem>();
                        foreach (var dtoItem in dtoCart.Items)
                        {
                            checkout.LineItems.Add(dtoItem.AsWebModel());
                        }
                    }

                    checkout.Name = dtoCart.Name;
                    checkout.Note = dtoCart.Note;
                    checkout.Order = null; // TODO

                    if (dtoCart.Payments != null)
                    {
                        var dtoPayment = dtoCart.Payments.FirstOrDefault();
                        if (dtoPayment != null)
                        {
                            checkout.PaymentMethod = new PaymentMethod
                            {
                                Handle = dtoPayment.PaymentGatewayCode
                            };
                        }
                    }

                    checkout.PaymentMethods = new List<PaymentMethod>
                    {
                        new PaymentMethod { Handle = "Klarna" },
                        new PaymentMethod { Handle = "MeS" },
                        new PaymentMethod { Handle = "PayPal" }
                    };

                    checkout.RequiresShipping = true; // TODO

                    if (dtoCart.Shipments != null)
                    {
                        var dtoShipment = dtoCart.Shipments.FirstOrDefault();
                        if (dtoShipment != null)
                        {
                            checkout.ShippingMethod = new ShippingMethod
                            {
                                Handle = dtoShipment.ShipmentMethodCode,
                                Price = dtoShipment.ShippingPrice,
                                Title = dtoShipment.ShipmentMethodCode
                            };
                        }
                    }

                    var dtoShippingMethods = await _cartClient.GetCartShippingMethods(dtoCart.Id);
                    if (dtoShippingMethods != null)
                    {
                        checkout.ShippingMethods = new List<ShippingMethod>();
                        foreach (var dtoShippingMethod in dtoShippingMethods)
                        {
                            checkout.ShippingMethods.Add(new ShippingMethod
                            {
                                Handle = dtoShippingMethod.ShipmentMethodCode,
                                Price = dtoShippingMethod.Price,
                                Title = dtoShippingMethod.Name
                            });
                        }
                    }

                    // TODO: TaxLines
                    // TODO: Transactions
                }
            }

            return checkout;
        }

        public async Task UpdateCheckoutAsync(Checkout checkout)
        {
            var dtoCart = await _cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);

            dtoCart.Addresses = new List<VirtoCommerce.ApiClient.DataContracts.Cart.Address>();

            if (checkout.BillingAddress != null)
            {
                var billingAddress = checkout.BillingAddress.AsCartServiceModel();
                billingAddress.Email = checkout.Email;
                billingAddress.Type = AddressType.Billing;
                dtoCart.Addresses.Add(billingAddress);
            }
            if (checkout.ShippingAddress != null)
            {
                var shippingAddress = checkout.ShippingAddress.AsCartServiceModel();
                shippingAddress.Email = checkout.Email;
                shippingAddress.Type = AddressType.Shipping;
                dtoCart.Addresses.Add(shippingAddress);
            }

            dtoCart.Currency = checkout.Currency;
            dtoCart.CustomerId = checkout.CustomerId;

            // TODO: Discounts
            // TODO: GiftCards

            if (checkout.LineItems != null)
            {
                dtoCart.Items = new List<CartItem>();
                foreach (var lineItem in checkout.LineItems)
                {
                    dtoCart.Items.Add(lineItem.AsServiceModel());
                }
            }

            dtoCart.Note = checkout.Note;

            if (checkout.PaymentMethod != null)
            {
                dtoCart.Payments = new List<Payment>();
                dtoCart.Payments.Add(new Payment
                {
                    PaymentGatewayCode = checkout.PaymentMethod.Handle
                });
            }

            dtoCart.StoreId = SiteContext.Current.StoreId;

            dtoCart.ShippingTotal = checkout.ShippingPrice;
            dtoCart.SubTotal = checkout.SubtotalPrice;
            dtoCart.TaxTotal = checkout.TaxPrice;
            dtoCart.Total = checkout.TotalPrice;

            await _cartClient.UpdateCurrentCartAsync(dtoCart);
        }

        public async Task<CustomerOrder> CreateOrderAsync(Checkout checkout)
        {
            var dtoCart = await _cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);
            dtoCart.Currency = checkout.Currency;
            dtoCart.CustomerId = checkout.CustomerId;

            dtoCart.Addresses = new List<ApiClient.DataContracts.Cart.Address>();

            var billingAddress = checkout.BillingAddress.AsCartServiceModel();
            billingAddress.Email = checkout.Email;
            billingAddress.Type = AddressType.Billing;
            dtoCart.Addresses.Add(billingAddress);

            if (checkout.Discounts != null)
            {
                foreach (var discount in checkout.Discounts)
                {
                    dtoCart.Discounts.Add(new ApiClient.DataContracts.Cart.Discount
                    {
                        Coupon = new ApiClient.DataContracts.Cart.Coupon { CouponCode = discount.Code },
                        Currency = dtoCart.Currency,
                        DiscountAmount = discount.Amount,
                        Description = discount.Code
                    });
                }
            }

            dtoCart.Items.Clear();
            foreach (var lineItem in checkout.LineItems)
            {
                dtoCart.Items.Add(lineItem.AsServiceModel());
            }

            dtoCart.Payments.Add(new ApiClient.DataContracts.Cart.Payment
            {
                Amount = checkout.TotalPrice,
                BillingAddress = checkout.BillingAddress.AsCartServiceModel(),
                Currency = dtoCart.Currency,
                OuterId = "", // TODO!!!
            });

            dtoCart.Shipments.Add(new ApiClient.DataContracts.Cart.Shipment
            {
                Currency = dtoCart.Currency,
                RecipientAddress = checkout.ShippingAddress.AsCartServiceModel(),
                ShipmentMethodCode = checkout.ShippingMethod.Handle,
                ShippingPrice = checkout.ShippingPrice
            });

            var shippingAddress = checkout.ShippingAddress.AsCartServiceModel();
            shippingAddress.Email = checkout.Email;

            dtoCart.ShippingTotal = checkout.ShippingPrice;
            dtoCart.SubTotal = checkout.SubtotalPrice;
            dtoCart.TaxTotal = checkout.TaxPrice;
            dtoCart.Total = checkout.TotalPrice;

            dtoCart = await _cartClient.UpdateCurrentCartAsync(dtoCart);

            var order = await _orderClient.CreateOrderAsync(dtoCart.Id);

            await DeleteCartAsync(dtoCart.Id);

            return order.AsWebModel();
        }

        public SubmitForm GetForm(string id)
        {
            var forms = SiteContext.Current.Forms;

            var form = forms.SingleOrDefault(f => f.Id == id);
            return form;
        }

        public ICollection<SubmitForm> GetForms()
        {
            var allForms = new List<SubmitForm>
                           {
                               new SubmitForm
                               {
                                   FormType = "customer_login",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/login"),
                                   PasswordNeeded = true
                               },
                               new SubmitForm
                               {
                                   FormType = "external_login",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/externallogin")
                               },
                               new SubmitForm
                               {
                                   FormType = "guest_login",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/guestlogin")
                               },
                               new SubmitForm
                               {
                                   FormType = "create_customer",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/register"),
                                   PasswordNeeded = true
                               },
                                new AddressForm
                                {
                                    FormType = "customer_address",
                                    Id = "new",
                                    ActionLink = "/Account/NewAddress"
                                },
                               new SubmitForm
                               {
                                   FormType = "recover_customer_password",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/forgotpassword")
                               },
                               new SubmitForm
                               {
                                   FormType = "reset_password",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/resetpassword")
                               },
                               new SubmitForm
                               {
                                   FormType = "send_code",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/sendcode")
                               },
                               new SubmitForm
                               {
                                   FormType = "verify_code",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/verifycode")
                               },
                               new SubmitForm
                               {
                                   FormType = "confirm_external_login",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/externalloginconfirmation")
                               },
                               new SubmitForm
                               {
                                   FormType = "edit_checkout_step_1",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/checkout/step1"),
                               },
                               new SubmitForm
                               {
                                   FormType = "edit_checkout_step_2",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/checkout/step2"),
                               }
                           };

            return allForms;
        }

        public async Task<LinkLists> GetListsAsync()
        {
            var store = SiteContext.Current.StoreId;
            var language = SiteContext.Current.Language;
            var response = await this._listClient.GetLinkListsAsync(store);
            return response.Where(r => r.Language == language).ToArray().AsWebModel();
        }

        public async Task<Product> GetProductAsync(string handle)
        {
            var product =
                await
                    this._browseClient.GetProductByCodeAsync(
                        SiteContext.Current.StoreId,
                        SiteContext.Current.Language,
                        handle,
                        ItemResponseGroups.ItemLarge);

            var variationIds = product.GetAllVariationIds();
            var prices = await this.GetProductPricesAsync(SiteContext.Current.PriceLists, variationIds);

            var price = prices.FirstOrDefault(p => p.ProductId == product.Code);
            if (product.Variations != null)
            {
                foreach (var variation in product.Variations)
                {
                    price = prices.FirstOrDefault(p => p.ProductId == variation.Code);
                }
            }

            var promoContext = new PromotionEvaluationContext
            {
                CustomerId = SiteContext.Current.CustomerId,
                CartTotal = SiteContext.Current.Cart.TotalPrice,
                Currency = SiteContext.Current.Shop.Currency,
                PromoEntries = new List<ProductPromoEntry>
                {
                    new ProductPromoEntry
                    {
                        CatalogId = product.CatalogId,
                        Price = price.Sale.HasValue ? price.Sale.Value : price.List,
                        ProductId = product.Id,
                        Quantity = 1
                    }
                },
                IsRegisteredUser = SiteContext.Current.Customer != null,
                StoreId = SiteContext.Current.StoreId
            };

            var rewards = await _marketingClient.GetPromotionRewardsAsync(promoContext);

            //var inventories = await this.GetItemInventoriesAsync(variationIds);

            return product.AsWebModel(prices, rewards/*, inventories*/);
        }

        public async Task<Product> GetProductByKeywordAsync(string keyword)
        {
            var product =
                await
                    this._browseClient.GetProductByKeywordAsync(
                        SiteContext.Current.StoreId,
                        SiteContext.Current.Language,
                        keyword,
                        ItemResponseGroups.ItemLarge);

            var variationIds = product.GetAllVariationIds();
            var prices = await this.GetProductPricesAsync(SiteContext.Current.PriceLists, variationIds);

            var price = prices.FirstOrDefault(p => p.ProductId == product.Code);
            if (product.Variations != null)
            {
                foreach (var variation in product.Variations)
                {
                    price = prices.FirstOrDefault(p => p.ProductId == variation.Code);
                }
            }

            var promoContext = new PromotionEvaluationContext
            {
                CustomerId = SiteContext.Current.CustomerId,
                CartTotal = SiteContext.Current.Cart.TotalPrice,
                Currency = SiteContext.Current.Shop.Currency,
                PromoEntries = new List<ProductPromoEntry>
                {
                    new ProductPromoEntry
                    {
                        CatalogId = product.CatalogId,
                        Price = price.Sale.HasValue ? price.Sale.Value : price.List,
                        ProductId = product.Id,
                        Quantity = 1
                    }
                },
                IsRegisteredUser = SiteContext.Current.Customer != null,
                StoreId = SiteContext.Current.StoreId
            };

            var rewards = await _marketingClient.GetPromotionRewardsAsync(promoContext);

            //var inventories = await this.GetItemInventoriesAsync(variationIds);

            return product.AsWebModel(prices, rewards/*, inventories*/);
        }

        public async Task<ProductSearchResult> GetProductsAsync(
            string storeId,
            string language,
            string[] priceLists,
            string collectionId,
            string sort,
            int skip,
            int take,
            Dictionary<string, string[]> filters = null)
        {
            // determine sort
            var sortProperty = String.Empty;
            var sortDirection = "ascending";

            if (sort.Equals("manual"))
            {
                sortProperty = "position";
            }
            else if (sort.Equals("best-selling"))
            {
                sortProperty = "position";
            }
            else
            {
                var sortArray = sort.Split(new[] { '-' });
                if (sortArray.Length > 1)
                {
                    sortProperty = sortArray[0];
                    sortDirection = sortArray[1];
                }
            }

            var response =
                await
                    this._browseClient.GetProductsAsync(
                        storeId,
                        language,
                        new BrowseQuery
                        {
                            Outline = collectionId,
                            Skip = skip,
                            Take = take,
                            SortProperty = sortProperty,
                            SortDirection = sortDirection,
                            Filters = filters,
                            PriceLists = priceLists
                        },
                        ItemResponseGroups.ItemSmall | ItemResponseGroups.Variations).ConfigureAwait(false);

            return response;
        }

        public async Task<ItemInventory> GetItemInventoryAsync(string itemId)
        {
            return await this._inventoryClient.GetItemInventory(itemId);
        }

        public async Task<IEnumerable<ItemInventory>> GetItemInventoriesAsync(string[] itemIds)
        {
            var inventories = new List<ItemInventory>();

            foreach (var itemId in itemIds)
            {
                inventories.Add(await this.GetItemInventoryAsync(itemId));
            }

            return inventories;
        }

        public async Task<IEnumerable<ApiClient.DataContracts.Marketing.PromotionReward>>
            GetPromoRewardsAsync(ApiClient.DataContracts.Marketing.PromotionEvaluationContext context)
        {
            return await _marketingClient.GetPromotionRewardsAsync(context);
        }

        public async Task<IEnumerable<ApiClient.DataContracts.Price>> GetProductPricesAsync(string[] priceLists, string[] productIds)
        {
            if (priceLists == null || productIds == null) return null;

            return await this._priceClient.GetPrices(priceLists, productIds).ConfigureAwait(false);
        }

        public async Task<string[]> GetPriceListsAsync(string catalog, string currency, TagQuery tags)
        {
            var response = await this._priceClient.GetPriceListsAsync(catalog, currency, tags).ConfigureAwait(false);
            return response;
        }

        public Settings GetSettings(string theme, string defaultValue)
        {
            var contextKey = "vc-liquid-settings" + theme;
            var value = HttpRuntime.Cache.Get(contextKey);

            var parameters = new Dictionary<string, object>();
            if (value != null)
            {
                parameters = value as Dictionary<string, object>;
                return new Settings(parameters, defaultValue);
            }

            var settingsResource = this._viewLocator.LocateResource("settings_data.json");

            if (settingsResource == null || settingsResource.Contents == null)
                return null;

            var fileContents = settingsResource.Contents.Invoke().ReadToEnd();
            var obj = JsonConvert.DeserializeObject<dynamic>(fileContents);

            // now get settings for current theme and add it as a settings parameter
            JObject settingsObject;
            if (obj.current is JObject)
            {
                settingsObject = obj.current as JObject;
            }
            else
            {
                settingsObject = obj.presets[obj.current.Value] as JObject;
            }

            if (settingsObject != null)
            {
                parameters = settingsObject.ToObject<Dictionary<string, object>>()
                    .ToDictionary(x => x.Key, y => this.GetTypedValue(y.Value));
            }

            HttpRuntime.Cache.Insert(contextKey, parameters, new CacheDependency(new[] { settingsResource.Location }));

            return new Settings(parameters, defaultValue);
        }

        public JObject GetLocale(bool loadDefault = false)
        {
            var theme = SiteContext.Current.Theme;
            var language = SiteContext.Current.Language;
            var contextKey = String.Format("vc-localizations-{0}-{1}-{2}", theme, language, loadDefault);
            var value = HttpRuntime.Cache.Get(contextKey);

            if (value != null)
            {
                if (value is JObject)
                {
                    return value as JObject;
                }

                return null;
            }

            ViewLocationResult localeResource = null;

            if (loadDefault)
            {
                localeResource = this._viewLocator.LocateResource("*default.json");
            }
            else
            {
                var culture = language.TryGetCultureInfo();

                // check specific culture file existance
                localeResource = this._viewLocator.LocateResource((String.Format("{0}.json", culture.Name)))
                                 ?? this._viewLocator.LocateResource(
                                     (String.Format("{0}.json", culture.TwoLetterISOLanguageName)));
            }

            if (localeResource == null)
            {
                return null;
            }

            var fileContents = localeResource.Contents.Invoke().ReadToEnd();

            var contents = JsonConvert.DeserializeObject<dynamic>(fileContents);
            HttpRuntime.Cache.Insert(contextKey, contents, new CacheDependency(new[] { localeResource.Location }));
            return contents;
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            var stores = await this._storeClient.GetStoresAsync();
            return stores.Select(s => s.AsWebModel());
        }

        public async Task<Cart> SaveChangesAsync(Cart cart)
        {
            var model = await this._cartClient.UpdateCurrentCartAsync(cart.AsServiceModel());
            return model.AsWebModel();
        }

        public async Task<ItemCollection<object>> SearchAsync(SiteContext context, string type, string terms, int skip, int? take)
        {
            var pageSize = take ?? 20;

            var query = new BrowseQuery { Search = terms, Skip = skip, Take = pageSize };
            var priceLists = context.PriceLists;

            var response =
                await
                    this._browseClient.GetProductsAsync(
                        context.StoreId,
                        context.Language,
                        query,
                        ItemResponseGroups.ItemMedium);

            var allIds = response.Items.ToArray().GetAllVariationIds();
            var prices = await GetProductPricesAsync(priceLists, allIds.ToArray());

            var promoContext = new PromotionEvaluationContext
            {
                CustomerId = context.CustomerId,
                CartTotal = context.Cart.TotalPrice,
                Currency = context.Shop.Currency,
                IsRegisteredUser = context.Customer != null,
                StoreId = context.StoreId
            };

            var promoEntries = new List<ProductPromoEntry>();
            foreach (var item in response.Items)
            {
                var price = prices.FirstOrDefault(p => p.ProductId == item.Code);
                if (item.Variations != null)
                {
                    foreach (var variation in item.Variations)
                    {
                        price = prices.FirstOrDefault(p => p.ProductId == variation.Code);
                    }
                }

                promoEntries.Add(new ProductPromoEntry
                {
                    CatalogId = item.CatalogId,
                    Price = price != null && price.Sale.HasValue ? price.Sale.Value : price.List,
                    ProductId = item.Id,
                    Quantity = 1
                });
            }
            promoContext.PromoEntries = promoEntries;

            var rewards = await _marketingClient.GetPromotionRewardsAsync(promoContext);

            //var inventories = await this.GetItemInventoriesAsync(allIds);

            var result = new ItemCollection<object>(response.Items.Select(i => i.AsWebModel(prices, rewards/*, inventories*/))) { TotalCount = response.TotalCount };

            return result;
        }

        public Theme GetTheme(string themeName)
        {
            var id = themeName;
            var name = themeName;
            if (id.Contains("/"))
            {
                name = id.Split('/')[1];
            }

            Theme currentTheme = null;
            if (SiteContext.Current.Themes != null)
            {
                currentTheme = SiteContext.Current.Themes.SingleOrDefault(x => x.Id.Equals(themeName, StringComparison.OrdinalIgnoreCase));
            }

            return currentTheme ?? new Theme { Id = id, Name = name, Role = "main", Path = id };
        }

        public async Task<Theme[]> GetThemesAsync()
        {
            var store = SiteContext.Current.StoreId;
            var response = await this._themeClient.GetThemesAsync(store);
            return response.AsWebModel();
        }

        public async Task UpdateThemeCacheAsync()
        {
            var store = SiteContext.Current.StoreId;
            var theme = SiteContext.Current.Theme.Name;
            var themePath = String.Format("{0}\\{1}", _themesCacheStoragePath, SiteContext.Current.Theme.Path);
            var storageClient = new FileStorageCacheService(HostingEnvironment.MapPath(themePath));
            var lastUpdate = storageClient.GetLatestUpdate();
            var response = await this._themeClient.GetThemeAssetsAsync(store, theme, lastUpdate, true);
            //var response = await this._storeClient.GetStoreAssetsAsync(store, theme, lastUpdate);

            if (response.Any())
            {
                lock (_LockObject)
                {
                    // check last update again, since going to the service is more expensive than checking local folders
                    var newLastUpdate = storageClient.GetLatestUpdate();
                    if (newLastUpdate == lastUpdate)
                    {
                        var reload = storageClient.ApplyUpdates(response.AsFileModel());
                        if (reload)
                        {
                            this._viewLocator.UpdateCache();
                        }
                    }
                }
            }
        }

        public async Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(string[] placeholders)
        {
            return await _contentClient.GetDynamicContentAsync(placeholders, null);
        }
        #endregion

        #region Methods
        private Collection CreateCollection(Category category, string sort = "")
        {
            if (category == null)
            {
                return null;
            }

            var newCollection = category.AsWebModel();
            newCollection.SortBy = sort;
            return newCollection;
        }

        private object GetTypedValue(object val)
        {
            if (val is string) // try to convert string to a typed value
            {
                var input = val as string;
                int valInt;
                if (Int32.TryParse(input, out valInt))
                {
                    return valInt;
                }

                Single valSingle;
                if (Single.TryParse(input, out valSingle))
                {
                    return valSingle;
                }
            }

            return val;
        }
        #endregion
    }
}