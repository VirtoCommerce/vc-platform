using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.Domain.Customer.Services
{
    /// <summary>
    /// Default members service abstract factory implementation 
    /// </summary>
    public class DefaultMembersServiceFactory : IMembersFactory
    {
        private List<IMemberService> _memberServices = new List<IMemberService>();
        public DefaultMembersServiceFactory()
        {
        }

        #region IMemberServicesFactory Members
        public IEnumerable<IMemberService> GetAllServices()
        {
            return _memberServices;
        }

        public void RegisterMemberService(IMemberService memberService, uint priority)
        {
            _memberServices.Insert((int)Math.Min(priority, _memberServices.Count()), memberService);
        } 
        #endregion
    }
}
