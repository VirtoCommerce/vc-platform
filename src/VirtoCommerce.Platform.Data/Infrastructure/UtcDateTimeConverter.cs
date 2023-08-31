using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    /// <summary>
    /// Converts DateTimeKind.Unspecified to DateTimeKind.Utc when read from the database and back to DateTimeKind.Utc when being saved.
    /// Resolves that SQL Server discards the DateTime.Kind flag when storing a DateTime as a datetime or datetime2.
    /// This means that DateTime values coming back from the database always have a DateTimeKind of Unspecified.
    /// https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations#specify-the-datetimekind-when-reading-dates
    /// </summary>
    /// <example>
    /// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    /// {
    ///     base.ConfigureConventions(configurationBuilder);
    ///     configurationBuilder.Properties&lt;DateTime&gt;().HaveConversion&lt;UtcDateTimeConverter&gt;();
    /// }
    /// </example>
    public class UtcDateTimeConverter : ValueConverter<DateTime?, DateTime?>
    {
        public UtcDateTimeConverter()
            : base(
                d => !d.HasValue ? null : ConvertToUtc(d.Value),
                d => !d.HasValue ? null : SpecifyUtc(d.Value))
        {
        }

        private static DateTime ConvertToUtc(DateTime date)
        {
            return date.Kind == DateTimeKind.Utc ? date : date.ToUniversalTime();
        }

        private static DateTime SpecifyUtc(DateTime date)
        {
            return date.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(date, DateTimeKind.Utc) : date;
        }
    }
}
