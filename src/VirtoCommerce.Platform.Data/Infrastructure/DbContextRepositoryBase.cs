using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Data.Extensions;

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

            // Mitigations the breaking changes with cascade deletion introduced in EF Core 3.0
            // https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#cascade
            // The new CascadeTiming.Immediate that is used by default in EF Core 3.0 is lead wrong track as Added  for Deleted dependent/child entities during
            // work of Patch method for data entities  
            DbContext.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            DbContext.ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;

            UnitOfWork = unitOfWork ?? new DbContextUnitOfWork(dbContext);

            dbContext.Database.SetCommandTimeout();
        }

      
        public TContext DbContext { get; private set; }

        #region IRepository Members
        /// <summary>
        /// Gets the unit of work. This class actually saves the data into underlying storage.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        public IUnitOfWork UnitOfWork { get; private set; }

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

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && DbContext != null)
            {
                DbContext.Dispose();
                DbContext = null;
                UnitOfWork = null;
            }
        }
        #endregion

    }
}
