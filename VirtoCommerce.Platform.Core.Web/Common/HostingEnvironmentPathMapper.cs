using System.Web.Hosting;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Web.Common
{
    public class HostingEnvironmentPathMapper : IPathMapper
    {
        public string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
    }
}