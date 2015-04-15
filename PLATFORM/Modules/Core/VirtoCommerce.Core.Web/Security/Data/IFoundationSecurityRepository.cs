using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.CoreModule.Web.Security.Data
{
    public interface IFoundationSecurityRepository : ISecurityRepository
    {
        Account GetAccountByName(string userName, UserDetails detailsLevel);
    }
}
