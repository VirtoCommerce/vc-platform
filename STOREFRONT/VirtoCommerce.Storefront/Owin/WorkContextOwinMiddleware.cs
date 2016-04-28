using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.LinkList.Services;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Model.StaticContent;

namespace VirtoCommerce.Storefront.Owin
{
    /// <summary>
    /// Populate main work context with such commerce data as store, user profile, cart, etc.
    /// </summary>
    public sealed class WorkContextOwinMiddleware : OwinMiddleware
    {
        private static readonly Country[] _allCountries = GetAllCounries();

        private readonly IStoreModuleApi _storeApi;
        private readonly ICommerceCoreModuleApi _commerceApi;
        private readonly IPricingModuleApi _pricingModuleApi;
        private readonly IQuoteRequestBuilder _quoteRequestBuilder;
        private readonly ILocalCacheManager _cacheManager;
        private readonly IStaticContentService _staticContentService;

        private readonly UnityContainer _container;

        public WorkContextOwinMiddleware(OwinMiddleware next, UnityContainer container)
            : base(next)
        {
            //Be AWARE! WorkContextOwinMiddleware crated once in first application start
            //and  there can not be resolved and stored in fields services using WorkContext as dependency (WorkCOntext has a per request lifetime)
            _storeApi = container.Resolve<IStoreModuleApi>();
            _quoteRequestBuilder = container.Resolve<IQuoteRequestBuilder>();
            _pricingModuleApi = container.Resolve<IPricingModuleApi>();
            _commerceApi = container.Resolve<ICommerceCoreModuleApi>();
            _cacheManager = container.Resolve<ILocalCacheManager>();
            _staticContentService = container.Resolve<IStaticContentService>();
            _container = container;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (IsStorefrontRequest(context.Request))
            {
                var workContext = _container.Resolve<WorkContext>();

                var linkListService = _container.Resolve<IMenuLinkListService>();
                var cartBuilder = _container.Resolve<ICartBuilder>();
                var catalogSearchService = _container.Resolve<ICatalogSearchService>();

                // Initialize common properties
                workContext.RequestUrl = context.Request.Uri;
                workContext.AllCountries = _allCountries;
                workContext.AllStores = await _cacheManager.GetAsync("GetAllStores", "ApiRegion", async () => await GetAllStoresAsync());
                if (workContext.AllStores != null && workContext.AllStores.Any())
                {
                    // Initialize request specific properties
                    workContext.CurrentStore = GetStore(context, workContext.AllStores);
                    workContext.CurrentLanguage = GetLanguage(context, workContext.AllStores, workContext.CurrentStore);
                    workContext.AllCurrencies = await _cacheManager.GetAsync("GetAllCurrencies-" + workContext.CurrentLanguage.CultureName, "ApiRegion", async () => { return (await _commerceApi.CommerceGetAllCurrenciesAsync()).Select(x => x.ToWebModel(workContext.CurrentLanguage)).ToArray(); });
                    //Sync store currencies with avail in system
                    foreach (var store in workContext.AllStores)
                    {
                        store.SyncCurrencies(workContext.AllCurrencies, workContext.CurrentLanguage);
                        store.CurrentSeoInfo = store.SeoInfos.FirstOrDefault(x => x.Language == workContext.CurrentLanguage);
                    }

                    //Set current currency
                    workContext.CurrentCurrency = GetCurrency(context, workContext.CurrentStore);

                    var qs = HttpUtility.ParseQueryString(workContext.RequestUrl.Query);
                    //Initialize catalog search criteria
                    workContext.CurrentCatalogSearchCriteria = new CatalogSearchCriteria(workContext.CurrentLanguage, workContext.CurrentCurrency, qs)
                    {
                        CatalogId = workContext.CurrentStore.Catalog
                    };

                    //This line make delay categories loading initialization (categories can be evaluated on view rendering time)
                    workContext.Categories = new MutablePagedList<Category>((pageNumber, pageSize) =>
                    {
                        var criteria = workContext.CurrentCatalogSearchCriteria.Clone();
                        criteria.PageNumber = pageNumber;
                        criteria.PageSize = pageSize;
                        var result = catalogSearchService.SearchCategories(criteria);
                        foreach (var category in result)
                        {
                            category.Products = new MutablePagedList<Product>((pageNumber2, pageSize2) =>
                            {
                                criteria.CategoryId = category.Id;
                                criteria.PageNumber = pageNumber2;
                                criteria.PageSize = pageSize2;
                                var searchResult = catalogSearchService.SearchProducts(criteria);
                                //Because catalog search products returns also aggregations we can use it to populate workContext using C# closure
                                //now workContext.Aggregation will be contains preloaded aggregations for current category
                                workContext.Aggregations = new MutablePagedList<Aggregation>(searchResult.Aggregations);
                                return searchResult.Products;
                            });
                        }
                        return result;
                    });
                    //This line make delay products loading initialization (products can be evaluated on view rendering time)
                    workContext.Products = new MutablePagedList<Product>((pageNumber, pageSize) =>
                    {
                        var criteria = workContext.CurrentCatalogSearchCriteria.Clone();
                        criteria.PageNumber = pageNumber;
                        criteria.PageSize = pageSize;

                        var result = catalogSearchService.SearchProducts(criteria);
                        //Prevent double api request for get aggregations
                        //Because catalog search products returns also aggregations we can use it to populate workContext using C# closure
                        //now workContext.Aggregation will be contains preloaded aggregations for current search criteria
                        workContext.Aggregations = new MutablePagedList<Aggregation>(result.Aggregations);
                        return result.Products;
                    });
                    //This line make delay aggregation loading initialization (aggregation can be evaluated on view rendering time)
                    workContext.Aggregations = new MutablePagedList<Aggregation>((pageNumber, pageSize) =>
                    {
                        var criteria = workContext.CurrentCatalogSearchCriteria.Clone();
                        criteria.PageNumber = pageNumber;
                        criteria.PageSize = pageSize;
                        //Force to load products and its also populate workContext.Aggregations by preloaded values
                        workContext.Products.Slice(pageNumber, pageSize);
                        return workContext.Aggregations;
                    });

                    workContext.CurrentOrderSearchCriteria = new Model.Order.OrderSearchCriteria(qs);
                    workContext.CurrentQuoteSearchCriteria = new Model.Quote.QuoteSearchCriteria(qs);

                    //Get current customer
                    workContext.CurrentCustomer = await GetCustomerAsync(context);
                    //Validate that current customer has to store access
                    ValidateUserStoreLogin(context, workContext.CurrentCustomer, workContext.CurrentStore);
                    MaintainAnonymousCustomerCookie(context, workContext);

                    // Gets the collection of external login providers
                    var externalAuthTypes = context.Authentication.GetExternalAuthenticationTypes();

                    workContext.ExternalLoginProviders = externalAuthTypes.Select(at => new LoginProvider
                    {
                        AuthenticationType = at.AuthenticationType,
                        Caption = at.Caption,
                        Properties = at.Properties
                    }).ToList();

                    workContext.ApplicationSettings = GetApplicationSettings();

                    //Do not load shopping cart and other for resource requests
                    if (!IsAssetRequest(context.Request))
                    {
                        //Shopping cart
                        await cartBuilder.GetOrCreateNewTransientCartAsync(workContext.CurrentStore, workContext.CurrentCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency);
                        workContext.CurrentCart = cartBuilder.Cart;

                        if (workContext.CurrentStore.QuotesEnabled)
                        {
                            await _quoteRequestBuilder.GetOrCreateNewTransientQuoteRequestAsync(workContext.CurrentStore, workContext.CurrentCustomer, workContext.CurrentLanguage, workContext.CurrentCurrency);
                            workContext.CurrentQuoteRequest = _quoteRequestBuilder.QuoteRequest;
                        }

                        var linkLists = await _cacheManager.GetAsync("GetAllStoreLinkLists-" + workContext.CurrentStore.Id, "ApiRegion", async () => await linkListService.LoadAllStoreLinkListsAsync(workContext.CurrentStore.Id));
                        workContext.CurrentLinkLists = linkLists.Where(x => x.Language == workContext.CurrentLanguage).ToList();
                        // load all static content
                        var staticContents = _cacheManager.Get(string.Join(":", "AllStoreStaticContent", workContext.CurrentStore.Id), "ContentRegion", () =>
                        {
                            var allContentItems = _staticContentService.LoadStoreStaticContent(workContext.CurrentStore).ToList();
                            var blogs = allContentItems.OfType<Blog>().ToArray();
                            var blogArticlesGroup = allContentItems.OfType<BlogArticle>().GroupBy(x => x.BlogName, x => x).ToList();

                            foreach (var blog in blogs)
                            {
                                var blogArticles = blogArticlesGroup.FirstOrDefault(x => string.Equals(x.Key, blog.Name, StringComparison.OrdinalIgnoreCase));
                                if (blogArticles != null)
                                {
                                    blog.Articles = new MutablePagedList<BlogArticle>(blogArticles);
                                }
                            }

                            return new { Pages = allContentItems, Blogs = blogs };
                        });
                        workContext.Pages = new MutablePagedList<ContentItem>(staticContents.Pages);
                        workContext.Blogs = new MutablePagedList<Blog>(staticContents.Blogs);

                        // Initialize blogs search criteria 
                        workContext.CurrentBlogSearchCritera = new BlogSearchCriteria(qs);

                        //Pricelists
                        var pricelistCacheKey = string.Join("-", "EvaluatePriceLists", workContext.CurrentStore.Id, workContext.CurrentCustomer.Id);
                        workContext.CurrentPricelists = await _cacheManager.GetAsync(pricelistCacheKey, "ApiRegion", async () =>
                        {
                            var evalContext = new VirtoCommerceDomainPricingModelPriceEvaluationContext
                            {
                                StoreId = workContext.CurrentStore.Id,
                                CatalogId = workContext.CurrentStore.Catalog,
                                CustomerId = workContext.CurrentCustomer.Id,
                                Quantity = 1
                            };
                            var pricingResult = await _pricingModuleApi.PricingModuleEvaluatePriceListsAsync(evalContext);
                            return pricingResult.Select(p => p.ToWebModel()).ToList();
                        });
                    }
                }
            }

            await Next.Invoke(context);
        }


