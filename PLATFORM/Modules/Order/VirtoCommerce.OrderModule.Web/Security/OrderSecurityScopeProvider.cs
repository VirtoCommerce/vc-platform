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
    public class OrderSecurityScopeProvider : IPermissionScopeProvider
    {
        private readonly ISecurityService _securityService;

        public OrderSecurityScopeProvider(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        /// <summary>
        /// Filter order search criteria to corespond user right
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual SearchCriteria FilterOrderSearchCriteria(string userId, SearchCriteria criteria)
        {
        
            if(!_securityService.UserHasAnyPermission(userId, null, "order:view"))
            {
                //Get user by Id
                var user = _securityService.FindByIdAsync(userId, UserDetails.Full).Result;
                var orderModuleViewScopes = user.Roles.SelectMany(x => x.Permissions)
                                            .Where(x => x.Name.StartsWith("order:view:"))
                                             .SelectMany(x => x.Scopes);
                //Check user has a scopes
                //Stores
                criteria.StoreIds = orderModuleViewScopes.Select(x=> OrderStoreScope.TryParse(x))
                                                         .OfType<OrderStoreScope>()
                                                         .Select(x=>x.StoreId)
                                                         .Where(x => !String.IsNullOrEmpty(x))
                                                         .ToArray();
                //employee id
                if(orderModuleViewScopes.Any(x=> OrderStoreScope.TryParse(x) != null))
                {
                    criteria.EmployeeId = userId;
                }
            }
            return criteria;
        } 

        #region ISecurityScopeProvider Members
        public virtual IEnumerable<PermissionScope> GetPermissionScopes(string permission)
        {
            return new PermissionScope[] { new OrderStoreScope(), new OrderResponsibleScope() };       
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
