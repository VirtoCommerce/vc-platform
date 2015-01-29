using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.ApiWebClient.Extensions;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.ApiWebClient.Modules
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Extensions;
    using VirtoCommerce.ApiClient.Session;

    /// <summary>
    /// Class MarketingHttpModule.
    /// </summary>
    public class MarketingHttpModule : BaseHttpModule
    {
        /// <summary>
        /// The referral l_ cookie
        /// </summary>
        private const string ReferralCookie = "vcf.referral";

        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public override void Dispose()
        {
            //clean-up code here.
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public override void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += context_PostAcquireRequestState;
        }


        /// <summary>
        /// Handles the PostAcquireRequestState event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void context_PostAcquireRequestState(object sender, EventArgs e)
        {
            if (IsResourceFile() || IsWebApi)
                return;

            var application = (HttpApplication)sender;
            var context = application.Context;

            var session = CustomerSession;
            Populate(context, session);

            /*
            //Price list can only be evaluated after all customer tagsets are ready
            var priceListHelper = DependencyResolver.Current.GetService<PriceListClient>();
            var priceLists = priceListHelper.GetPriceListStack(session.CatalogId, session.Currency, session.GetCustomerTagSet());
            session.Pricelists = priceLists;

            //Update prices in current currency
            var helper = new CartHelper(CartHelper.CartName);
            if (!helper.IsEmpty && 
                !string.IsNullOrEmpty(session.Currency) && 
                !string.IsNullOrEmpty(helper.Cart.BillingCurrency) && 
                session.Currency != helper.Cart.BillingCurrency)
            {
                helper.RunWorkflow("ShoppingCartValidateWorkflow");
                helper.SaveChanges();
            }
             */


        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Populates the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="session">The session.</param>
        private void Populate(HttpContext context, ICustomerSession session)
        {
            var set = session.GetCustomerTagSet();

            //Profile
            if (IsRequestAuthenticated(context))
            {
                //var customer = StoreHelper.UserClient.GetCurrentCustomer();
                //if (customer != null)
                //{
                //    if (customer.BirthDate.HasValue)
                //    {
                //        set.Add(ContextFieldConstants.UserAge, new Tag(GetAge(customer.BirthDate.Value)));
                //    }
                //}
            }

            PopulateBrowserBehavior(context, session);
            PopulateShoppingCart(context, session);
            PopulateGEOLocation(context, session);
        }

        /// <summary>
        /// Populates the geo location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="session">The session.</param>
        private void PopulateGEOLocation(HttpContext context, ICustomerSession session)
        {
            //TODO: populate geo location information
            var set = session.GetCustomerTagSet();

        }

        /// <summary>
        /// Populates the shopping cart.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="session">The session.</param>
        private void PopulateShoppingCart(HttpContext context, ICustomerSession session)
        {
            var set = session.GetCustomerTagSet();

            //TODO populate cart
            // cart total
            //var helper = new CartHelper(CartHelper.CartName);
            //if (!helper.IsEmpty)
            //{
            //    set.Add(ContextFieldConstants.CartTotal, new Tag(helper.Cart.Total));
            //}
        }

        /// <summary>
        /// Populates the browser behavior.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="session">The session.</param>
        private void PopulateBrowserBehavior(HttpContext context, ICustomerSession session)
        {
            var set = session.GetCustomerTagSet();

            var routeValues = context.Request.RequestContext.RouteData.Values;

            if (routeValues != null)
            {
                if (routeValues.ContainsKey(Extensions.Routing.Constants.Store))
                {
                    //Store id must be decoded
                    //session.StoreId = routeValues[Extensions.Routing.Constants.Store].ToString();
                }

                if (routeValues.ContainsKey(Extensions.Routing.Constants.Language))
                {
                    session.Language = routeValues[Extensions.Routing.Constants.Language].ToString();
                }

                if (routeValues.ContainsKey(Extensions.Routing.Constants.Category))
                {
                    var categoryPath = routeValues[Extensions.Routing.Constants.Category].ToString();

                    if (!string.IsNullOrEmpty(categoryPath)) 
                    {
                        var client = ClientContext.Clients.CreateBrowseClient(session.StoreId, session.Language);
                        var categorySlug = categoryPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                        var category = Task.Run(()=>client.GetCategoryAsync(categorySlug)).Result;

                        if (category != null)
                        {
                            session.CategoryOutline = string.Join("/", category.BuildOutline().Select(x => x.Key));
                            session.CategoryId = category.Id;
                        }
                    }
                }
            }

            // search
            var search = context.Request["q"];
            if (!String.IsNullOrEmpty(search))
            {
                set.Add(ContextFieldConstants.StoreSearchPhrase, search);
            }

            // store id
            if (!String.IsNullOrEmpty(session.StoreId))
            {
                set.Add(ContextFieldConstants.StoreId, session.StoreId);
            }

            // category id
            if (!String.IsNullOrEmpty(session.CategoryId))
            {
                set.Add(ContextFieldConstants.CategoryId, session.CategoryId);
            }

            //category path
            if (!String.IsNullOrEmpty(session.CategoryOutline))
            {
                set.Add(ContextFieldConstants.CategoryOutline, session.CategoryOutline);
            }

            // language
            if (!String.IsNullOrEmpty(session.Language))
            {
                set.Add(ContextFieldConstants.Language, session.Language);
            }

            // current URL
            set.Add(ContextFieldConstants.CurrentUrl, context.Request.Url.AbsoluteUri);

            // referral
            var referral = GetReferralUrl(context);
            if (!String.IsNullOrEmpty(referral))
            {
                set.Add(ContextFieldConstants.ReferredUrl, referral);

                var keywords = GetKeywords(referral);

                if (!String.IsNullOrEmpty(keywords))
                {
                    set.Add(ContextFieldConstants.InternetSearchPhrase, keywords);
                }
            }
        }

        /// <summary>
        /// Gets the referral URL.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        private string GetReferralUrl(HttpContext context)
        {
            var referral = context.Request.UrlReferrer;

            if (referral != null)
            {
                bool isLocal = referral.Host.Equals(context.Request.Url.Host, StringComparison.OrdinalIgnoreCase);

                if (!isLocal)
                {
                    // save value in the cookie
                    StoreHelper.SetCookie(ReferralCookie, referral.AbsoluteUri, DateTime.Now.AddDays(1));

                    return referral.AbsoluteUri;
                }
            }

            // now try getting it from cookie
            var url = StoreHelper.GetCookieValue(ReferralCookie);
            if (!String.IsNullOrEmpty(url))
            {
                return url;
            }

            return null;
        }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <param name="birthday">The birthday.</param>
        /// <returns>System.Int32.</returns>
        private int GetAge(DateTime birthday)
        {
            var now = DateTime.Today;
            var age = now.Year - birthday.Year;
            if (now < birthday.AddYears(age))
                age--;
            return age;
        }

        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <param name="urlReferrer">The URL referrer.</param>
        /// <returns>System.String.</returns>
        private string GetKeywords(string urlReferrer)
        {
            string searchQuery;
            var url = new Uri(urlReferrer);
            var query = HttpUtility.ParseQueryString(urlReferrer);
            switch (url.Host)
            {
                case "google":
                case "daum":
                case "msn":
                case "bing":
                case "ask":
                case "altavista":
                case "alltheweb":
                case "live":
                case "najdi":
                case "aol":
                case "seznam":
                case "search":
                case "szukacz":
                case "pchome":
                case "kvasir":
                case "sesam":
                case "ozu":
                case "mynet":
                case "ekolay":
                    searchQuery = query["q"];
                    break;
                case "naver":
                case "netscape":
                case "mama":
                case "mamma":
                case "terra":
                case "cnn":
                    searchQuery = query["query"];
                    break;
                case "virgilio":
                case "alice":
                    searchQuery = query["qs"];
                    break;
                case "yahoo":
                    searchQuery = query["p"];
                    break;
                case "onet":
                    searchQuery = query["qt"];
                    break;
                case "eniro":
                    searchQuery = query["search_word"];
                    break;
                case "about":
                    searchQuery = query["terms"];
                    break;
                case "voila":
                    searchQuery = query["rdata"];
                    break;
                case "baidu":
                    searchQuery = query["wd"];
                    break;
                case "yandex":
                    searchQuery = query["text"];
                    break;
                case "szukaj":
                    searchQuery = query["wp"];
                    break;
                case "yam":
                    searchQuery = query["k"];
                    break;
                case "rambler":
                    searchQuery = query["words"];
                    break;
                default:
                    searchQuery = query["q"];
                    break;
            }
            return searchQuery;
        }

        #endregion
    }
}
