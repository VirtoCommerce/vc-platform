using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetByIdAsync(string refreshTokenId);
        Task AddAsync(RefreshToken refreshToken);
        Task DeleteAsync(IEnumerable<string> refreshTokenIds);
    }
}
