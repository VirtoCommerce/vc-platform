using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace VirtoCommerce.Web.Client.Security
{
    using System;

    /// <summary>
    /// Class OAuthWebSecurityWrapper.
    /// </summary>
    [Obsolete("Use asp.net identity OAuth2")]
	public class OAuthWebSecurityWrapper : IOAuthWebSecurity
	{
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns>System.String.</returns>
		public string GetUserName(string providerName, string providerUserId)
		{
			return OAuthWebSecurity.GetUserName(providerName, providerUserId);
		}

        /// <summary>
        /// Determines whether [the specified user identifier] [has local account].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if [the specified user identifier] [has local account]; otherwise, <c>false</c>.</returns>
		public bool HasLocalAccount(string userId)
		{
			return OAuthWebSecurity.HasLocalAccount(Int32.Parse(userId));
		}

        /// <summary>
        /// Gets the name of the accounts from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>ICollection{OAuthAccount}.</returns>
		public ICollection<OAuthAccount> GetAccountsFromUserName(string userName)
		{
			return OAuthWebSecurity.GetAccountsFromUserName(userName);
		}

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool DeleteAccount(string providerName, string providerUserId)
		{
			return OAuthWebSecurity.DeleteAccount(providerName, providerUserId);
		}

        /// <summary>
        /// Verifies the authentication.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>AuthenticationResult.</returns>
		public AuthenticationResult VerifyAuthentication(string returnUrl)
		{
			return OAuthWebSecurity.VerifyAuthentication(returnUrl);
		}

        /// <summary>
        /// Logins the specified provider name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Login(string providerName, string providerUserId, bool createPersistentCookie)
		{
			return OAuthWebSecurity.Login(providerName, providerUserId, createPersistentCookie);
		}

        /// <summary>
        /// Creates the or update account.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <param name="userName">Name of the user.</param>
		public void CreateOrUpdateAccount(string providerName, string providerUserId, string userName)
		{
			OAuthWebSecurity.CreateOrUpdateAccount(providerName, providerUserId, userName);
		}

        /// <summary>
        /// Serializes the provider user identifier.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns>System.String.</returns>
		public string SerializeProviderUserId(string providerName, string providerUserId)
		{
			return OAuthWebSecurity.SerializeProviderUserId(providerName, providerUserId);
		}

        /// <summary>
        /// Gets the open authentication client data.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>AuthenticationClientData.</returns>
		public AuthenticationClientData GetOAuthClientData(string providerName)
		{
			return OAuthWebSecurity.GetOAuthClientData(providerName);
		}

        /// <summary>
        /// Tries to deserialize provider and user identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId)
		{
			return OAuthWebSecurity.TryDeserializeProviderUserId(data, out providerName, out providerUserId);
		}

        /// <summary>
        /// Gets the registered client data.
        /// </summary>
        /// <value>The registered client data.</value>
		public ICollection<AuthenticationClientData> RegisteredClientData { get { return OAuthWebSecurity.RegisteredClientData; } }

        /// <summary>
        /// Requests the authentication.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
		public void RequestAuthentication(string provider, string returnUrl)
		{
			OAuthWebSecurity.RequestAuthentication(provider, returnUrl);
		}
	}
}