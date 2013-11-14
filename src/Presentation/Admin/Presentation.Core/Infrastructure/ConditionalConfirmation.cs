using System;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public class ConditionalConfirmation : Confirmation
    {
        private Func<bool> _validateFunction;

        public bool IsValid { get; private set; }

        public ConditionalConfirmation()
            : this(null)
        {
        }

        /// <summary>
        /// data validation function
        /// </summary>
        /// <param name="validateFunction"></param>
        public ConditionalConfirmation(Func<bool> validateFunction)
        {
            _validateFunction = validateFunction;
        }

        public void ValidationMethod()
        {
            IsValid = _validateFunction == null ? true : _validateFunction();
        }
    }
}
