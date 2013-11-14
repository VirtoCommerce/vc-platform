using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.ViewModel
{
	public class StatusMessageViewModel : ViewModelBase
	{
		private string _shortText;
		private double _progress;
		private StatusMessageState _state;
		private string _details;
		private bool _isProgressVisible;

		public string StatusMessageId { get; set; }

		public string ShortText
		{
			get { return _shortText; }
			set { _shortText = value; OnPropertyChanged(); }
		}

		public double Progress
		{
			get { return _progress; }
			set { _progress = value; OnPropertyChanged(); }
		}

		public bool IsProgressVisible
		{
			get { return _isProgressVisible && State == StatusMessageState.InProgress; }
			set { _isProgressVisible = value; }
		}

		public StatusMessageState State
		{
			get { return _state; }
			set
			{
				_state = value;
				OnPropertyChanged();
				OnPropertyChanged("IsRunning");
				OnPropertyChanged("IsWarning");
				OnPropertyChanged("IsError");
				OnPropertyChanged("IsSuccess");
				OnPropertyChanged("IsProgressVisible");
			}
		}

		public string Details
		{
			get { return _details; }
			set { _details = value; OnPropertyChanged(); OnPropertyChanged("IsDetailsPresent"); }
		}

		public bool IsDetailsPresent
		{
			get { return string.IsNullOrEmpty(Details); }
		}

		public bool IsRunning
		{
			get { return State == StatusMessageState.InProgress; }
		}

		public bool IsWarning
		{
			get { return State == StatusMessageState.Warning; }
		}

		public bool IsError
		{
			get { return State == StatusMessageState.Error; }
		}

		public bool IsSuccess
		{
			get { return State == StatusMessageState.Success; }
		}
	}
}