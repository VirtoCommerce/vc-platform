using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions;
using VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations
{
	public class PriceListAssignmentViewModel : ViewModelDetailAndWizardBase<PricelistAssignment>, IPriceListAssignmentViewModel
	{
		#region Dependencies

		private readonly IAuthenticationContext _authContext;
		private readonly NavigationManager _navManager;
		private readonly IRepositoryFactory<IPricelistRepository> _repositoryFactory;
		private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
		private readonly IRepositoryFactory<ICountryRepository> _countryRepositoryFactory;

		#endregion

		#region Constructor

		public PriceListAssignmentViewModel(
			IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IRepositoryFactory<IPricelistRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			ICatalogEntityFactory entityFactory, IAuthenticationContext authContext,
			NavigationManager navManager, PricelistAssignment item)
			: base(entityFactory, item, false)
		{
			_countryRepositoryFactory = countryRepositoryFactory;
			_repositoryFactory = repositoryFactory;
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_navManager = navManager;
			_authContext = authContext;
			ViewTitle = new ViewTitleBase()
				{
                    Title = "Price List Assignment",
					SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
				};
			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		protected PriceListAssignmentViewModel(
			IRepositoryFactory<ICountryRepository> countryRepositoryFactory,
			IRepositoryFactory<IPricelistRepository> repositoryFactory,
			IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory,
			ICatalogEntityFactory entityFactory, IAuthenticationContext authContext, PricelistAssignment item)
			: base(entityFactory, item, true)
		{
			_authContext = authContext;
			_repositoryFactory = repositoryFactory;
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_countryRepositoryFactory = countryRepositoryFactory;
		}

		#endregion

		#region ViewModelBase members

		public override string IconSource
		{
			get
			{
				return "Icon_Assignment";
			}
		}

		public override string DisplayName
		{
			get
			{
				return OriginalItem == null ? this.ToString() : OriginalItem.Name;
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
					(SolidColorBrush)Application.Current.TryFindResource("PriceListDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.PricelistAssignmentId),
														NavigationNames.HomeNamePriceList,
														NavigationNames.MenuNamePriceList, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Price List Assignment ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool HasPermission()
		{
			return _authContext.CheckPermission(PredefinedPermissions.PricingPrice_List_AssignmentsManage);
		}

		protected override bool IsValidForSave()
		{
			InnerItem.Validate();
			return !InnerItem.Errors.Any();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to price list '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as IPricelistRepository).PricelistAssignments.Where(
					x => x.PricelistAssignmentId == OriginalItem.PricelistAssignmentId).SingleOrDefault();
			OnUIThread(() => { InnerItem = item; });
		}

		protected override void InitializePropertiesForViewing()
		{
			// use custom initialize in each wizard step
			if (!IsWizardMode)
			{
				InitializeExpressionElementBlock();
				InitializeAvailablePriceLists();
			}
		}

		protected override void BeforeSaveChanges()
		{
			// update InnerItem from Wizard has to do manually from CreateXXXXXViewModel.cs
			if (!IsWizardMode)
			{
				UpdateFromExpressionElementBlock();
			}
		}

		protected override void AfterSaveChangesUI()
		{
			//OriginalItem.PredicateVisualTreeSerialized = null;
			//OriginalItem.ConditionExpression = null;
			OriginalItem.InjectFrom(InnerItem);
		}

		protected override void SetSubscriptionUI()
		{
			AttachExpressionHandlers(ExpressionElementBlock);
		}

		protected override void CloseSubscriptionUI()
		{
			DetachExpressionHandlers(ExpressionElementBlock);
		}

		#endregion

		#region IExpressionViewModel

		public TypedExpressionElementBase ExpressionElementBlock { get; set; }

		#endregion

		#region IPriceListAssignmentViewModel

		public IRepositoryFactory<ICountryRepository> CountryRepositoryFactory { get { return _countryRepositoryFactory; } }
		public Pricelist[] AvailablePriceLists { get; private set; }
		public CatalogBase[] AvailableCatalogs { get; private set; }

		#endregion

		#region Private members

		private void AttachExpressionHandlers(ExpressionElement expressionBlock)
		{
			if (expressionBlock != null)
			{
				expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
				if (expressionBlock is CompositeElement)
				{
					(expressionBlock as CompositeElement).Children.CollectionChanged += ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).HeaderElements.CollectionChanged += ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).Children.ToList().ForEach(AttachExpressionHandlers);
					(expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(AttachExpressionHandlers);
				}
			}
		}

		private void DetachExpressionHandlers(ExpressionElement expressionBlock)
		{
			if (expressionBlock != null)
			{
				expressionBlock.PropertyChanged += ViewModel_PropertyChanged;
				if (expressionBlock is CompositeElement)
				{
					(expressionBlock as CompositeElement).Children.CollectionChanged -= ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).HeaderElements.CollectionChanged -= ViewModel_PropertyChanged;
					(expressionBlock as CompositeElement).Children.ToList().ForEach(DetachExpressionHandlers);
					(expressionBlock as CompositeElement).HeaderElements.ToList().ForEach(DetachExpressionHandlers);
				}
			}
		}

		#endregion

		#region Initialize and Update

		protected virtual void InitializeExpressionElementBlock()
		{
			OnUIThread(() =>
			{
				if (InnerItem.PredicateVisualTreeSerialized == null)
				{
					ExpressionElementBlock = new PriceListAssignmentExpression(this);
				}
				else
				{
					ExpressionElementBlock = SerializationUtil.Deserialize<TypedExpressionElementBase>(InnerItem.PredicateVisualTreeSerialized);
					ExpressionElementBlock.InitializeAfterDeserialized(this);
				}
				OnPropertyChanged("ExpressionElementBlock");
			});
		}

		/// <summary>
		/// Update InnerItem from ExpressionElementBlock
		/// Using from Wizard manually and from BeforeSaveChanges in Detail mode
		/// </summary>
		public void UpdateFromExpressionElementBlock()
		{
			if (((ConditionAndOrBlock)((PriceListAssignmentExpressionBlock)ExpressionElementBlock.Children[0]).Children[0]).Children.Any())
			{
				InnerItem.PredicateVisualTreeSerialized = SerializationUtil.Serialize(ExpressionElementBlock);
				InnerItem.ConditionExpression =
					SerializationUtil.SerializeExpression(
						((PriceListAssignmentExpression)ExpressionElementBlock).GetExpression());
			}
			else
			{
				InnerItem.PredicateVisualTreeSerialized = null;
				InnerItem.ConditionExpression = null;
			}
		}

		protected void InitializeAvailablePriceLists()
		{
			if (AvailablePriceLists == null)
			{
				try
				{
					using (var catalogRepository = _catalogRepositoryFactory.GetRepositoryInstance())
					using (var repository = _repositoryFactory.GetRepositoryInstance())
					{
						var availablePriceLists = repository.Pricelists.OrderBy(x => x.Name).ToArray();
						var availableCatalogs = catalogRepository.Catalogs.OrderBy(x => x.Name).ToArray();
						OnUIThread(() =>
						{
							AvailablePriceLists = availablePriceLists;
							AvailableCatalogs = availableCatalogs;
							OnPropertyChanged("AvailablePriceLists");
							OnPropertyChanged("AvailableCatalogs");
						});
					}
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to load {0}",
									ExceptionContextIdentity));
				}
			}
		}

		#endregion
	}
}






