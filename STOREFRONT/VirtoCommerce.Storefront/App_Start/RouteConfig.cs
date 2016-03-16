using System;
using System.Web.Mvc;
using System.Web.Routing;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Routing;
using VirtoCommerce.Storefront.Controllers.Api;

namespace VirtoCommerce.Storefront
{
    public class RouteConfig
    {
        
        public static void RegisterRoutes(RouteCollection routes, Func<WorkContext> workContextFactory, ICommerceCoreModuleApi commerceCoreApi, IStaticContentService staticContentService, ICacheManager<object> cacheManager)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //We disable simultanous using Convention and Attribute routing because we have SEO slug urls and optional Store and Languages url parts 
            //this leads to different kinds of collisions and we deside to use only Convention routing 

            //routes.MapMvcAttributeRoutes();

            #region Storefront API routes

            //API cart
            routes.MapLocalizedStorefrontRoute("API.GetCart", "storefrontapi/cart", defaults: new { controller = "ApiCart", action = "GetCart" });
            routes.MapLocalizedStorefrontRoute("API.Cart.GetCartItemsCount", "storefrontapi/cart/itemscount", defaults: new { controller = "ApiCart", action = "GetCartItemsCount" });
            routes.MapLocalizedStorefrontRoute("API.Cart.AddItemToCart", "storefrontapi/cart/items", defaults: new { controller = "ApiCart", action = "AddItemToCart" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.Cart.ChangeCartItem", "storefrontapi/cart/items", defaults: new { controller = "ApiCart", action = "ChangeCartItem" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "PUT" }) });
            routes.MapLocalizedStorefrontRoute("API.Cart.RemoveCartItem", "storefrontapi/cart/items", defaults: new { controller = "ApiCart", action = "RemoveCartItem" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "DELETE" }) });
            routes.MapLocalizedStorefrontRoute("API.Cart.ClearCart", "storefrontapi/cart/clear", defaults: new { controller = "ApiCart", action = "ClearCart" });
            routes.MapLocalizedStorefrontRoute("API.Cart.GetCartShipmentAvailShippingMethods", "storefrontapi/cart/shipments/{shipmentId}/shippingmethods", defaults: new { controller = "ApiCart", action = "GetCartShipmentAvailShippingMethods" });
            routes.MapLocalizedStorefrontRoute("API.Cart.GetCartAvailPaymentMethods", "storefrontapi/cart/paymentmethods", defaults: new { controller = "ApiCart", action = "GetCartAvailPaymentMethods" });
            routes.MapLocalizedStorefrontRoute("API.Cart.AddCartCoupon", "storefrontapi/cart/coupons/{couponCode}", defaults: new { controller = "ApiCart", action = "AddCartCoupon" });
            routes.MapLocalizedStorefrontRoute("API.Cart.RemoveCartCoupon", "storefrontapi/cart/coupons", defaults: new { controller = "ApiCart", action = "RemoveCartCoupon" });
            routes.MapLocalizedStorefrontRoute("API.Cart.AddOrUpdateCartShipment", "storefrontapi/cart/shipments", defaults: new { controller = "ApiCart", action = "AddOrUpdateCartShipment" });
            routes.MapLocalizedStorefrontRoute("API.Cart.AddOrUpdateCartPayment", "storefrontapi/cart/payments", defaults: new { controller = "ApiCart", action = "AddOrUpdateCartPayment" });
            routes.MapLocalizedStorefrontRoute("API.Cart.CreateOrder", "storefrontapi/cart/createorder", defaults: new { controller = "ApiCart", action = "CreateOrder" });

