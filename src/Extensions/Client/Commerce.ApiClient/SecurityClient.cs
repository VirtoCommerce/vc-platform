using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    using DataContracts.Contents;

    public class SecurityClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string FindById = "users/id/{0}";
            public const string FindByName = "users/name/{0}";
            public const string Create = "users/create";
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="token">Access token</param>
        public SecurityClient(Uri adminBaseEndpoint, string token)
            : base(adminBaseEndpoint, new TokenMessageProcessingHandler(token))
        {
        }

        /// <summary>
        /// Initializes a new instance of the AdminManagementClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public SecurityClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {

        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindById,userId));
            return GetAsync<ApplicationUser>(requestUri);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindByName, userName));
            return GetAsync<ApplicationUser>(requestUri);
        }

        public Task CreateAsync(ApplicationUser user)
        {
            var requestUri = CreateRequestUri(RelativePaths.Create);
            return SendAsync(requestUri, HttpMethod.Post, user);
        }
    }
}
