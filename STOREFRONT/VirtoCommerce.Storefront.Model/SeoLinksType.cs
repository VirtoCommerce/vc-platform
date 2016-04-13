namespace VirtoCommerce.Storefront.Model
{
    /// <summary>
    /// Determines how to generate links for products and categories.
    /// </summary>
    public enum SeoLinksType
    {
        /// <summary>
        /// /category/123
        /// /product/123
        /// </summary>
        None,
        /// <summary>
        /// /my-cool-category
        /// /my-cool-product
        /// </summary>
        Short,
        /// <summary>
        /// /grandparent-category/parent-category/my-cool-category
        /// /grandparent-category/parent-category/my-cool-category/my-cool-product
        /// </summary>
        Long,
    }
}
