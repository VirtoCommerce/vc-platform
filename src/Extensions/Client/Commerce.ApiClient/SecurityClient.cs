namespace VirtoCommerce.ApiClient
{
    #region

using System;
using System.Net.Http;
using System.Threading.Tasks;

using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Utilities;

    #endregion

    public class SecurityClient : BaseClient
    {
        #region Constructors and Destructors

        /// <summary>
		/// Initializes a new instance of the SecurityClient class.
        /// </summary>
        /// <param name="adminBaseEndpoint">Admin endpoint</param>
		/// <param name="appId">The API application ID.</param>
		/// <param name="secretKey">The API secret key.</param>
		public SecurityClient(Uri adminBaseEndpoint, string appId, string secretKey)
			: base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        /// <summary>
		/// Initializes a new instance of the SecurityClient class.
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
            var requestUri = this.CreateRequestUri(RelativePaths.Create);
            return SendAsync(requestUri, HttpMethod.Post, user);
        }

        public Task DeleteAsync(string userId)
        {
            var requestUri = this.CreateRequestUri(RelativePaths.Delete);
            return this.SendAsync(requestUri, HttpMethod.Post, userId);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var requestUri = this.CreateRequestUri(string.Format(RelativePaths.FindByEmail, email));
            return this.GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var requestUri = this.CreateRequestUri(string.Format(RelativePaths.FindById, userId));
            return this.GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var requestUri = this.CreateRequestUri(string.Format(RelativePaths.FindByName, userName));
            return this.GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<AuthInfo> GetUserInfo(string userName)
        {
            var requestUri = this.CreateRequestUri(string.Format(RelativePaths.UserInfo, userName));
            return this.GetAsync<AuthInfo>(requestUri);
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            var requestUri = this.CreateRequestUri(RelativePaths.Update);
            return SendAsync(requestUri, HttpMethod.Post, user);
        }

        #endregion

        protected class RelativePaths
        {
            #region Constants

            public const string Create = "users/create";

            public const string Delete = "users/delete";

            public const string FindByEmail = "users/email/{0}";

            public const string FindById = "users/id/{0}";

            public const string FindByName = "users/name/{0}";

            public const string Update = "users/update";

            public const string UserInfo = "usersession/{0}";

            #endregion
        }
    }
}