        private bool IsStorefrontRequest(IOwinRequest request)
        {
            return !request.Path.StartsWithSegments(new PathString("/admin"))
                && !request.Path.StartsWithSegments(new PathString("/areas/admin"))
                && !request.Path.StartsWithSegments(new PathString("/api"))
                && !request.Path.StartsWithSegments(new PathString("/assets"))
                && !request.Path.StartsWithSegments(new PathString("/favicon.ico"));
        }

        private async Task<Store[]> GetAllStoresAsync()
        {
            var stores = await _storeApi.StoreModuleGetStoresAsync();
            var result = stores.Select(s => s.ToWebModel()).ToArray();
            return result.Any() ? result : null;
        }


        private bool IsAssetRequest(IOwinRequest request)
        {
            var retVal = string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase);
            if (retVal)
            {
                retVal = request.Uri.AbsolutePath.Contains("themes/assets") || !string.IsNullOrEmpty(Path.GetExtension(request.Uri.ToString()));
            }
            return retVal;
        }

        private void ValidateUserStoreLogin(IOwinContext context, CustomerInfo customer, Store currentStore)
        {

            if (customer.IsRegisteredUser && !customer.AllowedStores.IsNullOrEmpty()
                && !customer.AllowedStores.Any(x => string.Equals(x, currentStore.Id, StringComparison.InvariantCultureIgnoreCase)))
            {
                context.Authentication.SignOut();
                context.Authentication.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            }
        }

