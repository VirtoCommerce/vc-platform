using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Exceptions;

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
            var result = DbContext.SaveChanges();
            return result;
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                var result = await DbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new PlatformException(ex.ExpandExceptionMessage());
            }
        }
    }
}
