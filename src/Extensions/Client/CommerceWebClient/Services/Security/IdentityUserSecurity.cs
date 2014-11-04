using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Client.Security.Identity.Configs;
using VirtoCommerce.Web.Client.Security.Identity.Model;

namespace VirtoCommerce.Web.Client.Services.Security
{
    public class IdentityUserSecurity : IUserIdentitySecurity
    {
        private IAuthenticationManager _authenticationManager;
        private readonly ISecurityRepository _securityRepository;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [InjectionConstructor]
        public IdentityUserSecurity(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        public IdentityUserSecurity(ApplicationUserManager userManager, 
            ApplicationSignInManager signInManager, 
            IAuthenticationManager authenticationManager, 
            ISecurityRepository securityRepository)
            : this(securityRepository)
        {
            AuthenticationManager = authenticationManager;
            UserManager = userManager;
            SignInManager = signInManager;
        }



        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? HttpContext.Current.GetOwinContext().Authentication;
            }
            private set { _authenticationManager = value; }
        }


        public async Task<SignInStatus> LoginAsync(string userName, string password, bool persistCookie = false)
        {
            return await SignInManager.PasswordSignInAsync(userName, password, persistCookie, true);
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return LoginAsync(userName, password, persistCookie).Result == SignInStatus.Success;
        }

        public async Task<string> LoginAsAsync(string userName, string csrUserName, string password, bool persistCookie = false)
        {
            var csrAccount = _securityRepository.Accounts.FirstOrDefault(
                a => a.UserName.Equals(csrUserName, StringComparison.OrdinalIgnoreCase));

            var appUser = await UserManager.FindByNameAsync(csrUserName);

            if (!await UserManager.CheckPasswordAsync(appUser, password))
            {
                return "CSR user name or password incorrect.";

            }

            if (csrAccount == null
            || !csrAccount.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                return "CSR account is not valid.";
            }

            if (csrAccount.RegisterType != (int)RegisterType.Administrator)
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
                    return "CSR has no permission to login as other user.";
                }
            }

            //Check user account
            var account = _securityRepository.Accounts.FirstOrDefault(
               a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            var loginAsUser = await UserManager.FindByNameAsync(userName);

            if (account == null || loginAsUser == null
                || !account.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                return "User account is not valid";
            }

            //Authenticate user
            await SignInManager.SignInAsync(loginAsUser, persistCookie, false);

            return null;
        }

        public string LoginAs(string userName, string csrUserName, string password, bool persistCookie = false)
        {
            return AsyncHelper.RunSync(() => LoginAsAsync(userName, csrUserName, password, persistCookie));
        }

        public void Logout()
        {
            AuthenticationManager.SignOut();
        }

        public async Task<string> CreateUserAndAccountAsync(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {

            var account = _securityRepository.Accounts.FirstOrDefault(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            if (account != null)
            {
                throw new InvalidOperationException(string.Format("username {0} already taken", userName));
            }

            IDictionary<string, object> values = propertyValues as RouteValueDictionary;

            if (values == null && propertyValues != null)
            {
                var propertyValuesAsDictionary = propertyValues as IDictionary<string, object>;
                values = propertyValuesAsDictionary != null ? new RouteValueDictionary(propertyValuesAsDictionary)
                                                            : new RouteValueDictionary(propertyValues);
            }

            account = new Account
            {
                UserName = userName
            };

            if (values != null)
            {
                foreach (var prop in account.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => values.ContainsKey(prop.Name)))
                {
                    prop.SetValue(account, values[prop.Name]);
                }
            }

            _securityRepository.Add(account);
            _securityRepository.UnitOfWork.Commit();

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
                //TODO should change AccountId to string
                Id = account.AccountId.ToString(CultureInfo.InvariantCulture) 
            };

            var result = await UserManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (requireConfirmationToken)
                {
                    return await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                }
            }

            return null;
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues = null,
            bool requireConfirmationToken = false)
        {
            return AsyncHelper.RunSync(() => CreateUserAndAccountAsync(userName, password, propertyValues, requireConfirmationToken));
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            return user != null ? user.Id : null;
        }

        public string GetUserId(string userName)
        {
            return AsyncHelper.RunSync(() => GetUserIdAsync(userName));
        }

        public async Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await UserManager.ChangePasswordAsync(user.Id, currentPassword, newPassword);
                return result.Succeeded;
            }
            return false;
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return AsyncHelper.RunSync(() => ChangePasswordAsync(userName, currentPassword, newPassword));
        }

        public async Task<bool> ResetPasswordAsync(string userName, string newPassword, string token = null)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                token = token ?? await GeneratePasswordResetTokenAsync(userName);
                var result = await UserManager.ResetPasswordAsync(user.Id, token, newPassword);
                return result.Succeeded;
            }
            return false;
        }

        public bool ResetPassword(string userName, string newPassword, string token = null)
        {
            return AsyncHelper.RunSync(() => ResetPasswordAsync(userName, newPassword, token));
        }

        public async Task<string> GeneratePasswordResetTokenAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            var result = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            return result;
        }
     
        public string GeneratePasswordResetToken(string userName)
        {
            return AsyncHelper.RunSync(() => GeneratePasswordResetTokenAsync(userName));
        }

        public async Task<bool> ResetPasswordWithTokenAsync(string resetToken, string newPassword, string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                var result = await UserManager.ResetPasswordAsync(user.Id, resetToken, newPassword);
                return result.Succeeded;
            }
            return false;
        }

        public bool ResetPasswordWithToken(string resetToken, string newPassword, string userName = null)
        {
            return AsyncHelper.RunSync(() => ResetPasswordWithTokenAsync(resetToken, newPassword, userName));
        }

        public async Task<IdentityResult> CreateAccountAsync(string userName, string password = null)
        {
            var account = _securityRepository.Accounts.FirstOrDefault(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName
            };

            if (account != null)
            {
                //Need to connect account with login
                user.Id = account.AccountId.ToString(CultureInfo.InvariantCulture);
            }

            IdentityResult result;
            if (string.IsNullOrWhiteSpace(password))
            {
                result = await UserManager.CreateAsync(user);
            }
            else
            {
                result = await UserManager.CreateAsync(user, password);
            }
           
            return result;
        }

        public string CreateAccount(string userName, string password, bool requireConfirmation = false)
        {
           AsyncHelper.RunSync(() => CreateAccountAsync(userName, password));
           return null;
        }

        public async Task<bool> DeleteUserAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            var result = await UserManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public bool DeleteUser(string userName)
        {
            return AsyncHelper.RunSync(() => DeleteUserAsync(userName));
        }

        public async Task<bool> ConfirmAccountEmailAsync(string emailConfirmationToken, string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            var result = await UserManager.ConfirmEmailAsync(user.Id, emailConfirmationToken);
            return result.Succeeded;
        }

        public bool ConfirmAccountEmail(string emailConfirmationToken, string userName)
        {
            return AsyncHelper.RunSync(() => ConfirmAccountEmailAsync(emailConfirmationToken, userName));
        }
    }
}
