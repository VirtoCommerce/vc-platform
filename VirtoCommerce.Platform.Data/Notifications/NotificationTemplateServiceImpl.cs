using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Notifications
{
    public class NotificationTemplateServiceImpl : ServiceBase, INotificationTemplateService
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        public NotificationTemplateServiceImpl(Func<IPlatformRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public IEnumerable<NotificationTemplate> GetAllTemplates()
        {
            using (var repository = _repositoryFactory())
            {
                var retVal = repository.NotificationTemplates.ToArray();
                return retVal.Select(x => x.ToCoreModel());
            }
        }

        public NotificationTemplate GetById(string notificationTemplateId)
        {
            using (var repository = _repositoryFactory())
            {
                var entity = repository.NotificationTemplates.FirstOrDefault(nt => nt.Id.Equals(notificationTemplateId));
                return entity.ToCoreModel();
            }
        }

        public NotificationTemplate GetByNotification(string notificationTypeId, string objectId, string objectTypeId, string language)
        {
            NotificationTemplate retVal = null;
            using (var repository = _repositoryFactory())
            {
                var templatesEntities = repository.NotificationTemplates.Where(x => x.ObjectId == objectId && x.NotificationTypeId == notificationTypeId && x.ObjectTypeId == objectTypeId).ToArray();

                if (!templatesEntities.Any())
                {
                    //Get default templates without object
                    templatesEntities = repository.NotificationTemplates.Where(x => x.NotificationTypeId == notificationTypeId && x.ObjectId == null && x.ObjectTypeId == null).ToArray();
                }
                var templateEntity = templatesEntities.Where(x => x.Language == null || x.Language.EqualsInvariant(language))
                                                       .OrderByDescending(x => x.Language).FirstOrDefault();
                if (templateEntity != null)
                {
                    retVal = templateEntity.ToCoreModel();
                }
            }

            return retVal;
        }

        public NotificationTemplate[] GetNotificationTemplatesByNotification(string notificationTypeId, string objectId, string objectTypeId)
        {
            List<NotificationTemplate> retVal = new List<NotificationTemplate>();

            using (var repository = _repositoryFactory())
            {
                var entities = repository.NotificationTemplates.Where(nt => nt.NotificationTypeId.Equals(notificationTypeId) && nt.ObjectId.Equals(objectId) && nt.ObjectTypeId.Equals(objectTypeId));
                if (entities.Any())
                {
                    foreach (var entity in entities)
                    {
                        retVal.Add(entity.ToCoreModel());
                    }
                }
            }

            return retVal.ToArray();
        }

        public NotificationTemplate Create(NotificationTemplate notificationTemplate)
        {
            using (var repository = _repositoryFactory())
            {
                var origEntity = repository.GetNotificationTemplateByNotification(notificationTemplate.NotificationTypeId, notificationTemplate.ObjectId, notificationTemplate.ObjectTypeId, notificationTemplate.Language);
                if (origEntity == null)
                {
                    origEntity = notificationTemplate.ToDataModel();
                    repository.Add(origEntity);
                    CommitChanges(repository);
                }

                var retVal = GetById(origEntity.Id);

                return retVal;
            }
        }

        public void Update(NotificationTemplate[] notificationTemplates)
        {
            using (var repository = _repositoryFactory())
            using (var changeTracker = base.GetChangeTracker(repository))
            {
                foreach (var notificationTemplate in notificationTemplates)
                {
                    var sourceEntity = notificationTemplate.ToDataModel();
                    NotificationTemplateEntity targetEntity = null;
                    if (!sourceEntity.IsTransient())
                    {
                        targetEntity = repository.NotificationTemplates.FirstOrDefault(x => x.Id == sourceEntity.Id);
                    }
                    else
                    {
                        targetEntity = repository.GetNotificationTemplateByNotification(notificationTemplate.NotificationTypeId, notificationTemplate.ObjectId, notificationTemplate.ObjectTypeId, notificationTemplate.Language);
                    }

                    if (targetEntity == null)
                    {
                        repository.Add(sourceEntity);
                    }
                    else
                    {
                        changeTracker.Attach(targetEntity);
                        sourceEntity.Patch(targetEntity);
                    }
                }
                CommitChanges(repository);
            }
        }

        public void Delete(string[] notificationTemplateIds)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in notificationTemplateIds)
                {
                    var deletedEntity = repository.NotificationTemplates.FirstOrDefault(x => x.Id == id);
                    if (deletedEntity != null)
                    {
                        repository.Remove(deletedEntity);
                    }
                }
                repository.UnitOfWork.Commit();
            }
        }
    }
}