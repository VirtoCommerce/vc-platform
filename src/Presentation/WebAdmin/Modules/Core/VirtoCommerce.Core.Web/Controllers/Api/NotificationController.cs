using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Framework.Web.Notification;


namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
	[RoutePrefix("api/notification")]
	public class NotificationController : ApiController
	{
		private readonly INotifier _notifier;
		public NotificationController(INotifier notifier)
		{
			_notifier = notifier;
		}

		[Authorize]
		[HttpGet]
		[ResponseType(typeof(NotifySearchResult))]
		[Route("allRecent")]
		public IHttpActionResult GetAllRecent()
		{
			var criteria = new NotifySearchCriteria { StartDate = DateTime.UtcNow.AddHours(-1), Start = 0, Count = int.MaxValue };
			var retVal = _notifier.SearchNotifies(User.Identity.Name, criteria);
			return Ok(retVal);
		}

		[Authorize]
		[HttpGet]
		[ResponseType(typeof(void))]
		[Route("markAllAsRead")]
		public IHttpActionResult MarkAllAsRead()
		{
			var criteria = new NotifySearchCriteria { OnlyNew = true, Start = 0, Count = int.MaxValue };
			var retVal = _notifier.SearchNotifies(User.Identity.Name, criteria);
			foreach (var notifyEvent in retVal.NotifyEvents)
			{
				notifyEvent.New = false;
				_notifier.Upsert(notifyEvent);
			}

			return Ok(retVal);
		}

		[Authorize]
		[HttpPost]
		[Route("")]
		public IHttpActionResult CreateNotify(NotifyEvent notify)
		{
			notify.New = true;
			notify.Created = DateTime.UtcNow;
			notify.Creator = User.Identity.Name;
			_notifier.Upsert(notify);
			return Ok(notify);
		}
	}
}
