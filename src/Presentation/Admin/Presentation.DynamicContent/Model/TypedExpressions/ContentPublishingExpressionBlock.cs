using System;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public class ContentPublishingExpressionBlock : TypedExpressionElementBase, IExpressionAdaptor
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

        public ContentPublishingExpressionBlock(IExpressionViewModel publishingViewModel)
            : base(null, publishingViewModel)
        {
            this.ConditionBlock = new ConditionAndOrBlock("if".Localize(), publishingViewModel, "of these conditions are true".Localize());

            InitializeAvailableExpressions();
        }

        private void InitializeAvailableExpressions()
        {

            var availableElements = new Func<CompositeElement>[] { 
				()=> { //Browse behavior menu items
                    var group = new CompositeElement { DisplayName = "Browse behavior".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionStoreSearchedPhrase(this.ExpressionViewModel),
						()=> new ConditionInternetSearchedPhrase(this.ExpressionViewModel),
						() => new ConditionCurrentUrlIs(this.ExpressionViewModel),
						()=> new ConditionStoreIs(this.ExpressionViewModel),
						()=> new ConditionLanguageIs(this.ExpressionViewModel),
						()=> new ConditionCategoryIs(this.ExpressionViewModel, false), //without subcategories
						()=> new ConditionCategoryIs(this.ExpressionViewModel, true) //with subcategories
					});
					return group;
				},
				()=> { //Shopper profile menu items
                    var group = new CompositeElement { DisplayName = "Shopper profile".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionShopperAge(this.ExpressionViewModel),
						()=> new ConditionGenderIs(this.ExpressionViewModel)});
					return group;
				},
				()=> { //Shopping cart menu items
                    var group = new CompositeElement { DisplayName = "Shopping cart".Localize() };
					group.AvailableChildrenGetters.AddRange(new Func<ExpressionElement>[] {
						()=> new ConditionCartTotalIs(this.ExpressionViewModel)});
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

        public override void InitializeAfterDeserialized(IExpressionViewModel publishingViewModel)
        {
            base.InitializeAfterDeserialized(publishingViewModel);
            InitializeAvailableExpressions();

        }

        public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
        {
            var retVal = ConditionBlock.GetExpression();
            return retVal;
        }

    }
}
