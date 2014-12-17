using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Importing.Model;
using VirtoCommerce.Framework.Web.Notification;

namespace VirtoCommerce.CatalogModule.Web.Model.Notifications
{
	public class ImportNotifyEvent : NotifyEvent
	{
		public ImportNotifyEvent(ImportJob job, string creator)
			:base(creator)
		{
			Job = job;
			Status = NotifyStatus.Pending;
			NotifyType = this.GetType().Name;
		}

		public void LogProgress(ImportResult result)
		{
			Status = !result.IsStarted ? NotifyStatus.Pending : (result.IsRunning ? NotifyStatus.Running : (result.IsFinished ? NotifyStatus.Finished : NotifyStatus.Aborted));
			Title = Description = string.Format("Processed records: {0}", result.ProcessedRecordsCount + result.ErrorsCount);
			if(result.IsCancelled)
			{
				Title = string.Format("Import job '{0}' processing was canceled.", Job.Name);
			}
			FinishDate = result.Stopped;
			if (result.ErrorsCount > 0)
			{
				Description += Environment.NewLine + "Errors:" + Environment.NewLine + string.Join(Environment.NewLine, result.Errors.Cast<string>());
			}
			Job.Notifier.Upsert(this);
		}

		public ImportJob Job { get; set; }
	}
}
