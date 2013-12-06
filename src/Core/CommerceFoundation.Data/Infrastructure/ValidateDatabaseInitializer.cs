using System.Configuration;
using System.Data.Entity;
using System.Transactions;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
    public class ValidateDatabaseInitializer<TContext> : IDatabaseInitializer<TContext>
      where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            bool databaseExists;

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                databaseExists = context.Database.Exists();
            }

            if (!databaseExists)
            {
                throw new ConfigurationErrorsException(
                  "Database does not exist");
            }
            else
            {
                //if (!context.Database.CompatibleWithModel(false))
                {
                    /*
                    throw new InvalidOperationException(
                      "The database is not compatible with the entity model.");
                     * */
                }
            }
        }
    }
}
