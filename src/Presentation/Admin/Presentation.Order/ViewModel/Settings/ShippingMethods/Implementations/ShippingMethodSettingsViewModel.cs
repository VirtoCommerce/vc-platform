using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Configuration.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Interfaces;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.ShippingMethods.Implementations
{
    public class ShippingMethodSettingsViewModel : HomeSettingsEditableViewModel<ShippingMethod>, IShippingMethodSettingsViewModel
    {

        #region Dependencies

        private readonly NavigationManager _navManager;
        private readonly TileManager _tileManager;
        private readonly IRepositoryFactory<IShippingRepository> _repositoryFactory;
        private readonly IAuthenticationContext _authContext;

        #endregion


        #region Constructor

        public ShippingMethodSettingsViewModel(IRepositoryFactory<IShippingRepository> repositoryFactory,
            IOrderEntityFactory entityFactory, IViewModelsFactory<ICreateShippingMethodViewModel> wizardVmFactory, IViewModelsFactory<IShippingMethodViewModel> editVmFactory, IAuthenticationContext authContext,
            NavigationManager navManager, TileManager tileManager)
            : base(entityFactory, wizardVmFactory, editVmFactory)
        {
            _navManager = navManager;
            _tileManager = tileManager;
            _repositoryFactory = repositoryFactory;
            _authContext = authContext;
            PopulateTiles();
        }

        #endregion


        #region HomeSettingsViewModel members

        protected override object LoadData()
        {
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                if (repository != null)
                {
                    var items = repository.ShippingMethods.OrderBy(sm => sm.Name).ToList();
                    return items;
                }
            }
            return null;
        }

        public override void RefreshItem(object item)
        {
            var itemToUpdate = item as ShippingMethod;
            if (itemToUpdate != null)
            {
                ShippingMethod itemFromInnerItem =
                    Items.SingleOrDefault(cps => cps.ShippingMethodId == itemToUpdate.ShippingMethodId);

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
            var item = EntityFactory.CreateEntity<ShippingMethod>();

            var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

            var confirmation = new ConditionalConfirmation()
            {
                Title = "Create Shipping Method".Localize(),
                Content = vm
            };
            ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
        }

        protected override void RaiseItemEditInteractionRequest(ShippingMethod item)
        {
            var itemVM = EditVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("parent", this));

            var openTracking = (IOpenTracking)itemVM;
            openTracking.OpenItemCommand.Execute();
        }

        protected override void RaiseItemDeleteInteractionRequest(ShippingMethod item)
        {
            var repository = _repositoryFactory.GetRepositoryInstance();
            var itemFromRep = repository.ShippingMethods.Where(s => s.ShippingMethodId == item.ShippingMethodId).SingleOrDefault();
            ItemDelete(item, string.Format("Are you sure you want to delete Shipping Method '{0}'?".Localize(), item.Name),
                repository, itemFromRep);
        }

        #endregion

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
            if (_authContext.CheckPermission(PredefinedPermissions.SettingsShippingMethods)
                || _authContext.CheckPermission(PredefinedPermissions.SettingsShippingOptions)
                || _authContext.CheckPermission(PredefinedPermissions.SettingsShippingPackages))
            {
                _tileManager.AddTile(new NumberTileItem()
                {
                    IdModule = Configuration.NavigationNames.MenuName,
                    IdTile = "ShippindMethodsSettings",
                    TileTitle = "Shipping methods",
                    TileCategory = NavigationNames.ModuleName,
                    Order = 3,
                    IdColorSchema = TileColorSchemas.Schema3,
                    NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.ShippingSettingsHomeName)),
                    Refresh = async (tileItem) =>
                    {
                        using (var repository = _repositoryFactory.GetRepositoryInstance())
                        {
                            try
                            {
                                if (tileItem is NumberTileItem)
                                {
                                    var query = await Task.Factory.StartNew(() => repository.ShippingMethods.Count());
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



    }
}
