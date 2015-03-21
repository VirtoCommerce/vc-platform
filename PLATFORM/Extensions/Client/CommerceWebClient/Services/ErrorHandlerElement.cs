using System;
using System.ServiceModel.Configuration;

namespace VirtoCommerce.Web.Client.Services
{
    /// <summary>
    /// Class ErrorHandlerElement.
    /// </summary>
    public class ErrorHandlerElement : BehaviorExtensionElement
    {
        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new ErrorHandler();
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <value>The type of the behavior.</value>
        /// <returns>The type of behavior.</returns>
        public override Type BehaviorType
        {
            get
            {
                return typeof(ErrorHandler);
            }
        }
    }
}
