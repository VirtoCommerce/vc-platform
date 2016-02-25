using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Common
{
    /// <summary>
    /// Create storefront url contains language and store information
    /// </summary>
    public class StorefrontUrlBuilder : IStorefrontUrlBuilder
    {
        private readonly WorkContext _workContext;
        public StorefrontUrlBuilder(WorkContext workContext)
        {
            _workContext = workContext;
        }
        #region IStorefrontUrlBuilder members

        public string ToAppAbsolute(string virtualPath)
        {
            return ToAppAbsolute(virtualPath, _workContext.CurrentStore, _workContext.CurrentLanguage);
        }

        public string ToAppAbsolute(string virtualPath, Store store, Language language)
        {
            var appRelativePath = ToAppRelative(virtualPath, store, language);
            var result = appRelativePath.StartsWith("~")
                ? VirtualPathUtility.ToAbsolute(appRelativePath)
                : appRelativePath;
            return result;
        }

        public string ToAppRelative(string virtualPath)
        {
            return ToAppRelative(virtualPath, _workContext.CurrentStore, _workContext.CurrentLanguage);
        }

        public string ToAppRelative(string virtualPath, Store store, Language language)
        {
            var result = new StringBuilder("~");

            if (store != null)
            {
                // If store has specific URL use it
                if (store.IsStoreUrl(_workContext.RequestUrl))
                {
                    var requestAddress = _workContext.RequestUrl.ToString();

                    if (!string.IsNullOrEmpty(store.Url) && requestAddress.StartsWith(store.Url, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.Clear();
                        result.Append(store.Url.TrimEnd('/'));
                    }
                    else if (!string.IsNullOrEmpty(store.SecureUrl) && requestAddress.StartsWith(store.SecureUrl, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.Clear();
                        result.Append(store.SecureUrl.TrimEnd('/'));
                    }
                }
                else
                {
                    // Do not add storeId to URL if there is only one store
                    if (_workContext.AllStores.Length > 1)
                    {
                        // If specified store does not exist, use current store
                        store = _workContext.AllStores.Contains(store) ? store : _workContext.CurrentStore;
                        if (!virtualPath.Contains("/" + store.Id + "/"))
                        {
                            result.Append("/");
                            result.Append(store.Id);
                        }
                    }
                }

                // Do not add language to URL if store has only one language
                if (language != null && store.Languages.Count > 1)
                {
                    language = store.Languages.Contains(language) ? language : store.DefaultLanguage;
                    if (!virtualPath.Contains("/" + language.CultureName + "/"))
                    {
                        result.Append("/");
                        result.Append(language.CultureName);
                    }
                }
            }

            result.Append("/");
            result.Append(virtualPath.TrimStart('~', '/'));

            return result.ToString();
        }

        public string ToLocalPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
        #endregion
    }
}
