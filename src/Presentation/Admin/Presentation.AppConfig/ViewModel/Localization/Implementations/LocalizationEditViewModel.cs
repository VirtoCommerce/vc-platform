using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.Localization.Implementations
{
	public class LocalizationEditViewModel : ViewModelDetailBase<Foundation.AppConfig.Model.Localization>,
											 ILocalizationEditViewModel
	{
		#region Dependencies

		private readonly IHomeSettingsViewModel _parent;
		private readonly NavigationManager _navManager;
		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

		#endregion

		#region Private Fields

		private bool _isAdded;

		#endregion

		#region ctor

		public LocalizationEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory,
										 IAppConfigEntityFactory entityFactory, NavigationManager navManager,
										 IHomeSettingsViewModel parent, LocalizationGroup item)
			: base(entityFactory, item.TranslateLocalization)
		{
			_repositoryFactory = repositoryFactory;
			_parent = parent;
			_navManager = navManager;
			OriginalLocalizationGroup = item;

            ViewTitle = new ViewTitleBase() { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Localization" };

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		protected LocalizationEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, Foundation.AppConfig.Model.Localization item)
			: base(entityFactory, item)
		{
			_repositoryFactory = repositoryFactory;
		}


		#endregion

		#region Public Properties

		public LocalizationGroup OriginalLocalizationGroup { get; set; }

		public string OriginalLangName
		{
			get
			{
				var landName = CultureInfo.GetCultureInfo(OriginalLocalizationGroup.OriginalLocalization.LanguageCode).DisplayName;
				return string.Format("{0} ({1}):", landName, OriginalLocalizationGroup.OriginalLocalization.LanguageCode);
			}
		}

		public string TranslateLangName
		{
			get
			{
				return string.Format("{0} ({1}):",
					CultureInfo.GetCultureInfo(OriginalLocalizationGroup.TranslateLocalization.LanguageCode).DisplayName,
					OriginalLocalizationGroup.TranslateLocalization.LanguageCode);
			}
		}

		#endregion

		#region ViewModelBase Members

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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalLocalizationGroup.LocalizationGroupId),
															 Configuration.NavigationNames.HomeName,
															 NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Localization ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			bool result = InnerItem.Validate() && !string.IsNullOrEmpty(InnerItem.Value);
			return result;
		}

		protected override void BeforeSaveChanges()
		{
			if (_isAdded)
			{
				(Repository as IAppConfigRepository).Add(InnerItem);
			}
		}

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Localization '{0}'?".Localize(), InnerItem.Name),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			if (!_isAdded)
			{
				// 'x => true && ... ' is a workaround to prevent searching by key (getting exception if Localization not found).
				var item = (Repository as IAppConfigRepository).Localizations
					.Where(x => true && x.Name == OriginalItem.Name && x.LanguageCode == OriginalItem.LanguageCode && x.Category == OriginalItem.Category)
					.SingleOrDefault();
				if (item == null)
				{
					item = new Foundation.AppConfig.Model.Localization();
					item.InjectFrom<CloneInjection>(OriginalItem);
					_isAdded = true;
				}
				OnUIThread(() => { InnerItem = item; });
			}
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.Value = InnerItem.Value;
				_parent.RefreshItemListCommand.Execute();
			}
		}

		#endregion

	}
}
