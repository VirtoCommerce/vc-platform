using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.PushNotification
{
    public class PushNotificationService : ServiceBase, IPushNotificationService
    {
        private readonly Func<IPlatformRepository> _platformRepository;

        public PushNotificationService(Func<IPlatformRepository> repositoryFactory)
        {
            _platformRepository = repositoryFactory;
        }

        public PushNotificationSearchResult SearchPushNotification(string userId, PushNotificationSearchCriteria criteria)
        {
            using (var repository = _platformRepository())
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

                if (criteria.OrderBy != null)
                {
                    var parts = criteria.OrderBy.Split(':');
                    if (parts.Any())
                    {
                        var fieldName = parts[0];
                        if (parts.Length > 1 && String.Equals(parts[1], "desc", StringComparison.InvariantCultureIgnoreCase))
                        {
                            query = query.OrderByDescending(fieldName);
                        }
                        else
                        {
                            query = query.OrderBy(fieldName);
                        }
                    }
                }

                var ids = query
                    .Skip(criteria.Start)
                    .Take(criteria.Count)
                    .Select(x => x.Id)
                    .ToList();

                var retVal = new PushNotificationSearchResult
                {
                    TotalCount = query.Count(),
                    NewCount = query.Count(x => x.IsNew),
                    NotifyEvents = repository.GetPushNotificationByIds(ids)
                        .Select( x => x.ToModel(AbstractTypeFactory<Core.PushNotifications.PushNotification>.TryCreateInstance()))
                        .ToList()

                };

                return retVal;
            }
        }

        public IEnumerable<Core.PushNotifications.PushNotification> GetByIds(IEnumerable<string> ids)
        {
            using (var repository = _platformRepository())
            {
                var entities = repository.GetPushNotificationByIds(ids);
                return entities.Select(x => x.ToModel(AbstractTypeFactory<Core.PushNotifications.PushNotification>.TryCreateInstance()));
            }
        }

        public void SaveChanges(IEnumerable<Core.PushNotifications.PushNotification> notification)
        {
            using (var repository = _platformRepository())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var nonTransientEntryIds = notification.Where(x => x.Id != null).Select(x => x.Id).ToArray();
                var originalDataEntities = repository.PushNotification.Where(x => nonTransientEntryIds.Contains(x.Id)).ToList();
                foreach (var entry in notification)
                {
                    var originalEntity = originalDataEntities.FirstOrDefault(x => x.Id == entry.Id);
                    var modifiedEntity = AbstractTypeFactory<PushNotificationEntity>.TryCreateInstance().FromModel(entry);
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
            throw new NotImplementedException();
        }
    }
}
