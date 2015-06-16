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
		private readonly Func<IPlatformRepository> _repositoryFunc;
		public NotificationTemplateServiceImpl(Func<IPlatformRepository> repositoryFunc)
		{
			_repositoryFunc = repositoryFunc;
		}

		public NotificationTemplate GetById(string notificationTemplateId)
		{
			using (var repository = _repositoryFunc())
			{
				var entity = repository.NotificationTemplates.FirstOrDefault(nt => nt.Id.Equals(notificationTemplateId));
				return entity.ToCoreModel();
			}
		}

		public NotificationTemplate GetByNotification(string notificationTypeId, string objectId)
		{
			using (var repository = _repositoryFunc())
			{
				var entity = repository.NotificationTemplates.FirstOrDefault(nt => nt.ObjectId.Equals(objectId) && nt.NotificationTypeId.Equals(notificationTypeId));
				return entity.ToCoreModel();
			}
		}

		public NotificationTemplate Create(NotificationTemplate notificationTemplate)
		{
			using (var repository = _repositoryFunc())
			{
				var entity = notificationTemplate.ToDataModel();
				entity.Id = Guid.NewGuid().ToString("N");
				repository.Add(entity);
				CommitChanges(repository);
				var retVal = GetById(entity.Id);

				return retVal;
			}
		}

		public void Update(NotificationTemplate[] notificationTemplates)
		{
			using (var repository = _repositoryFunc())
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
			throw new NotImplementedException();
		}
	}
}
