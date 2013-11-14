using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class AgeElement : TypedExpressionElementBase
    {
		private readonly UserInputElement _stringItemEl;
	    private const string ElementLabel = "Age ";

	    public AgeElement(IExpressionViewModel expressionViewModel)
			: base(ElementLabel, expressionViewModel)
        {
			CompareConditions = WithElement(new CompareConditions(true)) as CompareConditions;
			_stringItemEl = WithUserInput(18, 0) as UserInputElement;
        }

		public CompareConditions CompareConditions 
		{ 
			get; 
			set; 
		}

        public int Age
        {
            get
            {
				return Convert.ToInt32(_stringItemEl.InputValue);
            }
            set
            {
				_stringItemEl.InputValue = value;
            }
        }
    }
}
