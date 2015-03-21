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
			NotifyType = this.GetType().Name;
			Title = "Import job -" + job.Name;
		}

		public void LogProgress(ImportResult result)
		{
			Description = string.Format("Progress: {0}/{1}/{2}", result.Length, result.ProcessedRecordsCount, result.ErrorsCount);
			if(result.IsCancelled)
			{
				Description = string.Format("Import job '{0}' processing was canceled.", Job.Name);
			}
	
			Job.Notifier.Upsert(this);
		}

		public bool IsRunning { get; set; }
		public ImportJob Job { get; set; }
	}
}
