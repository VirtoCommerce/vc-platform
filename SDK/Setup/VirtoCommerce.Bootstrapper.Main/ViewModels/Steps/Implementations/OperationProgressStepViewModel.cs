using System.ComponentModel;
using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
	public class OperationProgressStepViewModel : WizardStepViewModelBase, IOperationProgressStepViewModel
	{
		#region Private shared fields

		private static readonly object _lock = new object();
		private string CurrentExecutingPackageName = string.Empty;

		private int _acquireProgress;
		private int _applyProgress;
		private int _progressPhases;
		#endregion

		#region Windows Installer fields

		private readonly BootstrapperApplication _installer;
		private readonly Engine _engine;

		#endregion

		#region Dependencies

		private readonly IViewModelsFactory<IMainViewModel> _mainVmFactory;
		#endregion

		#region Constructors

		public OperationProgressStepViewModel(
			BootstrapperApplication installer,
			Engine engine,
			IViewModelsFactory<IMainViewModel> mainVmFactory)
		{
			_installer = installer;
			_engine = engine;
			_mainVmFactory = mainVmFactory;

			SubscribeToInstallationEvents();
		}

		#endregion

		#region Windows Installer events

		private void SubscribeToInstallationEvents()
		{
			_installer.PlanBegin += OnPlanBegin;
			_installer.Progress += OnProgress;
			_installer.CachePackageBegin += OnCachePackageBegin;
			_installer.CacheAcquireBegin += OnCacheAcquireBegin;
			_installer.CacheAcquireProgress += OnCacheAcquireProgress;
			_installer.CacheComplete += OnCacheComplete;
			_installer.ExecuteMsiMessage += OnExecuteMsiMessage;
			_installer.ExecuteProgress += OnExecuteProgress;
			_installer.ApplyBegin += OnApplyBegin;
			_installer.ApplyComplete += OnApplyComplete;
			_installer.ExecutePackageBegin += OnExecutePackageBegin;
			_installer.ExecutePackageComplete += OnExecutePackageComplete;
		}

		void OnCachePackageBegin(object sender, CachePackageBeginEventArgs e)
		{
			CurrentExecutingPackageName = e.PackageId;
		}

		void OnCacheAcquireBegin(object sender, CacheAcquireBeginEventArgs e)
		{
			CurrentExecutingPackageName = e.PackageOrContainerId;
			Message = CurrentExecutingPackageName;
		}

		private void OnPlanBegin(object sender, PlanBeginEventArgs e)
		{
			lock (_lock)
			{
				_progressPhases = (LaunchAction.Layout == _installer.Command.Action) ? 1 : 2;
				MessageAction = "Preparing:";
			}
		}

		private void OnApplyBegin(object sender, ApplyBeginEventArgs e)
		{
			lock (_lock)
			{
				MessageAction = "Applying:";
				Message = CurrentExecutingPackageName;
			}
		}

		private void OnExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
		{
			lock (_lock)
			{
				CurrentExecutingPackageName = string.Empty;
				Message = "";
			}
		}

		private void OnExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
		{
			lock (_lock)
			{
				var packageId = e.PackageId;

				// TODO: get a real package name
				CurrentExecutingPackageName = packageId;
				Message = string.Format("{0}", packageId);
			}
		}

		private void OnProgress(object sender, ProgressEventArgs e)
		{
			lock (_lock)
			{
				_acquireProgress = e.OverallPercentage;
				Progress = (_acquireProgress + _applyProgress) / _progressPhases;
				ComponentProgress = _acquireProgress / _progressPhases;

				e.Result = _mainVmFactory.GetViewModelInstance().Cancelled ? Result.Cancel : Result.Ok;
			}
		}

		private void OnCacheAcquireProgress(object sender, CacheAcquireProgressEventArgs e)
		{
			lock (_lock)
			{
				_acquireProgress = e.OverallPercentage;
				Progress = (_acquireProgress + _applyProgress) / _progressPhases;
				ComponentProgress = _acquireProgress / _progressPhases;
				ComponentMessageAction = "Acquiring:";
				ComponentMessage = string.Format("{0}", e.PayloadId);
				e.Result = _mainVmFactory.GetViewModelInstance().Cancelled ? Result.Cancel : Result.Ok;
			}
		}

		private void OnCacheComplete(object sender, CacheCompleteEventArgs e)
		{
			lock (_lock)
			{
				ComponentMessageAction = "Downloaded:";
				ComponentMessage = string.Format("{0}", CurrentExecutingPackageName);
				_acquireProgress = 100;
				ComponentProgress = _acquireProgress;

				Progress = (_acquireProgress + _applyProgress) / _progressPhases;
			}
		}

		private void OnExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
		{
			lock (_lock)
			{
				if (e.Data != null && e.Data.Count > 1 && !string.IsNullOrWhiteSpace(e.Data[1]))
				{
					ComponentMessage = e.Data[1];
				}

				if (e.MessageType == InstallMessage.ActionStart)
				{
					ComponentMessage = e.Message;
				}

				if (string.IsNullOrEmpty(ComponentMessage))
				{
					ComponentMessage = e.PackageId;
				}

				ComponentMessageAction = "Executing:";
				e.Result = _mainVmFactory.GetViewModelInstance().Cancelled ? Result.Cancel : Result.Ok;
			}
		}

		private void OnExecuteProgress(object sender, ExecuteProgressEventArgs e)
		{
			lock (_lock)
			{
				_applyProgress = e.OverallPercentage;

				ComponentProgress = _applyProgress / _progressPhases;
				Progress = (_acquireProgress + _applyProgress) / _progressPhases;

				if (_installer.Command.Display == Display.Embedded)
				{
					_engine.SendEmbeddedProgress(e.ProgressPercentage, Progress);
				}

				e.Result = _mainVmFactory.GetViewModelInstance().Cancelled ? Result.Cancel : Result.Ok;
			}
		}

		private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
		{
			if (_installer.Command.Display == Display.Full)
			{
				_isValid = true;
				_applyProgress = 100;
				ComponentProgress = 100;
				_mainVmFactory.GetViewModelInstance().MoveNextCommand.Execute(null);
			}
		}

		#endregion

		#region WizardStepViewModelBase members

		public override string Description
		{
			get { return string.Empty; }
		}

		public override bool IsBackTrackingDisabled
		{
			get { return true; }
		}

		public override bool IsValid
		{
			get { return _isValid; }
		}

		private bool _isValid;

		public override bool IsLast
		{
			get { return false; }
		}

		#endregion

		#region IOperationProgressStepViewModel implementation

		public string Message
		{
			get
			{
				return IsInDesignMode ? "Virto Commerce SDK" : _message;
			}
			set
			{
				_message = value;
				OnPropertyChanged();
			}
		}

		public string MessageAction
		{
			get
			{
				return IsInDesignMode ? "Applying" : _messageAction;
			}
			private set { _messageAction = value; OnPropertyChanged(); }
		}

		private string _messageAction;


		private string _message;

		public int Progress
		{
			get
			{
				return IsInDesignMode ? 30 : _progress;
			}
			private set
			{
				_progress = value;
				OnPropertyChanged();
			}
		}

		private int _progress;

		public string ComponentMessage
		{
			get
			{
				return IsInDesignMode ? "Acquiring" : _componentMessage;
			}
			set
			{
				_componentMessage = value;
				OnPropertyChanged();
			}
		}

		private string _componentMessage;

		public int ComponentProgress
		{
			get
			{
				return IsInDesignMode ? 10 : _componentProgress;
			}
			private set
			{
				_componentProgress = value;
				OnPropertyChanged();
			}
		}

		private int _componentProgress;

		public string ComponentMessageAction
		{
			get
			{
				return IsInDesignMode ? "Virto Commerce SDK 1.13" : _componentMessageAction;
			}
			private set { _componentMessageAction = value; OnPropertyChanged(); }
		}

		private string _componentMessageAction;

		#endregion

		private static bool IsInDesignMode
		{
			get
			{
#if DEBUG
				return (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty
																		, typeof(DependencyObject))

																		.Metadata.DefaultValue;
#else
                return false;
#endif
			}
		}
	}
}