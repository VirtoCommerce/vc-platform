using System;
using System.Linq;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Catalog.Model.TypedExpressions.Conditions.GeoConditions
{
	[Serializable]
	public class ConditionGeoState : TypedExpressionElementBase, IExpressionAdaptor
	{
		private readonly UserInputElement _stateEl;
		private readonly MatchingContains _matchEl;
		public ConditionGeoState(IExpressionViewModel expressionViewModel)
			: base("Are browsing from state/province []".Localize(), expressionViewModel)
		{
			WithLabel("Browsing from state/province ".Localize());
			_matchEl = WithElement(new MatchingContains(false, false)) as MatchingContains;
			_stateEl = WithCustomDict(GetStatesList(expressionViewModel), new[] { "select state".Localize() }) as UserInputElement;
		}

		private string[] GetStatesList(IExpressionViewModel expressionViewModel)
		{
			var retVal = new List<string>();
			using (var repository = ((IPriceListAssignmentViewModel)expressionViewModel).CountryRepositoryFactory.GetRepositoryInstance())
			{ 
				var countries = repository.Countries.Expand("Regions");
				countries.ToList().ForEach(x => retVal.AddRange(x.Regions.Select(y => y.DisplayName)));
			}

			return retVal.ToArray();
		}
		
		public MatchingContains MatchCondition
		{
			get
			{
				return _matchEl;
			}
		}

		public string MatchConditionValue
		{
			get
			{
				return _matchEl.InputValue.ToString();
			}
			set
			{
				_matchEl.InputValue = value;
			}
		}

		public string SelectedState
		{
			get
			{
				return Convert.ToString(_stateEl.InputValue);
			}
			set
			{
				_stateEl.InputValue = value;
			}
		}

		public System.Linq.Expressions.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{			
			System.Linq.Expressions.ParameterExpression paramX = System.Linq.Expressions.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = System.Linq.Expressions.Expression.MakeUnary(System.Linq.Expressions.ExpressionType.Convert, paramX, typeof(PriceListAssignmentEvaluationContext));
			var propertyValue = System.Linq.Expressions.Expression.Property(castOp, typeof(PriceListAssignmentEvaluationContext).GetProperty("GeoState"));

			MethodInfo method;
			System.Linq.Expressions.Expression methodExp;

			if (string.Equals(MatchConditionValue, MatchCondition.Contains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedState.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.Matching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedState.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.ContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedState));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.MatchingCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedState));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedState.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotMatching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = System.Linq.Expressions.Expression.Call(propertyValue, toLowerMethod);
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(toLowerExp, method, System.Linq.Expressions.Expression.Constant(SelectedState.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedState)));
			}
			else
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = System.Linq.Expressions.Expression.Not(System.Linq.Expressions.Expression.Call(propertyValue, method, System.Linq.Expressions.Expression.Constant(SelectedState)));
			}

			var retVal = System.Linq.Expressions.Expression.Lambda<Func<IEvaluationContext, bool>>(methodExp, paramX);

			return retVal;
		}
	}
}
