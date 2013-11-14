using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace VirtoCommerce.Web.Client.Security
{
    /// <summary>
    /// Interface IOAuthWebSecurity
    /// </summary>
	public interface IOAuthWebSecurity
	{
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns>System.String.</returns>
		string GetUserName(string providerName, string providerUserId);
        /// <summary>
        /// Determines whether [the specified user identifier] [has local account].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if [the specified user identifier] [has local account]; otherwise, <c>false</c>.</returns>
		bool HasLocalAccount(string userId);
        /// <summary>
        /// Gets the name of the accounts from user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>ICollection{OAuthAccount}.</returns>
		ICollection<OAuthAccount> GetAccountsFromUserName(string userName);
        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool DeleteAccount(string providerName, string providerUserId);
        /// <summary>
        /// Verifies the authentication.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>AuthenticationResult.</returns>
		AuthenticationResult VerifyAuthentication(string returnUrl);
        /// <summary>
        /// Logins the specified provider name.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <param name="createPersistentCookie">if set to <c>true</c> [create persistent cookie].</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool Login(string providerName, string providerUserId, bool createPersistentCookie);
        /// <summary>
        /// Creates the or update account.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <param name="userName">Name of the user.</param>
		void CreateOrUpdateAccount(string providerName, string providerUserId, string userName);
        /// <summary>
        /// Serializes the provider user identifier.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns>System.String.</returns>
		string SerializeProviderUserId(string providerName, string providerUserId);
        /// <summary>
        /// Gets the open authentication client data.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <returns>AuthenticationClientData.</returns>
		AuthenticationClientData GetOAuthClientData(string providerName);
        /// <summary>
        /// Tries to deserialize provider and user identifier.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="providerUserId">The provider user identifier.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		bool TryDeserializeProviderUserId(string data, out string providerName, out string providerUserId);
        /// <summary>
        /// Gets the registered client data.
        /// </summary>
        /// <value>The registered client data.</value>
		ICollection<AuthenticationClientData> RegisteredClientData { get; }
        /// <summary>
        /// Requests the authentication.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="returnUrl">The return URL.</param>
		void RequestAuthentication(string provider, string returnUrl);
	}
}