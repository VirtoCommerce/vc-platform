using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Repositories
{
    public interface ISecurityRepository : IRepository
    {
        IQueryable<UserApiKeyEntity> UserApiKeys { get; }      
    }
}
