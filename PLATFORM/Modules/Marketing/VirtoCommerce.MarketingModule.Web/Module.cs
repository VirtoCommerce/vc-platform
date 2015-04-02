using ExpressionSerialization;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Common.Expressions;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.Model.TypedExpression;
using VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Actions;
using VirtoCommerce.MarketingModule.Web.Model.TypedExpression.Conditions;

namespace VirtoCommerce.MarketingModule.Web
{
    public class Module : IModule
    {
		private readonly IUnityContainer _container;

		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			_container.RegisterType<IFoundationMarketingRepository>(new InjectionFactory(c => new FoundationMarketingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor())));

			var memoryPromotionManager = new InMemoryCustomPromotionManagerImpl();
			memoryPromotionManager.DynamicExpression = new DynamicPromotionBlock();
			var customerConditionBlock = new CustomerConditionBlock();
			var catalogConditionBlock = new CatalogConditionBlock();
			var cartConditionBlock = new CartConditionBlock();
			var rewardBlock = new RewardBlock();

			cartConditionBlock.AvailableChildren = new ExpressionElement[] { new ConditionAtNumItemsInCart() };
			rewardBlock.AvailableChildren = new ExpressionElement[] { new  RewardCartGetOfAbsSubtotal() };
			memoryPromotionManager.DynamicExpression.AvailableChildren = new ExpressionElement[] { customerConditionBlock, catalogConditionBlock, cartConditionBlock, rewardBlock };

			_container.RegisterInstance<ICustomPromotionManager>(memoryPromotionManager);
			_container.RegisterType<IMarketingService, MarketingServiceImpl>();
			_container.RegisterType<IMarketingSearchService, MarketingSearchServiceImpl>();

		}

		#endregion
    }
}