using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Common
{
    public interface IStorefrontUrlBuilder
    {
        string ToAbsolute(string virtualPath);
        string ToAppRelative(string virtualPath);
        string ToLocalPath(string virtualPath);
    }
}
