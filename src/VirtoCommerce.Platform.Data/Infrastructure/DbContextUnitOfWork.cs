using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public class DbContextUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        public DbContext DbContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextUnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DbContextUnitOfWork(DbContext context)
        {
            DbContext = context;
        }

        public int Commit()
        {
            DbContext.BulkSaveChanges();
            return 0;
        }

        public async Task<int> CommitAsync()
        {
            await DbContext.BulkSaveChangesAsync();
            return 0;
        }
    }
}
