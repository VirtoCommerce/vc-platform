using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Tests
{
    public class AppConfiguration
    {
        public static IConfigurationRoot configuration;

        public AppConfiguration()
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
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
