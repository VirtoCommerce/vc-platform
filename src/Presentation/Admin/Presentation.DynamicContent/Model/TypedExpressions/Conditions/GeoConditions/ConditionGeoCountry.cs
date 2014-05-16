using System;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;
using linq = System.Linq.Expressions;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
	[Serializable]
	public class ConditionGeoCountry : TypedExpressionElementBase, IExpressionAdaptor
	{
		private readonly UserInputElement _valEl;
		private readonly MatchingContains _matchEl;

		public ConditionGeoCountry(IExpressionViewModel expressionViewModel)
            : base("Are browsing from country []".Localize(), expressionViewModel)
		{
            WithLabel("Browsing from country ".Localize());
			_matchEl = WithElement(new MatchingContains(false, false)) as MatchingContains;
            _valEl = WithCustomDict(GetCountryList(expressionViewModel), new[] { "select country".Localize() }) as UserInputElement;
		}

		private string[] GetCountryList(IExpressionViewModel expressionViewModel)
		{
			var retVal = new List<string>();

			using (var countryRepository = ((IContentPublishingItemViewModel)expressionViewModel).CountryRepositoryFactory.GetRepositoryInstance())
			{
				var countries = countryRepository.Countries;
				countries.ToList().ForEach(y => retVal.Add(y.DisplayName));
				return retVal.ToArray();
			}
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

		public string SelectedValue
		{
			get
			{
				return Convert.ToString(_valEl.InputValue);
			}
			set
			{
				_valEl.InputValue = value;
			}
		}

		public linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(DynamicContentEvaluationContext));
			var propertyValue = linq.Expression.Property(castOp, typeof(DynamicContentEvaluationContext).GetProperty("GeoCountry"));

			MethodInfo method;
			linq.Expression methodExp;

			if (string.Equals(MatchConditionValue, MatchCondition.Contains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(SelectedValue.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.Matching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(SelectedValue.ToLowerInvariant()));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.ContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(SelectedValue));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.MatchingCase))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Call(propertyValue, method, linq.Expression.Constant(SelectedValue));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContains))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(SelectedValue.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotMatching))
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				var toLowerMethod = typeof(string).GetMethod("ToLowerInvariant");
				var toLowerExp = linq.Expression.Call(propertyValue, toLowerMethod);
				methodExp = linq.Expression.Not(linq.Expression.Call(toLowerExp, method, linq.Expression.Constant(SelectedValue.ToLowerInvariant())));
			}
			else if (string.Equals(MatchConditionValue, MatchCondition.NotContainsCase))
			{
				method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
				methodExp = linq.Expression.Not(linq.Expression.Call(propertyValue, method, linq.Expression.Constant(SelectedValue)));
			}
			else
			{
				method = typeof(string).GetMethod("Equals", new[] { typeof(string) });
				methodExp = linq.Expression.Not(linq.Expression.Call(propertyValue, method, linq.Expression.Constant(SelectedValue)));
			}

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(methodExp, paramX);
			
			return retVal;
		}
	}
}
