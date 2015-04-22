using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
	/// <summary>
	/// Base class for repository implementations that are based on the Entity Framework.
	/// </summary>
	public abstract class EFRepositoryBase : DbContext, IRepository
	{
		protected const string DiscriminatorFieldName = "Discriminator";
		private IUnitOfWork _unitOfWork;
		private IInterceptor[] _interceptors;

		/// <summary>
		/// Initializes a new instance of the <see cref="EFRepositoryBase"/> class.
		/// </summary>
		protected EFRepositoryBase() : base("VirtoCommerce")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFRepositoryBase"/> class.
		/// </summary>
		/// <param name="nameOrConnectionString">The name or connection string.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="interceptors">The interceptors.</param>
		protected EFRepositoryBase(string nameOrConnectionString, IUnitOfWork unitOfWork = null, IInterceptor[] interceptors = null)
            : base(nameOrConnectionString)
		{
		  	_unitOfWork = unitOfWork;
			_interceptors = interceptors;
		
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EFRepositoryBase"/> class.
		/// </summary>
		/// <param name="existingConnection">The existing connection.</param>
		/// <param name="factory">The factory.</param>
		/// <param name="unitOfWork">The unit of work.</param>
		/// <param name="interceptors">The interceptors.</param>
		protected EFRepositoryBase(DbConnection existingConnection, IUnitOfWork unitOfWork = null, IInterceptor[] interceptors = null)
			: base(existingConnection, false)
		{
			_unitOfWork = unitOfWork;
			_interceptors = interceptors;
		}

		/// <summary>
		/// Sets the unit of work.
		/// </summary>
		/// <param name="unitOfWork">The unit of work.</param>
		protected void SetUnitOfWork(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// This method is called when the model for a derived context has been initialized, but
		/// before the model has been locked down and used to initialize the context.  The default
		/// implementation of this method does nothing, but it can be overridden in a derived class
		/// such that the model can be further configured before it is locked down.
		/// </summary>
		/// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
		/// <remarks>
		/// Typically, this method is called only once when the first instance of a derived context
		/// is created.  The model for that context is then cached and is for all further instances of
		/// the context in the app domain.  This caching can be disabled by setting the ModelCaching
		/// property on the given ModelBuidler, but note that this can seriously degrade performance.
		/// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
		/// classes directly.
		/// </remarks>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			base.OnModelCreating(modelBuilder);
		}

		/// <summary>
		/// Maps the entity.
		/// </summary>
		/// <param name="modelBuilder">The model builder.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="toTable">To table.</param>
		/// <param name="discriminatorColumn">The discriminator column.</param>
		/// <param name="discriminatorValue">The discriminator value.</param>
		protected void MapEntity(DbModelBuilder modelBuilder, Type entityType, string toTable, string discriminatorColumn = DiscriminatorFieldName, string discriminatorValue = "")
		{
			var method =
			  GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
			  .FirstOrDefault(mi => mi.Name.StartsWith("MapEntity") && mi.IsGenericMethodDefinition);

			if (method == null)
			{
				return;
			}
			var genericMethod = method.MakeGenericMethod(entityType);

			genericMethod.Invoke(this, new object[] { modelBuilder, toTable, discriminatorColumn, discriminatorValue });
		}

		/// <summary>
		/// Maps the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="modelBuilder">The model builder.</param>
		/// <param name="toTable">To table.</param>
		/// <param name="discriminatorColumn">The discriminator column.</param>
		/// <param name="discriminatorValue">The discriminator value.</param>
		protected void MapEntity<T>(
		  DbModelBuilder modelBuilder, string toTable, string discriminatorColumn = DiscriminatorFieldName, string discriminatorValue = "")
		  where T : class
		{
			/*
			modelBuilder.Entity<T>().Map(
			  entity =>
			  {
				  entity.ToTable(toTable);
			  });
			 * */

			var val = String.IsNullOrEmpty(discriminatorValue) ? typeof(T).Name : discriminatorValue;

			var config = modelBuilder.Entity<T>().Map(
			  entity => entity.Requires(discriminatorColumn).HasValue(val).IsOptional());

			config.ToTable(toTable);
		}

		#region IRepository Members

		/// <summary>
		/// Gets the unit of work. This class actually saves the data into underlying storage.
		/// </summary>
		/// <value>
		/// The unit of work.
		/// </value>
		public IUnitOfWork UnitOfWork
		{
			get 
			{
				if (_unitOfWork == null)
				{
					if (_interceptors == null || _interceptors.Length == 0)
					{
						_interceptors = new IInterceptor[] { new AuditableInterceptor() };
					}
						
					_unitOfWork = new BasicUnitOfWork(this, _interceptors);
				}

				return _unitOfWork; 
			}
		}

		/// <summary>
		/// Attaches the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		public void Attach<T>(T item) where T : class
		{
			Set(item.GetType()).Attach(item);
		}

		/// <summary>
		/// Determines whether [is attached to] [the specified entity].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns>
		///   <c>true</c> if [is attached to] [the specified entity]; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">entity</exception>
		public bool IsAttachedTo<T>(T entity) where T : class
		{            
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			ObjectStateEntry entry;
			if (ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
			{
				return (entry.State != EntityState.Detached);
			}
			return false;
		}

		/// <summary>
		/// Adds the or update.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		public void AddOrUpdate<T>(T item) where T : class
		{
			Set<T>().AddOrUpdate(item);
		}

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		public void Add<T>(T item) where T : class
		{
			Set(item.GetType()).Add(item);
		}

		/// <summary>
		/// Updates the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		public void Update<T>(T item) where T : class
		{
			Set(item.GetType()).Attach(item);
			Entry(item).State = EntityState.Modified;
		}

		/// <summary>
		/// Removes the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		public void Remove<T>(T item) where T : class
		{
			Set(item.GetType()).Remove(item);
		}

		/// <summary>
		/// Gets as queryable.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IQueryable<T> GetAsQueryable<T>() where T : class
		{
			return Set<T>();
		}

        /// <summary>
        /// Refreshes the specified collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
	    public void Refresh(IEnumerable collection)
	    {
            ObjectContext.Refresh(RefreshMode.StoreWins, collection);
	    }

	    #endregion

		/// <summary>
		/// Saves all changes made in this context to the underlying database.
		/// </summary>
		/// <returns>
		/// The number of objects written to the underlying database.
		/// </returns>
		public override int SaveChanges()
		{
			return UnitOfWork.Commit();
		}

		
		/// <summary>
		/// Saves the changes internal.
		/// </summary>
		/// <returns></returns>
		internal int SaveChangesInternal()
		{
			return base.SaveChanges();
		}

		#region Helper Methods
		/// <summary>
		/// Gets the object context.
		/// </summary>
		/// <value>
		/// The object context.
		/// </value>
		protected ObjectContext ObjectContext
		{
			get { return ((IObjectContextAdapter)this).ObjectContext; }
		}

		/// <summary>
		/// Gets the object state manager.
		/// </summary>
		/// <value>
		/// The object state manager.
		/// </value>
		protected ObjectStateManager ObjectStateManager
		{
			get { return ObjectContext.ObjectStateManager; }
		}
		#endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
	}
}
