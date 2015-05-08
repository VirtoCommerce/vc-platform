using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public class ChangeLogInterceptor : IInterceptor
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly string[] _entityTypes;
        private readonly ChangeLogPolicy _policy;

        public ChangeLogInterceptor(Func<IPlatformRepository> repositoryFactory, ChangeLogPolicy policy, string[] entityTypes)
        {
            _repositoryFactory = repositoryFactory;
            _entityTypes = entityTypes;
            _policy = policy;
        }

        /// <summary>
        /// Befores the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Before(InterceptionContext context)
        {
        }

        /// <summary>
        /// Executes after the changes to the context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void After(InterceptionContext context)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var entryWithState in context.EntriesByState.Where(x => x.Key != EntityState.Unchanged)) // added unchanged filter, so we don't log events for objects that haven't been changed
                {
                    foreach (var entry in entryWithState.Where(x => x.Entity is Entity))
                    {
                        var now = DateTime.UtcNow;
                        var entityType = entry.Entity.GetType();
                        if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                        {
                            entityType = entityType.BaseType;
                        }

                        if (_entityTypes == null || IsMatchInExpression(_entityTypes, entityType.Name))
                        {
                            var activityLog = StateEntry2OperationLog(entityType.Name, now, ((Entity)entry.Entity).Id, entryWithState.Key);
                            if (_policy == ChangeLogPolicy.Cumulative)
                            {
                                var alreadyExistLog = repository.OperationLogs.OrderByDescending(x => x.ModifiedDate)
                                                                .FirstOrDefault(x => x.ObjectId == activityLog.ObjectId && x.ObjectType == activityLog.ObjectType);
                                if (alreadyExistLog != null)
                                {
                                    alreadyExistLog.ModifiedBy = activityLog.ModifiedBy;
                                    alreadyExistLog.ModifiedDate = activityLog.ModifiedDate;
                                    alreadyExistLog.OperationType = activityLog.OperationType;

                                }
                                else
                                {
                                    repository.Add(activityLog);
                                }
                            }
                            else
                            {
                                repository.Add(activityLog);
                            }
                        }
                    }
                }

                repository.UnitOfWork.Commit();
            }
        }



        /// <summary>
        /// States the entry2 operation log.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="createdDate">The createdDate.</param>
        /// <param name="objectId">The key value.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        /// OperationLog object
        /// </returns>
        private OperationLogEntity StateEntry2OperationLog(string objectType, DateTime createdDate, string objectId, EntityState state)
        {
            var retVal = new OperationLogEntity
            {
                Id = Guid.NewGuid().ToString("N"),
                CreatedDate = createdDate,
                ObjectId = objectId,
                ObjectType = objectType,
                CreatedBy = CurrentPrincipal.GetCurrentUserName(),
                OperationType = state.ToString()
            };

            return retVal;
        }

        private static bool IsMatchInExpression(string[] expressions, string name)
        {
            var retVal = true;

            if (expressions != null)
            {
                var inverse = expressions.Any(x => x.Contains("!"));
                expressions = expressions.Select(x => x.Replace("!", "")).ToArray();

                if (!string.IsNullOrEmpty(name))
                {
                    retVal = expressions.Any(x => string.Equals(x, name, StringComparison.InvariantCultureIgnoreCase));
                    retVal = inverse ? !retVal : retVal;
                }
            }

            return retVal;
        }
    }
}
