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
        private readonly WorkContext _workContext;
        public StorefrontUrlBuilder(WorkContext workContext)
        {
            _workContext = workContext;
        }
     
        #region IStorefrontUrlBuilder members
        public string ToAbsolute(string virtualPath)
        {
            var retVal = VirtualPathUtility.ToAbsolute(ToAppRelative(virtualPath));
            return retVal;
        }

        public string ToAppRelative(string virtualPath)
        {
            virtualPath = virtualPath.Replace("~/", String.Empty);
            var retVal = "~/" + virtualPath.TrimStart('/');
            return retVal;
        } 

        public string ToLocalPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
        #endregion
    }
}
