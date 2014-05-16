using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.JurisdictionGroups.Implementations
{
	public class JurisdictionGroupSettingsViewModel : HomeSettingsEditableViewModel<JurisdictionGroup>, IJurisdictionGroupSettingsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<IOrderRepository> _repositoryFactory;

		#endregion

		#region Commands
		public DelegateCommand<JurisdictionGroup> ItemDuplicateCommand { get; private set; }
		#endregion

		private readonly JurisdictionTypes _jurisdictionType;

		#region Constructor

		public JurisdictionGroupSettingsViewModel(IRepositoryFactory<IOrderRepository> repositoryFactory, IOrderEntityFactory entityFactory, IViewModelsFactory<ICreateJurisdictionGroupViewModel> wizardVmFactory, IViewModelsFactory<IJurisdictionGroupViewModel> editVmFactory, JurisdictionTypes jurisdictionType)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
			_jurisdictionType = jurisdictionType;

			ItemDuplicateCommand = new DelegateCommand<JurisdictionGroup>(RaiseItemDuplicateInteractionRequest);
		}

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items =
						repository.JurisdictionGroups.Where(
							x => x.JurisdictionType == (int)JurisdictionTypes.All || x.JurisdictionType == (int)_jurisdictionType)
								  .OrderBy(cr => cr.DisplayName)
								  .ToList();
					return items;
				}
			}
			return null;
		}


		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as JurisdictionGroup;
			if (itemToUpdate != null)
			{
				JurisdictionGroup itemFromInnerItem =
					Items.SingleOrDefault(jg => jg.JurisdictionGroupId == itemToUpdate.JurisdictionGroupId);

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
			var item = EntityFactory.CreateEntity<JurisdictionGroup>();

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("jurisdictionType", _jurisdictionType));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Jurisdiction Group".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		private void RaiseItemDuplicateInteractionRequest(JurisdictionGroup item)
		{
			var clone = item.DeepClone(EntityFactory as IKnownSerializationTypes);
			clone.JurisdictionGroupId = clone.GenerateNewKey();
			clone.DisplayName = clone.DisplayName + "_1";
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				repository.Add(clone);
				repository.UnitOfWork.Commit();
			}
			RefreshItemListCommand.Execute();
		}

		protected override void RaiseItemEditInteractionRequest(JurisdictionGroup item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("jurisdictionType", _jurisdictionType),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(JurisdictionGroup item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Jurisdiction Group '{0}'?".Localize(), item.DisplayName),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion

	}
}
