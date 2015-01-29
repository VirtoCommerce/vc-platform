using System;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Utilities;

namespace VirtoCommerce.ApiClient
{
    public class SecurityClient : BaseClient
    {
        protected class RelativePaths
        {
            public const string FindById = "users/id/{0}";
            public const string FindByName = "users/name/{0}";
            public const string FindByEmail = "users/email/{0}";
            public const string Create = "users/create";
            public const string Update = "users/update";
            public const string Delete = "users/delete";
            public const string UserInfo = "usersession/{0}";
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

        #region Owin Security Integration

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindById,userId));
            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindByName, userName));
            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindByEmail, email));
            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }
        public Task CreateAsync(ApplicationUser user)
        {
            var requestUri = CreateRequestUri(RelativePaths.Create);
            return SendAsync(requestUri, HttpMethod.Post, user);
        }

        public Task DeleteAsync(string userId)
        {
            var requestUri = CreateRequestUri(RelativePaths.Delete);
            return SendAsync(requestUri, HttpMethod.Post, userId);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            var requestUri = CreateRequestUri(RelativePaths.Update);
            return SendAsync(requestUri, HttpMethod.Post, user);
        }

        #endregion

        public Task<AuthInfo> GetUserInfo(string userName)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.UserInfo, userName));
            return GetAsync<AuthInfo>(requestUri);
        }
    }
}
