namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// A resource, such as an API or an MCP endpoint, that an OAuth application can be allowed to request tokens for (RFC 8707).
    /// </summary>
    public class OAuthResource
    {
        /// <summary>
        /// Absolute URI of the resource, exactly as clients name it in the resource parameter.
        /// </summary>
        public string Uri { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// The kind of the resource, which the back office groups resources by, for example "mcp".
        /// </summary>
        public string Kind { get; set; }

        public string Description { get; set; }
    }
}
