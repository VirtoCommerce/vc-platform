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
    public class SecurityClient : BaseClient
    {
        public SecurityClient(Uri adminBaseEndpoint, string appId, string secretKey)
            : base(adminBaseEndpoint, new HmacMessageProcessingHandler(appId, secretKey))
        {
        }

        public SecurityClient(Uri adminBaseEndpoint, MessageProcessingHandler handler)
            : base(adminBaseEndpoint, handler)
        {
        }

        public Task<SignInStatus> PasswordSignInAsync(string username, string password)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("isPersistent", "false")
            };

            var requesrtUri = CreateRequestUri(RelativePaths.SignIn, parameters);

            return SendAsync<SignInStatus>(requesrtUri, HttpMethod.Post);
        }

        public Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            var requestUri = CreateRequestUri(RelativePaths.Common);

            return SendAsync<ApplicationUser, IdentityResult>(requestUri, HttpMethod.Post, user);
        }

        public Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.GetUserById, userId));

            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindUserByNameAsync(string userName)
        {
            var requestUri = CreateRequestUri(string.Format(RelativePaths.GetUserByName, userName));

            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task<ApplicationUser> FindUserByLoginAsync(string loginProvider, string providerKey)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, string>("loginProvider", loginProvider),
                new KeyValuePair<string, string>("providerKey", providerKey)
            };

            var requestUri = CreateRequestUri(RelativePaths.GetUserByLogin, parameters);

            return GetAsync<ApplicationUser>(requestUri, useCache: false);
        }

        public Task GenerateResetPasswordTokenAsync(string userId, string storeName, string callbackUrl)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, string>("userId", userId),
                new KeyValuePair<string, string>("storeName", callbackUrl),
                new KeyValuePair<string, string>("callbackUrl", callbackUrl)
            };

            var requestUri = CreateRequestUri(RelativePaths.GenerateResetPasswordToken, parameters);

            return SendAsync(requestUri, HttpMethod.Post);
        }

        public Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var parameters = new[]
            {
                new KeyValuePair<string, string>("userId", userId),
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("newPassword", newPassword)
            };

            var requestUri = CreateRequestUri(RelativePaths.ResetPassword, parameters);

            return SendAsync<IdentityResult>(requestUri, HttpMethod.Post);
        }

        protected class RelativePaths
        {
            public const string Common = "frontend/user";
            public const string GetUserById = Common + "/id/{0}";
            public const string GetUserByName = Common + "/name/{0}";
            public const string GetUserByLogin = Common + "/login";
            public const string SignIn = Common + "/signin";
            public const string GenerateResetPasswordToken = Common + "/password/resettoken";
            public const string ResetPassword = Common + "/password/reset";
        }
    }
}