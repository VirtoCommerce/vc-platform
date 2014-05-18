using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [Serializable]
    public class MatchContainsStringElement : TypedExpressionElementBase
    {
		private readonly UserInputElement _stringItemEl;
        private static string ElementLabel = "Current value is ".Localize();

		public MatchContainsStringElement(IExpressionViewModel expressionViewModel)
			: base(ElementLabel, expressionViewModel)
        {
			MatchingContains = WithElement(new MatchingContains(false, true)) as MatchingContains;
			_stringItemEl = WithUserInput(string.Empty) as UserInputElement;
        }


		public MatchingContains MatchingContains { get; set; }

        public string ValueString
        {
            get
            {
				return Convert.ToString(_stringItemEl.InputValue);
            }
            set
            {
				_stringItemEl.InputValue = value;
            }
        }
    }
}
