namespace VirtoCommerce.Platform.Core.Swagger
{
    public class SwaggerPlatformOptions
    {
        /// <summary>
        /// Disable swagger at the startup (switch off endpoints for swagger UI and docs)
        /// </summary>
        public bool Disable { get; set; } = false;
    }
}
