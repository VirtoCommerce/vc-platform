using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Client.Extensions.Validation
{
	/// <summary>
	/// Class DynamicRangeValidatorAttribute.
	/// </summary>
    public class DynamicRangeValidatorAttribute : ValidationAttribute, IClientValidatable
    {
		/// <summary>
		/// The _min property name
		/// </summary>
        private readonly string _minPropertyName;
		/// <summary>
		/// The _max property name
		/// </summary>
        private readonly string _maxPropertyName;
		/// <summary>
		/// Initializes a new instance of the <see cref="DynamicRangeValidatorAttribute"/> class.
		/// </summary>
		/// <param name="minPropertyName">Minimum name of the property.</param>
		/// <param name="maxPropertyName">Maximum name of the property.</param>
        public DynamicRangeValidatorAttribute(string minPropertyName, string maxPropertyName)
        {
            _minPropertyName = minPropertyName;
            _maxPropertyName = maxPropertyName;
        }

		/// <summary>
		/// Validates the specified value with respect to the current validation attribute.
		/// </summary>
		/// <param name="value">The value to validate.</param>
		/// <param name="validationContext">The context information about the validation operation.</param>
		/// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var minProperty = validationContext.ObjectType.GetProperty(_minPropertyName);
            var maxProperty = validationContext.ObjectType.GetProperty(_maxPropertyName);
            if (minProperty == null)
            {
                return new ValidationResult(string.Format("Unknown property {0}", _minPropertyName));
            }
            if (maxProperty == null)
            {
                return new ValidationResult(string.Format("Unknown property {0}", _maxPropertyName));
            }

            var minValue = (int)minProperty.GetValue(validationContext.ObjectInstance, null);
			var maxValue = (int)maxProperty.GetValue(validationContext.ObjectInstance, null);
			var currentValue = (int)value;
            if (currentValue < minValue || currentValue > maxValue)
            {
                return new ValidationResult(
                    string.Format(
                        ErrorMessage,
                        minValue,
                        maxValue
                    )
                );
            }

            return null;
        }

		/// <summary>
		/// When implemented in a class, returns client validation rules for that class.
		/// </summary>
		/// <param name="metadata">The model metadata.</param>
		/// <param name="context">The controller context.</param>
		/// <returns>The client validation rules for this validator.</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "dynamicrange",
                ErrorMessage = ErrorMessage,
            };
            rule.ValidationParameters["minvalueproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldName(_minPropertyName);
            rule.ValidationParameters["maxvalueproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldName(_maxPropertyName);
            yield return rule;
        }
    }
}