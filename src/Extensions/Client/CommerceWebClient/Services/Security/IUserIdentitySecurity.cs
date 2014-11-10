using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VirtoCommerce.Web.Client.Security.Identity.Configs;

namespace VirtoCommerce.Web.Client.Services.Security
{
    public interface IUserIdentitySecurity
    {
        /// <summary>
        /// Logins the specified user name asynchronously.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        Task<SignInStatus> LoginAsync(string userName, string password, bool persistCookie = false);
        /// <summary>
        /// Special login case for customer service representative (CSR)
        /// </summary>
        /// <param name="userName">customer user name</param>
        /// <param name="csrUserName">CSR user name</param>
        /// <param name="password">CSR password</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns>error message</returns>
        Task<string> LoginAsAsync(string userName, string csrUserName, string password, bool persistCookie = false);
        /// <summary>
        /// Logouts current user.
        /// </summary>
        void Logout();
        /// <summary>
        /// Creates the user and account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="propertyValues">The property values.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns>Confirmation token if requeted.</returns>
        Task<string> CreateUserAndAccountAsync(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>user id in the membership provider</returns>
        Task<string> GetUserIdAsync(string userName);
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword);
        /// <summary>
        /// Resets the password. Should only be used by the super admin user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="token">Password reset token</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        Task<bool> ResetPasswordAsync(string userName, string newPassword, string token = null);
        /// <summary>
        /// Generates the password reset token that can be sent via email.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<string> GeneratePasswordResetTokenAsync(string userName);
        /// <summary>
        /// Resets the password by using a password reset token.
        /// </summary>
        /// <param name="resetToken">The reset token.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<bool> ResetPasswordWithTokenAsync(string resetToken, string newPassword, string userName);
        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        Task<IdentityResult> CreateAccountAsync(string userName, string password = null);
        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        Task<bool> DeleteUserAsync(string userName);
        /// <summary>
        /// Confirms the account.
        /// </summary>
        /// <param name="emailConfirmationToken">The email confirmation token.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        Task<bool> ConfirmAccountEmailAsync(string emailConfirmationToken, string userName);
    }
}
