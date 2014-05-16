using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.TaxCategories.Implementations
{
	public class TaxCategorySettingsViewModel : HomeSettingsEditableViewModel<TaxCategory>, ITaxCategorySettingsViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public TaxCategorySettingsViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, ICatalogEntityFactory entityFactory, IViewModelsFactory<ICreateTaxCategoryViewModel> wizardVmFactory, IViewModelsFactory<ITaxCategoryViewModel> editVmFactory)
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
					var items = repository.TaxCategories.OrderBy(tc => tc.Name).ToList();
					return items;
				}
			}

			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as TaxCategory;
			if (itemToUpdate != null)
			{
				TaxCategory itemFromInnerItem =
					Items.SingleOrDefault(tc => tc.TaxCategoryId == itemToUpdate.TaxCategoryId);

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
			var item = EntityFactory.CreateEntity<TaxCategory>();

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Tax Category".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(TaxCategory item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(TaxCategory item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Tax Category '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion

	}
}
