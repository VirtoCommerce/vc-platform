#region

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotLiquid;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Cms;
using VirtoCommerce.Web.Models.Lists;

#endregion

namespace VirtoCommerce.Web
{
    public class SiteContext : Drop
    {
        #region Fields
        private readonly Dictionary<string, object> _storage;

        private readonly string[] _poweredLinks = {
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">.NET ecommerce platform</a> by Virto",
                                             "<a href=\"http://virtocommerce.com/shopping-cart\" rel=\"nofollow\" target=\"_blank\">Shopping Cart</a> by Virto",
                                             "<a href=\"http://virtocommerce.com/shopping-cart\" rel=\"nofollow\" target=\"_blank\">.NET Shopping Cart</a> by Virto",
                                             "<a href=\"http://virtocommerce.com/shopping-cart\" rel=\"nofollow\" target=\"_blank\">ASP.NET Shopping Cart</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">.NET ecommerce</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">.NET ecommerce framework</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">ASP.NET ecommerce</a> by Virto Commerce",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">ASP.NET ecommerce platform</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">ASP.NET ecommerce framework</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">Enterprise ecommerce</a> by Virto",
                                             "<a href=\"http://virtocommerce.com\" rel=\"nofollow\" target=\"_blank\">Enterprise ecommerce platform</a> by Virto",
                                         };
        #endregion

        #region Constructors and Destructors
        private SiteContext()
        {
            this._storage = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // set defaults
            this.Layout = "theme";
            this.Template = "index";

            this.PoweredByLink = this.GetPoweredLink();
        }
        #endregion

        #region Public Properties
        public static SiteContext Current
        {
            get
            {
                const string key = "vc-sitecontext";
                var ctx = HttpContext.Current;
                if (ctx != null)
                {
                    var global = ctx.Items[key] as SiteContext;
                    if (global == null)
                    {
                        global = new SiteContext();
                        ctx.Items.Add(key, global);
                        return global;
                    }

                    return global;
                }

                return new SiteContext();
            }
        }

        public string ErrorMessage
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("error_message", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("error_message", value);
            }

        }

        public LoginProvider[] LoginProviders
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("login_providers", out retValue) ? retValue as LoginProvider[] : null;
            }
            set
            {
                this.Set("login_providers", value);
            }
        }

        public Cart Cart
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("cart", out retValue) ? retValue as Cart : null;
            }
            set
            {
                this.Set("cart", value);
            }
        }

        public Checkout Checkout
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("checkout", out retValue) ? retValue as Checkout : null;
            }
            set
            {
                this.Set("checkout", value);
            }
        }

        public Collections Collections
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("collections", out retValue) ? retValue as Collections : null;
            }
            set
            {
                this.Set("collections", value);
            }
        }

        public string CountryOptionTags
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("country_option_tags", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("country_option_tags", value);
            }
        }

        public string CustomerId { get; set; }

        public Customer Customer
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("customer", out retValue) ? retValue as Customer : null;
            }
            set
            {
                this.Set("customer", value);
            }
        }

        public ICollection<SubmitForm> Forms
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("forms", out retValue) ? retValue as List<SubmitForm> : null;
            }
            set
            {
                this.Set("forms", value);
            }
        }

        public string Language
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("language", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("language", value);
            }
        }

        public string Layout
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("layout", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("layout", value);
            }
        }

        public LinkLists Linklists
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("Linklists", out retValue) ? retValue as LinkLists : null;
            }
            set
            {
                this.Set("Linklists", value);
            }
        }

        public CustomerOrder Order
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("order", out retValue) ? retValue as CustomerOrder : null;
            }
            set
            {
                this.Set("order", value);
            }
        }

        public string PageDescription
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("page_description", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("page_description", value);
            }
        }

        public string PageTitle
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("page_title", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("page_title", value);
            }
        }

        public string PageKeywords
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("page_keywords", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("page_keywords", value);
            }
        }

        public PageCollection Pages
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("pages", out retValue) ? retValue as PageCollection : null;
            }
            set
            {
                this.Set("pages", value);
            }
        }

        public BlogCollection Blogs
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("blogs", out retValue) ? retValue as BlogCollection : null;
            }
            set
            {
                this.Set("blogs", value);
            }
        }

        public Settings Settings
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("settings", out retValue) ? retValue as Settings : null;
            }
            set
            {
                this.Set("settings", value);
            }
        }

        public Shop Shop
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("shop", out retValue) ? retValue as Shop : null;
            }
            set
            {
                this.Set("shop", value);
            }
        }

        public QuoteRequest ActualQuoteRequest
        {
            get
            {
                object retValue;
                return _storage.TryGetValue("actual_quote_request", out retValue) ? retValue as QuoteRequest : null;
            }
            set
            {
                Set("actual_quote_request", value);
            }
        }

        public QuoteRequest QuoteRequest
        {
            get
            {
                object retValue;
                return _storage.TryGetValue("quote_request", out retValue) ? retValue as QuoteRequest : null;
            }
            set
            {
                Set("quote_request", value);
            }
        }

        public IEnumerable<Shop> Shops
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("shops", out retValue) ? retValue as IEnumerable<Shop> : null;
            }
            set
            {
                this.Set("shops", value);
            }
        }

        public string StoreId
        {
            get
            {
                return this.Shop != null ? this.Shop.StoreId : null;
            }
        }

        public string Template
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("template", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("template", value);
            }
        }

        public Theme Theme
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("theme", out retValue) ? retValue as Theme : null;
            }
            set
            {
                this.Set("theme", value);
            }
        }

        public string[] PriceLists
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("pricelists", out retValue) ? retValue as string[] : null;
            }
            set
            {
                this.Set("pricelists", value);
            }
        }

        public Theme[] Themes
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("themes", out retValue) ? retValue as Theme[] : null;
            }
            set
            {
                this.Set("themes", value);
            }
        }

        public string PoweredByLink
        {
            get
            {
                object retValue;
                return this._storage.TryGetValue("powered_by_link", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("powered_by_link", value);
            }
        }


        #endregion

        #region Public Methods and Operators
        public void Set(string key, object val)
        {
            if (this._storage.ContainsKey(key))
            {
                this._storage[key] = val;
            }
            else
            {
                this._storage.Add(key, val);
            }
        }

        public override object ToLiquid()
        {
            return Hash.FromDictionary(this._storage);
        }
        #endregion

        #region private methods

        private string GetPoweredLink()
        {
            string poweredLink = _poweredLinks[0];

            if (HttpContext.Current != null)
            {
                var host = HttpContext.Current.Request.Url.Host.ToLowerInvariant();
                var hostHashCode = Math.Abs(host.GetHashCode());

                var index = hostHashCode % _poweredLinks.Length;
                if (index < _poweredLinks.Length)
                {
                    poweredLink = _poweredLinks[index];
                }
            }

            return poweredLink;
        }
        #endregion
    }
}