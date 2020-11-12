using FluentValidation;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Validators
{
    public class ObjectSettingEntryValidator : AbstractValidator<ObjectSettingEntry>
    {
        public ObjectSettingEntryValidator()
        {
            When(x => x.ValueType == SettingValueType.PositiveInteger, () =>
            {
                RuleFor(x => x.Value)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .WithMessage("Value cannot be empty!")
                    .Must(value => int.TryParse(value.ToString(), out var number) && number > 0)
                    .WithMessage("Value should be natural number.");
            });
        }
    }
}
