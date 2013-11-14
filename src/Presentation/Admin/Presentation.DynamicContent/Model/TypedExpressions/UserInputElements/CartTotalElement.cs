using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Controls;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public class CartTotalElement : TypedExpressionElementBase
    {
		private readonly UserInputElement _stringItemEl;
	    private const string ElementLabel = "Cart total $";

	    public CartTotalElement(IExpressionViewModel expressionViewModel)
			: base(ElementLabel, expressionViewModel)
        {
			CompareConditions = WithElement(new CompareConditions(false)) as CompareConditions;
			_stringItemEl = WithUserInput<decimal>(0, 0) as UserInputElement;
        }

		public CompareConditions CompareConditions { get; set; }

        public decimal NumVal
        {
            get
            {
				return Convert.ToDecimal(_stringItemEl.InputValue);
            }
            set
            {
				_stringItemEl.InputValue = value;
            }
        }
    }
}
