using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Client
{
    public static class ClientContext
    {
        /// <summary>
        /// Initializes static members of the <see cref="Context" /> class.
        /// </summary>
        static ClientContext()
        {
            Clients = new CommerceClients();
            //Configuration = new CommerceConfiguration();
        }

        /// <summary>
        /// Gets an object providing a common location for service client
        /// discovery.  The VirtoCommerce namespace should be imported
        /// when used because CommerceClients is intended to be the target of
        /// extension methods by each service client library.
        /// </summary>
        public static CommerceClients Clients { get; private set; }

        /// <summary>
        /// Gets utilities for easily retrieving configuration settings across
        /// a variety of platform appropriate sources.
        /// </summary>
        //public static CommerceConfiguration Configuration { get; private set; }
    }
}
