using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public interface IStorefrontUrlBuilder
    {
        string ToAppAbsolute(string virtualPath, Store store, Language language);
        string ToAppRelative(string virtualPath, Store store, Language language);
        string ToLocalPath(string virtualPath);
    }
}
