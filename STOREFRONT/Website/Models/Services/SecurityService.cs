using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.ApiClient.Extensions;

namespace VirtoCommerce.Web.Models.Services
{
    public class SecurityService
    {
        private SecurityClient _securityClient
        {
            get
            {
                return ClientContext.Clients.CreateSecurityClient();
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
            return await _securityClient.PasswordSignInAsync(username, password);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await _securityClient.CreateUserAsync(user);
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return await _securityClient.FindUserByNameAsync(userName);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _securityClient.FindUserByIdAsync(userId);
        }

        public async Task<ApplicationUser> GetUserByLoginAsync(UserLoginInfo loginInfo)
        {
            return await _securityClient.FindUserByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
        }

        public async Task GenerateResetPasswordTokenAsync(string userId, string storeName, string callbakUrl)
        {
            await _securityClient.GenerateResetPasswordTokenAsync(userId, storeName, callbakUrl);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            return await _securityClient.ResetPasswordAsync(userId, token, newPassword);
        }
    }
}