using System;
using System.Collections.Generic;
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
                    Controller = "Catalog",
                    PreservedRouteParameters = new[] {Constants.Category},
                    ChangeFrequency = ChangeFrequency.Always,
                    UpdatePriority = UpdatePriority.High
                },
                new DynamicNode
                {
                    Action = "DisplayItem",
                    Controller = "Catalog",
                    PreservedRouteParameters = new[] {Constants.Item},
                    ChangeFrequency = ChangeFrequency.Always,
                    UpdatePriority = UpdatePriority.High
                }
            };
        }
    }
}