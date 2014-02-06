using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;

namespace VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.ViewModel
{
	public class StatusIndicatorViewModel : ViewModelBase
	{
		// singleton instance
		private static StatusIndicatorViewModel _instance;

		private StatusIndicatorViewModel()
		{
			Messages = new ObservableCollection<StatusMessageViewModel>();
			EventSystem.Subscribe<StatusMessage>(x => OnUIThread(() => OnStatusMessageReceived(x)));

			ClearCompleted = new DelegateCommand(RaiseClearCompletedInteractionRequest);
			ShowDetailsCommand = new DelegateCommand<string>(RaiseShowDetailsInteractionRequest);
			MessageDismissCommand = new DelegateCommand<string>(RaiseMessageDismissInteractionRequest);
			ToggleDetailsCommand = new DelegateCommand(RaiseToggleDetailsInteractionRequest);
		}

		public static StatusIndicatorViewModel GetInstance()
		{
			return _instance ?? (_instance = new StatusIndicatorViewModel());
		}

		public ObservableCollection<StatusMessageViewModel> Messages { get; private set; }

		private bool _isRunning;
		private bool _isWarning;
		private bool _isError;
		private bool _isDetailsVisible;
		private bool _isMessageDetailsVisible;
		private StatusMessageViewModel _messageDetailsObject;

		public bool IsRunningExist
		{
			get { return _isRunning; }
			private set
			{
				_isRunning = value;
				OnPropertyChanged();
			}
		}

		public bool IsSucceededExist
		{
			get { return Messages.Any(x => x.State == StatusMessageState.Success); }
		}

		public bool IsWarning
		{
			get { return _isWarning; }
			private set
			{
				_isWarning = value;
				OnPropertyChanged();
			}
		}

		public bool IsError
		{
			get { return _isError; }
			private set
			{
				_isError = value;
				OnPropertyChanged();
			}
		}

		public bool IsIndicatorVisible
		{
			get { return Messages.Any(x => x.State != StatusMessageState.Warning); }
		}

		public bool IsCompletedPresent
		{
			get { return Messages.Any(x => x.State != StatusMessageState.InProgress); }
		}

		public bool IsDetailsVisible
		{
			get { return _isDetailsVisible; }
			set { _isDetailsVisible = value; OnPropertyChanged(); }
		}

		public DelegateCommand ClearCompleted { get; private set; }
		public DelegateCommand<string> ShowDetailsCommand { get; private set; }
		public DelegateCommand<string> MessageDismissCommand { get; private set; }
		public DelegateCommand ToggleDetailsCommand { get; private set; }

		public int CountRunning
		{
			get { return Messages.Where(x => x.IsRunning).Count(); }
		}

		public int CountSucceeded
		{
			get { return Messages.Where(x => x.State == StatusMessageState.Success).Count(); }
		}

		public int CountFailed
		{
			get { return Messages.Where(x => x.State == StatusMessageState.Error).Count(); }
		}

		public int CountWarnings
		{
			get { return Messages.Where(x => x.State == StatusMessageState.Warning).Count(); }
		}

		public int CountNonWarnings
		{
			get { return Messages.Where(x => x.State != StatusMessageState.Warning).Count(); }
		}

		private bool _hasFirstMessageReceived;
		private void OnStatusMessageReceived(StatusMessage message)
		{
			var statusVM = Messages.FirstOrDefault(x => x.StatusMessageId == message.StatusMessageId);
			if (statusVM == null)
			{
				var item = new StatusMessageViewModel
					{
						ShortText = message.ShortText,
						StatusMessageId = message.StatusMessageId,
						Progress = message.CanReportProgress ? message.Progress : 100,
						IsProgressVisible = message.CanReportProgress,
						State = message.State,
						Details = message.Details
					};

				Messages.Add(item);

				// display details for the very first message
				if (!_hasFirstMessageReceived)
				{
					_hasFirstMessageReceived = true;
					IsDetailsVisible = true;
				}
			}
			else
			{
				statusVM.Details = message.Details;
				if (!string.IsNullOrEmpty(message.ShortText))
					statusVM.ShortText = message.ShortText;
				if (statusVM.IsProgressVisible)
					statusVM.Progress = message.Progress;
				statusVM.State = message.State;
			}

			UpdateStatuses();
		}

		private void RaiseClearCompletedInteractionRequest()
		{
			var inactive = Messages.Where(x => !x.IsRunning).ToList();
			inactive.ForEach(x => Messages.Remove(x));

			IsDetailsVisible = Messages.Count > 0;
			UpdateStatuses();
		}

		private void RaiseShowDetailsInteractionRequest(string id)
		{
			var item = Messages.Where(x => x.StatusMessageId == id).FirstOrDefault();
			if (item != null)
			{
				IsMessageDetailsVisible = true;
				MessageDetailsObject = item;
			}
		}

		public StatusMessageViewModel MessageDetailsObject
		{
			get { return _messageDetailsObject; }
			set { _messageDetailsObject = value; OnPropertyChanged(); }
		}

		public bool IsMessageDetailsVisible
		{
			get { return _isMessageDetailsVisible; }
			set { _isMessageDetailsVisible = value; OnPropertyChanged(); }
		}

		private void RaiseMessageDismissInteractionRequest(string id)
		{
			var item = Messages.Where(x => x.StatusMessageId == id).FirstOrDefault();
			if (item != null)
				Messages.Remove(item);

			IsDetailsVisible = Messages.Count > 0;
			UpdateStatuses();
		}

		private void RaiseToggleDetailsInteractionRequest()
		{
			IsDetailsVisible = !IsDetailsVisible;
		}

		private void UpdateStatuses()
		{
			IsRunningExist = Messages.Any(x => x.State == StatusMessageState.InProgress);
			IsWarning = Messages.Any(x => x.State == StatusMessageState.Warning);
			IsError = Messages.Any(x => x.State == StatusMessageState.Error);

			OnPropertyChanged("IsSucceededExist");
			OnPropertyChanged("IsIndicatorVisible");
			OnPropertyChanged("IsCompletedPresent");

			OnPropertyChanged("CountRunning");
			OnPropertyChanged("CountSucceeded");
			OnPropertyChanged("CountFailed");
			OnPropertyChanged("CountWarnings");
			OnPropertyChanged("CountNonWarnings");
		}
	}
}