            //Catalog API
            routes.MapLocalizedStorefrontRoute("API.Catalog.SearchProducts", "storefrontapi/catalog/search", defaults: new { controller = "ApiCatalog", action = "SearchProducts" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.Catalog.GetProductsByIds", "storefrontapi/products", defaults: new { controller = "ApiCatalog", action = "GetProductsByIds" });
            routes.MapLocalizedStorefrontRoute("API.Catalog.SearchCategories", "storefrontapi/categories/search", defaults: new { controller = "ApiCatalog", action = "SearchCategories" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.Catalog.GetCategoriesByIds", "storefrontapi/categories", defaults: new { controller = "ApiCatalog", action = "GetCategoriesByIds" });

            //Common storefront API
            routes.MapLocalizedStorefrontRoute("API.Common.GetCountries", "storefrontapi/countries", defaults: new { controller = "ApiCommon", action = "GetCountries" });
            routes.MapLocalizedStorefrontRoute("API.Common.GetCountryRegions", "storefrontapi/countries/{countryCode}/regions", defaults: new { controller = "ApiCommon", action = "GetCountryRegions" });

            //Pricing API
            routes.MapLocalizedStorefrontRoute("API.Pricing.GetActualProductPrices", "storefrontapi/pricing/actualprices", defaults: new { controller = "ApiPricing", action = "GetActualProductPrices" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            //Marketing API
            routes.MapLocalizedStorefrontRoute("API.Marketing.GetDynamicContent", "storefrontapi/marketing/dynamiccontent/{placeName}", defaults: new { controller = "ApiMarketing", action = "GetDynamicContent" });
            //Account API
            routes.MapLocalizedStorefrontRoute("API.Account.GetCurrentCustomer", "storefrontapi/account", defaults: new { controller = "ApiAccount", action = "GetCurrentCustomer" });
           
            //Quote requests API
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.GetItemsCount", "storefrontapi/quoterequests/{number}/itemscount", defaults: new { controller = "ApiQuoteRequest", action = "GetItemsCount" });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.Get", "storefrontapi/quoterequests/{number}", defaults: new { controller = "ApiQuoteRequest", action = "Get" });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.GetCurrent", "storefrontapi/quoterequest/current", defaults: new { controller = "ApiQuoteRequest", action = "GetCurrent" });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.AddItem", "storefrontapi/quoterequests/current/items", defaults: new { controller = "ApiQuoteRequest", action = "AddItem" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.RemoveItem", "storefrontapi/quoterequests/{number}/items/{itemId}", defaults: new { controller = "ApiQuoteRequest", action = "RemoveItem" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "DELETE" }) });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.Update", "storefrontapi/quoterequests/{number}", defaults: new { controller = "ApiQuoteRequest", action = "Update" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "PUT" }) });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.Submit", "storefrontapi/quoterequests/{number}/submit", defaults: new { controller = "ApiQuoteRequest", action = "Submit" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.Reject", "storefrontapi/quoterequests/{number}/reject", defaults: new { controller = "ApiQuoteRequest", action = "Reject" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.CalculateTotals", "storefrontapi/quoterequests/{number}/totals", defaults: new { controller = "ApiQuoteRequest", action = "CalculateTotals" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("API.QuoteRequest.Confirm", "storefrontapi/quoterequests/{number}/confirm", defaults: new { controller = "ApiQuoteRequest", action = "Confirm" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });

            #endregion

            //Account
            routes.MapLocalizedStorefrontRoute("Account", "account", defaults: new { controller = "Account", action = "GetAccount" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("Account.UpdateAccount", "account", defaults: new { controller = "Account", action = "UpdateAccount" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("Account.GetOrderDetails ", "account/order/{number}", defaults: new { controller = "Account", action = "GetOrderDetails" });
            routes.MapLocalizedStorefrontRoute("Account.UpdateAddress", "account/addresses/{id}", defaults: new { controller = "Account", action = "UpdateAddress", id = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("Account.GetAddresses", "account/addresses", defaults: new { controller = "Account", action = "GetAddresses" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("Account.Register", "account/register", defaults: new { controller = "Account", action = "Register" });
            routes.MapLocalizedStorefrontRoute("Account.Login", "account/login", defaults: new { controller = "Account", action = "Login" });
            routes.MapLocalizedStorefrontRoute("Account.Logout", "account/logout", defaults: new { controller = "Account", action = "Logout" });
            routes.MapLocalizedStorefrontRoute("Account.ForgotPassword", "account/forgotpassword", defaults: new { controller = "Account", action = "ForgotPassword" });
            routes.MapLocalizedStorefrontRoute("Account.ResetPassword", "account/resetpassword", defaults: new { controller = "Account", action = "ResetPassword" });
            routes.MapLocalizedStorefrontRoute("Account.ChangePassword", "account/password", defaults: new { controller = "Account", action = "ChangePassword" });
            routes.MapLocalizedStorefrontRoute("Account.ExternalLogin", "account/externallogin", defaults: new { controller = "Account", action = "ExternalLogin" });
            routes.MapLocalizedStorefrontRoute("Account.ExternalLoginCallback", "account/externallogincallback", defaults: new { controller = "Account", action = "ExternalLoginCallback" });

            //Cart
            routes.MapLocalizedStorefrontRoute("Cart.Index", "cart", defaults: new { controller = "Cart", action = "Index" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("Cart.Checkout", "cart/checkout", defaults: new { controller = "Cart", action = "Checkout" });
            routes.MapLocalizedStorefrontRoute("Cart.ExternalPaymentCallback", "cart/externalpaymentcallback", defaults: new { controller = "Cart", action = "ExternalPaymentCallback" });
            routes.MapLocalizedStorefrontRoute("Cart.Thanks", "cart/thanks/{orderNumber}", defaults: new { controller = "Cart", action = "Thanks" });
            routes.MapLocalizedStorefrontRoute("Cart.PaymentForm", "cart/checkout/paymentform", defaults: new { controller = "Cart", action = "PaymentForm" });
            //Cart (Shopify compatible)
            routes.MapLocalizedStorefrontRoute("ShopifyCart.Cart", "cart", defaults: new { controller = "ShopifyCompatibility", action = "Cart" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.CartJs", "cart.js", defaults: new { controller = "ShopifyCompatibility", action = "CartJs" });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.Add", "cart/add", defaults: new { controller = "ShopifyCompatibility", action = "Add" });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.AddJs", "cart/add.js", defaults: new { controller = "ShopifyCompatibility", action = "AddJs" });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.Change", "cart/change", defaults: new { controller = "ShopifyCompatibility", action = "Change" });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.ChangeJs", "cart/change.js", defaults: new { controller = "ShopifyCompatibility", action = "ChangeJs" });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.Clear", "cart/clear", defaults: new { controller = "ShopifyCompatibility", action = "Clear" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.ClearJs", "cart/clear.js", defaults: new { controller = "ShopifyCompatibility", action = "ClearJs" });
            routes.MapLocalizedStorefrontRoute("ShopifyCart.UpdateJs", "cart/update.js", defaults: new { controller = "ShopifyCompatibility", action = "UpdateJs" });

            // QuoteRequest
          
            routes.MapLocalizedStorefrontRoute("QuoteRequest.CurrentQuoteRequest", "quoterequest", defaults: new { controller = "QuoteRequest", action = "CurrentQuoteRequest" });
            routes.MapLocalizedStorefrontRoute("Account.QuoteRequests", "account/quoterequests", defaults: new { controller = "QuoteRequest", action = "QuoteRequests" });
            routes.MapLocalizedStorefrontRoute("Account.QuoteRequestByNumber", "quoterequest/{number}", defaults: new { controller = "QuoteRequest", action = "QuoteRequestByNumber" });

            //CatalogSearch
            routes.MapLocalizedStorefrontRoute("CatalogSearch.CategoryBrowsing", "search/{categoryId}", defaults: new { controller = "CatalogSearch", action = "CategoryBrowsing" });
            routes.MapLocalizedStorefrontRoute("CatalogSearch.SearchProducts", "search", defaults: new { controller = "CatalogSearch", action = "SearchProducts" });
            //Common
            routes.MapLocalizedStorefrontRoute("Common.SetCurrency", "common/setcurrency/{currency}", defaults: new { controller = "Common", action = "SetCurrency" });
            routes.MapLocalizedStorefrontRoute("Common.ContactUsPost", "contact/{viewName}", defaults: new { controller = "Common", action = "СontactUs", viewName = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("Common.ContactUs", "contact/{viewName}", defaults: new { controller = "Common", action = "СontactUs", viewName = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("Common.NoStore", "common/nostore", defaults: new { controller = "Common", action = "NoStore" });
            routes.MapLocalizedStorefrontRoute("Common.Maintenance", "maintenance", defaults: new { controller = "Common", action = "Maintenance" });

         
            //Product routes
            routes.MapLocalizedStorefrontRoute("Product.GetProduct", "product/{productId}", defaults: new { controller = "Product", action = "ProductDetails" });
            routes.MapLocalizedStorefrontRoute("Product.GetProductJson", "product/{productId}/json", defaults: new { controller = "Product", action = "ProductDetailsJson" });
            //Assets
            routes.MapLocalizedStorefrontRoute("Assets", "themes/assets/{asset}", defaults: new { controller = "Asset", action = "GetAssets" });
            routes.MapLocalizedStorefrontRoute("GlobalAssets", "themes/global/assets/{asset}", defaults: new { controller = "Asset", action = "GetGlobalAssets" });

            //Static content (no cms)
            routes.MapLocalizedStorefrontRoute("Pages.GetPage", "pages/{page}", defaults: new { controller = "Page", action = "GetContentPageByName" });
            routes.MapLocalizedStorefrontRoute("Blogs.GetBlog", "blogs/{blog}", defaults: new { controller = "Blog", action = "GetBlog" });
            routes.MapLocalizedStorefrontRoute("Blogs.GetBlogArticle", "blogs/{blog}/{article}", defaults: new { controller = "Blog", action = "GetBlogArticle" });

            Func<string, Route> seoRouteFactory = url => new SeoRoute(url, new MvcRouteHandler(), workContextFactory, commerceCoreApi, staticContentService, cacheManager);
            routes.MapLocalizedStorefrontRoute(name: "SeoRoute", url: "{*path}", defaults: new { controller = "StorefrontHome", action = "Index" }, constraints: null, routeFactory: seoRouteFactory);
            
        }
    }
}
