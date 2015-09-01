//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VirtoCommerce.Platform.Data.Notification;
//using VirtoCommerce.Platform.Data.Repositories;
//using VirtoCommerce.Platform.Tests.Fixtures;
//using Xunit;

//namespace VirtoCommerce.Platform.Tests
//{
//	public class NotificationScenarios : IClassFixture<RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer>>
//	{
//		private readonly RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer> _fixture;
//		public NotificationScenarios(RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer> fixture)
//		{
//			_fixture = fixture;
//		}

//		[Fact]
//		[Trait("Category", "Notifications")]
//		public void CreateNotitfication()
//		{
//			var service = new NotificationTemplateServiceImpl(() => new RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer>().Db);
//			var template = service.Create(new Core.Notification.NotificationTemplate
//				{
//					Body = @"<p> Dear {{ context.first_name }} {{ context.last_name }}, you has registered on our site</p> <p> Your e-mail  - {{ context.email }} </p>",
//					Subject = @"<p> Thanks for registration {{ context.first_name }} {{ context.last_name }}!!! </p>",
//					NotificationTypeId = "RegistrationEmailNotification",
//					ObjectId = "Platform",
//					TemplateEngine = "Liquid",
//					DisplayName = "Registration template #1"
//				});


//			var manager = new NotificationManager(new LiquidNotificationTemplateResolver(), () => new RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer>().Db, service);

//			Func<RegistrationEmailNotification> registrationNotification = () =>
//			{
//				return new RegistrationEmailNotification(new DefaultEmailNotificationSendingGateway())
//				{
//					AttemptCount = 0,
//					IsActive = true,
//					MaxAttemptCount = 10,
//					ObjectId = "Platform",
//					Type = typeof(RegistrationEmailNotification).Name
//				};
//			};

//			manager.RegisterNotificationType(registrationNotification);

//			var notification = manager.GetNewNotification<RegistrationEmailNotification>();

//			notification.Login = notification.Recipient = "eo@virtoway.com";
//			notification.Sender = "evg@foo.boo";
//			notification.FirstName = "Evgeny";
//			notification.LastName = "Okhrimenko";

//			manager.ScheduleSendNotification(notification);

//			var templatesC = new RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer>().Db.NotificationTemplates.Count();
//			var notificationC = new RepositoryDatabaseFixture<PlatformRepository, PlatformDatabaseInitializer>().Db.Notifications.Count();

//			Assert.Equal(1, templatesC);
//			Assert.Equal(1, notificationC);
//			Assert.Equal("Registration template #1", template.DisplayName);
//			Assert.True(notification.Subject.Contains("Evgeny Okhrimenko"));
//		}
//	}
//}
