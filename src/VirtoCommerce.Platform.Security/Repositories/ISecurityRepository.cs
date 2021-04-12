using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public interface ISecurityRepository : IRepository
    {
        IQueryable<UserApiKeyEntity> UserApiKeys { get; }

        IQueryable<UserPasswordHistoryEntity> UserPasswordsHistory { get; }

        Task<IEnumerable<UserPasswordHistoryEntity>> GetUserPasswordsHistoryAsync(string userId, int passwordsCountToCheck);
    }
}
