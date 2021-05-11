using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Assets.AzureBlobStorage.Tests
{
    public class AppConfiguration
    {
        public static IConfigurationRoot configuration;

        public AppConfiguration()
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .AddUserSecrets<AppConfiguration>()
                .Build();
        }

        public T GetApplicationConfiguration<T>()
            where T : new()
        {
            var result = new T();
            configuration.GetSection("Assets:AzureBlobStorage").Bind(result);

            return result;
        }
    }
}
