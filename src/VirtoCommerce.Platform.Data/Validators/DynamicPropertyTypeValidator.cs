using FluentValidation;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Data.Validators
{
    public class DynamicPropertyTypeValidator : AbstractValidator<DynamicProperty>
    {
        public DynamicPropertyTypeValidator()
        {
            RuleFor(property => property.IsDictionary).Equal(false)
                .When(property => property.ValueType == DynamicPropertyValueType.Integer)
                .When(property => property.ValueType == DynamicPropertyValueType.Decimal)
                .When(property => property.ValueType == DynamicPropertyValueType.LongText)
                .When(property => property.ValueType == DynamicPropertyValueType.Html)
                .When(property => property.ValueType == DynamicPropertyValueType.DateTime)
                .When(property => property.ValueType == DynamicPropertyValueType.Boolean)
                .When(property => property.ValueType == DynamicPropertyValueType.Image);

            RuleFor(property => property.IsMultilingual).Equal(false)
                .When(property => property.ValueType == DynamicPropertyValueType.Integer)
                .When(property => property.ValueType == DynamicPropertyValueType.Decimal)
                .When(property => property.ValueType == DynamicPropertyValueType.DateTime)
                .When(property => property.ValueType == DynamicPropertyValueType.Boolean)
                .When(property => property.ValueType == DynamicPropertyValueType.Image);

            RuleFor(property => property.IsArray).Equal(false)
                .When(property => property.ValueType == DynamicPropertyValueType.LongText)
                .When(property => property.ValueType == DynamicPropertyValueType.Html)
                .When(property => property.ValueType == DynamicPropertyValueType.DateTime)
                .When(property => property.ValueType == DynamicPropertyValueType.Boolean)
                .When(property => property.ValueType == DynamicPropertyValueType.Image);
        }
    }
}
