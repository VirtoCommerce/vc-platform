using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Security.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        public RefreshTokenService(Func<IPlatformRepository> repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }

        protected Func<IPlatformRepository> RepositoryFactory { get; }

        public virtual async Task<RefreshToken> GetByIdAsync(string refreshTokenId)
        {
            RefreshTokenEntity refreshTokenEntity;
            using (var repository = RepositoryFactory())
            {
                refreshTokenEntity = await repository.RefreshTokens.SingleOrDefaultAsync(token => token.Id == refreshTokenId);
            }

            RefreshToken result = null;
            if (refreshTokenEntity != null)
            {
                result = refreshTokenEntity.ToModel(AbstractTypeFactory<RefreshToken>.TryCreateInstance());
            }

            return result;
        }

        public virtual Task AddAsync(RefreshToken refreshToken)
        {
            using (var repository = RepositoryFactory())
            {
                var tokenEntity = AbstractTypeFactory<RefreshTokenEntity>.TryCreateInstance().FromModel(refreshToken);
                repository.Add(tokenEntity);

                repository.UnitOfWork.Commit();
            }

            return Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(IEnumerable<string> refreshTokenIds)
        {
            if (refreshTokenIds.Any())
            {
                using (var repository = RepositoryFactory())
                {
                    await RemoveInternalAsync(repository, refreshTokenIds);
                    repository.UnitOfWork.Commit();
                }
            }
        }

        protected virtual async Task RemoveInternalAsync(IPlatformRepository repository, IEnumerable<string> refreshTokenIds)
        {
            var tokenEntities = await repository.RefreshTokens.Where(r => refreshTokenIds.Contains(r.Id)).ToListAsync();
            foreach (var tokenEntity in tokenEntities)
            {
                repository.Remove(tokenEntity);
            }
        }
    }
}
