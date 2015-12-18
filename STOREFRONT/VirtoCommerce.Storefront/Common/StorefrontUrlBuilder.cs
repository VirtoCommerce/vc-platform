using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Common
{
    /// <summary>
    /// Create storefront url with all localization and store information
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
            var retVal = VirtualPathUtility.ToAbsolute(ToAppRelative(virtualPath, store, language));
            return retVal;
        }

        public string ToAppRelative(string virtualPath)
        {
            return ToAppRelative(virtualPath, _workContext.CurrentStore, _workContext.CurrentLanguage);
        }

        public string ToAppRelative(string virtualPath, Store store, Language language)
        {
            var result = new StringBuilder("~");
            //Do not add storeId to Url if it single or have strict  defined Url
            if (store != null && !store.IsStoreUri(_workContext.RequestUrl))
            {
                //Do not use store in url if it single
                if (_workContext.AllStores.Length > 1)
                {
                    //Check that store exist for not exist store use current
                    store = _workContext.AllStores.Contains(store) ? store : _workContext.CurrentStore;
                    if (!virtualPath.Contains("/" + store.Id + "/"))
                    {
                        result.Append("/");
                        result.Append(store.Id);
                    }
                }
            }

            //Do not use language in url if it single for store
            if (language != null && store != null && store.Languages.Count > 1)
            {
                language = store.Languages.Contains(language) ? language : store.DefaultLanguage;
                if (!virtualPath.Contains("/" + language.CultureName + "/"))
                {
                    result.Append("/");
                    result.Append(language.CultureName);
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
