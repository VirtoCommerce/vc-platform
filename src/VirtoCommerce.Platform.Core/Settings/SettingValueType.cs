namespace VirtoCommerce.Platform.Core.Settings
{
    public enum SettingValueType
    {
        ShortText,
        LongText,
        Integer,
        Decimal,
        DateTime,
        Boolean,
        SecureString,
        Json,

        /// <summary>
        /// A000027 - Natural numbers
        /// </summary>
        PositiveInteger,

        /// <summary>
        /// A cron expression (5- or 6-field). Stored as text; validated on save and edited in the admin UI with a
        /// preset picker and a live plain-English description.
        /// </summary>
        Cron,
    }
}
