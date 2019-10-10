using System;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public static class RepositoryExtension
    {
        public static void DisableChangesTracking(this IRepository repository)
        {
            //http://stackoverflow.com/questions/29106477/nullreferenceexception-in-entity-framework-from-trygetcachedrelatedend
            if (repository.UnitOfWork is DbContextUnitOfWork dbContextUoW)
            {
                dbContextUoW.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            }
        }
    }
}