        private async Task<CustomerInfo> GetCustomerAsync(IOwinContext context)
        {
            var retVal = new CustomerInfo();

            var principal = context.Authentication.User;
            var identity = principal.Identity;

            if (identity.IsAuthenticated)
            {
                var userId = identity.GetUserId();
                if (userId == null)
                {
                    //If somehow claim not found in user cookies need load user by name from API
                    var user = await _commerceApi.StorefrontSecurityGetUserByNameAsync(identity.Name);
                    if (user != null)
                    {
                        userId = user.Id;
                    }
                }

                if (userId != null)
                {
                    var customerService = _container.Resolve<ICustomerService>();
                    var customer = await customerService.GetCustomerByIdAsync(userId);
                    retVal = customer ?? retVal;
                    retVal.Id = userId;
                    retVal.UserName = identity.Name;
                    retVal.IsRegisteredUser = true;
                }

                retVal.OperatorUserId = principal.FindFirstValue(StorefrontConstants.OperatorUserIdClaimType);
                retVal.OperatorUserName = principal.FindFirstValue(StorefrontConstants.OperatorUserNameClaimType);

                var allowedStores = principal.FindFirstValue(StorefrontConstants.AllowedStoresClaimType);
                if (!string.IsNullOrEmpty(allowedStores))
                {
                    retVal.AllowedStores = allowedStores.Split(',');
                }
            }

            if (!retVal.IsRegisteredUser)
            {
                retVal.Id = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];
                retVal.UserName = StorefrontConstants.AnonymousUsername;
                retVal.FullName = StorefrontConstants.AnonymousUsername;
            }

