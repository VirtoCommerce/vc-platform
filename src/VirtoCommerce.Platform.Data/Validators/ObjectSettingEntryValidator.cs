using System;
using System.Text.RegularExpressions;
using FluentValidation;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Validators
{
    public class ObjectSettingEntryValidator : AbstractValidator<ObjectSettingEntry>
    {
        // Structural cron validation with a Regex (no parser dependency): 5 fields, or 6 with a leading seconds
        // field. Each field is a comma-separated list of "*", a number, a range, or a step — with numeric ranges
        // enforced per field, so e.g. "99" in the minute field is rejected. Month / day-of-week names are accepted.
        private static readonly Regex _cronRegex = BuildCronRegex();

        public ObjectSettingEntryValidator()
        {
            When(x => x.ValueType == SettingValueType.PositiveInteger, () =>
            {
                RuleFor(x => x.Value)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Value cannot be empty!")
                    .Must(value => int.TryParse(value.ToString(), out var number) && number > 0)
                    .WithMessage("Value should be natural number.");
            });

            When(x => x.ValueType == SettingValueType.Cron, () =>
            {
                RuleFor(x => x.Value)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage("Cron expression cannot be empty!")
                    .Must(BeValidCron)
                    .WithMessage("Value is not a valid cron expression (use a 5- or 6-field cron).");
            });
        }

        private static bool BeValidCron(object value)
        {
            var expression = value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(expression))
            {
                return false;
            }

            try
            {
                return _cronRegex.IsMatch(expression);
            }
            catch (RegexMatchTimeoutException)
            {
                // Pathological input hit the ReDoS guard — treat as invalid rather than hang the save.
                return false;
            }
        }

        private static Regex BuildCronRegex()
        {
            const string minute = "[0-5]?[0-9]";                                                    // 0-59
            const string hour = "[01]?[0-9]|2[0-3]";                                                 // 0-23
            const string dayOfMonth = "0?[1-9]|[12][0-9]|3[01]";                                     // 1-31
            const string month = "0?[1-9]|1[0-2]|JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC";   // 1-12 or names
            const string dayOfWeek = "[0-7]|SUN|MON|TUE|WED|THU|FRI|SAT";                            // 0-7 or names

            // One field: a comma-separated list where each element is "*" (optionally stepped), or a value /
            // range (optionally stepped). Values are wrapped in (?:...) since each may itself be an alternation.
            static string Field(string value)
            {
                var element = $@"(?:\*(?:/\d+)?|(?:{value})(?:-(?:{value}))?(?:/\d+)?)";
                return $@"{element}(?:,{element})*";
            }

            var fiveFields = $@"{Field(minute)}\s+{Field(hour)}\s+{Field(dayOfMonth)}\s+{Field(month)}\s+{Field(dayOfWeek)}";

            // Optional leading seconds field makes it a 6-field expression; otherwise a standard 5-field one.
            var pattern = $@"^(?:{Field(minute)}\s+)?{fiveFields}$";

            return new Regex(
                pattern,
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled,
                TimeSpan.FromMilliseconds(200));
        }
    }
}
