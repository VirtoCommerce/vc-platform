using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.OrderModule.Web.Security
{
    public class OrderPermissionScopeProvider : IPermissionScopeProvider
    {
        private readonly ISecurityService _securityService;

        public OrderPermissionScopeProvider(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        /// <summary>
        /// Filter order search criteria to corespond user right
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual SearchCriteria FilterOrderSearchCriteria(string userName, SearchCriteria criteria)
        {
        
            if(!_securityService.UserHasAnyPermission(userName, null, OrderPredefinedPermissions.Read))
            {
                //Get user by Id
                var orderModuleViewScopes = _securityService.GetUserPermissions(userName)
                                                      .Where(x => x.Id.StartsWith(OrderPredefinedPermissions.Read))
                                                      .SelectMany(x => x.Scopes);
            
                //Check user has a scopes
                //Stores
                criteria.StoreIds = orderModuleViewScopes.Select(x=> OrderStoreScope.TryParse(x))
                                                         .OfType<OrderStoreScope>()
                                                         .Select(x=>x.StoreId)
                                                         .Where(x => !String.IsNullOrEmpty(x))
                                                         .ToArray();
                //employee id
                if(orderModuleViewScopes.Any(x=> OrderResponsibleScope.TryParse(x) != null))
                {
                    criteria.EmployeeId = userName;
                }
            }
            return criteria;
        } 

        #region ISecurityScopeProvider Members
        public virtual IEnumerable<PermissionScope> GetPermissionScopes(string permission)
        {
            if (permission == OrderPredefinedPermissions.Read ||
                permission == OrderPredefinedPermissions.Update)
            {
                return new PermissionScope[] { new OrderStoreScope(), new OrderResponsibleScope() };
            }
            return Enumerable.Empty<PermissionScope>();
        }

        public virtual IEnumerable<string> GetEntityScopes(Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var customerOrder = entity as CustomerOrder;
            if(customerOrder == null)
            {
                throw new NullReferenceException("customerOrder");
            }
            var retVal = new List<PermissionScope>();
            retVal.Add(new OrderStoreScope(customerOrder));
            retVal.Add(new OrderResponsibleScope(customerOrder));
            return retVal.Select(x=>x.ToString());           

        } 
        #endregion

    }
}
