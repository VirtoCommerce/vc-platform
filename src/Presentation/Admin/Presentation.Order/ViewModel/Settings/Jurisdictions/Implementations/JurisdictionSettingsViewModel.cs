using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Jurisdictions.Implementations
{
	public class JurisdictionSettingsViewModel : HomeSettingsEditableViewModel<Jurisdiction>, IJurisdictionSettingsViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;

		#endregion

		#region Commands
		public DelegateCommand<Jurisdiction> ItemDuplicateCommand { get; private set; }
		#endregion

		private readonly JurisdictionTypes _jurisdictionType;

		public List<Country> AllCountries { get; private set; }

		public JurisdictionSettingsViewModel(
			IRepositoryFactory<IOrderRepository> repositoryFactory,
			IOrderEntityFactory entityFactory,
			IViewModelsFactory<ICreateJurisdictionViewModel> wizardVmFactory,
			IViewModelsFactory<IJurisdictionViewModel> editVmFactory,
			JurisdictionTypes jurisdictionType,
			ICountryRepository countryRepository)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_jurisdictionType = jurisdictionType;
			_repositoryFactory = repositoryFactory;
			AllCountries = countryRepository.Countries.ToList();

			ItemDuplicateCommand = new DelegateCommand<Jurisdiction>(RaiseItemDuplicateInteractionRequest);
		}

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items =
						repository.Jurisdictions.Where(
							x => x.JurisdictionType == (int)JurisdictionTypes.All || x.JurisdictionType == (int)_jurisdictionType)
								  .OrderBy(j => j.DisplayName).ToList();
					return items;
				}
			}
			return null;

		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as Jurisdiction;
			if (itemToUpdate != null)
			{
				var itemFromInnerItem =
					Items.SingleOrDefault(j => j.JurisdictionId == itemToUpdate.JurisdictionId);

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
			var item = EntityFactory.CreateEntity<Jurisdiction>();

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("jurisdictionType", _jurisdictionType)
				);

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Jurisdiction".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		private void RaiseItemDuplicateInteractionRequest(Jurisdiction item)
		{
			var clone = item.DeepClone(EntityFactory as IKnownSerializationTypes);
			clone.JurisdictionId = clone.GenerateNewKey();
			clone.DisplayName = clone.DisplayName + "_1";
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				repository.Add(clone);
				repository.UnitOfWork.Commit();
			}
			RefreshItemListCommand.Execute();
		}

		protected override void RaiseItemEditInteractionRequest(Jurisdiction item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)
				, new KeyValuePair<string, object>("jurisdictionType", _jurisdictionType),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(Jurisdiction item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Jurisdiction '{0}'?".Localize(), item.DisplayName),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion
	}
}
