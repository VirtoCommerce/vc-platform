using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Extensions;

namespace VirtoCommerce.Web.Services
{
    public class SecurityService
    {
        private SecurityClient _securityClient;
        private SecurityClient SecurityClient
        {
            get
            {
                return this._securityClient ?? (this._securityClient = ClientContext.Clients.CreateSecurityClient());
            }
        }

        public ClaimsIdentity CreateClaimsIdentity(string userName)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));

            var identity = new ClaimsIdentity(claims, Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

            return identity;
        }

        public async Task<SignInStatus> PasswordSingInAsync(
            string username, string password, bool isPersistent)
        {
            return await this.SecurityClient.PasswordSignInAsync(username, password);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await this.SecurityClient.CreateUserAsync(user);
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return await this.SecurityClient.FindUserByNameAsync(userName);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await this.SecurityClient.FindUserByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByLoginAsync(UserLoginInfo loginInfo)
        {
            return await this.SecurityClient.FindUserByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        }

        public async Task GenerateResetPasswordTokenAsync(string userId, string storeName, string callbakUrl)
        {
            await this.SecurityClient.GenerateResetPasswordTokenAsync(userId, storeName, callbakUrl);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            return await this.SecurityClient.ResetPasswordAsync(userId, token, newPassword);
        }
    }
}