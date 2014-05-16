using System;
using System.Linq;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions;
using VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions.GeoConditions;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions
{
	[Serializable]
	public class PriceListAssignmentExpressionBlock : TypedExpressionElementBase, IExpressionAdaptor
	{
		private ConditionAndOrBlock _ConditionBlock;
		public ConditionAndOrBlock ConditionBlock
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

		public PriceListAssignmentExpressionBlock(IExpressionViewModel priceListAssignmentViewModel, bool isAddBlock)
			: base("Add conditions block".Localize(), priceListAssignmentViewModel)
		{
			this.ConditionBlock = new ConditionAndOrBlock("if ".Localize(), priceListAssignmentViewModel, " of these conditions are true".Localize(), isAddBlock);
			InitializeAvailableExpressions();
		}

		private void InitializeAvailableExpressions()
		{

			var availableElements = new Func<CompositeElement>[] {
				()=> { //Browse behavior menu items
					var group = new CompositeElement { DisplayName = "Browse behavior".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						//()=> new ConditionStoreSearchedPhrase(this.ExpressionViewModel),
						()=> new ConditionInternetSearchedPhrase(this.ExpressionViewModel),
						() => new ConditionCurrentUrlIs(this.ExpressionViewModel),
						() => new ConditionReferedFromUrl(this.ExpressionViewModel)
						//()=> new ConditionStoreIs(this.ExpressionViewModel),						
					});
					return group;
				},
				()=> { //Shoppers profile menu items
					var group = new CompositeElement { DisplayName = "Shopper profile".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionAge(this.ExpressionViewModel),
						()=> new ConditionGenderIs(this.ExpressionViewModel)
					});
					return group;
				},			
				()=> { //Shoppers geo location menu items
					var group = new CompositeElement { DisplayName = "Geo location".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionGeoCity(this.ExpressionViewModel),
						()=> new ConditionGeoState(this.ExpressionViewModel),
						()=> new ConditionGeoCountry(this.ExpressionViewModel),
						()=> new ConditionGeoContinent(this.ExpressionViewModel),
						()=> new ConditionGeoZipCode(this.ExpressionViewModel),						
						()=> new ConditionGeoTimeZone(this.ExpressionViewModel),
						()=> new ConditionGeoConnectionType(this.ExpressionViewModel),
						()=> new ConditionGeoIpRoutingType(this.ExpressionViewModel),
						()=> new ConditionGeoIspSecondLevel(this.ExpressionViewModel),
						()=> new ConditionGeoIspTopLevel(this.ExpressionViewModel),
					});
					return group;
				},			
				
            };
			ConditionBlock.WithAvailabeChildren(availableElements);
			ConditionBlock.NewChildLabel = "+ add condition".Localize(null, LocalizationScope.DefaultCategory);
		}

		public override void InitializeAfterDeserialized(IExpressionViewModel priceListAssignmentViewModel)
		{
			base.InitializeAfterDeserialized(priceListAssignmentViewModel);
			InitializeAvailableExpressions();

		}

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{

			linq.Expression<Func<IEvaluationContext, bool>> retVal = ((IExpressionAdaptor)this.Children[0]).GetExpression();
			foreach (var adaptor in this.Children.OfType<IExpressionAdaptor>())
			{
				var expression = adaptor.GetExpression();
				if (adaptor is PriceListAssignmentExpressionBlock)
				{
					expression = ((PriceListAssignmentExpressionBlock)adaptor).ConditionBlock.GetExpression();
					if (!((PriceListAssignmentExpressionBlock)adaptor).ConditionBlock.AndOr.IsAnd)
					{
						retVal = retVal.Or(expression);
					}
					else
					{
						retVal = retVal.And(expression);
					}
				}
				//else
				//	retVal = retVal.And(expression);
			}

			return retVal;
		}
	}
}
