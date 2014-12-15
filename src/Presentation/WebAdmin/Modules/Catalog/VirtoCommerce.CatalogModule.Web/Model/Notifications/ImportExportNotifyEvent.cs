using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Framework.Web.Notification;

namespace VirtoCommerce.CatalogModule.Web.Model.Notifications
{
	public class ImportExportNotifyEvent : NotifyEvent
	{
		private readonly ImportJob _job;
		
		public ImportExportNotifyEvent(ImportJob job, string creator)
			:base(creator)
		{
			_job = job;
			Status = NotifyStatus.Pending;
			NotifyType = NotifyType.LongRunningTask;
		}

		public void LogProgress(ImportResult result)
		{
			Status = !result.IsStarted ? NotifyStatus.Pending : (result.IsRunning ? NotifyStatus.Running : (result.IsFinished ? NotifyStatus.Finished : NotifyStatus.Aborted));
			Title = Description = string.Format("Processed records: {0}", result.ProcessedRecordsCount + result.ErrorsCount);
			if(result.IsCancelled)
			{
				Title =  string.Format("Import job '{0}' processing was canceled.", _job.Name);
			}
			FinishDate = result.Stopped;
			if (result.ErrorsCount > 0)
			{
				Description += Environment.NewLine + "Errors:" + Environment.NewLine + string.Join(Environment.NewLine, result.Errors.Cast<string>());
			}
			_job.Notifier.Upsert(this);
		}
	}
}
