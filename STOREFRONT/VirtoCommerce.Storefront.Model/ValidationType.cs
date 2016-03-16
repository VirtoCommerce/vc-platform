namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Validation type
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Validate prices and quantities
        /// </summary>
        PriceAndQuantity,
        /// <summary>
        /// Validate only prices
        /// </summary>
        Price,
        /// <summary>
        /// Validate only quantities
        /// </summary>
        Quantity,
        /// <summary>
        /// Skip validation
        /// </summary>
        None
    }
}