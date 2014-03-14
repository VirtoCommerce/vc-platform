using System;
using System.Collections.Generic;
using System.Web;
using MvcSiteMapProvider;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Routing;

namespace VirtoCommerce.Web.Virto.Helpers.MVC
{
    public class CatalogNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            return new List<DynamicNode>
            {
                new DynamicNode
                {
                    Action = "Display",
                    PreservedRouteParameters = new[] {Constants.Language, Constants.Store, Constants.Category},
                },
                new DynamicNode
                {
                    Action = "DisplayItem",
                    PreservedRouteParameters = new[] {Constants.Language, Constants.Store, Constants.Category, Constants.Item},
                }
            };
        }
    }
}