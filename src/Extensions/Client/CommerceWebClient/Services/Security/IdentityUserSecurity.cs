using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Web.Client.Security.Identity.Configs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VirtoCommerce.Web.Client.Security.Identity.Model;

namespace VirtoCommerce.Web.Client.Services.Security
{
    public class IdentityUserSecurity : IUserSecurity
    {
        private readonly ISecurityRepository _securityRepository;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [InjectionConstructor]
        public IdentityUserSecurity(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        public IdentityUserSecurity(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ISecurityRepository securityRepository)
            : this(securityRepository)
        {
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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }


        public bool Login(string userName, string password, bool persistCookie = false)
        {
            var result = SignInManager.PasswordSignIn(userName, password, persistCookie, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return true;
                default:
                    return false;
            }
        }

        public bool LoginAs(string userName, string csrUserName, string password, out string errorMessage, bool persistCookie = false)
        {
            errorMessage = null;
            var csrAccount = _securityRepository.Accounts.FirstOrDefault(
                a => a.UserName.Equals(csrUserName, StringComparison.OrdinalIgnoreCase));

            var appUser = UserManager.FindByName(csrUserName);

            if (!UserManager.CheckPassword(appUser, password))
            {
                errorMessage = "CSR user name or password incorrect.";
                return false;
            }

            if (csrAccount == null
            || !csrAccount.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                errorMessage = "CSR account is not valid.";
                return false;
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
                    errorMessage = "CSR has no permission to login as other user.";
                    return false;
                }
            }

            //Check user account
            var account = _securityRepository.Accounts.FirstOrDefault(
               a => a.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

            var loginAsUser = UserManager.FindByName(userName);

            if (account == null || loginAsUser == null
                || !account.AccountState.GetHashCode().Equals(AccountState.Approved.GetHashCode()))
            {
                errorMessage = "User account is not valid";
                return false;
            }

            //Authenticate user
            SignInManager.SignIn(loginAsUser, persistCookie, false);

            return true;
        }

        public void Logout()
        {
            AuthenticationManager.SignOut();
        }

        public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
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

            var result = UserManager.Create(user, password);
            if (result.Succeeded)
            {
                if (requireConfirmationToken)
                {
                    return UserManager.GenerateEmailConfirmationToken(user.Id);
                }
            }

            return null;
        }

        public string GetUserId(string userName)
        {
            var user = UserManager.FindByName(userName);
            return user != null ? user.Id : null;
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            var user = UserManager.FindByName(userName);
            if (user != null)
            {
                var result = UserManager.ChangePassword(user.Id, currentPassword, newPassword);
                return result.Succeeded;
            }
            return false;
        }

        public bool ResetPassword(string userName, string newPassword, string token = null)
        {
            var user = UserManager.FindByName(userName);
            if (user != null)
            {
                token = token ?? GeneratePasswordResetToken(userName);
                var result = UserManager.ResetPassword(user.Id, token, newPassword);
                return result.Succeeded;
            }
            return false;
        }

        public string GeneratePasswordResetToken(string userName)
        {
            var user = UserManager.FindByName(userName);
            var result = UserManager.GeneratePasswordResetToken(user.Id);
            return result;
        }

        public bool ResetPasswordWithToken(string resetToken, string newPassword, string userName = null)
        {
            var user = UserManager.FindByName(userName);
            if (user != null)
            {
                var result = UserManager.ResetPassword(user.Id, resetToken, newPassword);
                return result.Succeeded;
            }
            return false;
        }

        public string CreateAccount(string userName, string password, bool requireConfirmationToken = false)
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

            var result = UserManager.Create(user, password);
            if (result.Succeeded)
            {
                if (requireConfirmationToken)
                {
                    return UserManager.GenerateEmailConfirmationToken(user.Id);
                }
            }

            return null;
        }

        public bool DeleteUser(string userName)
        {
            var user = UserManager.FindByName(userName);
            var result = UserManager.Delete(user);
            return result.Succeeded;
        }

        public bool ConfirmAccountEmail(string emailConfirmationToken, string userName)
        {
            var user = UserManager.FindByName(userName);
            var result = UserManager.ConfirmEmail(user.Id, emailConfirmationToken);
            return result.Succeeded;
        }
    }
}
