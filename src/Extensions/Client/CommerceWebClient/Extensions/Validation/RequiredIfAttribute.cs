using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Client.Extensions.Validation
{
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();

        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(DependentProperty);

            if (field != null)
            {
                // get the value of the dependent property
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);

                // compare the value against the target value
                if ((dependentValue == null && TargetValue == null) ||
                    (dependentValue != null && dependentValue.Equals(TargetValue)))
                {
                    // match => means we should try validating this field
                    if (!_innerAttribute.IsValid(value))
                        // validation failed - return an error
                        return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
	            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredif",
            };

            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);

            // find the value on the control we depend on;
            // if it's a bool, format it javascript style 
            // (the default is True or False!)
            string targetValue = (TargetValue ?? "").ToString();
            if (TargetValue.GetType() == typeof(bool))
                targetValue = targetValue.ToLower();

            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", targetValue);

            yield return rule;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(DependentProperty);
            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            return depProp;
        }
    }
}