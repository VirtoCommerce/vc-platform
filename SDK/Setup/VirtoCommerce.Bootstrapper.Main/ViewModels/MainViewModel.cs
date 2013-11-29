using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Bootstrapper.Main.Infrastructure;
using VirtoCommerce.Bootstrapper.Main.Infrastructure.Extensions;
using VirtoCommerce.Bootstrapper.Main.Infrastructure.Native;
using VirtoCommerce.Bootstrapper.Main.Properties;

namespace VirtoCommerce.Bootstrapper.Main.ViewModels
{
	public class MainViewModel : WizardViewModelBare, IMainViewModel
	{
		#region Private shared fields

		private static readonly object _lock = new object();

		#endregion

		#region Windows Installer fields

		private readonly BootstrapperApplication _installer;
		private readonly Engine _engine;
		private readonly IViewModelsFactory<IOperationCompletedStepViewModel> _completedStepVmFactory;
		private readonly IViewModelsFactory<IInstallationStepViewModel> _installationStepVmFactory;
		private readonly NavigationManager _navigationManager;
		private readonly IViewModelsFactory<IOperationProgressStepViewModel> _progressStepVmFactory;
		private readonly IViewModelsFactory<IModificationStepViewModel> _modificationStepVmFactory;
		private readonly IViewModelsFactory<ILayoutStepViewModel> _layoutVmFactory; 

		#endregion

		#region Constructors

		public MainViewModel()
		{
#if DESIGN
            if (IsInDesignMode)
            {
                RegisterWizardSteps();
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/VirtoCommerce.Bootstrapper.Main;component/MainModuleDictionary.xaml", UriKind.Relative) });
            }
#endif
			ViewTitle = new ViewTitleBase { Title = Resources.SDKTitle };
			Cancelled = false;
			CancelCommand = new DelegateCommand<object>(x => Cancel(), x => !IsInitializing && !CurrentStep.IsLast && !Cancelled);
			CancelConfirmRequest = new InteractionRequest<Confirmation>();

			OKDialogRequest = new InteractionRequest<Notification>();
			OKCancelDialogRequest = new InteractionRequest<Confirmation>();
			YesNoDialogRequest = new InteractionRequest<Confirmation>();
			YesNoCancelDialogRequest = new InteractionRequest<Confirmation>();
			AbortRetryIgnoreDialogRequest = new InteractionRequest<Confirmation>();
		}

		public MainViewModel(
			BootstrapperApplication installer, 
			Engine engine, 
			IViewModelsFactory<IOperationCompletedStepViewModel> completedStepVmFactory,
			IViewModelsFactory<IInstallationStepViewModel> installationStepVmFactory,
			NavigationManager navigationManager,
			IViewModelsFactory<IOperationProgressStepViewModel> progressStepVmFactory,
			IViewModelsFactory<IModificationStepViewModel> modificationStepVmFactory,
			IViewModelsFactory<ILayoutStepViewModel> layoutVmFactory)
			: this()
		{
			_completedStepVmFactory = completedStepVmFactory;
			_installationStepVmFactory = installationStepVmFactory;
			_navigationManager = navigationManager;
			_progressStepVmFactory = progressStepVmFactory;
			_modificationStepVmFactory = modificationStepVmFactory;
			_layoutVmFactory = layoutVmFactory;

			IsInitializing = true;

			PropertyChanged += OnPropertyChanged;
			Cancelled = false;
			_installer = installer;
			_engine = engine;

			if (_installer.Command.Display == Display.Passive || _installer.Command.Display == Display.Full)
			{
				OnUIThread(() => Application.Current.MainWindow.Closing += OnClosing);
			}

			SubscribeToInstallationEvents();
		}

		#endregion

		#region IMainViewModel implementation

		private bool _cancelled;
		public bool Cancelled
		{
			get { return _cancelled; }
			private set { _cancelled = value; OnPropertyChanged(); }
		}

