using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
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

        public virtual IQueryable<UserApiKeyEntity> UserApiKeys => DbContext.Set<UserApiKeyEntity>();

        public virtual IQueryable<UserPasswordHistoryEntity> UserPasswordsHistory => DbContext.Set<UserPasswordHistoryEntity>();

        public virtual IQueryable<ServerCertificateEntity> ServerCertificates => DbContext.Set<ServerCertificateEntity>();
        public async Task<IEnumerable<UserPasswordHistoryEntity>> GetUserPasswordsHistoryAsync(string userId, int passwordsCountToCheck)
        {
            return await UserPasswordsHistory
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(passwordsCountToCheck)
                .ToArrayAsync();
        }


    }
}
