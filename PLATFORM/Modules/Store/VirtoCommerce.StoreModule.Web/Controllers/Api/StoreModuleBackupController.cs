using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Hangfire;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.StoreModule.Web.BackgroundJobs;
using VirtoCommerce.StoreModule.Web.Model;
using VirtoCommerce.StoreModule.Web.Model.Notifications;

namespace VirtoCommerce.StoreModule.Web.Controllers.Api
{
	[RoutePrefix("api/store")]
	public class StoreModuleBackupController : ApiController
	{
		private readonly IStoreService _storeService;
		private readonly INotifier _notifier;

		public StoreModuleBackupController(IStoreService storeService, INotifier notifier)
		{
			_storeService = storeService;
			_notifier = notifier;
		}

		/// <summary>
		/// POST api/store/export
		/// </summary>
		/// <param name="exportConfiguration"></param>
		/// <returns></returns>
		[ResponseType(typeof(ExportNotification))]
		[HttpPost]
		[Route("export")]
		public IHttpActionResult DoExport(StoreExportConfiguration exportConfiguration)
		{
			var notification = new ExportNotification(CurrentPrincipal.GetCurrentUserName())
			{
				Title = "Store export task",
				Description = "starting export...."
			};
			_notifier.Upsert(notification);

			var store = _storeService.GetById(exportConfiguration.StoreId);
			if (store == null)
			{
				throw new NullReferenceException("store");
			}

            var backupStoreJob = new BackupStoreJob();
            BackgroundJob.Enqueue(() => backupStoreJob.DoExport(exportConfiguration, notification));

			return Ok(notification);

		}


		/// <summary>
		/// POST api/store/import
		/// </summary>
        /// <param name="importConfiguration"></param>
		/// <returns></returns>
		[ResponseType(typeof(ExportNotification))]
		[HttpPost]
		[Route("import")]
		public IHttpActionResult DoImport(StoreImportConfiguration importConfiguration)
		{
			var notification = new ImportNotification(CurrentPrincipal.GetCurrentUserName())
			{
				Title = "Import store task",
				Description = "starting import...."
			};
			_notifier.Upsert(notification);

            var importJob = new BackupStoreJob();
		    BackgroundJob.Enqueue(() =>
		            importJob.DoImport(importConfiguration, notification));

			return Ok(notification);
		}


		/// <summary>
		///  GET api/catalog/importjobs/123/cancel
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id}/cancel")]
		[ResponseType(typeof(void))]
		public IHttpActionResult Cancel(string id)
		{
			return StatusCode(HttpStatusCode.NoContent);
			//var job = _jobList.FirstOrDefault(x => x.Id == id);
			//if (job != null && job.CanBeCanceled)
			//{
			//	job.CancellationToken.Cancel();
			//}

			//return StatusCode(HttpStatusCode.NoContent);
		}


	}
}