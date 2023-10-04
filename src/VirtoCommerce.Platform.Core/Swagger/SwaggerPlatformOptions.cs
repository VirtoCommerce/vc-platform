using System;

namespace VirtoCommerce.Platform.Core.Swagger
{
    public class SwaggerPlatformOptions
    {
        private bool _enable = true;

        /// <summary>
        /// Disable swagger at the startup (switch off endpoints for swagger UI and docs)
        /// </summary>
        [Obsolete("Use Enable", DiagnosticId = "VC0005", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public bool Disable
        {
            get => !_enable;
            set => _enable = !value;
        }

        // We want the Enable property to have a higher priority than Disable,
        // so Enable should come after the Disable because of the implementation details of IConfiguration.Bind().
        public bool Enable
        {
            get => _enable;
            set => _enable = value;
        }
    }
}
