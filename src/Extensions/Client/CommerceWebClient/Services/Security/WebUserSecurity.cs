using Microsoft.WindowsAzure;
using System;
using System.Linq;
using System.Threading;
using System.Web.Security;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using WebMatrix.WebData;

namespace VirtoCommerce.Web.Client.Services.Security
{
    /// <summary>
    /// Class WebUserSecurity.
    /// </summary>
    public class WebUserSecurity : IUserSecurity
    {
        /// <summary>
        /// The _security repository
        /// </summary>
        private readonly ISecurityRepository _securityRepository;
        /// <summary>
        /// The _initializer
        /// </summary>
        private static SimpleMembershipInitializer _initializer;
        /// <summary>
        /// The _initializer lock
        /// </summary>
        private static object _initializerLock = new object();
        /// <summary>
        /// The _is initialized
        /// </summary>
        private static bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebUserSecurity"/> class.
        /// </summary>
        /// <param name="securityRepository">The security repository.</param>
        public WebUserSecurity(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        /// <summary>
        /// Logins the specified user name.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool Login(string userName, string password, bool persistCookie = false)
        {
            //Try to get account and check account state
            //If account is not found or state is not Approved - user cannot login
            var account = _securityRepository.Accounts.FirstOrDefault(
                a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (account == null
                || !account.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                return false;
            }
            return WebSecurity.Login(userName, password, persistCookie);
        }

        /// <summary>
        /// Special login case for customer service representative (CSR)
        /// </summary>
        /// <param name="userName">customer user name</param>
        /// <param name="csrUserName">CSR user name</param>
        /// <param name="password">CSR password</param>
        /// <param name="errrorMessage"></param>
        /// <param name="persistCookie">if set to <c>true</c> [persist cookie].</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool LoginAs(string userName, string csrUserName, string password, out string errrorMessage, bool persistCookie = false)
        {
            errrorMessage = null;
            var csrAccount = _securityRepository.Accounts.FirstOrDefault(
                a => a.UserName.Equals(csrUserName, StringComparison.OrdinalIgnoreCase));

            if (!Membership.ValidateUser(csrUserName, password))
            {
                errrorMessage = "CSR user name or password incorrect.";
                return false;
            }

            if (csrAccount == null
            || !csrAccount.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                errrorMessage = "CSR account is not valid.";
                return false;
            }

            if (csrAccount.RegisterType != (int) RegisterType.Administrator)
            {
                //Check if CSR has permission to login as
                var hasPermission = false;

                foreach (
                    var assignment in
                        _securityRepository.RoleAssignments.Where(x => x.AccountId == csrAccount.AccountId)
                            .Expand("Role/RolePermissions"))
                {
                    hasPermission = assignment.Role != null && assignment.Role.RolePermissions.Any(p =>
                        p.PermissionId.Equals(PredefinedPermissions.CustomersLoginAsCustomer,
                            StringComparison.OrdinalIgnoreCase));
                    if (hasPermission)
                    {
                        break;
                    }
                }

                if (!hasPermission)
                {
                    errrorMessage = "CSR has no permission to login as other user.";
                    return false;
                }
            }

            //Check user account
            var account = _securityRepository.Accounts.FirstOrDefault(
               a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (account == null
                || !account.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                errrorMessage = "User account is not valid";
                return false;
            }

            //Authenticate user
            FormsAuthentication.SetAuthCookie(userName, persistCookie);

            return true;
        }

        /// <summary>
        /// Logouts this user.
        /// </summary>
        public void Logout()
        {
            WebSecurity.Logout();
        }

        /// <summary>
        /// Creates the user and account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="propertyValues">The property values.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns>System.String.</returns>
        public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>
        /// user id in the membership provider
        /// </returns>
        public string GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName).ToString();
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="currentPassword">The current password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }

        /// <summary>
        /// Resets the password. Should only be used by the super admin user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <param name="resetToken">Password reset token</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool ResetPassword(string userName, string newPassword, string resetToken = null)
        {
            resetToken = resetToken ?? WebSecurity.GeneratePasswordResetToken(userName);
            return WebSecurity.ResetPassword(resetToken, newPassword);
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="requireConfirmationToken">if set to <c>true</c> [require confirmation token].</param>
        /// <returns>System.String.</returns>
        public string CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool DeleteUser(string userName)
        {
            return ((SimpleMembershipProvider)Membership.Provider).DeleteUser(userName, true);
        }

        /// <summary>
        /// Class SimpleMembershipInitializer.
        /// </summary>
        private class SimpleMembershipInitializer
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="SimpleMembershipInitializer"/> class.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588</exception>
            public SimpleMembershipInitializer()
            {
                try
                {
                    if (WebSecurity.Initialized)
                        return;

                    var nameOrConnectionString = SecurityConfiguration.Instance.Connection.SqlConnectionStringName;
                    var settingValue = CloudConfigurationManager.GetSetting(nameOrConnectionString);

                    if (String.IsNullOrEmpty(settingValue))
                    {
                        WebSecurity.InitializeDatabaseConnection(
                            nameOrConnectionString,
                            "Account", "AccountId", "UserName", autoCreateTables: false);
                    }
                    else
                    {
                        WebSecurity.InitializeDatabaseConnection(
                            settingValue,
                            "System.Data.SqlClient",
                            "Account", "AccountId", "UserName", autoCreateTables: false);
                    }

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}