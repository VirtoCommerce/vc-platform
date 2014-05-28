using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.AppConfig.Model;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.DisplayTemplates.Implementations
{
	public class DisplayTemplateEditViewModel : ViewModelDetailAndWizardBase<DisplayTemplateMapping>, IDisplayTemplateViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IViewModelsFactory<ISearchItemViewModel> _searchItemVmFactory;
		private readonly IViewModelsFactory<ISearchCategoryViewModel> _searchCategoryVmFactory;
		private readonly HomeSettingsEditableViewModel<DisplayTemplateMapping> _parent;
		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;

		#endregion

		#region ctor

		public DisplayTemplateEditViewModel(
			IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
			IRepositoryFactory<IAppConfigRepository> repositoryFactory,
			IAppConfigEntityFactory entityFactory,
			IViewModelsFactory<ISearchCategoryViewModel> categoryVmFactory,
			IViewModelsFactory<ISearchItemViewModel> itemVmFactory,
			INavigationManager navManager,
			DisplayTemplateMapping item,
			HomeSettingsEditableViewModel<DisplayTemplateMapping> parent)
			: base(entityFactory, item, false)
		{
			_storeRepositoryFactory = storeRepositoryFactory;
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;
			_searchCategoryVmFactory = categoryVmFactory;
			_searchItemVmFactory = itemVmFactory;
            ViewTitle = new ViewTitleBase() { Title = "Display template", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };
			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		protected DisplayTemplateEditViewModel(
			IRepositoryFactory<IStoreRepository> storeRepositoryFactory,
			IRepositoryFactory<IAppConfigRepository> repositoryFactory,
			IAppConfigEntityFactory entityFactory,
			IViewModelsFactory<ISearchCategoryViewModel> categoryVmFactory,
			IViewModelsFactory<ISearchItemViewModel> itemVmFactory,
			DisplayTemplateMapping item)
			: base(entityFactory, item, true)
		{
			_storeRepositoryFactory = storeRepositoryFactory;
			_repositoryFactory = repositoryFactory;
			_searchCategoryVmFactory = categoryVmFactory;
			_searchItemVmFactory = itemVmFactory;
		}

		#endregion

		#region ViewModelBase Members

		public override string IconSource
		{
			get
			{
				return "Icon_DisplayTemplate";
			}
		}
		public override string DisplayName
		{
			get
			{
				return InnerItem != null ? InnerItem.Name : string.Empty;
			}
		}
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
					   (_navigationData = new NavigationItem(OriginalItem.DisplayTemplateMappingId,
						Configuration.NavigationNames.HomeName,
						Configuration.NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Display template ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Display template '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as IAppConfigRepository).DisplayTemplateMappings.
				Where(x => x.DisplayTemplateMappingId == OriginalItem.DisplayTemplateMappingId).SingleOrDefault();

			OnUIThread(() => { InnerItem = item; });
		}

		protected override void InitializePropertiesForViewing()
		{
			if (this is IDisplayTemplateConditionsStepViewModel || !IsWizardMode)
			{
				OnUIThread(() =>
				{
					if (InnerItem.PredicateVisualTreeSerialized == null)
					{
						ExpressionElementBlock = new DisplayTemplateExpressionBlock(this);
					}
					else
					{
						ExpressionElementBlock =
							SerializationUtil.Deserialize<TypedExpressionElementBase>(
								InnerItem.PredicateVisualTreeSerialized);
						ExpressionElementBlock.InitializeAfterDeserialized(this);
					}
					((DisplayTemplateExpressionBlock)ExpressionElementBlock).InitializeAvailableExpressions(
						(TargetTypes)InnerItem.TargetType);
					OnPropertyChanged("ExpressionElementBlock");

				});
			}
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				// update InnerItem from Wizard has to do manually from CreateXXXXXViewModel.cs
				UpdateFromExpressionElementBlock();
			}
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItemListCommand.Execute();
			}
		}

		protected override void SetSubscriptionUI()
		{
			AttachExpressionHandlers(ExpressionElementBlock);
		}

		protected override void CloseSubscriptionUI()
		{
			DetachExpressionHandlers(ExpressionElementBlock);
		}

		protected override void OnViewModelPropertyChangedUI(object sender, PropertyChangedEventArgs e)
		{
			base.OnViewModelPropertyChangedUI(sender, e);
			if (e != null && e.PropertyName == "TargetType" && (this is IDisplayTemplateConditionsStepViewModel || !IsWizardMode))
			{
				((DisplayTemplateExpressionBlock)ExpressionElementBlock).ResetChildren();
				((DisplayTemplateExpressionBlock)ExpressionElementBlock).InitializeAvailableExpressions(
					(TargetTypes)InnerItem.TargetType);
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsLast
		{
			get
			{
				return this is IDisplayTemplateConditionsStepViewModel;
			}
		}

		#endregion

		#region IDisplayTemplateViewModel

		public TypedExpressionElementBase ExpressionElementBlock { get; set; }

		public bool IsTargetTypeChangeable
		{
			get
			{
				return IsWizardMode;
			}
		}

		public IViewModelsFactory<ISearchCategoryViewModel> SearchCategoryVmFactory
		{
			get
			{
				return _searchCategoryVmFactory;
			}
		}

		public IViewModelsFactory<ISearchItemViewModel> SearchItemVmFactory
		{
			get
			{
				return _searchItemVmFactory;
			}
		}

		public IRepositoryFactory<IStoreRepository> StoreRepositoryFactory { get { return _storeRepositoryFactory; } }

		public void UpdateFromExpressionElementBlock()
		{
			if (((ConditionAndOrBlock)ExpressionElementBlock.Children[0]).Children.Any())
			{
				InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
				InnerItem.PredicateSerialized =
					SerializationUtil.SerializeExpression(
						((DisplayTemplateExpressionBlock)ExpressionElementBlock).GetExpression());
			}
			else
			{
				InnerItem.PredicateVisualTreeSerialized = null;
				InnerItem.PredicateSerialized = null;
			}
		}

		#endregion

		#region private methods

		private void AttachExpressionHandlers(ExpressionElement expressionBlock)
		{
			if (expressionBlock != null)
			{
				expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
				if (expressionBlock is CompositeElement)
				{
					(expressionBlock as CompositeElement).Children.CollectionChanged += ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).HeaderElements.CollectionChanged += ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).Children.ToList().ForEach(y => AttachExpressionHandlers((ExpressionElement)y));
					(expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(y => AttachExpressionHandlers((ExpressionElement)y));
				}
			}
		}

		private void DetachExpressionHandlers(ExpressionElement expressionBlock)
		{
			if (expressionBlock != null)
			{
				expressionBlock.PropertyChanged -= ViewModel_PropertyChanged;
				if (expressionBlock is CompositeElement)
				{
					(expressionBlock as CompositeElement).Children.CollectionChanged -= ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).HeaderElements.CollectionChanged -= ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).Children.ToList().ForEach(y => DetachExpressionHandlers((ExpressionElement)y));
					(expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(y => DetachExpressionHandlers((ExpressionElement)y));
				}
			}
		}

		#endregion

	}
}
