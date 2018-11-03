using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public class BasicUnitOfWork : IUnitOfWork
    {
        private readonly EFRepositoryBase _observableContext;
        private readonly IInterceptor[] _interceptors;

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
            catch (Exception ex)
            {
                ex.ThrowFaultException();
                return -1;
            }
        }

        public virtual void RollbackChanges()
        {
            var saveFailed = false;

            do
            {
                try
                {
                    _observableContext.SaveChangesInternal();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList().ForEach(entry =>
                    {
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    });
                }
            }
            while (saveFailed);
        }

        public virtual void CommitAndRefreshChanges()
        {
            _observableContext.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        #endregion


        protected virtual int SaveChanges()
        {
            _observableContext.ChangeTracker.DetectChanges();

            InterceptionContext interceptionContext = null;

            if (_interceptors != null)
            {
                var entries = _observableContext.ChangeTracker.Entries().ToList();

                interceptionContext = new InterceptionContext(_interceptors)
                {
                    DatabaseContext = _observableContext,
                    ObjectContext = ObjectContext,
                    ObjectStateManager = ObjectStateManager,
                    ChangeTracker = _observableContext.ChangeTracker,
                    Entries = entries,
                    EntriesByState = entries.ToLookup(entry => entry.State),
                    BatchDeletedEntities = _observableContext.BatchDeletedEntities,
                };
            }

            interceptionContext?.Before();

            var result = _observableContext.SaveChangesInternal();

            interceptionContext?.After();

            return result;
        }

        protected ObjectContext ObjectContext => ((IObjectContextAdapter)_observableContext).ObjectContext;
        protected ObjectStateManager ObjectStateManager => ObjectContext.ObjectStateManager;

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
