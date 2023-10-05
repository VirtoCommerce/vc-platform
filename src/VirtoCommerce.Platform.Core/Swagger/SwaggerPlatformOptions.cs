using System;

namespace VirtoCommerce.Platform.Core.Swagger
{
    public class SwaggerPlatformOptions
    {
        /// <summary>
        /// Disable swagger at the startup (switch off endpoints for swagger UI and docs)
        /// </summary>
        [Obsolete("Use Enable", DiagnosticId = "VC0005", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public bool Disable
        {
            get => !Enable;
            set => Enable = !value;
        }

        // We want the Enable property to have a higher priority than Disable,
        // so Enable should come after the Disable because of the implementation details of IConfiguration.Bind().
        public bool Enable { get; set; } = true;
    }
}
