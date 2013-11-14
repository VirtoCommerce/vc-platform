namespace VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model
{
    public struct StatusMessage
    {
        public StatusMessage(string statusMessageId)
            : this()
        {
            StatusMessageId = statusMessageId;
            //ShortText = null;
            //Progress = 0;
            //State = StatusMessageState.InProgress;
            //Details = null;
        }

        public string StatusMessageId { get; set; }
        public string ShortText { get; set; }

        public bool CanReportProgress { get; set; }
        public double Progress { get; set; }
        public StatusMessageState State { get; set; }

        public string Details { get; set; }
    }

	public enum StatusMessageState
	{
		InProgress,
		Success,
		Warning,
		Error
	}
}