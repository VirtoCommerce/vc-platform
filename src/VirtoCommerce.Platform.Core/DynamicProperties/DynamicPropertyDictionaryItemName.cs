using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyDictionaryItemName : ValueObject
    {
        /// <summary>
        /// Language ID, e.g. en-US.
        /// </summary>
        public string Locale { get; set; }
        public string Name { get; set; }
    }
}
