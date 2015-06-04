using System;
using System.Text;
using System.Web;
using System.Web.Security;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using System.Collections;

namespace VirtoCommerce.Foundation.Customers
{
    /// <summary>
    /// Class CustomerSession.
    /// </summary>
	public class CustomerSession : ICustomerSession
	{
        /// <summary>
        /// The _set
        /// </summary>
		readonly TagSet _set = new TagSet();
        
        /// <summary>
        /// The language code
        /// </summary>
		private string _languageCode;

        /// <summary>
        /// The coupon code
        /// </summary>
        private string _couponCode;

        /// <summary>
        /// The CSR user name
        /// </summary>
        private string _csrUserName;

        /// <summary>
        /// The last shopping page
        /// </summary>
        private string _lastShoppingPage;

        /// <summary>
        /// The category identifier
        /// </summary>
        private string _categoryId;

        /// <summary>
        /// The category outline
        /// </summary>
        private string _categoryOutline;

        /// <summary>
        /// The last order identifier
        /// </summary>
        private string _lastOrderId;

        /// <summary>
        /// Gets the customer tag set.
        /// </summary>
        /// <returns>
        /// TagSet.
        /// </returns>
		public TagSet GetCustomerTagSet()
		{
			return _set;
		}

        /// <summary>
        /// Gets or sets the pricelists.
        /// </summary>
        /// <value>The pricelists.</value>
		public string[] Pricelists { get; set; }
        
        /// <summary>
        /// Gets or sets the store identifier.
        /// </summary>
        /// <value>
        /// The store identifier.
        /// </value>
		public string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the name of the store.
        /// </summary>
        /// <value>
        /// The name of the store.
        /// </value>
		public string StoreName { get; set; }

        /// <summary>
        /// Gets or sets the customer id. Corresponds to MemberId used throughout the application and by various modules.
        /// </summary>
        /// <value>
        /// The customer id.
        /// </value>
		public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the username, used by customer to login. Typically is the same as HttpContext.Current.User.Identity.Name, but in some CMS systems can be different.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get;set;
        }

        public string CsrUsername
        {
            get
            {
                return _csrUserName ?? (_csrUserName = GetCookieValue("vcf.CsrUsername", true));
            }
            set
            {
                _csrUserName = value;
                SetCookie("vcf.CsrUsername", value, encrypt: true);
            }
        }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>The name of the customer.</value>
		public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>The catalog identifier.</value>
		public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
		public string CategoryId
		{
            get
            {
                return _categoryId ?? (_categoryId = GetCookieValue("vcf.CategoryId"));
            }
            set
            {
                _categoryId = value;
                SetCookie("vcf.CategoryId", value);
            }
		}

        public string CategoryOutline
        {
            get
            {
                return _categoryOutline ?? (_categoryOutline = GetCookieValue("vcf.CategoryOutline"));
            }
            set
            {
                _categoryOutline = value;
                SetCookie("vcf.CategoryOutline", value);
            }
        }

        /// <summary>
        /// Gets or sets the last order identifier.
        /// </summary>
        /// <value>
        /// The last order identifier.
        /// </value>
        public string LastOrderId
        {
            get
            {
                return _lastOrderId ?? (_lastOrderId = GetCookieValue("vcf.LastOrderId", true));
            }
            set
            {
                _lastOrderId = value;
                SetCookie("vcf.LastOrderId", value, encrypt:true);
            }
        }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>The currency.</value>
		public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
		public string Language
		{
			get
			{
				if (string.IsNullOrEmpty(_languageCode))
				{
					_languageCode = GetCookieValue("vcf.Language");
				}
				return !string.IsNullOrEmpty(_languageCode) ? _languageCode.ToLower() : null;
			}
			set
			{
				if (_languageCode != value)
				{
					_languageCode = value;
					SetCookie("vcf.Language", _languageCode, DateTime.Now.AddMonths(1));
				}
			}
		}

        /// <summary>
        /// Gets or sets the coupon code.
        /// </summary>
        /// <value>The coupon code.</value>
        public string CouponCode
        {
            get
            {
                return _couponCode ?? (_couponCode = GetCookieValue("vcf.CouponCode", true));
            }
            set
            {
                _couponCode = value;
                SetCookie("vcf.CouponCode", value, encrypt: true);
            }
        }
        /// <summary>
        /// Gets or sets the last shopping page.
        /// </summary>
        /// <value>
        /// The last shopping page.
        /// </value>
		public string LastShoppingPage
		{
			get
			{
			    if (string.IsNullOrEmpty(_lastShoppingPage))
			    {
			        _lastShoppingPage = GetCookieValue("vcf.LastShoppingPage");
			    }
                if (string.IsNullOrEmpty(_lastShoppingPage))
                {
                    _lastShoppingPage = HttpContext.Current.Request.ApplicationPath;
                }
                return _lastShoppingPage;
			}
			set
			{
                _lastShoppingPage = value;
                SetCookie("vcf.LastShoppingPage", value);
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether this user is registered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this user is registered; otherwise, <c>false</c>.
        /// </value>
		public bool IsRegistered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user is first time buyer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this user is first time buyer; otherwise, <c>false</c>.
        /// </value>
		public bool IsFirstTimeBuyer { get; set; }

        /// <summary>
        /// The _current date time
        /// </summary>
		DateTime _currentDateTime = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the current date time.
        /// </summary>
        /// <value>
        /// The current date time.
        /// </value>
		public DateTime CurrentDateTime
		{
			get
			{
				return _currentDateTime;
			}
			set
			{
				_currentDateTime = value;
			}
		}

        /// <summary>
        /// The _hash
        /// </summary>
		readonly Hashtable _hash = new Hashtable();
        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
		public object this[string key]
		{
			get
			{
				return _hash[key];
			}
			set
			{
				_hash[key] = value;
			}
		}

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The value.</param>
        /// <param name="expires">The expires.</param>
		public static void SetCookie(string key, string val, DateTime? expires = null, bool secure = false, bool encrypt = false)
		{
			if (HttpContext.Current != null)
			{
			    if (encrypt)
			    {
			        val = EncryptCookie(val);
			    }
				var responseCookie = HttpContext.Current.Response.Cookies[key];

				if (responseCookie == null)
				{
					responseCookie = new HttpCookie(key);
					HttpContext.Current.Response.Cookies.Add(responseCookie);
				}

				if (val != responseCookie.Value)
				{
                    if (expires.HasValue)
                    {
                        responseCookie.Expires = expires.Value;
                    }
				    responseCookie.Secure = secure;
					responseCookie.Value = val;
				}
			}
		}


        /// <summary>
        /// Gets the cookie value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="decrypt">if set to <c>true</c> [decrypt].</param>
        /// <returns>
        /// System.String.
        /// </returns>
		public static string GetCookieValue(string key, bool decrypt = false)
		{
			string val = null;
			if (HttpContext.Current != null)
			{
				if (HttpContext.Current.Request.Cookies[key] != null)
				{
					val = HttpContext.Current.Request.Cookies[key].Value;

                    if (decrypt && !string.IsNullOrEmpty(val))
                    {
                        val = DecryptCookie(val);
                    }
				}
			}
            return val;
		}

        public static string EncryptCookie(string value)
        {
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var encryptedBytes = MachineKey.Protect(plainBytes, "Cookie protection");
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptCookie(string value)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(value);
                var decryptedBytes = MachineKey.Unprotect(encryptedBytes, "Cookie protection");
                return decryptedBytes != null ? Encoding.UTF8.GetString(decryptedBytes) : value;
            }
            catch
            {
                return value;
            }
        }
    }
}
