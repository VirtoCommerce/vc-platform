using System.Linq;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public class SecurityRepositoryInMemory : BaseRepositoryInMemory, ISecurityRepository
    {
        public IQueryable<UserApiKeyEntity> UserApiKeys => Get<UserApiKeyEntity>().AsQueryable();
    }
}
