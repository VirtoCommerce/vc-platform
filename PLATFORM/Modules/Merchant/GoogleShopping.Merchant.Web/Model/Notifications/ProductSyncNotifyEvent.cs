using VirtoCommerce.Platform.Core.Notification;

namespace GoogleShopping.MerchantModule.Web.Model.Notifications
{
    public class ProductSyncNotifyEvent : NotifyEvent
    {
        public ProductSyncNotifyEvent(ProductsSyncJob job, string creator)
            : base(creator)
        {
            Job = job;
            NotifyType = this.GetType().Name;
            Title = "Google shopping synchronization";
        }

        public void SyncProgress(SyncResult result)
        {
            Description = string.Format("Progress: {0}/{1}/{2}", result.Length, result.ProcessedRecordsCount, result.ErrorsCount);
            if (result.IsCancelled)
            {
                Description = string.Format("Import job '{0}' processing was canceled.", Job.Name);
            }

            Job.Notifier.Upsert(this);
        }

        public bool IsRunning { get; set; }
        public ProductsSyncJob Job { get; set; }
    }
}