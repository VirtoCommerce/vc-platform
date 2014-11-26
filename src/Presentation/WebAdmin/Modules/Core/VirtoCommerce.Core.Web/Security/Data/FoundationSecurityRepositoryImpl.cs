using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class FoundationSecurityRepositoryImpl : EFSecurityRepository, IFoundationSecurityRepository
    {
        public FoundationSecurityRepositoryImpl()
            : this("VirtoCommerce")
        {
        }
       public FoundationSecurityRepositoryImpl(string nameOrConnectionString)
            : this(nameOrConnectionString, null)
        {
        }
       public FoundationSecurityRepositoryImpl(string nameOrConnectionString, IInterceptor[] interceptors = null)
            : base(nameOrConnectionString, null, interceptors)
		{
		}

        public Account[] GetAccounts()
        {
            return Accounts.Include("RoleAssignments.Role.RolePermissions.Permission").ToArray();
        }

        public Account GetAccount(string userName)
        {
            return Accounts.Include("RoleAssignments.Role.RolePermissions.Permission").FirstOrDefault(x => x.UserName == userName);
        }
    }
}
