#region
using System;
using System.Collections.Generic;
using System.Web;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class SiteContext : Drop
    {
        #region Fields
        private readonly Dictionary<string, object> _Storage;
        #endregion

        #region Constructors and Destructors
        private SiteContext()
        {
            this._Storage = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // set defaults
            this.Layout = "theme";
            this.Template = "index";
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

        public LoginProvider[] LoginProviders
        {
            get
            {
                object retValue;
                return _Storage.TryGetValue("login_providers", out retValue) ? retValue as LoginProvider[] : null;
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
                return this._Storage.TryGetValue("cart", out retValue) ? retValue as Cart : null;
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
                return _Storage.TryGetValue("checkout", out retValue) ? retValue as Checkout : null;
            }
            set
            {
                Set("checkout", value);
            }
        }

        public Collections Collections
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("collections", out retValue) ? retValue as Collections : null;
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
                return this._Storage.TryGetValue("country_option_tags", out retValue) ? retValue as string : null;
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
                return this._Storage.TryGetValue("customer", out retValue) ? retValue as Customer : null;
            }
            set
            {
                this.Set("customer", value);
            }
        }

        public SubmitForm[] Forms
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("forms", out retValue) ? retValue as SubmitForm[] : null;
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
                return this._Storage.TryGetValue("language", out retValue) ? retValue as string : null;
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
                return this._Storage.TryGetValue("layout", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("layout", value);
            }
        }

        public dynamic Linklists
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("Linklists", out retValue) ? retValue : null;
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
                return this._Storage.TryGetValue("order", out retValue) ? retValue as CustomerOrder : null;
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
                return this._Storage.TryGetValue("page_description", out retValue) ? retValue as string : null;
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
                return this._Storage.TryGetValue("page_title", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("page_title", value);
            }
        }

        public PageCollection Pages
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("pages", out retValue) ? retValue as PageCollection : null;
            }
            set
            {
                this.Set("pages", value);
            }
        }

        public string PoweredByLink
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("powered_by_link", out retValue) ? retValue as string : null;
            }
            set
            {
                this.Set("powered_by_link", value);
            }
        }

        public Settings Settings
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("settings", out retValue) ? retValue as Settings : null;
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
                return this._Storage.TryGetValue("shop", out retValue) ? retValue as Shop : null;
            }
            set
            {
                this.Set("shop", value);
            }
        }

        public IEnumerable<Shop> Shops
        {
            get
            {
                object retValue;
                return this._Storage.TryGetValue("shops", out retValue) ? retValue as IEnumerable<Shop> : null;
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
                return this._Storage.TryGetValue("template", out retValue) ? retValue as string : null;
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
                return this._Storage.TryGetValue("theme", out retValue) ? retValue as Theme : null;
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
                return this._Storage.TryGetValue("pricelists", out retValue) ? retValue as string[] : null;
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
                return this._Storage.TryGetValue("themes", out retValue) ? retValue as Theme[] : null;
            }
            set
            {
                this.Set("themes", value);
            }
        }

        #endregion

        #region Public Methods and Operators
        public void Set(string key, object val)
        {
            if (this._Storage.ContainsKey(key))
            {
                this._Storage[key] = val;
            }
            else
            {
                this._Storage.Add(key, val);
            }
        }

        public override object ToLiquid()
        {
            return Hash.FromDictionary(this._Storage);
        }
        #endregion
    }
}