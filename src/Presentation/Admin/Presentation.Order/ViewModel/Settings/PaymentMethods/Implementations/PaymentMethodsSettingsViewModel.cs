using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.PaymentMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Configuration.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.PaymentMethods.Implementations
{
	public class PaymentMethodsSettingsViewModel : HomeSettingsEditableViewModel<PaymentMethod>, IPaymentMethodsSettingsViewModel
	{
		#region Dependencies

		private readonly NavigationManager _navManager;
		private readonly TileManager _tileManager;
		private readonly IRepositoryFactory<IPaymentMethodRepository> _repositoryFactory;
		private readonly IAuthenticationContext _authContext;

		#endregion

		public PaymentMethodsSettingsViewModel(IRepositoryFactory<IPaymentMethodRepository> repositoryFactory,
			IOrderEntityFactory entityFactory, IViewModelsFactory<ICreatePaymentMethodViewModel> wizardVmFactory, IViewModelsFactory<IPaymentMethodViewModel> editVmFactory, IAuthenticationContext authContext,
			NavigationManager navManager, TileManager tileManager)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_tileManager = tileManager;
			_authContext = authContext;
			PopulateTiles();
		}

		#region Tiles

		private bool NavigateToTabPage(string id)
		{

			var navigationData = _navManager.GetNavigationItemByName(Configuration.NavigationNames.HomeName);
			if (navigationData != null)
			{
				_navManager.Navigate(navigationData);
				var mainViewModel = _navManager.GetViewFromRegion(navigationData) as ConfigurationHomeViewModel;

				return (mainViewModel != null && mainViewModel.SetCurrentTabById(id));
			}
			return false;
		}

		private void PopulateTiles()
		{

			if (_authContext.CheckPermission(PredefinedPermissions.Name_OrdersOrder_PaymentsAll))
			{
				_tileManager.AddTile(new NumberTileItem()
				{
					IdModule = VirtoCommerce.ManagementClient.Configuration.NavigationNames.MenuName,
					IdTile = "PaymentMethodsSettings",
                    TileTitle = "Payment methods",
                    TileCategory = NavigationNames.ModuleName,
					Order = 2,
					IdColorSchema = TileColorSchemas.Schema2,
					NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.PaymentsSettingsHomeName)),
					Refresh = async (tileItem) =>
					{
						using (var repository = _repositoryFactory.GetRepositoryInstance())
						{
							try
							{
								if (tileItem is NumberTileItem)
								{
									var query = await Task.Factory.StartNew(() => repository.PaymentMethods.Count());
									(tileItem as NumberTileItem).TileNumber = query.ToString();
								}
							}
							catch
							{
							}
						}
					}
				});
			}
		}

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.PaymentMethods.OrderBy(pm => pm.Name).ToList();
					return items;
				}
			}
			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as PaymentMethod;
			if (itemToUpdate != null)
			{
				PaymentMethod itemFromInnerItem =
					Items.SingleOrDefault(pm => pm.PaymentMethodId == itemToUpdate.PaymentMethodId);

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
			var item = EntityFactory.CreateEntity<PaymentMethod>();

			var vm = WizardVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item));
			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Payment Method".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(PaymentMethod item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(PaymentMethod item)
		{
			var confirmation = new ConditionalConfirmation()
				{
					Content = string.Format("Are you sure you want to delete Payment method '{0}'?".Localize(), item.Name)
				};

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var itemFromRep = repository.PaymentMethods.Where(x => x.PaymentMethodId == item.PaymentMethodId).FirstOrDefault();
				ItemDelete(item, confirmation, repository, itemFromRep);
			}
		}

		#endregion
	}
}
