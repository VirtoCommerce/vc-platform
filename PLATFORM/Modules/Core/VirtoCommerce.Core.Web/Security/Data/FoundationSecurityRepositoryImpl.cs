using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.CoreModule.Web.Security.Data
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


        public Account GetAccountByName(string userName, UserDetails detailsLevel)
        {
            var query = Accounts;

            if (detailsLevel == UserDetails.Full)
            {
                query = query
                    .Include(a => a.RoleAssignments.Select(ra => ra.Role.RolePermissions.Select(rp => rp.Permission)))
                    .Include(a => a.ApiAccounts);
            }

            return query.FirstOrDefault(a => a.UserName == userName);
        }
    }
}
