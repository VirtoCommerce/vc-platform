using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    public class PushNotificationService : ServiceBase, IPushNotificationService
    {
        private readonly Func<IPlatformRepository> _platformRepositoryFactory;

        public PushNotificationService(Func<IPlatformRepository> platformRepositoryFactory)
        {
            _platformRepositoryFactory = platformRepositoryFactory;
        }

        public PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria)
        {
            using (var repository = _platformRepositoryFactory())
            {
                var query = repository.PushNotification;

                if (criteria.Ids != null && criteria.Ids.Any())
                {
                    query = query.Where(x => criteria.Ids.Contains(x.Id));
                }

                if (criteria.OnlyNew)
                {
                    query = query.Where(x => x.IsNew);
                }

                if (criteria.StartDate != null)
                {
                    query = query.Where(x => x.CreatedDate >= criteria.StartDate);
                }

                if (criteria.EndDate != null)
                {
                    query = query.Where(x => x.CreatedDate <= criteria.EndDate);
                }

                var sortInfos = SortInfo.Parse(criteria.OrderBy).ToArray();
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Creator", SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                var ids = query
                    .Skip(criteria.Start)
                    .Take(criteria.Count)
                    .Select(x => x.Id)
                    .ToList();

                var result = new PushNotificationSearchResult
                {
                    TotalCount = query.Count(),
                    NewCount = query.Count(x => x.IsNew),
                    NotifyEvents = repository.GetPushNotificationByIds(ids)
                        .Select( x => x.ToModel(AbstractTypeFactory<PushNotification>.TryCreateInstance()))
                        .ToList()
                };

                return result;
            }
        }

        public IEnumerable<PushNotification> GetByIds(IEnumerable<string> ids)
        {
            using (var repository = _platformRepositoryFactory())
            {
                var entities = repository.GetPushNotificationsByIds(ids);
                return entities.Select(x => x.ToModel(AbstractTypeFactory<PushNotification>.TryCreateInstance()));
            }
        }

        public void SaveChanges(IEnumerable<PushNotification> notifications)
        {
            using (var repository = _platformRepositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var nonTransientEntryIds = notifications.Where(x => x.Id != null).Select(x => x.Id).ToArray();
                var originalDataEntities = repository.PushNotification.Where(x => nonTransientEntryIds.Contains(x.Id)).ToList();

                foreach (var entry in notifications)
                {
                    var modifiedEntity = AbstractTypeFactory<PushNotificationEntity>.TryCreateInstance().FromModel(entry);

                    var originalEntity = originalDataEntities.FirstOrDefault(x => x.Id == entry.Id);
                    if (originalEntity != null)
                    {
                        changeTracker.Attach(originalEntity);
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                    }
                }

                CommitChanges(repository);
            }
        }

        public void Delete(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            using (var repository = _platformRepositoryFactory())
            {
                var items = repository.PushNotification
                    .Where(p => ids.Contains(p.Id))
                    .ToList();

                foreach (var item in items)
                {
                    repository.Remove(item);
                }

                repository.UnitOfWork.Commit();
            }
        }
    }
}
