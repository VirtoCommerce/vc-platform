using System;
using System.Web.Mvc;
using System.Web.Routing;
using CacheManager.Core;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Routing;

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
            //Account
            routes.MapLocalizedStorefrontRoute("Account", "account", defaults: new { controller = "Account", action = "Index" });
            routes.MapLocalizedStorefrontRoute("Account.GetOrderDetails ", "account/order/{number}", defaults: new { controller = "Account", action = "GetOrderDetails" });
            routes.MapLocalizedStorefrontRoute("Account.UpdateAddress", "account/addresses/{id}", defaults: new { controller = "Account", action = "UpdateAddress", id = UrlParameter.Optional }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("Account.GetAddresses", "account/addresses", defaults: new { controller = "Account", action = "GetAddresses" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("Account.Register", "account/register", defaults: new { controller = "Account", action = "Register" });
            routes.MapLocalizedStorefrontRoute("Account.Login", "account/login", defaults: new { controller = "Account", action = "Login" });
            routes.MapLocalizedStorefrontRoute("Account.Logout", "account/logout", defaults: new { controller = "Account", action = "Logout" });
            routes.MapLocalizedStorefrontRoute("Account.ForgotPassword", "account/forgotpassword", defaults: new { controller = "Account", action = "ForgotPassword" });
            routes.MapLocalizedStorefrontRoute("Account.ResetPassword", "account/resetpassword", defaults: new { controller = "Account", action = "ResetPassword" });
            routes.MapLocalizedStorefrontRoute("Account.UpdateProfile", "account/profile", defaults: new { controller = "Account", action = "UpdateProfile" });
            routes.MapLocalizedStorefrontRoute("Account.ChangePassword", "account/password", defaults: new { controller = "Account", action = "ChangePassword" });
            routes.MapLocalizedStorefrontRoute("Account.Json", "account/json", defaults: new { controller = "Account", action = "GetCurrentCustomer" });

            //Cart
            routes.MapLocalizedStorefrontRoute("Cart.AddJs", "cart/add.js", defaults: new { controller = "Cart", action = "AddItemJson" });
            routes.MapLocalizedStorefrontRoute("Cart.Add", "cart/add", defaults: new { controller = "Cart", action = "AddItemJson" });
            routes.MapLocalizedStorefrontRoute("Cart.ChangeJs", "cart/change.js", defaults: new { controller = "Cart", action = "ChangeItemJson" });
            routes.MapLocalizedStorefrontRoute("Cart.Change", "cart/change", defaults: new { controller = "Cart", action = "Change" });
            routes.MapLocalizedStorefrontRoute("Cart.Cart", "cart", defaults: new { controller = "Cart", action = "Cart" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("Cart.Index", "cart", defaults: new { controller = "Cart", action = "Index" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });
            routes.MapLocalizedStorefrontRoute("Cart.Json", "cart/json", defaults: new { controller = "Cart", action = "CartJson" });
            routes.MapLocalizedStorefrontRoute("Cart.AddItem", "cart/additem", defaults: new { controller = "Cart", action = "AddItemJson" });
            routes.MapLocalizedStorefrontRoute("Cart.ChangeItem", "cart/changeitem", defaults: new { controller = "Cart", action = "ChangeItemJson" });
            routes.MapLocalizedStorefrontRoute("Cart.RemoveItem", "cart/removeitem", defaults: new { controller = "Cart", action = "RemoveItemJson" });
            routes.MapLocalizedStorefrontRoute("Cart.Clear", "cart/clear", defaults: new { controller = "Cart", action = "ClearJson" });
            routes.MapLocalizedStorefrontRoute("Cart.Checkout", "cart/checkout", defaults: new { controller = "Cart", action = "Checkout" });
            routes.MapLocalizedStorefrontRoute("Cart.ShippingMethods", "cart/shippingmethods/json", defaults: new { controller = "Cart", action = "CartShippingMethodsJson" });
            routes.MapLocalizedStorefrontRoute("Cart.PaymentMethods", "cart/paymentmethods/json", defaults: new { controller = "Cart", action = "CartPaymentMethodsJson" });
            routes.MapLocalizedStorefrontRoute("Cart.AddCoupon", "cart/addcoupon/{couponCode}", defaults: new { controller = "Cart", action = "AddCouponJson" });
            routes.MapLocalizedStorefrontRoute("Cart.RemoveCoupon", "cart/removecoupon", defaults: new { controller = "Cart", action = "RemoveCouponJson" });
            routes.MapLocalizedStorefrontRoute("Cart.AddAddress", "cart/addaddress", defaults: new { controller = "Cart", action = "AddAddressJson" });
            routes.MapLocalizedStorefrontRoute("Cart.SetShippingMethods", "cart/shippingmethod", defaults: new { controller = "Cart", action = "SetShippingMethodsJson" });
            routes.MapLocalizedStorefrontRoute("Cart.SetPaymentMethods", "cart/paymentmethod", defaults: new { controller = "Cart", action = "SetPaymentMethodsJson" });
            routes.MapLocalizedStorefrontRoute("Cart.CreateOrder", "cart/createorder", defaults: new { controller = "Cart", action = "CreateOrderJson" });
            routes.MapLocalizedStorefrontRoute("Cart.ExternalPaymentCallback", "cart/externalpaymentcallback", defaults: new { controller = "Cart", action = "ExternalPaymentCallback" });
            routes.MapLocalizedStorefrontRoute("Cart.Thanks", "cart/thanks/{orderNumber}", defaults: new { controller = "Cart", action = "Thanks" });
            routes.MapLocalizedStorefrontRoute("Cart.PaymentForm", "cart/checkout/paymentform", defaults: new { controller = "Cart", action = "PaymentForm" });

            //CatalogSearch
            routes.MapLocalizedStorefrontRoute("CatalogSearch.CategoryBrowsing", "search/{categoryId}", defaults: new { controller = "CatalogSearch", action = "CategoryBrowsing" });
            routes.MapLocalizedStorefrontRoute("CatalogSearch.SearchProducts", "search", defaults: new { controller = "CatalogSearch", action = "SearchProducts" });
            //Common
            routes.MapLocalizedStorefrontRoute("Common.SetCurrency", "common/setcurrency/{currency}", defaults: new { controller = "Common", action = "SetCurrency" });
            routes.MapLocalizedStorefrontRoute("Common.Getcountries", "common/getcountries/json", defaults: new { controller = "Common", action = "GetCountries" });
            routes.MapLocalizedStorefrontRoute("Common.Getregions", "common/getregions/{countryCode}/json", defaults: new { controller = "Common", action = "GetRegions" });
            routes.MapLocalizedStorefrontRoute("Common.ContactUsPost", "contact", defaults: new { controller = "Common", action = "СontactUs" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "POST" }) });
            routes.MapLocalizedStorefrontRoute("Common.ContactUs", "contact", defaults: new { controller = "Common", action = "СontactUs" }, constraints: new { httpMethod = new HttpMethodConstraint(new string[] { "GET" }) });

            //Marketing 
            routes.MapLocalizedStorefrontRoute("Marketing.DynamicContent", "marketing/dynamiccontent/{placeName}/json", defaults: new { controller = "Marketing", action = "GetDynamicContentJson" });
            routes.MapLocalizedStorefrontRoute("Marketing.ActualPrices", "marketing/actualprices", defaults: new { controller = "Marketing", action = "GetActualProductPricesJson" });
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


            routes.MapSeoRoute(workContextFactory, commerceCoreApi, staticContentService, cacheManager, "SeoRoute", "{*path}", new { controller = "Home", action = "Index" });
           
        }
    }
}
