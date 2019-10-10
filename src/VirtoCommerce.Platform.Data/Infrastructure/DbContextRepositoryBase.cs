using System.Linq;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    /// <summary>
    /// Base class for repository implementations that are based on the Entity Framework.
    /// </summary>
    public abstract class DbContextRepositoryBase<TContext> : IRepository where TContext : DbContext
    {
        protected DbContextRepositoryBase(TContext dbContext, IUnitOfWork unitOfWork = null)
        {
            DbContext = dbContext;
            UnitOfWork = unitOfWork ?? new DbContextUnitOfWork(dbContext);

            var connectionTimeout = dbContext.Database.GetDbConnection().ConnectionTimeout;
            dbContext.Database.SetCommandTimeout(connectionTimeout);
        }

        public TContext DbContext { get; }

        #region IRepository Members
        /// <summary>
        /// Gets the unit of work. This class actually saves the data into underlying storage.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        public IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Attaches the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Attach<T>(T item) where T : class
        {
            DbContext.Attach(item);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Add<T>(T item) where T : class
        {
            DbContext.Add(item);
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Update<T>(T item) where T : class
        {
            DbContext.Update(item);
            DbContext.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        public void Remove<T>(T item) where T : class
        {
            DbContext.Remove(item);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
        #endregion
    }
}
