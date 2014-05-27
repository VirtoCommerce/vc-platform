using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Implementations
{
	public class PackagingViewModel : ViewModelDetailAndWizardBase<Packaging>, IPackagingViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public PackagingViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, ICatalogEntityFactory entityFactory, IHomeSettingsViewModel parent,
			INavigationManager navManager, Packaging item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase() { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Shipping package" };
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

		}

		public PackagingViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, ICatalogEntityFactory entityFactory, Packaging item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region ViewModelBase members

		public override string DisplayName
		{
			get
			{
				return InnerItem.Name;
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.PackageId),
															Configuration.NavigationNames.HomeName,
															NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity
		{
			get { return string.Format("Packaging ({0})", DisplayName); }
		}

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
				Content = string.Format("Save changes to Packaging '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as ICatalogRepository).Packagings.Where(p => p.PackageId == OriginalItem.PackageId).SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void InitializePropertiesForViewing()
		{
			OnPropertyChanged("IsDimensionSetActive");
		}

		protected override void BeforeSaveChanges()
		{
			if (!IsWizardMode)
			{
				// update InnerItem from Wizard has to do manually from CreateXXXXXViewModel.cs
				UpdateDimensions();
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

		#endregion

		#region IWizardStep members

		public override string Description
		{
			get { return null; }
		}

		public override bool IsValid
		{
			get
			{
				bool result = true;

				result = InnerItem.Validate(false) && !string.IsNullOrEmpty(InnerItem.Name) &&
						 !string.IsNullOrEmpty(InnerItem.Description);

				return result;
			}
		}

		public override bool IsLast
		{
			get { return true; }
		}

		#endregion

		#region Properties

		private bool _isDimensionsInit = false;
		private bool _isDimensionSetActive = false;
		public bool IsDimensionSetActive
		{
			get
			{
				if (!_isDimensionsInit)
				{
					bool result = false;

					result = (InnerItem.Width == 0 && InnerItem.Height == 0 && InnerItem.Depth == 0);

					_isDimensionSetActive = !result;
					_isDimensionsInit = true;
					return !result;
				}
				else
				{
					return _isDimensionSetActive;
				}
			}
			set
			{
				_isDimensionSetActive = value;
				OnPropertyChanged();
				OnViewModelPropertyChangedUI(null, null);
			}
		}

		public void UpdateDimensions()
		{
			if (!IsDimensionSetActive)
			{
				InnerItem.Width = 0;
				InnerItem.Height = 0;
				InnerItem.Depth = 0;
			}
		}

		#endregion


	}


}
