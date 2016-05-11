namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyName
    {
        /// <summary>
        /// Language ID, e.g. en-US.
        /// </summary>
        public string Locale { get; set; }
        public string Name { get; set; }

        public DynamicPropertyName Clone()
        {
            return new DynamicPropertyName
            {
                Locale = Locale,
                Name = Name,
            };
        }
    }
}
