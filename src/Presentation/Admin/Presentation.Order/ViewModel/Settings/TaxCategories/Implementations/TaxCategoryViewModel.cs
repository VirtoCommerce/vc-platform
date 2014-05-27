using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Implementations
{
	public class TaxCategoryViewModel : ViewModelDetailAndWizardBase<TaxCategory>, ITaxCategoryViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public TaxCategoryViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IOrderEntityFactory entityFactory, IHomeSettingsViewModel parent,
			INavigationManager navManager, TaxCategory item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Tax Category" };
			_repositoryFactory = repositoryFactory;
			_parent = parent;
			_navManager = navManager;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		protected TaxCategoryViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IOrderEntityFactory entityFactory, TaxCategory item)
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
				return (InnerItem == null) ? string.Empty : InnerItem.Name;
			}
		}

		public override string IconSource
		{
			get
			{
				return "Icon_TaxCategories";
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.TaxCategoryId),
															Configuration.NavigationNames.HomeName, NavigationNames.MenuName,
															this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase members

		public override string ExceptionContextIdentity { get { return string.Format("Tax category ({0})", DisplayName); } }

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
				Content = string.Format("Save changes to Tax category '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as ICatalogRepository).TaxCategories.Where(tc => tc.TaxCategoryId == OriginalItem.TaxCategoryId)
					.SingleOrDefault();

			OnUIThread(() => InnerItem = item);
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

		public override bool IsValid
		{
			get
			{
				bool result = false;

				result = InnerItem.Validate(false) && !string.IsNullOrEmpty(InnerItem.Name);

				return result;
			}
		}

		public override bool IsLast
		{
			get { return true; }
		}

		public override string Description
		{
			get { return "Enter Tax Category information.".Localize(); }
		}

		#endregion



	}
}
