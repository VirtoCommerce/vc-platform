using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class LinkedCategoryViewModel : ViewModelDetailBase<LinkedCategory>, ILinkedCategoryViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
		private readonly INavigationManager _navManager;

		#endregion

		public LinkedCategoryViewModel(LinkedCategory item, IRepositoryFactory<ICatalogRepository> repositoryFactory, INavigationManager navManager)
			: base(null, item)
		{
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			ViewTitle = new ViewTitleBase
				{
                    Title = "Linked Category",
					SubTitle = GetDisplayName(item).ToUpper(CultureInfo.InvariantCulture)
				};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		public ICatalogRepository ItemRepository { get { return (ICatalogRepository)Repository; } }

		#region ViewModelBase members

		public override string IconSource
		{
			get
			{
				return "Icon_LinkedCategory";
			}
		}

		public override string DisplayName
		{
			get
			{
				return GetDisplayName(InnerItem);
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result = (SolidColorBrush)Application.Current.TryFindResource("CatalogDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CategoryId),
												NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailBase members

		public override string ExceptionContextIdentity { get { return string.Format("Linked Category ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override void LoadInnerItem()
		{
			var item = ItemRepository.Categories
				.Where(x => x.CategoryId == OriginalItem.CategoryId).OfType<LinkedCategory>()
				.Expand("CategoryLink").SingleOrDefault();

			OnUIThread(() => { InnerItem = item; });
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Category '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		#endregion

		private string GetDisplayName(LinkedCategory item)
		{
			var result = String.Empty;

			if (item != null && item.CategoryLink != null)
			{
				var link = item.CategoryLink;
				result = link is Category
							 ? String.Format("{0} ({1})", ((Category)link).Name, OriginalItem.LinkedCatalogId)
							 : String.Format("{0} ({1})", link.CategoryId, OriginalItem.LinkedCatalogId);
			}
			return result;
		}

	}
}
