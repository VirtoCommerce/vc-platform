using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Extensibility;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions
{
    /// <summary>
    /// Filtered SiteMapNode Visibility Provider.
    /// 
    /// The Web.config setting should specify attributesToIgnore=&quot;visibility&quot; in order to use this class!
    /// 
    /// Rules are parsed left-to-right, first match wins. Asterisk can be used to match any control. Exclamation mark can be used to negate a match.
    /// </summary>
    public class NamedSiteMapNodeVisibilityProvider
        : ISiteMapNodeVisibilityProvider
    {
        #region ISiteMapNodeVisibilityProvider Members

        static ISecurityService _securityService;
        public static ISecurityService SecurityService
        {
            get { return _securityService ?? (_securityService = DependencyResolver.Current.GetService<ISecurityService>()); }
        }

		public ICustomerSession CustomerSession
		{
			get
			{
				var session = ServiceLocator.Current.GetInstance<ICustomerSessionService>();
				return session.CustomerSession;
			}
		}

        /// <summary>
        /// Determines whether the node is visible.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="context">The context.</param>
        /// <param name="sourceMetadata">The source metadata.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is visible; otherwise, <c>false</c>.
        /// </returns>
        public bool IsVisible(SiteMapNode node, HttpContext context, IDictionary<string, object> sourceMetadata)
        {
            // Convert to MvcSiteMapNode
            var mvcNode = node as MvcSiteMapNode;

	        if (mvcNode != null)
	        {
		        var menu = String.Empty;
		        if (context.Items.Contains("menu"))
			        menu = context.Items["menu"].ToString();

		        // Is a visibility attribute specified?
		        var visibility = mvcNode["visibility"];
		        if (string.IsNullOrEmpty(visibility) && !String.IsNullOrEmpty(menu))
			        return false;

		        if (mvcNode.MetaAttributes.ContainsKey("permissions"))
		        {
                    if (!CustomerSession.IsRegistered)
				        return false;

			        var allPermissions =
				        mvcNode.MetaAttributes["permissions"].Split(',').Select(x => new Permission {PermissionId = x});
			        if (
				        allPermissions.Any(
					        permission => !SecurityService.CheckMemberPermission(StoreHelper.CustomerSession.CustomerId, permission)))
			        {
				        return false;
			        }
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

		        var stores = mvcNode["stores"];
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
        #endregion
    }
}