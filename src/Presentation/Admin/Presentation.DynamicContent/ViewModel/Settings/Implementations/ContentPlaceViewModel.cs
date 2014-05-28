using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Implementations
{
	public class ContentPlaceViewModel : ViewModelDetailAndWizardBase<DynamicContentPlace>, IContentPlaceViewModel
	{


		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<IDynamicContentRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public ContentPlaceViewModel(IRepositoryFactory<IDynamicContentRepository> repositoryFactory, IDynamicContentEntityFactory entityFactory, IHomeSettingsViewModel parent,
			INavigationManager navManager, DynamicContentPlace item)
			: base(entityFactory, item, false)
		{
            ViewTitle = new ViewTitleBase { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Content place" };

			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}


		protected ContentPlaceViewModel(IRepositoryFactory<IDynamicContentRepository> repositoryFactory, IDynamicContentEntityFactory entityFactory, DynamicContentPlace item)
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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.DynamicContentPlaceId), Configuration.NavigationNames.HomeName,
						NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDEtailAndWizardBase members

		public override string ExceptionContextIdentity { get { return string.Format("Content place ({0})", DisplayName); } }

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
                Content = string.Format("Save changes to Content place '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}


		protected override void LoadInnerItem()
		{

			var item =
				(Repository as IDynamicContentRepository).Places.Where(
					dcp => dcp.DynamicContentPlaceId == OriginalItem.DynamicContentPlaceId).SingleOrDefault();

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
				return true;
			}
		}

		public override bool IsLast
		{
			get
			{
				return this is IContentPlaceOverviewStepViewModel;
			}
		}

		public override string Description
		{
			get
			{
                return string.Format("Enter content place details".Localize());
			}
		}

		#endregion

	}
}
