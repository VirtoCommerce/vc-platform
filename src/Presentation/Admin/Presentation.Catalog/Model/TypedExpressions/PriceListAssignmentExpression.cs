using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions;
using VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions.GeoConditions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions
{
	[Serializable]
	public class PriceListAssignmentExpression : TypedExpressionElementBase, IExpressionAdaptor
	{
		private PriceListAssignmentExpressionBlock _ConditionBlock;
		public PriceListAssignmentExpressionBlock ConditionBlock
		{
			get
			{
				return _ConditionBlock;
			}
			private set
			{
				_ConditionBlock = value;
				Children.Add(_ConditionBlock);
			}
		}

		public PriceListAssignmentExpression(IPriceListAssignmentViewModel priceListAssignmentViewModel)
			: base(null, priceListAssignmentViewModel)
		{
			this.ConditionBlock = new PriceListAssignmentExpressionBlock(priceListAssignmentViewModel, false);

			InitializeAvailableExpressions();
		}

		private void InitializeAvailableExpressions()
		{
			var availableElements = new Func<CompositeElement>[] { 
					()=> new PriceListAssignmentExpressionBlock(this.ExpressionViewModel, true)				
				};
			ConditionBlock.WithAvailabeChildren(availableElements);
			ConditionBlock.NewChildLabel = "+ add block".Localize();
		}


		public override void InitializeAfterDeserialized(IExpressionViewModel priceListAssignmentViewModel)
		{
			base.InitializeAfterDeserialized(priceListAssignmentViewModel);
			InitializeAvailableExpressions();

		}

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var retVal = ConditionBlock.GetExpression();
			return retVal;
		}
	}
}
