using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Notification
{
	public class NotificationTemplateServiceImpl : ServiceBase, INotificationTemplateService
	{
		private readonly Func<IPlatformRepository> _repositoryFactory;
		public NotificationTemplateServiceImpl(Func<IPlatformRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		public NotificationTemplate GetById(string notificationTemplateId)
		{
			using (var repository = _repositoryFactory())
			{
				var entity = repository.NotificationTemplates.FirstOrDefault(nt => nt.Id.Equals(notificationTemplateId));
				return entity.ToCoreModel();
			}
		}

		public NotificationTemplate GetByNotification(string notificationTypeId, string objectId)
		{
			using (var repository = _repositoryFactory())
			{
				var entity = repository.NotificationTemplates.FirstOrDefault(nt => nt.ObjectId.Equals(objectId) && nt.NotificationTypeId.Equals(notificationTypeId));
				return entity.ToCoreModel();
			}
		}

		public NotificationTemplate Create(NotificationTemplate notificationTemplate)
		{
			using (var repository = _repositoryFactory())
			{
				var origEntity = repository.GetNotificationTemplateByNotification(notificationTemplate.NotificationTypeId, notificationTemplate.ObjectId);
				if (origEntity == null)
				{
					origEntity = notificationTemplate.ToDataModel();
					origEntity.Id = Guid.NewGuid().ToString("N");
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
					var targetEntity = repository.GetNotificationTemplateByNotification(notificationTemplate.NotificationTypeId, notificationTemplate.ObjectId);
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

		public void Delete(string[] notificationIds)
		{
			using (var repository = _repositoryFactory())
			{
				foreach(var id in notificationIds)
				{
					var deletedEntity = repository.Notifications.FirstOrDefault(x => x.Id == id);
					if(deletedEntity != null)
					{
						repository.Remove(deletedEntity);
					}
				}
				repository.UnitOfWork.Commit();
			}
		}
	}
}
