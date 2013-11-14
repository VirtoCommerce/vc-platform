namespace VirtoCommerce.Web.Client.Services.Security
{
    /// <summary>
    /// Interface IUserSecurity
    /// </summary>
	public interface IUserSecurity
    {
        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool Login(string userName, string password, bool persistCookie = false);
        /// <summary>
        /// Logouts this user.
        /// </summary>
        void Logout();
        /// <summary>
        /// Creates the user and account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="propertyValues">The property values.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns>System.String.</returns>
        string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>user id in the membership provider</returns>
        string GetUserId(string userName);
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool ChangePassword(string userName, string currentPassword, string newPassword);
        /// <summary>
        /// Resets the password. Should only be used by the super admin user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool ResetPassword(string userName, string newPassword);
        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns>System.String.</returns>
        string CreateAccount(string userName, string password, bool requireConfirmationToken = false);
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        bool DeleteUser(string userName);
    }
}
