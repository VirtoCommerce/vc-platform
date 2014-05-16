using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingPackages.Implementations
{
	public class ShippingPackageSettingsViewModel : HomeSettingsEditableViewModel<Packaging>, IShippingPackageSettingsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;

		#endregion

		#region ctor

		public ShippingPackageSettingsViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, ICatalogEntityFactory entityFactory, IViewModelsFactory<ICreatePackagingViewModel> wizardVmFactory, IViewModelsFactory<IPackagingViewModel> editVmFactory)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.Packagings.OrderBy(cr => cr.Name).ToList();
					return items;
				}
			}
			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as Packaging;
			if (itemToUpdate != null)
			{
				Packaging itemFromInnerItem =
					Items.SingleOrDefault(p => p.PackageId == itemToUpdate.PackageId);

				if (itemFromInnerItem != null)
				{
					OnUIThread(() =>
					{
						itemFromInnerItem.InjectFrom<CloneInjection>(itemToUpdate);
						OnPropertyChanged("Items");
					});
				}
			}
		}

		#endregion

		#region HomeSettingsEditableViewModel members

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<Packaging>();

			var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Packaging".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(Packaging item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(Packaging item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Packaging '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion

	}
}
