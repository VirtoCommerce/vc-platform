using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Infrastructure.Interceptors
{
    public class ChangeLogInterceptor : IInterceptor
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly ChangeLogPolicy _policy;
        private readonly string[] _entityTypes;

        public ChangeLogInterceptor(Func<IPlatformRepository> repositoryFactory, ChangeLogPolicy policy, string[] entityTypes)
        {
            _repositoryFactory = repositoryFactory;
            _policy = policy;
            _entityTypes = entityTypes;
        }

        [Obsolete("Don't pass IUserNameResolver")]
        public ChangeLogInterceptor(Func<IPlatformRepository> repositoryFactory, ChangeLogPolicy policy, string[] entityTypes, IUserNameResolver userNameResolver)
            : this(repositoryFactory, policy, entityTypes)
        {
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
                // Don't log events for objects that haven't been changed
                foreach (var entryWithState in context.EntriesByState.Where(x => x.Key != EntityState.Unchanged))
                {
                    var entityState = entryWithState.Key;
                    var entities = entryWithState.Where(x => x.Entity is Entity).Select(x => (Entity)x.Entity).ToList();

                    SaveChangesToLog(repository, entityState, entities);
                }

                // Process entities deleted by batch command
                if (context.BatchDeletedEntities != null)
                {
                    SaveChangesToLog(repository, EntityState.Deleted, context.BatchDeletedEntities);
                }

                repository.UnitOfWork.Commit();
            }
        }


        private void SaveChangesToLog(IPlatformRepository repository, EntityState entityState, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var entityType = entity.GetType();
                if (entityType.BaseType != null && entityType.Namespace == "System.Data.Entity.DynamicProxies")
                {
                    entityType = entityType.BaseType;
                }

                // This line allows you to use the base types to check that the current object type is matches the specified patterns
                var inheritanceChain = entityType.GetTypeInheritanceChainTo(typeof(Entity));
                var suitableEntityType = inheritanceChain.FirstOrDefault(x => IsMatchInExpression(_entityTypes, x.Name));

                if (suitableEntityType != null)
                {
                    var operationLogEntity = new OperationLogEntity
                    {
                        ObjectId = entity.Id,
                        ObjectType = suitableEntityType.Name,
                        OperationType = entityState.ToString()
                    };

                    if (_policy == ChangeLogPolicy.Cumulative)
                    {
                        var existingLogEntity = repository.OperationLogs.OrderByDescending(x => x.ModifiedDate)
                            .FirstOrDefault(x => x.ObjectId == operationLogEntity.ObjectId && x.ObjectType == operationLogEntity.ObjectType);
                        if (existingLogEntity != null)
                        {
                            existingLogEntity.ModifiedDate = DateTime.UtcNow;
                            existingLogEntity.OperationType = operationLogEntity.OperationType;
                        }
                        else
                        {
                            repository.Add(operationLogEntity);
                        }
                    }
                    else
                    {
                        repository.Add(operationLogEntity);
                    }
                }
            }
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
