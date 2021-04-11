using System.Linq;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public class SecurityRepository : DbContextRepositoryBase<SecurityDbContext>, ISecurityRepository
    {
        public SecurityRepository(SecurityDbContext dbContext)
            : base(dbContext)
        {
        }

        public virtual IQueryable<UserApiKeyEntity> UserApiKeys { get { return DbContext.Set<UserApiKeyEntity>(); } }


    }
}
