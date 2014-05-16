using System.Collections.Generic;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Linq;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingOptions.Implementations
{
	public class ShippingOptionSettingsViewModel : HomeSettingsEditableViewModel<ShippingOption>, IShippingOptionSettingsViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IShippingRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public ShippingOptionSettingsViewModel(IRepositoryFactory<IShippingRepository> repositoryFactory, IOrderEntityFactory entityFactory, IViewModelsFactory<ICreateShippingOptionViewModel> wizardVmFactory, IViewModelsFactory<IShippingOptionViewModel> editVmFactory)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;

			ShippingOptionNotificationRequest = new InteractionRequest<Confirmation>();
		}

		#endregion

		#region Requests
		public InteractionRequest<Confirmation> ShippingOptionNotificationRequest { get; private set; }

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.ShippingOptions.Expand(so => so.ShippingGateway).OrderBy(cr => cr.Name).ToList();
					return items;
				}
			}
			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as ShippingOption;
			if (itemToUpdate != null)
			{
				ShippingOption itemFromInnerItem =
					Items.SingleOrDefault(so => so.ShippingOptionId == itemToUpdate.ShippingOptionId);

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

		#region HomeSettingsEditableViewModel

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<ShippingOption>();

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Shipping Option".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(ShippingOption item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(ShippingOption item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Shipping Option '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var itemFromRep = repository.ShippingOptions.Where(x => x.ShippingOptionId == item.ShippingOptionId).FirstOrDefault();
				ItemDelete(item, confirmation, repository, itemFromRep);
			}
		}

		protected override void ItemDelete(ShippingOption item, ConditionalConfirmation confirmation, IRepository repository, object itemFromRep)
		{
			CommonConfirmRequest.Raise(confirmation, async (x) =>
			{
				if (x.Confirmed)
				{
					string shipMethodName;
					if (IsShippingOptionAssociatedWithShipingMethod(item.ShippingOptionId, out shipMethodName))
					{
						var shipOptionConfirmation = new ConditionalConfirmation()
						{
							Content =
								string.Format(
									"Shiping option '{0}' are associated with Shipping method '{1}'. You can't delete it.".Localize(),
									item.Name, shipMethodName),
							Title = "Warning".Localize(null, LocalizationScope.DefaultCategory)
						};

						ShippingOptionNotificationRequest.Raise(shipOptionConfirmation);
						return;
					}

					ShowLoadingAnimation = true;
					try
					{
						await Task.Run(() =>
						{
							repository.Remove(itemFromRep);
							repository.UnitOfWork.Commit();
						});

						Items.Remove(item);
					}
					finally
					{
						ShowLoadingAnimation = false;
					}
				}
			});
		}


		private bool IsShippingOptionAssociatedWithShipingMethod(string shippingOptionId, out string shipMethodName)
		{
			bool result = false;
			shipMethodName = string.Empty;

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var shipMethod =
					repository.ShippingMethods.Where(sm => sm.ShippingOptionId == shippingOptionId).FirstOrDefault();
				if (shipMethod != null)
				{
					result = true;
					shipMethodName = shipMethod.Name;
				}
			}

			return result;
		}

		#endregion
	}
}
