#region

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Utilities;

#endregion

namespace VirtoCommerce.ApiClient
{

    #region

    #endregion

    public class SecurityClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the SecurityClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="appId">The API application ID.</param>
        /// <param name="secretKey">The API secret key.</param>
        public SecurityClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the SecurityClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
        /// <param name="handler"></param>
        public SecurityClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        #endregion

        #region Public Methods and Operators

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

        public Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, string>("loginProvider", loginInfo.LoginProvider),
                new KeyValuePair<string, string>("providerKey", loginInfo.ProviderKey)
            };

            var requestUri = CreateRequestUri(RelativePaths.Users, parameters);

            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            if (user.Logins == null)
            {
                user.Logins = new List<UserLoginInfo>();
            }

            user.Logins.Add(login);

            return UpdateAsync(user);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindByEmail, email));
            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindById, userId));
            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.FindByName, userName));
            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<AuthInfo> GetUserInfo(string userName)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.UserInfo, userName));
            return GetAsync<AuthInfo>(requestUri, useCache: false);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            var requestUri = CreateRequestUri(RelativePaths.Update);
            return SendAsync(requestUri, HttpMethod.Put, user);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Users = "users";

            public const string Create = "users/create";

            public const string Delete = "users/delete";

            public const string FindByEmail = "users/email/{0}";

            public const string FindById = "users/id/{0}";

            public const string FindByName = "users/name/{0}";

            public const string Update = "users";

            public const string UserInfo = "usersession/{0}";

            #endregion
        }
    }
}
