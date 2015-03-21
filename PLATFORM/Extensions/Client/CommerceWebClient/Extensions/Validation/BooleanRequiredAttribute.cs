using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Client.Extensions.Validation
{
	/// <summary>
	/// Class BooleanRequiredAttribute.
	/// </summary>
    public class BooleanRequiredAttribute : RequiredAttribute, IClientValidatable
    {
		/// <summary>
		/// Checks that the value of the required data field is not empty.
		/// </summary>
		/// <param name="value">The data field value to validate.</param>
		/// <returns>true if validation is successful; otherwise, false.</returns>
        public override bool IsValid(object value)
        {
            return value != null && (bool)value;
        }

		/// <summary>
		/// When implemented in a class, returns client validation rules for that class.
		/// </summary>
		/// <param name="metadata">The model metadata.</param>
		/// <param name="context">The controller context.</param>
		/// <returns>The client validation rules for this validator.</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationRule() { ValidationType = "brequired", ErrorMessage = this.ErrorMessage } };
        }
    }
}