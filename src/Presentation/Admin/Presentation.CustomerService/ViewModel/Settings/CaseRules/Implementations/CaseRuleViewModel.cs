using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseRules.Implementations
{
	public class CaseRuleViewModel : ViewModelDetailAndWizardBase<CaseRule>, ICaseRuleViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
		private readonly IViewModelsFactory<IMultiLineEditViewModel> _multiLineEditVmFactory;

		#endregion

		#region Constructor

		public CaseRuleViewModel(IViewModelsFactory<IMultiLineEditViewModel> multiLineEditVmFactory, IRepositoryFactory<ICustomerRepository> repositoryFactory, ICustomerEntityFactory entityFactory, IHomeSettingsViewModel parent,
			INavigationManager navManager, CaseRule item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase() { Title = "Edit Rule", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };
			_repositoryFactory = repositoryFactory;
			_parent = parent;
			_navManager = navManager;
			_multiLineEditVmFactory = multiLineEditVmFactory;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}


		protected CaseRuleViewModel(IViewModelsFactory<IMultiLineEditViewModel> multiLineEditVmFactory, IRepositoryFactory<ICustomerRepository> repositoryFactory, ICustomerEntityFactory entityFactory, CaseRule item)
			: base(entityFactory, item, true)
		{
			_multiLineEditVmFactory = multiLineEditVmFactory;
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region ViewModelBase members

		public override string DisplayName { get { return InnerItem == null ? string.Empty : InnerItem.Name; } }

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
				   (SolidColorBrush)Application.Current.TryFindResource("SettingsDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CaseRuleId),
															Configuration.NavigationNames.HomeName, NavigationNames.MenuName,
															this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity { get { return string.Format("Case rule ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Case rule '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as ICustomerRepository).CaseRules.ExpandAll()
					.Where(x => x.CaseRuleId == OriginalItem.CaseRuleId)
					.SingleOrDefault();
			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{

			if (ExpressionElementBlock != null)
			{
				ExpressionElementBlock.Children.ToList().ForEach(x =>
				{
					x.PropertyChanged -= OnViewModelPropertyChangedUI;
					if (x is CompositeElement)
					{
						(x as CompositeElement).Children.CollectionChanged -= OnViewModelCollectionChangedUI;
					}
				});
			}

			if (this is ICaseRuleOverviewStepViewModel || !IsWizardMode)
			{
				if (InnerItem.PredicateVisualTreeSerialized == null)
				{
					ExpressionElementBlock = new CaseRuleExpressionBlock(this);
				}
				else
				{
					ExpressionElementBlock = SerializationUtil.Deserialize<TypedExpressionElementBase>(InnerItem.PredicateVisualTreeSerialized);
					ExpressionElementBlock.InitializeAfterDeserialized(this);
				}
				SetHandlers(ExpressionElementBlock);
				OnPropertyChanged("ExpressionElementBlock");
			}
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				PrepareOriginalItemForSave();
			}
		}

		protected override void AfterSaveChangesUI()
		{

			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem != null && ExpressionElementBlock != null)
			{
				SetHandlers(ExpressionElementBlock);
			}

		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem != null && ExpressionElementBlock != null)
			{
				UnsetHandlers(ExpressionElementBlock);
			}
		}

		#endregion

		#region IWizardStep overrides


		public override string Description { get { return ""; } }
		public override bool IsValid { get { return true; } }
		public override bool IsLast { get { return true; } }


		#endregion

		#region ICaseRuleViewModel members

		public TypedExpressionElementBase ExpressionElementBlock { get; set; }

		public IViewModelsFactory<IMultiLineEditViewModel> MultiLineEditVmFactory
		{
			get { return _multiLineEditVmFactory; }
		}

		public void PrepareOriginalItemForSave()
		{

			foreach (var alert in InnerItem.Alerts)
			{
				Repository.Attach(alert);
				Repository.Remove(alert);
			}

			InnerItem.Alerts.Clear();
			foreach (var alert in ((IExpressionCaseAlertsAdaptor)ExpressionElementBlock).GetCaseAlerts())
			{
				InnerItem.Alerts.Add(alert);
			}

			if (((ConditionAndOrBlock)ExpressionElementBlock.Children[0]).Children.Count() > 0)
			{
				InnerItem.PredicateSerialized = SerializationUtil.SerializeExpression(((CaseRuleExpressionBlock)ExpressionElementBlock).GetExpression());
			}
			else
			{
				InnerItem.PredicateSerialized = null;
			}

			if (((IExpressionCaseAlertsAdaptor)ExpressionElementBlock).GetCaseAlerts().Count() > 0)
			{
				InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
			}
			else
			{
				InnerItem.PredicateVisualTreeSerialized = null;
			}

			if (!IsWizardMode)
			{
				Repository.UnitOfWork.Commit();
			}
		}

		#endregion

		#region private members

		private void SetHandlers(ExpressionElement expressionBlock)
		{
			if (expressionBlock != null)
			{
				expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
				if (expressionBlock is CompositeElement)
				{
					(expressionBlock as CompositeElement).Children.CollectionChanged += ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).HeaderElements.CollectionChanged += ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).Children.ToList().ForEach(y => SetHandlers((ExpressionElement)y));
					(expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(y => SetHandlers((ExpressionElement)y));
				}
			}
		}

		private void UnsetHandlers(ExpressionElement expressionBlock)
		{
			if (expressionBlock != null)
			{
				expressionBlock.PropertyChanged -= ViewModel_PropertyChanged;
				if (expressionBlock is CompositeElement)
				{
					(expressionBlock as CompositeElement).Children.CollectionChanged -= ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).HeaderElements.CollectionChanged -= ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).Children.ToList().ForEach(y => SetHandlers((ExpressionElement)y));
					(expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(y => SetHandlers((ExpressionElement)y));
				}
			}
		}


		#endregion


	}
}
