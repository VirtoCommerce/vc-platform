using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard
{
	public abstract class WizardViewModelBare : ViewModelBase, IWizard
	{
		private readonly List<IWizardStep> _registeredStepList = new List<IWizardStep>();
		private int _currentStepIndex;

		protected WizardViewModelBare()
		{
			MoveNextCommand = new DelegateCommand<object>(x => MoveToNextStep(), CanMoveToNextStep);
			MovePreviousCommand = new DelegateCommand<object>(x => MoveToPreviousStep(), CanMoveToPreviousStep);
		}

		public void OnLoaded()
		{
			foreach (var step in AllRegisteredSteps)
			{
				if (step is ISupportDelayInitialization)
				{
					var curStep = (step as ISupportDelayInitialization);
					Task.Run(() => curStep.InitializeForOpen());
				}
			}
		}

		protected void MoveToFirstStep()
		{
			if (_registeredStepList.Any())
			{
				while (_currentStepIndex > 0)
				{
					MoveToPreviousStep();
				}
			}
		}

		public int TotalStepsCount
		{
			get
			{
				return _registeredStepList.Count();
			}
		}

		public int CurrentStepNumber
		{
			get
			{
				if (!_registeredStepList.Any())
				{
					return 0;
				}
				return _currentStepIndex + 1;
			}
		}

		public IWizardStep CurrentStep
		{
			get
			{
				if (IsInitializing && !_registeredStepList.Any())
				{
					return null;
				}
				return _registeredStepList[_currentStepIndex];
			}
		}

		public ReadOnlyCollection<IWizardStep> AllRegisteredSteps
		{
			get
			{
				return _registeredStepList.AsReadOnly();
			}
		}

		public DelegateCommand<object> MoveNextCommand
		{
			get;
			protected set;
		}

		public DelegateCommand<object> MovePreviousCommand
		{
			get;
			protected set;
		}

		protected void RegisterStep(IWizardStep step)
		{
			step.StepIsValidChangedEvent += OnStepIsValidChangedEvent;
			_registeredStepList.Add(step);
			OnPropertyChanged("AllRegisteredSteps");
			OnPropertyChanged("TotalStepsCount");
			OnPropertyChanged("CurrentStep");
			OnPropertyChanged("CurrentStepNumber");
		}

		protected void UnregisterStep(IWizardStep step)
		{
			if (!_registeredStepList.Contains(step))
			{
				throw new ArgumentException("Step " + step.GetType() + " not registered.");
			}
			if (_registeredStepList.IndexOf(step) <= _currentStepIndex)
			{
				throw new ArgumentException("Can't unregister previous or current step");
			}
			step.StepIsValidChangedEvent -= OnStepIsValidChangedEvent;
			_registeredStepList.Remove(step);
			OnPropertyChanged("AllRegisteredSteps");
			OnPropertyChanged("TotalStepsCount");
			OnPropertyChanged("CurrentStep");
			OnPropertyChanged("CurrentStepNumber");
		}

		private void OnStepIsValidChangedEvent(object sender, EventArgs e)
		{
			MoveNextCommand.RaiseCanExecuteChanged();
			MovePreviousCommand.RaiseCanExecuteChanged();
		}

		private bool CanMoveToPreviousStep(object param)
		{
			return _currentStepIndex > 0 && !CurrentStep.IsBackTrackingDisabled;
		}

		private void MoveToPreviousStep()
		{
			if (CanMoveToPreviousStep(null))
			{
				if (_currentStepIndex > 0)
				{
					_currentStepIndex--;

					OnPropertyChanged("CurrentStep");
					OnPropertyChanged("CurrentStepNumber");
					MoveNextCommand.RaiseCanExecuteChanged();
					MovePreviousCommand.RaiseCanExecuteChanged();
				}
			}
		}

		private void MoveToNextStep()
		{
			if (CanMoveToNextStep(null))
			{
				if (_currentStepIndex < _registeredStepList.Count() - 1)
				{
					_currentStepIndex++;

					OnPropertyChanged("CurrentStep");
					OnPropertyChanged("CurrentStepNumber");
					MoveNextCommand.RaiseCanExecuteChanged();
					MovePreviousCommand.RaiseCanExecuteChanged();
				}
			}
		}

		private bool CanMoveToNextStep(object param)
		{
			return CurrentStep != null && CurrentStep.IsValid;
		}

		public static bool IsInDesignMode
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
