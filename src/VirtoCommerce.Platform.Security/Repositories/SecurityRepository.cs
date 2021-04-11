using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public class SecurityRepository : DbContextRepositoryBase<SecurityDbContext>, ISecurityRepository
    {
        private readonly PasswordOptionsExtended _passwordOptions;

        public SecurityRepository(SecurityDbContext dbContext, IOptions<PasswordOptionsExtended> passwordOptions)
            : base(dbContext)
        {
            _passwordOptions = passwordOptions.Value;
        }

        public virtual IQueryable<UserApiKeyEntity> UserApiKeys => DbContext.Set<UserApiKeyEntity>();

        public virtual IQueryable<UserPasswordHistoryEntity> UserPasswordsHistory => DbContext.Set<UserPasswordHistoryEntity>();

        public async Task<UserPasswordHistoryEntity[]> GetUserPasswordsHistoryAsync(string userId)
        {
            var passwordHistoryLength = _passwordOptions.PasswordHistory.GetValueOrDefault();

            return await UserPasswordsHistory
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(passwordHistoryLength)
                .ToArrayAsync();
        }
    }
}
