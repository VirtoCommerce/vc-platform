using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MvcSiteMapProvider;

namespace VirtoCommerce.ApiWebClient.Providers
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Session;

    /// <summary>
    /// Filtered SiteMapNode Visibility Provider.
    /// 
    /// The Web.config setting should specify attributesToIgnore=&quot;visibility&quot; in order to use this class!
    /// 
    /// Rules are parsed left-to-right, first match wins. Asterisk can be used to match any control. Exclamation mark can be used to negate a match.
    /// </summary>
    public class NamedSiteMapNodeVisibilityProvider
        : SiteMapNodeVisibilityProviderBase
    {
        #region ISiteMapNodeVisibilityProvider Members


		public ICustomerSession CustomerSession
		{
			get
			{
                return ClientContext.Session;
			}
		}


        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        ///   <c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            // Convert to MvcSiteMapNode
            var mvcNode = node as SiteMapNode;

	        if (mvcNode != null)
	        {
		        var menu = String.Empty;
                if (sourceMetadata != null && sourceMetadata.ContainsKey("name"))
                    menu = sourceMetadata["name"].ToString();

		        // Is a visibility attribute specified?
		        var visibility = GetValue(mvcNode, "visibility");

                if (mvcNode.Attributes.ContainsKey("permissions"))
                {
                    if (!CustomerSession.IsRegistered)
                        return false;

                    //TODO get permissions
                    //var allPermissions =
                    //    GetValue(mvcNode, "permissions").Split(',').Select(x => new Permission { PermissionId = x });
                    //if (
                    //    allPermissions.Any(
                    //        permission => !SecurityService.CheckMemberPermission(StoreHelper.CustomerSession.CustomerId, permission)))
                    //{
                    //    return false;
                    //}
                }

		        if (!string.IsNullOrEmpty(visibility))
		        {
			        visibility = visibility.Trim();

			        // All set. Now parse the visibility variable.
			        if (visibility.Split(new[] {',', ';'})
						.TakeWhile(visibilityKeyword => visibilityKeyword != menu && visibilityKeyword != "*")
						.Any(visibilityKeyword => visibilityKeyword == "!" + menu || visibilityKeyword == "!*"))
			        {
				        return false;
			        }
		        }

		        var stores = GetValue(mvcNode, "stores");
		        if (!string.IsNullOrEmpty(stores))
		        {
			        if(!stores.Trim().Split(new[] {',', ';'}, StringSplitOptions.RemoveEmptyEntries)
						.Any(store => store.Equals(CustomerSession.StoreId, StringComparison.OrdinalIgnoreCase)))
			        {
				        return false;
			        }
		        }
	        }

	        // Still nothing? Then it's OK!
            return true;
        }

        private string GetValue(ISiteMapNode node, string key)
        {
            if (node.Attributes.ContainsKey(key))
            {
                return node.Attributes[key] as string;
            }

            return null;
        }
        #endregion
    }
}