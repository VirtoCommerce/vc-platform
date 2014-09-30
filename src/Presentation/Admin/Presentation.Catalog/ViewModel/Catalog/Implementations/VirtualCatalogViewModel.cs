using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class VirtualCatalogViewModel : ViewModelDetailAndWizardBase<VirtualCatalog>, IVirtualCatalogViewModel
	{
		private readonly ITreeVirtualCatalogViewModel _parentTreeVM;
		private const string _catalogImageSource = "Icon_VirtualCatalog";

		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly INavigationManager _navManager;
		/// <summary>
		/// public. For viewing
		/// </summary>
		public VirtualCatalogViewModel(ICatalogEntityFactory entityFactory,
			VirtualCatalog item, IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory,
			ITreeVirtualCatalogViewModel parentTreeVM, INavigationManager navManager)
			: this(entityFactory, item, false, repositoryFactory, appConfigRepositoryFactory)
		{
			_parentTreeVM = parentTreeVM;
			_navManager = navManager;
			ViewTitle = new ViewTitleBase
				{
                    Title = "Virtual Catalog",
                    SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name : ""
				};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		/// <summary>
		/// protected. For a step
		/// </summary>
		protected VirtualCatalogViewModel(VirtualCatalog item, IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory)
			: this(null, item, true, repositoryFactory, appConfigRepositoryFactory)
		{
		}

		private VirtualCatalogViewModel(ICatalogEntityFactory entityFactory, VirtualCatalog item, bool isWizardMode, IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory)
			: base(entityFactory, item, isWizardMode)
		{
			_repositoryFactory = repositoryFactory;
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
		}

		#region ICatalogViewModel Members

		public ObservableCollection<CatalogLanguageDisplay> AllAvailableLanguages
		{
			get;
			private set;
		}

		#endregion

		#region ViewModelBase members
		public override string IconSource
		{
			get
			{
				return _catalogImageSource;
			}
		}

		public override string DisplayName
		{
			get
			{
				return OriginalItem.Name;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CatalogId),
												NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		protected override void InitializePropertiesForViewing()
		{
			if (AllAvailableLanguages == null)
			{
				AllAvailableLanguages = CatalogViewModel.GetAvailableLanguages(InnerItem.CatalogId, _appConfigRepositoryFactory);
				OnPropertyChanged("AllAvailableLanguages");
			}
		}

		public override string ExceptionContextIdentity { get { return string.Format("Virtual Catalog ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as ICatalogRepository).Catalogs.Where(
				 x => x.CatalogId == OriginalItem.CatalogId).SingleOrDefault();
			OnUIThread(() => { InnerItem = (VirtualCatalog)item; });
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override void AfterSaveChangesUI()
		{
			// just basic properties inject is enough. Injecting collections can generate repository errors.
			OriginalItem.InjectFrom(InnerItem);

			_parentTreeVM.RefreshUI();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Virtual Catalog '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		#endregion

	}
}
