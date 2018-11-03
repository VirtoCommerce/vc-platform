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
    }
}
