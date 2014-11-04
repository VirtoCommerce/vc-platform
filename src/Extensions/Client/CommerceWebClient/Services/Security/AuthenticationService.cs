using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Security.Swt;

namespace VirtoCommerce.Web.Client.Services.Security
{
    /// <summary>
    /// Class AuthenticationService.
    /// </summary>
    [ServiceBehavior(Namespace = "http://schemas.virtocommerce.com/1.0/authentication/")]
    [UnityInstanceProviderServiceBehaviorAttribute]
    public class AuthenticationService : IAuthenticationService
    {
        #region Private Methods
        /// <summary>
        /// The membership provider
        /// </summary>
        protected IUserIdentitySecurity MembershipProvider;
        /// <summary>
        /// The security repository
        /// </summary>
        protected ISecurityRepository SecurityRepository;
        #endregion

        #region ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="membershipProvider">The membership provider.</param>
        public AuthenticationService(ISecurityRepository repository, IdentityUserSecurity membershipProvider)
        {
            MembershipProvider = membershipProvider;
            SecurityRepository = repository;
        }
        #endregion

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scope">The scope.</param>
        /// <returns>System.String.</returns>
        public async Task<string> AuthenticateUserAsync(string userName, string password, Uri scope)
        {
            string token = null;

            if (await MembershipProvider.LoginAsync(userName, password) == SignInStatus.Success)
            {
                Account account;
                // now check authorization, only administrators and site administrators can access this service                
                var isAuthorized = IsAuthorized(userName, out account, RegisterType.Administrator, RegisterType.SiteAdministrator);

                if (isAuthorized) // create claims
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, account.MemberId),
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(SecurityClaims.AccountRegistrationType,
                                      ((RegisterType)account.RegisterType).ToString())
                        };

                    // now lets get permissions
                    var permissions = GetAllMemberPermissions(account.MemberId);

                    if (permissions.Any())
                    {
                        claims.AddRange(permissions.Select(x => new Claim(SecurityClaims.AccountPermission, x)));
                    }

                    token = IssueToken(scope, claims.ToArray());
                }
            }

            return token;
        }

        /// <summary>
        /// Issues the token.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="claims">The claims.</param>
        /// <returns>System.String.</returns>
        private string IssueToken(Uri scope, params Claim[] claims)
        {
            var tokenIssuer = SecurityConfiguration.Instance.TokenIssuer;
            if (tokenIssuer != null)
            {
                return SimpleWebToken.Create(tokenIssuer.Uri, scope, DateTime.UtcNow + tokenIssuer.Lifetime,
                                             tokenIssuer.SignatureKey, claims);
            }
            return "";
        }

        /// <summary>
        /// Determines whether the specified user name is authorized.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="account">The account.</param>
        /// <param name="types">The types.</param>
        /// <returns><c>true</c> if the specified user name is authorized; otherwise, <c>false</c>.</returns>
        public bool IsAuthorized(string userName, out Account account, params RegisterType[] types)
        {
            var isAuthorized = true;

            var usr = GetAccountByUserName(userName, types);

            if (usr == null)
            {
                isAuthorized = false;
            }
            else // check if user is approved and can access the store
            {
                // check state
                if (usr.AccountState != AccountState.Approved.GetHashCode())
                {
                    isAuthorized = false;
                }
            }

            account = null;
            if (isAuthorized)
            {
                account = usr;
            }

            return isAuthorized;
        }

        #region Helper methods
        /// <summary>
        /// Gets the name of the account by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="types">The types.</param>
        /// <returns>Account.</returns>
        public Account GetAccountByUserName(string userName, params RegisterType[] types)
        {
            var typesInt = new int[] { };
            if (types != null)
            {
                typesInt = (from t in types select t.GetHashCode()).ToArray();
            }

            var account = SecurityRepository.Accounts.FirstOrDefault(x => x.UserName == userName && (typesInt.Count() == 0 || typesInt.Contains(x.RegisterType)));

            return account;
        }

        /// <summary>
        /// Gets all member permissions.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>System.String[][].</returns>
        public string[] GetAllMemberPermissions(string memberId)
        {
            var repo = SecurityRepository;

            var permissions = (from a in repo.Accounts 
                             join r in repo.RoleAssignments on a.AccountId equals r.AccountId
                             join p in repo.RolePermissions on r.RoleId equals p.RoleId
                             where a.MemberId == memberId
                             select p.PermissionId).Distinct().ToArray();

            return permissions;
        }


        /// <summary>
        /// Gets all member roles.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <returns>Role[][].</returns>
        public Role[] GetAllMemberRoles(string memberId)
        {
            return SecurityRepository.Accounts
                .Where(x => x.MemberId == memberId)
                .SelectMany(x => x.RoleAssignments)
                .Select(x => x.Role)
                .Distinct()
                .ToArray();
        }
        #endregion
    }
}
