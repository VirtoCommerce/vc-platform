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
            var result = virtualPath;

            // Don't process absolute URL
            Uri absoluteUri;
            if (!Uri.TryCreate(virtualPath, UriKind.Absolute, out absoluteUri))
            {
                var builder = new StringBuilder("~");

                if (store != null)
                {
                    // If store has public or secure URL, use them
                    if (!string.IsNullOrEmpty(store.Url) || !string.IsNullOrEmpty(store.SecureUrl))
                    {
                        string baseAddress = null;

                        // If current request is secure, use secure URL
                        if (_workContext.RequestUrl != null && !string.IsNullOrEmpty(store.SecureUrl) &&
                            _workContext.RequestUrl.ToString()
                                .StartsWith(store.SecureUrl, StringComparison.InvariantCultureIgnoreCase))
                        {
                            baseAddress = store.SecureUrl;
                        }

                        if (baseAddress == null)
                        {
                            baseAddress = !string.IsNullOrEmpty(store.Url) ? store.Url : store.SecureUrl;
                        }

                        builder.Clear();
                        builder.Append(baseAddress.TrimEnd('/'));
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
                                builder.Append("/");
                                builder.Append(store.Id);
                            }
                        }
                    }

                    // Do not add language to URL if store has only one language
                    if (language != null && store.Languages.Count > 1)
                    {
                        language = store.Languages.Contains(language) ? language : store.DefaultLanguage;
                        if (!virtualPath.Contains("/" + language.CultureName + "/"))
                        {
                            builder.Append("/");
                            builder.Append(language.CultureName);
                        }
                    }
                }

                builder.Append("/");
                builder.Append(virtualPath.TrimStart('~', '/'));
                result = builder.ToString();
            }

            return result;
        }

        public string ToLocalPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
        #endregion
    }
}
