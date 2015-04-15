using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Web.Models.FormModels;
using VirtoCommerce.Web.Models.Security;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models.Services
{
    public class SecurityService
    {
        private const string XsrfKey = "XsrfId";

        #region Fields
        private readonly IAuthenticationManager _authManager;

        private readonly ApplicationSignInManager _signInManager;

        private readonly ApplicationUserManager _userManager;
        #endregion

        #region Constructors and Destructors
        public SecurityService(HttpContextBase httpContext)
        {
            this._signInManager = httpContext.GetOwinContext().Get<ApplicationSignInManager>();
            this._userManager = httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            this._authManager = httpContext.GetOwinContext().Authentication;
        }
        #endregion

        #region Public Methods and Operators
        public async Task<string[]> Login(string username, string password)
        {
            string[] errors = null;

            var loginResult = await this._signInManager.PasswordSignInAsync(username, password, true, false);

            switch (loginResult)
            {
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                    errors = new string[] { "Invalid login attempt" };
                    break;
            }

            return errors;
        }

        public void Logout()
        {
            this._authManager.SignOut();
        }

        public async Task<string[]> RegisterUser(string email, string firstName, string lastName, string password, string storeId)
        {
            string[] errors = null;

            var user = new ApplicationUser
                       {
                           Email = email,
                           FullName =
                               string.Format("{0} {1}", firstName, lastName),
                           StoreId = storeId,
                           UserName = email
                       };

            var registerResult = string.IsNullOrEmpty(password) ?
                await this._userManager.CreateAsync(user) :
                await this._userManager.CreateAsync(user, password);

            if (!registerResult.Succeeded)
            {
                errors = registerResult.Errors.ToArray();
            }

            return errors;
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public void ExternalLogin(string loginProvider, string redirectUrl, string userId)
        {
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            if (!string.IsNullOrEmpty(userId))
            {
                properties.Dictionary[XsrfKey] = userId;
            }

            _authManager.Challenge(properties, loginProvider);
        }
        #endregion
    }
}