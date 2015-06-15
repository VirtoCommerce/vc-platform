#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models.Helpers;
using VirtoCommerce.Web.Models.Lists;
using VirtoCommerce.Web.Models.Searching;
using VirtoCommerce.Web.Services;
using VirtoCommerce.Web.Views.Engines.Liquid;
using VirtoCommerce.ApiClient.DataContracts.Cart;
using VirtoCommerce.ApiClient.DataContracts.Marketing;
using VirtoCommerce.ApiClient.DataContracts.Contents;
using System.Collections.Specialized;

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
        private readonly ReviewsClient _reviewsClient;
        private readonly string _themesCacheStoragePath;
        private readonly string _pagesCacheStoragePath;
        private readonly MarketingClient _marketingClient;

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
        public IDictionary<string, string> CurrencyDictionary
        {
            #region Currency Dictionary
            get
            {
                var dictionary = new Dictionary<string, string>() {
                                                    {"AED", "د.إ.‏"},
                                                    {"AFN", "؋ "},
                                                    {"ALL", "Lek"},
                                                    {"AMD", "դր."},
                                                    {"ARS", "$"},
                                                    {"AUD", "$"}, 
                                                    {"AZN", "man."}, 
                                                    {"BAM", "KM"}, 
                                                    {"BDT", "৳"}, 
                                                    {"BGN", "лв."}, 
                                                    {"BHD", "د.ب.‏ "},
                                                    {"BND", "$"}, 
                                                    {"BOB", "$b"}, 
                                                    {"BRL", "R$"}, 
                                                    {"BYR", "р."}, 
                                                    {"BZD", "BZ$"}, 
                                                    {"CAD", "$"}, 
                                                    {"CHF", "fr."}, 
                                                    {"CLP", "$"}, 
                                                    {"CNY", "¥"}, 
                                                    {"COP", "$"}, 
                                                    {"CRC", "₡"}, 
                                                    {"CSD", "Din."}, 
                                                    {"CZK", "Kč"}, 
                                                    {"DKK", "kr."}, 
                                                    {"DOP", "RD$"}, 
                                                    {"DZD", "DZD"}, 
                                                    {"EEK", "kr"}, 
                                                    {"EGP", "ج.م.‏ "},
                                                    {"ETB", "ETB"}, 
                                                    {"EUR", "€"}, 
                                                    {"GBP", "£"}, 
                                                    {"GEL", "Lari"}, 
                                                    {"GTQ", "Q"}, 
                                                    {"HKD", "HK$"}, 
                                                    {"HNL", "L."}, 
                                                    {"HRK", "kn"}, 
                                                    {"HUF", "Ft"}, 
                                                    {"IDR", "Rp"}, 
                                                    {"ILS", "₪"}, 
                                                    {"INR", "रु"}, 
                                                    {"IQD", "د.ع.‏ "},
                                                    {"IRR", "ريال "},
                                                    {"ISK", "kr."}, 
                                                    {"JMD", "J$"}, 
                                                    {"JOD", "د.ا.‏ "},
                                                    {"JPY", "¥"}, 
                                                    {"KES", "S"}, 
                                                    {"KGS", "сом"}, 
                                                    {"KHR", "៛"}, 
                                                    {"KRW", "₩"}, 
                                                    {"KWD", "د.ك.‏ "},
                                                    {"KZT", "Т"}, 
                                                    {"LAK", "₭"}, 
                                                    {"LBP", "ل.ل.‏ "},
                                                    {"LKR", "රු."}, 
                                                    {"LTL", "Lt"}, 
                                                    {"LVL", "Ls"}, 
                                                    {"LYD", "د.ل.‏ "},
                                                    {"MAD", "د.م.‏ "},
                                                    {"MKD", "ден."}, 
                                                    {"MNT", "₮"}, 
                                                    {"MOP", "MOP"}, 
                                                    {"MVR", "ރ."}, 
                                                    {"MXN", "$"}, 
                                                    {"MYR", "RM"}, 
                                                    {"NIO", "N"}, 
                                                    {"NOK", "kr"}, 
                                                    {"NPR", "रु"}, 
                                                    {"NZD", "$"}, 
                                                    {"OMR", "ر.ع.‏ "},
                                                    {"PAB", "B/."}, 
                                                    {"PEN", "S/."}, 
                                                    {"PHP", "PhP"}, 
                                                    {"PKR", "Rs"}, 
                                                    {"PLN", "zł"}, 
                                                    {"PYG", "Gs"}, 
                                                    {"QAR", "ر.ق.‏ "},
                                                    {"RON", "lei"}, 
                                                    {"RSD", "Din."}, 
                                                    {"RUB", "р."}, 
                                                    {"RWF", "RWF"}, 
                                                    {"SAR", "ر.س.‏ "},
                                                    {"SEK", "kr"}, 
                                                    {"SGD", "$"}, 
                                                    {"SYP", "ل.س.‏ "},
                                                    {"THB", "฿"}, 
                                                    {"TJS", "т.р."}, 
                                                    {"TMT", "m."}, 
                                                    {"TND", "د.ت.‏ "},
                                                    {"TRY", "TL"}, 
                                                    {"TTD", "TT$"}, 
                                                    {"TWD", "NT$"}, 
                                                    {"UAH", "₴"}, 
                                                    {"USD", "$"}, 
                                                    {"UYU", "$U"}, 
                                                    {"UZS", "so'm"}, 
                                                    {"VEF", "Bs. F."}, 
                                                    {"VND", "₫"}, 
                                                    {"XOF", "XOF"}, 
                                                    {"YER", "ر.ي.‏ "},
                                                    {"ZAR", "R"}, 
                                                    {"ZWL", "Z$"} 
                };

                return dictionary;
            }
            #endregion
        }

        public async Task<ICollection<Review>> GetReviewsAsync(string productId)
        {
            var webReviews = new List<Review>();

            var response = await this._reviewsClient.GetReviewsAsync(productId);

            if (response != null)
            {
                webReviews.AddRange(response.Items.Select(review => review.AsWebModel()));
            }

            return webReviews;
        }

        public async Task<Collection> GetAllCollectionAsync(SiteContext context, string sort = "")
        {
            var collections = await this.GetCollectionsAsync(context, sort);
            return collections.First();
        }

        public async Task<Collection> GetCollectionAsync(SiteContext context, string handle, string sort = "")
        {
            if (handle == "undefined")
            {
                return null;
            }

            var category =
                await
                    this._browseClient.GetCategoryByCodeAsync(
                        context.StoreId,
                        context.Language,
                        handle);

            return this.CreateCollection(category, sort);
        }

        public async Task<Collection> GetCollectionByKeywordAsync(SiteContext context, string keyword, string sort = "")
        {
            var category =
                await
                    this._browseClient.GetCategoryByKeywordAsync(
                        context.StoreId,
                        context.Language,
                        keyword);

            return this.CreateCollection(category, sort);
        }

        public async Task<Collections> GetCollectionsAsync(SiteContext context, string sort = "")
        {
            var categories =
                await this._browseClient.GetCategoriesAsync(context.StoreId, context.Language);

            var collections = new Collections(
                categories.Items.Select(category => this.CreateCollection(category, sort)));

            return collections;
        }

        public string GetCountryTags()
        {
            var countries = new Dictionary<string, ICollection<string>>();
            countries.Add("United States", new List<string>
            {
                "Delaware", "Pennsylvania", "New Jersey", "Georgia", "Connecticut", "Massachusetts",
                "Maryland", "South Carolina", "New Hampshire", "Virginia", "New York", "North Carolina",
                "Rhode Island", "Vermont", "Kentucky", "Tennessee", "Ohio", "Louisiana",
                "Indiana", "Mississippi", "Illinois", "Alabama", "Maine", "Missouri",
                "Arkansas", "Michigan", "Florida", "Texas", "Iowa", "Wisconsin",
                "California", "Minnesota", "Oregon", "Kansas", "West Virginia", "Nevada",
                "Nebraska", "Colorado", "North Dakota", "South Dakota", "Montana", "Washington",
                "Idaho", "Wyoming", "Utah", "Oklahoma", "New Mexico", "Arizona",
                "Alaska", "Hawaii"
            });
            countries.Add("Germany", new List<string>());

            return new CountryOptionTags(countries).ToString();
        }

        public async Task<Cart> CreateCartAsync(ShoppingCart dtoCart)
        {
            await _cartClient.CreateCartAsync(dtoCart);

            dtoCart = await _cartClient.GetCartAsync(dtoCart.StoreId, dtoCart.CustomerId);

            return dtoCart != null ? dtoCart.AsWebModel() : null;
        }

        public async Task<Cart> MergeCartsAsync(Cart anonymousCart)
        {
            var customerCart = SiteContext.Current.Cart;

            foreach (var anonymousLineItem in anonymousCart.Items)
            {
                var customerLineItem = customerCart.Items
                    .FirstOrDefault(i => i.Handle == anonymousLineItem.Handle);

                if (customerLineItem != null)
                {
                    customerLineItem.Quantity += anonymousLineItem.Quantity;
                }
                else
                {
                    anonymousLineItem.Id = null;
                    customerCart.Items.Add(anonymousLineItem);
                }
            }

            var cart = await SaveChangesAsync(customerCart);
            await DeleteCartAsync(anonymousCart.Key);

            return cart;
        }

        public async Task DeleteCartAsync(string cartId)
        {
            await _cartClient.DeleteCartAsync(new[] { cartId });
        }

        public async Task<Cart> GetCartAsync(string storeId, string customerId)
        {
            var cart = await this._cartClient.GetCartAsync(storeId, customerId);
            return cart != null ? cart.AsWebModel() : null;
        }

        public async Task<Checkout> GetCheckoutAsync()
        {
            var checkout = new Checkout();

            var dtoCart = await _cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);

            if (dtoCart != null)
            {
                checkout = dtoCart.AsCheckoutWebModel();

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    checkout.GuestLogin = false;
                    checkout.Email = SiteContext.Current.Customer.Email;
                }
                else
                {
                    checkout.GuestLogin = true;

                    var addressWithEmail = dtoCart.Addresses != null ?
                        dtoCart.Addresses.FirstOrDefault(a => !string.IsNullOrEmpty(a.Email)) : null;

                    if (addressWithEmail != null)
                    {
                        checkout.Email = addressWithEmail.Email;
                    }
                }
            }

            return checkout;
        }

        public async Task<ICollection<ShippingMethod>> GetShippingMethodsAsync(string cartId)
        {
            ICollection<ShippingMethod> shippingMethodModels = null;

            var shippingMethods = await _cartClient.GetCartShippingMethods(cartId);

            if (shippingMethods != null)
            {
                shippingMethodModels = new List<ShippingMethod>();
                foreach (var shippingMethod in shippingMethods)
                {
                    shippingMethodModels.Add(new ShippingMethod
                    {
                        Handle = shippingMethod.ShipmentMethodCode,
                        Price = shippingMethod.Price,
                        Title = shippingMethod.Name
                    });
                }
            }

            return shippingMethodModels;
        }

        public async Task<ICollection<PaymentMethod>> GetCartPaymentMethodsAsync(string cartId)
        {
            ICollection<PaymentMethod> paymentMethodModels = null;

            var paymentMethods = await _cartClient.GetCartPaymentMethods(cartId);

            if (paymentMethods != null)
            {
                paymentMethodModels = new List<PaymentMethod>();
                foreach (var paymentMethod in paymentMethods)
                {
                    paymentMethodModels.Add(paymentMethod.AsWebModel());
                }
            }

            return paymentMethodModels;
        }

        public async Task<ICollection<PaymentMethod>> GetStorePaymentMethodsAsync(string storeId)
        {
            ICollection<PaymentMethod> paymentMethodModels = null;

            var paymentMethods = await _cartClient.GetStorePaymentMethods(storeId);

            if (paymentMethods != null)
            {
                paymentMethodModels = new List<PaymentMethod>();
                foreach (var paymentMethod in paymentMethods)
                {
                    paymentMethodModels.Add(paymentMethod.AsWebModel());
                }
            }

            return paymentMethodModels;
        }

        public async Task UpdateCheckoutAsync(Checkout checkout)
        {
            var cart = await _cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);

            if (cart != null)
            {
                checkout.AsServiceModel(ref cart);
            }

            await _cartClient.UpdateCurrentCartAsync(cart);
        }

        public async Task<VirtoCommerce.ApiClient.DataContracts.Orders.CustomerOrder> CreateOrderAsync()
        {
            VirtoCommerce.ApiClient.DataContracts.Orders.CustomerOrder order = null;

            var cart = await _cartClient.GetCartAsync(SiteContext.Current.StoreId, SiteContext.Current.CustomerId);

            if (cart != null)
            {
                order = await _orderClient.CreateOrderAsync(cart.Id);
                await DeleteCartAsync(cart.Id);
            }

            return order;
        }

        public async Task<ProcessPaymentResult> ProcessPaymentAsync(string orderId, string paymentMethodId)
        {
            return await _orderClient.ProcessPayment(orderId, paymentMethodId);
        }

        public async Task<PostProcessPaymentResult> PostPaymentProcessAsync(ICollection<KeyValuePair<string, string>> parameters)
        {
            return await _orderClient.PostPaymentProcess(parameters);
        }

        public SubmitForm GetForm(SiteContext context, string id)
        {
            var forms = context.Forms;

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
                               },
                               new SubmitForm
                               {
                                   FormType = "pay_order",
                                   ActionLink = VirtualPathUtility.ToAbsolute("~/account/payorder"),
                               }
                           };

            return allForms;
        }

        public BlogCollection GetBlogs(SiteContext context)
        {
            var service = new PagesService();
            return new BlogCollection(service.GetBlogs(context));
        }

        public async Task<LinkLists> GetListsAsync(SiteContext context)
        {
            var store = context.StoreId;
            var language = context.Language;
            var response = await this._listClient.GetLinkListsAsync(store);
            return response == null ? null : response.Where(r => r.Language == language).AsWebModel();
        }

        public async Task<Product> GetProductAsync(SiteContext context, string handle, ItemResponseGroups responseGroup = ItemResponseGroups.ItemLarge)
        {
            var product =
                await
                    this._browseClient.GetProductByCodeAsync(
                        context.StoreId,
                        context.Language,
                        handle,
                        responseGroup);

            var variationIds = product.GetAllVariationIds();
            var prices = await this.GetProductPricesAsync(context.PriceLists, variationIds);

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
                CustomerId = context.CustomerId,
                CartTotal = context.Cart.TotalPrice,
                Currency = context.Shop.Currency,
                PromoEntries = new List<ProductPromoEntry>
                {
                    new ProductPromoEntry
                    {
                        CatalogId = product.CatalogId,
                        Price = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M,
                        ProductId = product.Id,
                        Quantity = 1
                    }
                },
                IsRegisteredUser = context.Customer != null,
                StoreId = context.StoreId
            };

            var rewards = await _marketingClient.GetPromotionRewardsAsync(promoContext);

            var inventories = await this.GetItemsInventoriesAsync(variationIds);

            return product.AsWebModel(prices, rewards, inventories);
        }

        public async Task<Product> GetProductByKeywordAsync(SiteContext context, string keyword, ItemResponseGroups responseGroup = ItemResponseGroups.ItemLarge)
        {
            var product =
                await
                    this._browseClient.GetProductByKeywordAsync(
                        context.StoreId,
                        context.Language,
                        keyword,
                        responseGroup);

            if (product == null)
                return null;

            var variationIds = product.GetAllVariationIds();
            var prices = await this.GetProductPricesAsync(context.PriceLists, variationIds);

            var price = prices.FirstOrDefault(p => p.ProductId == product.Id);
            //if (product.Variations != null)
            //{
            //    foreach (var variation in product.Variations)
            //    {
            //        price = prices.FirstOrDefault(p => p.ProductId == variation.Id);
            //    }
            //}

            var promoContext = new PromotionEvaluationContext
            {
                CustomerId = context.CustomerId,
                CartTotal = context.Cart.TotalPrice,
                Currency = context.Shop.Currency,
                PromoEntries = new List<ProductPromoEntry>
                {
                    new ProductPromoEntry
                    {
                        CatalogId = product.CatalogId,
                        Price = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M,
                        ProductId = product.Id,
                        Quantity = 1
                    }
                },
                IsRegisteredUser = context.Customer != null,
                StoreId = context.StoreId
            };

            var rewards = await _marketingClient.GetPromotionRewardsAsync(promoContext);

            var inventories = await this.GetItemsInventoriesAsync(variationIds);

            return product.AsWebModel(prices, rewards, inventories);
        }

        public async Task<IEnumerable<InventoryInfo>> GetItemsInventoriesAsync(string[] itemIds)
        {
            if (itemIds == null)
            {
                return null;
            }

            return await _inventoryClient.GetItemsInventories(itemIds);
        }

        public async Task<IEnumerable<ApiClient.DataContracts.Marketing.PromotionReward>>
            GetPromoRewardsAsync(ApiClient.DataContracts.Marketing.PromotionEvaluationContext context)
        {
            return await _marketingClient.GetPromotionRewardsAsync(context);
        }

        public async Task<IEnumerable<ApiClient.DataContracts.Price>> GetProductPricesAsync(string[] priceLists, string[] productIds)
        {
            if (priceLists == null || productIds == null)
                return null;

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

        public JObject GetLocale(SiteContext context, bool loadDefault = false)
        {
            var theme = context.Theme;
            var language = context.Language;
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

        public async Task<SearchResults<T>> SearchAsync<T>(SiteContext context, BrowseQuery query, Collection parentCollection = null, ItemResponseGroups? responseGroups = ItemResponseGroups.ItemMedium)
        {
            var priceLists = context.PriceLists;
            query.PriceLists = priceLists;

            var response =
                await
                    this._browseClient.GetProductsAsync(
                        context.StoreId,
                        context.Language,
                        query,
                        responseGroups);

            if (!response.Items.Any()) // no results found
            {
                return new SearchResults<T>(Enumerable.Empty<T>());
            }

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
                    Price = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M,
                    ProductId = item.Id,
                    Quantity = 1
                });
            }
            promoContext.PromoEntries = promoEntries;

            var rewards = await _marketingClient.GetPromotionRewardsAsync(promoContext);

            var inventories = await this.GetItemsInventoriesAsync(allIds);

            var result = new SearchResults<T>(response.Items.Select(i => i.AsWebModel(prices, rewards, inventories, parentCollection)).OfType<T>()) { TotalCount = response.TotalCount };

            if (response.Facets != null && response.Facets.Any())
                result.Facets = response.Facets.Select(x => x.AsWebModel()).ToArray();

            return result;
        }

        public Theme GetTheme(SiteContext context, string themeName)
        {
            var id = themeName;
            var name = themeName;
            if (id.Contains("/"))
            {
                name = id.Split('/')[1];
            }

            Theme currentTheme = null;
            if (context.Themes != null)
            {
                currentTheme = context.Themes.SingleOrDefault(x => x.Id.Equals(themeName, StringComparison.OrdinalIgnoreCase));
            }

            return currentTheme ?? new Theme { Id = id, Name = name, Role = "main", Path = id };
        }

        public async Task<Theme[]> GetThemesAsync(SiteContext context)
        {
            var store = context.StoreId;
            var response = await this._themeClient.GetThemesAsync(store);
            return response.AsWebModel();
        }

        public async Task UpdateThemeCacheAsync(SiteContext context)
        {
            var store = context.StoreId;
            var theme = context.Theme.Name;
            var themePath = String.Format("{0}\\{1}", _themesCacheStoragePath, context.Theme.Path);
            var themeStorageClient = new FileStorageCacheService(HostingEnvironment.MapPath(themePath));
            var pagesStorageClient = new FileStorageCacheService(HostingEnvironment.MapPath(String.Format("~/App_Data/Pages/{0}", store)));

            // get last updated for both pages or theme files
            var themeLastUpdated = themeStorageClient.GetLatestUpdate();
            var pagesLastUpdated = pagesStorageClient.GetLatestUpdate();

            var response = await this._storeClient.GetStoreAssetsAsync(store, theme, themeLastUpdated, pagesLastUpdated);

            if (response.Any())
            {
                lock (_LockObject)
                {
                    // check last update again, since going to the service is more expensive than checking local folders
                    var newThemeLastUpdated = themeStorageClient.GetLatestUpdate();
                    var newPagesLastUpdated = pagesStorageClient.GetLatestUpdate();

                    if (newThemeLastUpdated == themeLastUpdated && newPagesLastUpdated == pagesLastUpdated)
                    {
                        foreach (var syncAssetGroup in response)
                        {
                            if (syncAssetGroup.Type == "theme")
                            {
                                var reload = themeStorageClient.ApplyUpdates(syncAssetGroup.AsFileModel());
                                if (reload)
                                {
                                    this._viewLocator.UpdateCache();
                                }
                            }
                            else if (syncAssetGroup.Type == "pages")
                            {
                                pagesStorageClient.ApplyUpdates(syncAssetGroup.AsFileModel());
                            }
                        }
                    }
                }
            }
        }

        public async Task<ResponseCollection<DynamicContentItemGroup>> GetDynamicContentAsync(string[] placeholders)
        {
            var tagQuery = new TagQuery();

            var customerTags = SiteContext.Current.Customer != null ? SiteContext.Current.Customer.Tags : null;

            if (customerTags != null)
            {
                foreach (var tag in customerTags)
                {
                    tagQuery.Add("customer_tag", tag.ToString());
                }
            }

            return await _marketingClient.GetDynamicContentAsync(placeholders, tagQuery);
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