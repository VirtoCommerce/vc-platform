using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public static class RepositoryExtension
    {
        public static void DisableChangesTracking(this IRepository repository)
        {
            //http://stackoverflow.com/questions/29106477/nullreferenceexception-in-entity-framework-from-trygetcachedrelatedend
            if (repository is System.Data.Entity.DbContext)
            {
                var dbConfiguration = ((System.Data.Entity.DbContext)repository).Configuration;
                dbConfiguration.ProxyCreationEnabled = false;
                dbConfiguration.AutoDetectChangesEnabled = false;
            }
        }

        /// <summary>
        /// Sets the command timeout for the repository.
        /// </summary>
        /// <param name="repository">Repository to set command timeout for.</param>
        /// <param name="commandTimeout">Command timeout, null to default on underlying provider settings.</param>
        public static void SetCommandTimeout(this IRepository repository, TimeSpan? commandTimeout)
        {
            if (repository is System.Data.Entity.DbContext dbContext)
            {
                dbContext.Database.CommandTimeout = (int?)commandTimeout?.TotalSeconds;
            }
        }
    }
}