            return retVal;
        }

        private void MaintainAnonymousCustomerCookie(IOwinContext context, WorkContext workContext)
        {
            string anonymousCustomerId = context.Request.Cookies[StorefrontConstants.AnonymousCustomerIdCookie];

            if (workContext.CurrentCustomer.IsRegisteredUser)
            {
                if (!string.IsNullOrEmpty(anonymousCustomerId))
                {
                    // Remove anonymous customer cookie for registered customer
                    context.Response.Cookies.Append(StorefrontConstants.AnonymousCustomerIdCookie, string.Empty, new CookieOptions { Expires = DateTime.UtcNow.AddDays(-30) });
                }
            }
            else
            {
                if (string.IsNullOrEmpty(anonymousCustomerId))
                {
                    // Add anonymous customer cookie for nonregistered customer
                    anonymousCustomerId = Guid.NewGuid().ToString();
                    workContext.CurrentCustomer.Id = anonymousCustomerId;
                    context.Response.Cookies.Append(StorefrontConstants.AnonymousCustomerIdCookie, anonymousCustomerId, new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) });
                }
            }
        }

        private Store GetStore(IOwinContext context, ICollection<Store> stores)
        {
            //Remove store name from url need to prevent writing store in routing
            var storeId = GetStoreIdFromUrl(context, stores);

            if (string.IsNullOrEmpty(storeId))
            {
                storeId = context.Request.Cookies[StorefrontConstants.StoreCookie];
            }

            if (string.IsNullOrEmpty(storeId))
            {
                storeId = ConfigurationManager.AppSettings["DefaultStore"];
            }

            var store = stores.FirstOrDefault(s => string.Equals(s.Id, storeId, StringComparison.OrdinalIgnoreCase));

            if (store == null)
            {
                store = stores.FirstOrDefault();
            }

            return store;
        }

        private string GetStoreIdFromUrl(IOwinContext context, ICollection<Store> stores)
        {
            //Try first find by store url (if it defined)
            string retVal = null;

            foreach (var store in stores)
            {
                var pathString = new PathString("/" + store.Id);
                PathString remainingPath;
                if (context.Request.Path.StartsWithSegments(pathString, out remainingPath))
                {
                    retVal = store.Id;
                    break;
                }
            }

            if (retVal == null)
            {
                retVal = stores.Where(x => x.IsStoreUrl(context.Request.Uri)).Select(x => x.Id).FirstOrDefault();
            }

            return retVal;
        }

        private Language GetLanguage(IOwinContext context, ICollection<Store> stores, Store store)
        {
            var languages = stores.SelectMany(s => s.Languages)
                .Union(stores.Select(s => s.DefaultLanguage))
                .Select(x => x.CultureName)
                .Distinct()
                .ToArray();

            //Get language from request url and remove it from from url need to prevent writing language in routing
            var languageCode = GetLanguageFromUrl(context, languages);

            //Get language from Cookies
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = context.Request.Cookies[StorefrontConstants.LanguageCookie];
            }
            var retVal = store.DefaultLanguage;
            //Get store default language if language not in the supported by stores list
            if (!string.IsNullOrEmpty(languageCode))
            {
                var language = new Language(languageCode);
                retVal = store.Languages.Contains(language) ? language : retVal;
            }
            return retVal;
        }

        private string GetLanguageFromUrl(IOwinContext context, string[] languages)
        {
            var requestPath = context.Request.Path.ToString();
            var regexpPattern = string.Format(@"\/({0})\/?", string.Join("|", languages));
            var match = Regex.Match(requestPath, regexpPattern, RegexOptions.IgnoreCase);
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            return null;
        }

        private static Currency GetCurrency(IOwinContext context, Store store)
        {
            //Get currency from request url
            var currencyCode = context.Request.Query.Get("currency");
            //Next try get from Cookies
            if (string.IsNullOrEmpty(currencyCode))
            {
                currencyCode = context.Request.Cookies[StorefrontConstants.CurrencyCookie];
            }

            var retVal = store.DefaultCurrency;
            //Get store default currency if currency not in the supported by stores list
            if (!string.IsNullOrEmpty(currencyCode))
            {
                retVal = store.Currencies.FirstOrDefault(x => x.Equals(currencyCode)) ?? retVal;
            }
            return retVal;
        }

        private static Country[] GetAllCounries()
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                .Select(GetRegionInfo)
                .Where(r => r != null)
                .ToList();

            var countriesJson = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/countries.json"));
            var countriesDict = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(countriesJson);

            var countries = countriesDict
                .Select(kvp => ParseCountry(kvp, regions))
                .Where(c => c.Code3 != null)
                .ToArray();

            return countries;
        }

        private static RegionInfo GetRegionInfo(CultureInfo culture)
        {
            RegionInfo result = null;

            try
            {
                result = new RegionInfo(culture.LCID);
            }
            catch
            {
                // ignored
            }

            return result;
        }

        private static Country ParseCountry(KeyValuePair<string, JObject> pair, List<RegionInfo> regions)
        {
            var region = regions.FirstOrDefault(r => string.Equals(r.EnglishName, pair.Key, StringComparison.OrdinalIgnoreCase));

            var country = new Country
            {
                Name = pair.Key,
                Code2 = region != null ? region.TwoLetterISORegionName : string.Empty,
                Code3 = region != null ? region.ThreeLetterISORegionName : string.Empty,
                RegionType = pair.Value["label"] != null ? pair.Value["label"].ToString() : null
            };

            var provinceCodes = pair.Value["province_codes"].ToObject<Dictionary<string, string>>();
            if (provinceCodes != null && provinceCodes.Any())
            {
                country.Regions = provinceCodes
                    .Select(kvp => new CountryRegion { Name = kvp.Key, Code = kvp.Value })
                    .ToArray();
            }

            return country;
        }

        private IDictionary<string, object> GetApplicationSettings()
        {
            var appSettings = new Dictionary<string, object>();

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                appSettings.Add(key, ConfigurationManager.AppSettings[key]);
            }

            return appSettings;
        }
    }
}
