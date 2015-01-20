namespace VirtoCommerce.ApiClient
{
    using VirtoCommerce.ApiClient.Session;

    public static class ClientContext
    {
        static ClientContext()
        {
            Clients = new CommerceClients();
            SessionService = new CustomerSessionService();
            Configuration = new CommerceConfiguration();
        }

        /// <summary>
        /// Gets an object providing a common location for service client
        /// discovery.  The VirtoCommerce namespace should be imported
        /// when used because CommerceClients is intended to be the target of
        /// extension methods by each service client library.
        /// </summary>
        public static CommerceClients Clients { get; private set; }

        public static ICustomerSessionService SessionService { get; private set; }

        /// <summary>
        /// Gets utilities for easily retrieving configuration settings across
        /// a variety of platform appropriate sources.
        /// </summary>
        public static CommerceConfiguration Configuration { get; private set; }

        public static ICustomerSession Session
        {
            get
            {
                return SessionService.CustomerSession;
            }
        }
    }
}
