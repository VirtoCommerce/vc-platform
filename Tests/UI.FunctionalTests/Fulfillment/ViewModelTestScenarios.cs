//using System;
//using System.Linq;
//using CommerceFoundation.UI.FunctionalTests.Taxes;
//using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
//using Microsoft.Practices.ServiceLocation;
//using Microsoft.Practices.Unity;
//using VirtoCommerce.Foundation.Frameworks;
//using VirtoCommerce.Foundation.Orders.Model;
//using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
//using VirtoCommerce.Foundation.Orders.Repositories;
//using VirtoCommerce.Foundation.Stores.Model;
//using VirtoCommerce.Foundation.Stores.Repositories;
//using VirtoCommerce.ManagementClient.Core.Infrastructure;
//using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
//using VirtoCommerce.ManagementClient.Fulfillment.ViewModel;
//using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Wizard;
//using Xunit;

//namespace CommerceFoundation.UI.FunctionalTests.Fulfilment
//{
//    public class AuthenticationContext:IAuthenticationContext
//    {
//        public bool IsUserAuthenticated { get; private set; }
//        public bool IsAdminUser { get; private set; }
//        public string CurrentUserName { get; private set; }
//        public string CurrentUserId { get; private set; }
//        public string Token { get; private set; }
//        public void UpdateToken()
//        {
//            //throw new System.NotImplementedException();
//        }

//        public bool Login(string userName, string password, string baseUrl)
//        {
//            return true;
//        }

//        public bool CheckPermission(string permission)
//        {
//            return true;
//        }
//    }

	
//    public class FulfillmentCenterRepository:IFulfillmentCenterRepository
//    {
//        public void Dispose()
//        {
//        }

//        public IUnitOfWork UnitOfWork { get; private set; }
//        public void Attach<T>(T item) where T : class
//        {
//        }

//        public bool IsAttachedTo<T>(T item) where T : class
//        {
//            return true;
//        }

//        public void Add<T>(T item) where T : class
//        {
//        }

//        public void Update<T>(T item) where T : class
//        {
//        }

//        public void Remove<T>(T item) where T : class
//        {
//        }

//        public IQueryable<T> GetAsQueryable<T>() where T : class
//        {
//            return null;
//        }

//        public IQueryable<FulfillmentCenter> FulfillmentCenters { get; private set; }
//    }

//    public class OrderRepository : IOrderRepository
//    {
//        public void Dispose()
//        {
//        }

//        public IUnitOfWork UnitOfWork { get; private set; }
//        public void Attach<T>(T item) where T : class
//        {
//        }

//        public bool IsAttachedTo<T>(T item) where T : class
//        {
//            return true;
//        }

//        public void Add<T>(T item) where T : class
//        {
//        }

//        public void Update<T>(T item) where T : class
//        {
//        }

//        public void Remove<T>(T item) where T : class
//        {
//        }

//        public IQueryable<T> GetAsQueryable<T>() where T : class
//        {
//            return null;
//        }

//        public IQueryable<Order> Orders { get; private set; }
//        public IQueryable<ShoppingCart> ShoppingCarts { get; private set; }
//        public IQueryable<Shipment> Shipments { get; private set; }
//        public IQueryable<RmaRequest> RmaRequests { get; private set; }
//        public IQueryable<LineItem> LineItems { get; private set; }
//        public IQueryable<OrderAddress> OrderAddresses { get; private set; }
//        public IQueryable<Payment> Payments { get; private set; }
//        public IQueryable<Jurisdiction> Jurisdictions { get; private set; }
//        public IQueryable<JurisdictionGroup> JurisdictionGroups { get; private set; }
//    }

//    public class ViewModelTestScenarios : Taxes.TaxesTestBase
//    {
//        public ViewModelTestScenarios()
//        {
//            Container.RegisterType<IAuthenticationContext, AuthenticationContext>();
//            Container.RegisterType<ICreatePicklistWizardViewModel, CreatePicklistWizardViewModel>();
//            Container.RegisterType<IFulfillmentCenterRepository, FulfillmentCenterRepository>();
//            Container.RegisterType<IViewModelsFactory, ViewModelsFactory>();
//            Container.RegisterType<ICreatePicklistStepViewModel, CreatePicklistStepViewModel>();
//            Container.RegisterType<IOrderRepository, OrderRepository>();
			
//            Container.RegisterType(typeof(IRepositoryFactory<>), typeof(UnityRepositoryFactory<>), new ContainerControlledLifetimeManager());
			
//        }

								
//        [Fact]
//        public void FirstViewModelTest()
//        {
//            var uc = ServiceLocator.Current.GetInstance<IUnityContainer>();
//            var vm = uc.Resolve<PicklistHomeViewModel>();
//            vm.CommonWizardDialogRequest.Raised += (o, e) =>
//                {
//                    var c = (Confirmation)e.Context;
//                    if (!(e.Context.Content is WizardViewModelBare))
//                        throw new ArgumentException(
//                            string.Format(msg,
//                                e.Context.GetType().FullName));
//                };

//            vm.ItemAddCommand.Execute();
						
//        }

//    }
//}
