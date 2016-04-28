using System;
using System.Web.Mvc;
using System.Web.Routing;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Routing;

namespace VirtoCommerce.Storefront
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, IStaticContentService staticContentService, ILocalCacheManager cacheManager)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // We disable simultanous using Convention and Attribute routing because we have SEO slug urls and optional Store and Languages url parts 
            // this leads to different kinds of collisions and we deside to use only Convention routing 
            //routes.MapMvcAttributeRoutes();

            #region Storefront API routes

            // API cart
            routes.AddStorefrontRoute("API.GetCart", "storefrontapi/cart", defaults: new { controller = "ApiCart", action = "GetCart" });
            routes.AddStorefrontRoute("API.Cart.GetCartItemsCount", "storefrontapi/cart/itemscount", defaults: new { controller = "ApiCart", action = "GetCartItemsCount" });
            routes.AddStorefrontRoute("API.Cart.AddItemToCart", "storefrontapi/cart/items", defaults: new { controller = "ApiCart", action = "AddItemToCart" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.Cart.ChangeCartItem", "storefrontapi/cart/items", defaults: new { controller = "ApiCart", action = "ChangeCartItem" }, constraints: new { httpMethod = new HttpMethodConstraint("PUT") });
            routes.AddStorefrontRoute("API.Cart.RemoveCartItem", "storefrontapi/cart/items", defaults: new { controller = "ApiCart", action = "RemoveCartItem" }, constraints: new { httpMethod = new HttpMethodConstraint("DELETE") });
            routes.AddStorefrontRoute("API.Cart.ClearCart", "storefrontapi/cart/clear", defaults: new { controller = "ApiCart", action = "ClearCart" });
            routes.AddStorefrontRoute("API.Cart.GetCartShipmentAvailShippingMethods", "storefrontapi/cart/shipments/{shipmentId}/shippingmethods", defaults: new { controller = "ApiCart", action = "GetCartShipmentAvailShippingMethods" });
            routes.AddStorefrontRoute("API.Cart.GetCartAvailPaymentMethods", "storefrontapi/cart/paymentmethods", defaults: new { controller = "ApiCart", action = "GetCartAvailPaymentMethods" });
            routes.AddStorefrontRoute("API.Cart.AddCartCoupon", "storefrontapi/cart/coupons/{couponCode}", defaults: new { controller = "ApiCart", action = "AddCartCoupon" });
            routes.AddStorefrontRoute("API.Cart.RemoveCartCoupon", "storefrontapi/cart/coupons", defaults: new { controller = "ApiCart", action = "RemoveCartCoupon" });
            routes.AddStorefrontRoute("API.Cart.AddOrUpdateCartShipment", "storefrontapi/cart/shipments", defaults: new { controller = "ApiCart", action = "AddOrUpdateCartShipment" });
            routes.AddStorefrontRoute("API.Cart.AddOrUpdateCartPayment", "storefrontapi/cart/payments", defaults: new { controller = "ApiCart", action = "AddOrUpdateCartPayment" });
            routes.AddStorefrontRoute("API.Cart.CreateOrder", "storefrontapi/cart/createorder", defaults: new { controller = "ApiCart", action = "CreateOrder" });

            // Catalog API
            routes.AddStorefrontRoute("API.Catalog.SearchProducts", "storefrontapi/catalog/search", defaults: new { controller = "ApiCatalog", action = "SearchProducts" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.Catalog.GetProductsByIds", "storefrontapi/products", defaults: new { controller = "ApiCatalog", action = "GetProductsByIds" });
            routes.AddStorefrontRoute("API.Catalog.SearchCategories", "storefrontapi/categories/search", defaults: new { controller = "ApiCatalog", action = "SearchCategories" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.Catalog.GetCategoriesByIds", "storefrontapi/categories", defaults: new { controller = "ApiCatalog", action = "GetCategoriesByIds" });

            // Common storefront API
            routes.AddStorefrontRoute("API.Common.GetCountries", "storefrontapi/countries", defaults: new { controller = "ApiCommon", action = "GetCountries" });
            routes.AddStorefrontRoute("API.Common.GetCountryRegions", "storefrontapi/countries/{countryCode}/regions", defaults: new { controller = "ApiCommon", action = "GetCountryRegions" });
            routes.AddStorefrontRoute("API.Common.Feedback", "storefrontapi/feedback", defaults: new { controller = "ApiCommon", action = "Feedback" });

            // Pricing API
            routes.AddStorefrontRoute("API.Pricing.GetActualProductPrices", "storefrontapi/pricing/actualprices", defaults: new { controller = "ApiPricing", action = "GetActualProductPrices" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });

            // Marketing API
            routes.AddStorefrontRoute("API.Marketing.GetDynamicContent", "storefrontapi/marketing/dynamiccontent/{placeName}", defaults: new { controller = "ApiMarketing", action = "GetDynamicContent" });

            // Account API
            routes.AddStorefrontRoute("API.Account.GetCurrentCustomer", "storefrontapi/account", defaults: new { controller = "ApiAccount", action = "GetCurrentCustomer" });

            // Quote requests API
            routes.AddStorefrontRoute("API.QuoteRequest.GetItemsCount", "storefrontapi/quoterequests/{number}/itemscount", defaults: new { controller = "ApiQuoteRequest", action = "GetItemsCount" });
            routes.AddStorefrontRoute("API.QuoteRequest.Get", "storefrontapi/quoterequests/{number}", defaults: new { controller = "ApiQuoteRequest", action = "Get" });
            routes.AddStorefrontRoute("API.QuoteRequest.GetCurrent", "storefrontapi/quoterequest/current", defaults: new { controller = "ApiQuoteRequest", action = "GetCurrent" });
            routes.AddStorefrontRoute("API.QuoteRequest.AddItem", "storefrontapi/quoterequests/current/items", defaults: new { controller = "ApiQuoteRequest", action = "AddItem" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.QuoteRequest.RemoveItem", "storefrontapi/quoterequests/{number}/items/{itemId}", defaults: new { controller = "ApiQuoteRequest", action = "RemoveItem" }, constraints: new { httpMethod = new HttpMethodConstraint("DELETE") });
            routes.AddStorefrontRoute("API.QuoteRequest.Update", "storefrontapi/quoterequests/{number}", defaults: new { controller = "ApiQuoteRequest", action = "Update" }, constraints: new { httpMethod = new HttpMethodConstraint("PUT") });
            routes.AddStorefrontRoute("API.QuoteRequest.Submit", "storefrontapi/quoterequests/{number}/submit", defaults: new { controller = "ApiQuoteRequest", action = "Submit" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.QuoteRequest.Reject", "storefrontapi/quoterequests/{number}/reject", defaults: new { controller = "ApiQuoteRequest", action = "Reject" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.QuoteRequest.CalculateTotals", "storefrontapi/quoterequests/{number}/totals", defaults: new { controller = "ApiQuoteRequest", action = "CalculateTotals" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("API.QuoteRequest.Confirm", "storefrontapi/quoterequests/{number}/confirm", defaults: new { controller = "ApiQuoteRequest", action = "Confirm" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });

            #endregion

            // Account
            routes.AddStorefrontRoute("Account", "account", defaults: new { controller = "Account", action = "GetAccount" }, constraints: new { httpMethod = new HttpMethodConstraint("GET") });
            routes.AddStorefrontRoute("Account.UpdateAccount", "account", defaults: new { controller = "Account", action = "UpdateAccount" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("Account.GetOrderDetails ", "account/order/{number}", defaults: new { controller = "Account", action = "GetOrderDetails" });
            routes.AddStorefrontRoute("Account.UpdateAddress", "account/addresses/{id}", defaults: new { controller = "Account", action = "UpdateAddress", id = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("Account.GetAddresses", "account/addresses", defaults: new { controller = "Account", action = "GetAddresses" }, constraints: new { httpMethod = new HttpMethodConstraint("GET") });
            routes.AddStorefrontRoute("Account.Register", "account/register", defaults: new { controller = "Account", action = "Register" });
            routes.AddStorefrontRoute("Account.Login", "account/login", defaults: new { controller = "Account", action = "Login" });
            routes.AddStorefrontRoute("Account.Logout", "account/logout", defaults: new { controller = "Account", action = "Logout" });
            routes.AddStorefrontRoute("Account.ForgotPassword", "account/forgotpassword", defaults: new { controller = "Account", action = "ForgotPassword" });
            routes.AddStorefrontRoute("Account.ResetPassword", "account/resetpassword", defaults: new { controller = "Account", action = "ResetPassword" });
            routes.AddStorefrontRoute("Account.ChangePassword", "account/password", defaults: new { controller = "Account", action = "ChangePassword" });
            routes.AddStorefrontRoute("Account.ExternalLogin", "account/externallogin", defaults: new { controller = "Account", action = "ExternalLogin" });
            routes.AddStorefrontRoute("Account.ExternalLoginCallback", "account/externallogincallback", defaults: new { controller = "Account", action = "ExternalLoginCallback" });

            // Cart
            routes.AddStorefrontRoute("Cart.Index", "cart", defaults: new { controller = "Cart", action = "Index" }, constraints: new { httpMethod = new HttpMethodConstraint("GET") });
            routes.AddStorefrontRoute("Cart.Checkout", "cart/checkout", defaults: new { controller = "Cart", action = "Checkout" });
            routes.AddStorefrontRoute("Cart.ExternalPaymentCallback", "cart/externalpaymentcallback", defaults: new { controller = "Cart", action = "ExternalPaymentCallback" });
            routes.AddStorefrontRoute("Cart.Thanks", "cart/thanks/{orderNumber}", defaults: new { controller = "Cart", action = "Thanks" });
            routes.AddStorefrontRoute("Cart.PaymentForm", "cart/checkout/paymentform", defaults: new { controller = "Cart", action = "PaymentForm" });

            // Cart (Shopify compatible)
            routes.AddStorefrontRoute("ShopifyCart.Cart", "cart", defaults: new { controller = "ShopifyCompatibility", action = "Cart" }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("ShopifyCart.CartJs", "cart.js", defaults: new { controller = "ShopifyCompatibility", action = "CartJs" });
            routes.AddStorefrontRoute("ShopifyCart.Add", "cart/add", defaults: new { controller = "ShopifyCompatibility", action = "Add" });
            routes.AddStorefrontRoute("ShopifyCart.AddJs", "cart/add.js", defaults: new { controller = "ShopifyCompatibility", action = "AddJs" });
            routes.AddStorefrontRoute("ShopifyCart.Change", "cart/change", defaults: new { controller = "ShopifyCompatibility", action = "Change" });
            routes.AddStorefrontRoute("ShopifyCart.ChangeJs", "cart/change.js", defaults: new { controller = "ShopifyCompatibility", action = "ChangeJs" });
            routes.AddStorefrontRoute("ShopifyCart.Clear", "cart/clear", defaults: new { controller = "ShopifyCompatibility", action = "Clear" }, constraints: new { httpMethod = new HttpMethodConstraint("GET") });
            routes.AddStorefrontRoute("ShopifyCart.ClearJs", "cart/clear.js", defaults: new { controller = "ShopifyCompatibility", action = "ClearJs" });
            routes.AddStorefrontRoute("ShopifyCart.UpdateJs", "cart/update.js", defaults: new { controller = "ShopifyCompatibility", action = "UpdateJs" });

            // QuoteRequest
            routes.AddStorefrontRoute("QuoteRequest.CurrentQuoteRequest", "quoterequest", defaults: new { controller = "QuoteRequest", action = "CurrentQuoteRequest" });
            routes.AddStorefrontRoute("Account.QuoteRequests", "account/quoterequests", defaults: new { controller = "QuoteRequest", action = "QuoteRequests" });
            routes.AddStorefrontRoute("Account.QuoteRequestByNumber", "quoterequest/{number}", defaults: new { controller = "QuoteRequest", action = "QuoteRequestByNumber" });

            // CatalogSearch
            routes.AddStorefrontRoute("CatalogSearch.CategoryBrowsing", "search/{categoryId}", defaults: new { controller = "CatalogSearch", action = "CategoryBrowsing" });
            routes.AddStorefrontRoute("CatalogSearch.SearchProducts", "search", defaults: new { controller = "CatalogSearch", action = "SearchProducts" });

            // Common
            routes.AddStorefrontRoute("Common.SetCurrency", "common/setcurrency/{currency}", defaults: new { controller = "Common", action = "SetCurrency" });
            routes.AddStorefrontRoute("Common.ContactUsPost", "contact/{viewName}", defaults: new { controller = "Common", action = "СontactUs", viewName = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint("POST") });
            routes.AddStorefrontRoute("Common.ContactUs", "contact/{viewName}", defaults: new { controller = "Common", action = "СontactUs", viewName = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint("GET") });
            routes.AddStorefrontRoute("Common.NoStore", "common/nostore", defaults: new { controller = "Common", action = "NoStore" });
            routes.AddStorefrontRoute("Common.Maintenance", "maintenance", defaults: new { controller = "Common", action = "Maintenance" });

            // Category routes
            routes.AddStorefrontRoute("Category.BrowseCategory", "category/{categoryId}", defaults: new { controller = "CatalogSearch", action = "CategoryBrowsing" });

            // Product routes
            routes.AddStorefrontRoute("Product.GetProduct", "product/{productId}", defaults: new { controller = "Product", action = "ProductDetails" });
           
            // Assets
            routes.AddStorefrontRoute("Assets", "themes/assets/{*asset}", defaults: new { controller = "Asset", action = "GetAssets" });
            routes.AddStorefrontRoute("GlobalAssets", "themes/global/assets/{*asset}", defaults: new { controller = "Asset", action = "GetGlobalAssets" });

            // Static content (no cms)
            routes.AddStorefrontRoute("Pages.GetPage", "pages/{*page}", defaults: new { controller = "Page", action = "GetContentPageByName" });
            routes.AddStorefrontRoute("Blogs.GetBlog", "blogs/{blog}", defaults: new { controller = "Blog", action = "GetBlog" });
            routes.AddStorefrontRoute("Blogs.GetBlogArticle", "blogs/{blog}/{article}", defaults: new { controller = "Blog", action = "GetBlogArticle" });

            Func<string, Route> seoRouteFactory = url => new SeoRoute(url, new MvcRouteHandler(), workContextFactory, commerceCoreApi, cacheManager);
            routes.AddStorefrontRoute(name: "SeoRoute", url: "{*path}", defaults: new { controller = "StorefrontHome", action = "Index" }, constraints: null, routeFactory: seoRouteFactory);
        }
    }
}