		public DelegateCommand<object> CancelCommand { get; private set; }

		public InteractionRequest<Confirmation> CancelConfirmRequest { get; private set; }

		private void Cancel()
		{
			var confirmation = new ConditionalConfirmation { Content = Resources.ConfirmCancellation, Title = Resources.Cancellation };
			CancelConfirmRequest.Raise(confirmation, x =>
			{
				if (x.Confirmed)
				{
					_engine.Log(LogLevel.Error, Resources.Cancelled);
					Cancelled = true;
					CancelCommand.RaiseCanExecuteChanged();
					if (!IsApplying && !CurrentStep.IsLast)
					{
						_engine.Log(LogLevel.Error, Resources.Cancelled);
						Shutdown((int)SystemErrorCodes.ERROR_CANCELLED);
					}
				}
			});
		}

		public bool IsReadyToInstall
		{
			get
			{
				return CurrentStepNumber == 1;
			}
		}

		public bool IsUninstall
		{
			get
			{
				if (!(AllRegisteredSteps != null && AllRegisteredSteps.Count >= CurrentStepNumber))
				{
					return false;
				}

				var currentStep = AllRegisteredSteps[CurrentStepNumber - 1];
				return currentStep is IModificationStepViewModel;
			}
		}

		public bool Finished
		{
			get { return _finished; }
			set
			{
				_finished = value;
				CancelCommand.RaiseCanExecuteChanged();
			}
		}

		private bool _finished;

		private void OnClosing(object sender, CancelEventArgs e)
		{
			if (!Cancelled && Operation != Operation.Help)
			{
				CancelCommand.Execute(null);
				e.Cancel = !Cancelled;
			}
		}

		public void Finish()
		{
			Shutdown(0);
		}

		public void Shutdown(int exitCode)
		{
			OnUIThread(() => Application.Current.Shutdown(exitCode));
		}

		#endregion

		#region Windows Installer events

		private void SubscribeToInstallationEvents()
		{
			_installer.ResolveSource += OnResolveSource;

			_installer.DetectBegin += OnDetectBegin;
			_installer.DetectRelatedBundle += OnDetectRelatedBundle;
			_installer.DetectComplete += OnDetectComplete;
			_installer.DetectPackageComplete += OnDetectPackageComplete;

			_installer.PlanPackageBegin += OnPlanPackageBegin;
			_installer.PlanComplete += OnPlanComplete;

			_installer.ApplyBegin += OnApplyBegin;
			_installer.ApplyComplete += OnApplyComplete;

			_installer.CacheAcquireComplete += OnCacheAcquireComplete;
			_installer.ExecuteComplete += OnExecuteComplete;
			_installer.Error += OnError;
		}

		void OnCacheAcquireComplete(object sender, CacheAcquireCompleteEventArgs e)
		{
			if (e.Status != 0)
			{
				_completedStepVmFactory.GetViewModelInstance().Message = "Failed to acquire one of the prerequisites, you must be connected to internet.";
				_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.Failed;
			}
		}

		private bool IsApplying { get; set; }

		private Operation Operation { get; set; }

		private const string InstallFolderArgument = "/installfolder ";

		private void ParseCommandLine()
		{
			var args = _installer.Command.GetCommandLineArgs();
			var installFolderArgument = args.FirstOrDefault(arg => arg.StartsWith(InstallFolderArgument, StringComparison.InvariantCultureIgnoreCase));
			if (!string.IsNullOrWhiteSpace(installFolderArgument))
			{
				var installFolder = installFolderArgument.Remove(0, InstallFolderArgument.Length);
				_installationStepVmFactory.GetViewModelInstance().InstallFolder = installFolder;
			}
		}

		private void OnResolveSource(object sender, ResolveSourceEventArgs e)
		{
			e.Result = !string.IsNullOrEmpty(e.DownloadSource) ? Result.Download : Result.Ok;
		}

