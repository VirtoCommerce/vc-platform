using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public static class ClientContext
    {
        #region Constructors and Destructors

        static ClientContext()
        {
            Configuration = new CommerceConfiguration();
            Clients = new CommerceClients
            {
                CreateMessageProcessingHandler =
                    () => new HmacMessageProcessingHandler(Configuration.ApiAppId, Configuration.ApiSecretKey)
            };
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets an object providing a common location for service client
        ///     discovery.  The VirtoCommerce namespace should be imported
        ///     when used because CommerceClients is intended to be the target of
        ///     extension methods by each service client library.
        /// </summary>
        public static CommerceClients Clients { get; private set; }

        /// <summary>
        ///     Gets utilities for easily retrieving configuration settings across
        ///     a variety of platform appropriate sources.
        /// </summary>
        public static CommerceConfiguration Configuration { get; private set; }

        #endregion
    }
}
