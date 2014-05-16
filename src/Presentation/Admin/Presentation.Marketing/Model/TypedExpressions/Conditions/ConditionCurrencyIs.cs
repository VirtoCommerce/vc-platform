using System;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public class ConditionCurrencyIs : PromotionExpressionBlock
	{
		const string SettingName_Currencies = "Currencies";

		private readonly UserInputElement _currencyEl;
		public ConditionCurrencyIs(IExpressionViewModel expressionViewModel)
			: base("Currency is []".Localize(), expressionViewModel)
		{
			WithLabel("Currency is ".Localize());

			var allCurrencies = GetAvailableCurrencies(expressionViewModel);
			var defaultCurrency = allCurrencies.Length > 0 ? allCurrencies[0] : null;
			_currencyEl = WithDict(allCurrencies, defaultCurrency) as UserInputElement;
		}

		public string SelectedCurrency
		{
			get
			{
				return Convert.ToString(_currencyEl.InputValue);
			}
			set
			{
				_currencyEl.InputValue = value;
			}
		}

		public override linq.Expression<Func<IEvaluationContext, bool>> GetExpression()
		{
			var paramX = linq.Expression.Parameter(typeof(IEvaluationContext), "x");
			var castOp = linq.Expression.MakeUnary(linq.ExpressionType.Convert, paramX, typeof(PromotionEvaluationContext));
			var memberInfo = typeof(PromotionEvaluationContext).GetMember("Currency").First();
			var currency = linq.Expression.MakeMemberAccess(castOp, memberInfo);
			var selectedCurrency = linq.Expression.Constant(SelectedCurrency);
			var binaryOp = linq.Expression.Equal(selectedCurrency, currency);

			var retVal = linq.Expression.Lambda<Func<IEvaluationContext, bool>>(binaryOp, paramX);

			return retVal;
		}

		private static string[] GetAvailableCurrencies(IExpressionViewModel expressionViewModel)
		{
			string[] result = null;
			using (var repository = ((IPromotionViewModel)expressionViewModel).AppConfigRepositoryFactory.GetRepositoryInstance())
			{
				var setting = repository.Settings.Where(x => x.Name == SettingName_Currencies).ExpandAll().SingleOrDefault();
				if (setting != null)
				{
					result = setting.SettingValues.Select(x => x.ShortTextValue).ToArray();
				}
			}

			return result;
		}
	}
}
