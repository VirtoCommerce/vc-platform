using Microsoft.Practices.Unity;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.MarketingModule.Expressions.Promotion;

namespace CustomExpression
{
	public class Module : IModule, IPostInitialize
	{
		private readonly IUnityContainer _container;

		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
		}

		#endregion
		

		#region IPostInitialize Members

		public void PostInitialize()
		{
			//Resolve promotionExtensionManager
			var marketingExtensionManager = _container.Resolve<IMarketingExtensionManager>();
			//Register custom prmotion dynamic expression
			var tree = marketingExtensionManager.PromotionDynamicExpressionTree as PromoDynamicExpressionTree;
			if(tree != null)
			{
				var catalogBlock = tree.FindAvailableExpression<BlockCatalogCondition>();
				catalogBlock.AvailableChildren.Add(new ConditionItemWithTag());
			}
			//Regsiter custom promotion
			marketingExtensionManager.RegisterPromotion(new EvenNumberItemInCartPromotion(0.05m));
		}


		#endregion

		
	}
}