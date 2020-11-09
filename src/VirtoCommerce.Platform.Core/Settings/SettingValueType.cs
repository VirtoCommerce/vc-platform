namespace VirtoCommerce.Platform.Core.Settings
{
    public enum SettingValueType
    {
        Boolean,
        DateTime,
        Decimal,
        Integer,
        Json,
        LongText,

        /// <summary>
        /// A000027 - Natural numbers
        /// </summary>
        PositiveInteger,

        SecureString,
        ShortText,
    }
}
