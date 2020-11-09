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
    }
}
