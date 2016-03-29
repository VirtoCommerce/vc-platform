using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Services
{
    /// <summary>
    /// Represent abstract factory for any custom members  services (with this extension point developer can extend exist members system by new types)
    /// </summary>
    public interface IMemberServicesFactory 
    {
        /// <summary>
        /// Register new service for customer member type
        /// </summary>
        /// <param name="memberService"></param>
        void RegisterMemberService(IMemberService memberService);
        /// <summary>
        /// Returns all registered members services
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMemberService> MemberServices { get; }
    }
}
