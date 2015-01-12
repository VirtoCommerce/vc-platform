using System.Collections.Generic;
using MvcSiteMapProvider;
using VirtoCommerce.ApiWebClient.Extensions.Routing;

namespace VirtoCommerce.Web.Helpers.MVC.SiteMap
{
    public class BreadcrumbNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            return new List<DynamicNode>
            {
                new DynamicNode
                {
                    Action = "DisplayItem",
                    PreservedRouteParameters = new[] {Constants.Language, Constants.Store, Constants.Category, Constants.Item},
                }
            };
        }
    }
}