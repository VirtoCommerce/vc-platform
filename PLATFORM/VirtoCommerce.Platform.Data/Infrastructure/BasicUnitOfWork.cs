using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Common;
namespace VirtoCommerce.Platform.Data.Infrastructure
{
	public class BasicUnitOfWork : IUnitOfWork
	{
		EFRepositoryBase _observableContext = null;
		private IInterceptor[] _interceptors = null;

		public BasicUnitOfWork(EFRepositoryBase observableContext, IInterceptor[] interceptors)
		{
			_observableContext = observableContext;
			_interceptors = interceptors;
		}

		#region IUnitOfWork Members
		public virtual int Commit()
		{
			try
			{
				return SaveChanges();
			}
			catch(Exception ex)
			{
				ex.ThrowFaultException();
				return -1;
			}
		}

		public virtual void RollbackChanges()
		{
			bool saveFailed = false;

			do
			{
				try
				{
					_observableContext.SaveChangesInternal();

				}
				catch (DbUpdateConcurrencyException ex)
				{
					saveFailed = true;

					ex.Entries.ToList()
							  .ForEach(entry =>
							  {
								  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
							  });

				}
			} while (saveFailed);
		}

		public virtual void CommitAndRefreshChanges()
		{
			_observableContext.ChangeTracker.Entries()
							  .ToList()
							  .ForEach(entry => entry.State = EntityState.Unchanged);
		}

		protected virtual int SaveChanges()
		{
			/*
			const EntityState entitiesToTrack = EntityState.Added |
									EntityState.Modified |
									EntityState.Deleted;

			 * */
			_observableContext.ChangeTracker.DetectChanges();
			ObjectContext context = this.ObjectContext;

			/*
			var elementsToSave =
				context
					.ObjectStateManager
					.GetObjectStateEntries(entitiesToTrack)
					.ToList();

			 * */
			var entries = _observableContext.ChangeTracker.Entries().ToList();

			var entriesByState = entries.ToLookup(row => row.State);

			var processInterceptors = _interceptors != null;

			InterceptionContext intercept = null;

			if (_interceptors != null)
			{
				intercept = new InterceptionContext(_interceptors)
				{
					DatabaseContext = _observableContext,
					ObjectContext = this.ObjectContext,
					ObjectStateManager = this.ObjectStateManager,
					ChangeTracker = _observableContext.ChangeTracker,
					Entries = entries,
					EntriesByState = entriesByState,
				};
			}

			if (intercept != null)
			{
				intercept.Before();
			}
			var result = _observableContext.SaveChangesInternal();

			if (intercept != null)
			{
				intercept.After();
			}

			return result;
		}
		#endregion

		protected ObjectContext ObjectContext
		{
			get { return ((IObjectContextAdapter)_observableContext).ObjectContext; }
		}

		protected ObjectStateManager ObjectStateManager
		{
			get { return this.ObjectContext.ObjectStateManager; }
		}

		public void ExecuteSql(string sql, params object[] parameters)
		{
			ObjectContext.ExecuteStoreCommand(sql, parameters);
		}

		public ObjectResult<T> ExecuteQuery<T>(string sql, params object[] parameters)
		{
			return ObjectContext.ExecuteStoreQuery<T>(sql, parameters);
		}

		public int ExecuteStoredProcedure(string procedureName, params ObjectParameter[] parameters)
		{
			return ObjectContext.ExecuteFunction(procedureName, parameters);
		}

		public ObjectResult<T> ExecuteStoredProcedure<T>(string procedureName, params ObjectParameter[] parameters)
		{
			return ObjectContext.ExecuteFunction<T>(procedureName, parameters);
		}
	}
}