		private void OnDetectBegin(object sender, DetectBeginEventArgs e)
		{
			ParseCommandLine();
			Operation = !e.Installed ? Operation.Installation : Operation.Modification;
		}

		private void OnDetectRelatedBundle(object sender, DetectRelatedBundleEventArgs e)
		{
			if (e.Operation == RelatedOperation.Downgrade)
			{
				Operation = Operation.Information;
				_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.Downgrade;
			}
		}

		private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
		{
		}

		private void OnDetectComplete(object sender, DetectCompleteEventArgs e)
		{
			if (HResult.Succeeded(e.Status))
			{
				switch (_installer.Command.Action)
				{
					case LaunchAction.Layout:
						Operation = Operation.Layout;
						break;
					case LaunchAction.Help:
						Operation = Operation.Help;
						OnUIThread(() =>
						{
							var helpNavigationItem = _navigationManager.GetNavigationItemByName(NavigationNames.Help);
							_navigationManager.Navigate(helpNavigationItem);
						});
						break;
					default:
						if (_installer.Command.Display != Display.Full)
						{
							_engine.Log(LogLevel.Verbose, Resources.InvokingAutomaticPlanForNonInteractiveMode);
							_engine.PlanAsync(_installer.Command.Action);
						}
						break;
				}
			}
			else
			{
				Operation = Operation.Information;
				_completedStepVmFactory.GetViewModelInstance().ExitCode = e.Status;
				_completedStepVmFactory.GetViewModelInstance().Result = Cancelled ? OperationResult.Cancelled : OperationResult.Failed;
			}

			RegisterWizardSteps();

			IsInitializing = false;
		}

		private void RegisterWizardSteps()
		{
#if DESIGN
            if (IsInDesignMode)
            {
                RegisterStep(new LayoutStepViewModel());
                RegisterStep(new InstallationStepViewModel());
                RegisterStep(new ModificationStepViewModel());
                RegisterStep(new OperationProgressStepViewModel());
                RegisterStep(new OperationCompletedStepViewModel());
                return;
            }
#endif
			if (_installer.Command.Display == Display.Full)
			{
				switch (Operation)
				{
					case Operation.Layout:
						RegisterStep(_layoutVmFactory.GetViewModelInstance());
						break;
					case Operation.Installation:
						RegisterStep(_installationStepVmFactory.GetViewModelInstance());
						break;
					case Operation.Modification:
						RegisterStep(_modificationStepVmFactory.GetViewModelInstance());
						break;
				}
			}
			if ((_installer.Command.Display == Display.Passive || _installer.Command.Display == Display.Full) && (Operation != Operation.Information || Operation != Operation.Help))
			{
				RegisterStep(_progressStepVmFactory.GetViewModelInstance());
			}
			if (_installer.Command.Display == Display.Full && Operation != Operation.Help)
			{
				RegisterStep(_completedStepVmFactory.GetViewModelInstance());
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "CurrentStep")
			{
				ViewTitle.SubTitle = CurrentStep.Description;
				if (!IsInitializing)
				{
					var previousStep = AllRegisteredSteps[CurrentStepNumber - 2];
					if (previousStep is ILayoutStepViewModel)
					{
						_engine.Plan(LaunchAction.Layout);
					}
					else if (previousStep is IInstallationStepViewModel)
					{
						_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.SuccessfulInstall;
						_engine.Plan(LaunchAction.Install);
					}
					else if (previousStep is IModificationStepViewModel)
					{
						var modificationStepViewModel = _modificationStepVmFactory.GetViewModelInstance();
						if (modificationStepViewModel.Repair)
						{
							_engine.Plan(LaunchAction.Repair);
						}
						else if (modificationStepViewModel.Uninstall)
						{
							_engine.Plan(LaunchAction.Uninstall);
						}
					}
					if (CurrentStep.IsLast)
					{
						CancelCommand.RaiseCanExecuteChanged();
					}
				}
			}
		}

