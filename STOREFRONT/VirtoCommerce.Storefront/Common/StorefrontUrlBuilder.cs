using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #region IStorefrontUrlBuilder members
        public string ToAppAbsolute(WorkContext context, string virtualPath, Store store, Language language)
        {
            var retVal = VirtualPathUtility.ToAbsolute(ToAppRelative(context, virtualPath, store, language));
            return retVal;
        }

        public string ToAppRelative(WorkContext context, string virtualPath, Store store, Language language)
        {
            virtualPath = virtualPath.Replace("~/", String.Empty);
            var retVal = "~/";
       
            if (store != null)
            {
                //Do not use store in url if it single
                if (context.AllStores.Count() > 1)
                {
                    //Check that store exist for not exist store use current
                    store = context.AllStores.Contains(store) ? store : context.CurrentStore;
                    if (!virtualPath.Contains("/" + store.Id + "/"))
                    {
                        retVal += store.Id + "/";
                    }
                }
            }

            //Do not use language in url if it single for store
            if (language != null && store != null && store.Languages.Count() > 1)
            {
                language = store.Languages.Contains(language) ? language : store.DefaultLanguage;
                if (!virtualPath.Contains("/" + language.CultureName + "/"))
                {
                    retVal += language.CultureName + "/";
                }
            }

            retVal += virtualPath.TrimStart('/');

            return retVal;
        } 

        public string ToLocalPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
        #endregion
    }
}
