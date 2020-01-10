using System.Collections.Specialized;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    public static class RepositoryExtension
    {
        public static void DisableChangesTracking(this IRepository repository)
        {
            if (repository.UnitOfWork is DbContextUnitOfWork dbContextUoW)
            {
                dbContextUoW.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
                //We can't use QueryTrackingBehavior.NoTracking  because this will cause stop breaking-queries working that we use load dependencies in many places.
                //https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#notrackingresolution
                // dbContextUoW.DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }
        }

        /// <summary>
        /// This extension is allow to get around breaking changes is introduced in EF Core 3.0 that leads to throw
        /// Database operation expected to affect 1 row(s) but actually affected 0 row(s) exception when trying to add the new children entities with manually set keys
        /// https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#detectchanges-honors-store-generated-key-values
        /// </summary>
        /// <param name="repository"></param>
        public static void TrackModifiedAsAddedForNewChildEntities(this IRepository repository, IEntity entity)
        {
            if (repository.UnitOfWork is DbContextUnitOfWork dbContextUoW)
            {
                var flatObservableCollections = entity.GetFlatObjectsListWithInterface<INotifyCollectionChanged>();
                foreach (var collection in flatObservableCollections)
                {
                    collection.CollectionChanged += (sender, args) =>
                    {
                        if (args.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach (var newItem in args.NewItems)
                            {
                                dbContextUoW.DbContext.Add(newItem);
                            }
                        }                      
                    };
                }
            }

        }
    }
}