		private void OnPlanPackageBegin(object sender, PlanPackageBeginEventArgs e)
		{
			if (_engine.StringVariables.Contains("MbaNetfxPackageId") && e.PackageId.Equals(_engine.StringVariables["MbaNetfxPackageId"], StringComparison.Ordinal))
			{
				e.State = RequestState.None;
			}
		}

		private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
		{
			if (HResult.Succeeded(e.Status))
			{
				_engine.Apply(OnUIThread(() => Application.Current.MainWindow != null ? new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle() : IntPtr.Zero));
				
			}
			else
			{
				_completedStepVmFactory.GetViewModelInstance().ExitCode = e.Status;
				_completedStepVmFactory.GetViewModelInstance().Result = Cancelled ? OperationResult.Cancelled : OperationResult.Failed;
			}
		}

		private void OnApplyBegin(object sender, ApplyBeginEventArgs e)
		{
			IsApplying = true;
		}

		private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
		{
			IsApplying = false;
		}

		void OnExecuteComplete(object sender, ExecuteCompleteEventArgs e)
		{
			if (e.Status != 0)
			{
				_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.Failed;
			}
		}

		private void OnError(object sender, ErrorEventArgs e)
		{
			lock (_lock)
			{
				if (!Cancelled)
				{
					if (e.ErrorCode == (int)SystemErrorCodes.ERROR_CANCELLED)
					{
						_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.Cancelled;
					}
					else
					{
						_progressStepVmFactory.GetViewModelInstance().Message = e.ErrorMessage;
						_completedStepVmFactory.GetViewModelInstance().Message = e.ErrorMessage;
						_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.Failed;

						if (_installer.Command.Display == Display.Full)
						{
							var code = e.UIHint & 0xF;
							switch (code)
							{
								case 0:
									{
										var notification = new Notification { Title = Resources.SDKTitle, Content = e.ErrorMessage };
										OKDialogRequest.Raise(notification);
										e.Result = Result.Ok;
									}
									break;
								case 1:
								case 4:
									{
										var confirmation = new Confirmation { Title = Resources.SDKTitle, Content = e.ErrorMessage };
										switch (e.UIHint & 0xF)
										{
											case 1:
												OKCancelDialogRequest.Raise(confirmation);
												e.Result = confirmation.Confirmed ? Result.Ok : Result.Cancel;
												break;
											case 4:
												YesNoDialogRequest.Raise(confirmation);
												e.Result = confirmation.Confirmed ? Result.Ok : Result.Cancel;
												break;
										}
									}
									break;
								case 2:
								case 3:
									{
										var refusedConfirmation = new RefusedConfirmation();
										switch (e.UIHint & 0xF)
										{
											case 2:
												AbortRetryIgnoreDialogRequest.Raise(refusedConfirmation);
												e.Result = refusedConfirmation.Refused ? Result.Abort : (refusedConfirmation.Confirmed ? Result.Retry : Result.Ignore);
												if (!refusedConfirmation.Refused)
												{
													_completedStepVmFactory.GetViewModelInstance().Result = OperationResult.Successful;
												}
												break;
											case 3:
												YesNoCancelDialogRequest.Raise(refusedConfirmation);
												e.Result = refusedConfirmation.Confirmed ? Result.Yes : (refusedConfirmation.Refused ? Result.No : Result.Cancel);
												break;
										}
									}
									break;
							}
						}
					}
				}
				else
				{
					e.Result = Result.Cancel;
				}
			}
		}

		#endregion

		#region Error handling

		public InteractionRequest<Notification> OKDialogRequest { get; private set; }

		public InteractionRequest<Confirmation> OKCancelDialogRequest { get; private set; }

		public InteractionRequest<Confirmation> YesNoDialogRequest { get; private set; }

		public InteractionRequest<Confirmation> YesNoCancelDialogRequest { get; private set; }

		public InteractionRequest<Confirmation> AbortRetryIgnoreDialogRequest { get; private set; }

		#endregion
	}
}