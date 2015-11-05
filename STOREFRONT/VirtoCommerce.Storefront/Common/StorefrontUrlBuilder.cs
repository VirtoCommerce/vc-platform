using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace VirtoCommerce.Storefront.Model.Common
{
    /// <summary>
    /// Create storefront url with all localization and store information
    /// </summary>
    public class StorefrontUrlBuilder : IStorefrontUrlBuilder
    {
        #region IStorefrontUrlBuilder members
        public string ToAbsolute(string virtualPath, string store, string language)
        {
            if(String.IsNullOrEmpty(virtualPath))
            {
                virtualPath = "~/ ";
            }
            var retVal = VirtualPathUtility.ToAbsolute(ToAppRelative(virtualPath, store, language));
            return retVal;
        }

        public string ToAppRelative(string virtualPath, string store, string language)
        {
            virtualPath = virtualPath.Replace("~/", String.Empty);
            var retVal = "~/" + store + "/" + language + "/" + virtualPath.TrimStart('/');
            return retVal;
        } 

        public string ToLocalPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
        #endregion
    }
}
