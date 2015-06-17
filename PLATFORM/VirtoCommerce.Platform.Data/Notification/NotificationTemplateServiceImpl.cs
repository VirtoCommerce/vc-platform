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
		private readonly IPlatformRepository _repository;
		public NotificationTemplateServiceImpl(IPlatformRepository repository)
		{
			_repository = repository;
		}

		public NotificationTemplate GetById(string notificationTemplateId)
		{
			var entity = _repository.NotificationTemplates.FirstOrDefault(nt => nt.Id.Equals(notificationTemplateId));
			return entity.ToCoreModel();
		}

		public NotificationTemplate GetByNotification(string notificationTypeId, string objectId)
		{
			var entity = _repository.NotificationTemplates.FirstOrDefault(nt => nt.ObjectId.Equals(objectId) && nt.NotificationTypeId.Equals(notificationTypeId));
			return entity.ToCoreModel();
		}

		public NotificationTemplate Create(NotificationTemplate notificationTemplate)
		{
			var origEntity = _repository.GetNotificationTemplateByNotification(notificationTemplate.NotificationTypeId, notificationTemplate.ObjectId);
			if (origEntity == null)
			{
				origEntity = notificationTemplate.ToDataModel();
				origEntity.Id = Guid.NewGuid().ToString("N");
				_repository.Add(origEntity);
				CommitChanges(_repository);
			}

			var retVal = GetById(origEntity.Id);

			return retVal;
		}

		public void Update(NotificationTemplate[] notificationTemplates)
		{
			using (var changeTracker = base.GetChangeTracker(_repository))
			{
				foreach (var notificationTemplate in notificationTemplates)
				{
					var sourceEntity = notificationTemplate.ToDataModel();
					var targetEntity = _repository.GetNotificationTemplateByNotification(notificationTemplate.NotificationTypeId, notificationTemplate.ObjectId);
					if (targetEntity == null)
					{
						_repository.Add(sourceEntity);
					}
					else
					{
						changeTracker.Attach(targetEntity);
						sourceEntity.Patch(targetEntity);
					}
				}
				CommitChanges(_repository);
			}
		}

		public void Delete(string[] notificationIds)
		{
			throw new NotImplementedException();
		}
	}
}
