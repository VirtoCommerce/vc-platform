using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Swashbuckle.Swagger.Annotations;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Platform.Data.Notifications;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Web.Model.Security;
using VirtoCommerce.Platform.Web.Resources;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    /// <summary>
    /// </summary>
    [RoutePrefix("api/platform/security")]
    public class SecurityController : ApiController
    {
        private readonly Func<IAuthenticationManager> _authenticationManagerFactory;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly IRoleManagementService _roleService;
        private readonly ISecurityService _securityService;
        private readonly ISecurityOptions _securityOptions;
        private readonly INotificationManager _notificationManager;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// </summary>
        public SecurityController(Func<ApplicationSignInManager> signInManagerFactory, Func<IAuthenticationManager> authManagerFactory,
                                  INotificationManager notificationManager,
                                  IRoleManagementService roleService, ISecurityService securityService, ISecurityOptions securityOptions, IEventPublisher eventPublisher)
        {
            _signInManagerFactory = signInManagerFactory;
            _authenticationManagerFactory = authManagerFactory;
            _roleService = roleService;
            _securityService = securityService;
            _securityOptions = securityOptions;
            _notificationManager = notificationManager;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <param name="model">User credentials.</param>
        [HttpPost]
        [Route("login")]
        [ResponseType(typeof(ApplicationUserExtended))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(UserLogin model)
        {
            var signInManager = _signInManagerFactory();
            var signInStatus = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

            if (signInStatus == SignInStatus.Success)
            {
                var user = await _securityService.FindByNameAsync(model.UserName, UserDetails.Full);

                // Rejected users and customers are not allowed to sign in
                if (user.UserState != AccountState.Rejected && !user.UserType.EqualsInvariant(AccountType.Customer.ToString()))
                {
                    await _eventPublisher.Publish(new UserLoginEvent(user));
                    return Ok(user);
                }
            }

            if (signInStatus == SignInStatus.RequiresVerification)
            {
                // TODO: Add UI for choosing a two factor provider, sending the code and verifying the entered code.
                // var userId = await signInManager.GetVerifiedUserIdAsync();
                // var providers = await signInManager.UserManager.GetValidTwoFactorProvidersAsync(userId);
                // var provider = providers.FirstOrDefault(); // User should choose a provider
                // var sendCodeResult = await signInManager.SendTwoFactorCodeAsync(provider);
                // var code = "123456"; // Get code from user
                // var twoFactorSignInStatus = await signInManager.TwoFactorSignInAsync(provider, code, false, false);
            }

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        /// <summary>
        /// Sign out
        /// </summary>
        [HttpPost]
        [Route("logout")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Logout()
        {
            var user = await _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Reduced);
            if (user != null)
            {
                _authenticationManagerFactory().SignOut();
                await _eventPublisher.Publish(new UserLogoutEvent(user));
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get current user details
        /// </summary>
        [HttpGet]
        [Route("currentuser")]
        [ResponseType(typeof(ApplicationUserExtended))]
        public async Task<IHttpActionResult> GetCurrentUser()
        {
            var user = await _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full);
            ApplyAuthorizationRulesForUser(user);
            return Ok(user);
        }

       
        /// <summary>
        /// Get user details by user email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/email/{email}")]
        [ResponseType(typeof(ApplicationUserExtended))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> GetUserByEmail(string email)
        { 
            var user = await _securityService.FindByEmailAsync(email, UserDetails.Export);
            ApplyAuthorizationRulesForUser(user);
            return Ok(user);
        }

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/login/external")]
        [ResponseType(typeof(ApplicationUserExtended))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> GetUserByLogin(string loginProvider, string providerKey)
        {
            var user = await _securityService.FindByLoginAsync(loginProvider, providerKey, UserDetails.Export);
            ApplyAuthorizationRulesForUser(user);
            return Ok(user);
        }



        /// <summary>
        /// Get all registered permissions
        /// </summary>
        [HttpGet]
        [Route("permissions")]
        [ResponseType(typeof(Permission[]))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult GetPermissions()
        {
            var result = _securityService.GetAllPermissions()
                .OrderBy(p => p.GroupName)
                .ThenBy(p => p.Name)
                .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Search roles by keyword
        /// </summary>
        /// <param name="request">Search parameters.</param>
        [HttpPost]
        [Route("roles")]
        [ResponseType(typeof(RoleSearchResponse))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult SearchRoles(RoleSearchRequest request)
        {
            var result = _roleService.SearchRoles(request);
            return Ok(result);
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <param name="roleId"></param>
        [HttpGet]
        [Route("roles/{roleId}")]
        [ResponseType(typeof(Role))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult GetRole(string roleId)
        {
            var result = _roleService.GetRole(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <param name="roleIds">An array of role IDs.</param>
        [HttpDelete]
        [Route("roles")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityDelete)]
        public IHttpActionResult DeleteRoles([FromUri(Name = "ids")] string[] roleIds)
        {
            if (roleIds != null)
            {
                foreach (var roleId in roleIds)
                {
                    _roleService.DeleteRole(roleId);
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add a new role or update an existing role
        /// </summary>
        /// <param name="role"></param>
        [HttpPut]
        [Route("roles")]
        [ResponseType(typeof(Role))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public IHttpActionResult UpdateRole(Role role)
        {
            var result = _roleService.AddOrUpdateRole(role);
            return Ok(result);
        }

        /// <summary>
        /// Generate new API account
        /// </summary>
        /// <remarks>
        /// Generates new account but does not save it.
        /// </remarks>
        /// <param name="type"></param>
        [HttpGet]
        [Route("apiaccounts/new")]
        [ResponseType(typeof(ApiAccount))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public IHttpActionResult GenerateNewApiAccount(ApiAccountType type)
        {
            var result = _securityService.GenerateNewApiAccount(type);
            result.IsActive = null;
            return Ok(result);
        }

        /// <summary>
        /// Generate new API key for specified account
        /// </summary>
        /// <remarks>
        /// Generates new key for specified account but does not save it.
        /// </remarks>
        [HttpPut]
        [Route("apiaccounts/newKey")]
        [ResponseType(typeof(ApiAccount))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(ApiAccount))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(string))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public IHttpActionResult GenerateNewApiKey(ApiAccount account)
        {
            if (account.ApiAccountType != ApiAccountType.Hmac)
            {
                return BadRequest(SecurityResources.NonHmacKeyGenerationException);
            }
            var retVal = _securityService.GenerateNewApiKey(account);
            return Ok(retVal);
        }

        /// <summary>
        /// Search users by keyword
        /// </summary>
        /// <param name="request">Search parameters.</param>
        [HttpPost]
        [Route("users")]
        [ResponseType(typeof(UserSearchResponse))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> SearchUsersAsync(UserSearchRequest request)
        {
            var result = await _securityService.SearchUsersAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <param name="userName"></param>
        [HttpGet]
        [Route("users/{userName}")]
        [ResponseType(typeof(ApplicationUserExtended))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> GetUserByName(string userName)
        {
            var user = await _securityService.FindByNameAsync(userName, UserDetails.Export);
            ApplyAuthorizationRulesForUser(user);
            return Ok(user);
        }

        /// <summary>
        /// Check specified user has passed permissions in specified scope
        /// </summary>
        /// <param name="userName">security account name</param>
        /// <param name="permissions">checked permissions Example: ?permissions=read&amp;permissions=write </param>
        /// <param name="scopes">security bounded scopes. Read mode: http://docs.virtocommerce.com/display/vc2devguide/Working+with+platform+security </param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/{userName}/hasPermissions")]
        [ResponseType(typeof(CheckPermissionsResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult UserHasAnyPermission(string userName, [FromUri] string[] permissions, [FromUri] string[] scopes)
        {
            return Ok(new CheckPermissionsResult { Result = _securityService.UserHasAnyPermission(userName, scopes, permissions) });
        }

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("users/id/{id}")]
        [ResponseType(typeof(ApplicationUserExtended))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> GetUserById(string id)
        {
            var user = await _securityService.FindByIdAsync(id, UserDetails.Export);
            ApplyAuthorizationRulesForUser(user);
            return Ok(user);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">User details.</param>
        [HttpPost]
        [Route("users/create")]
        [ResponseType(typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityCreate)]
        public async Task<IHttpActionResult> CreateAsync(ApplicationUserExtended user)
        {
            //ClearSecurityProperties(user);
            var result = await _securityService.CreateAsync(user);
            return Content(result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        [HttpPost]
        [Route("users/{userName}/changepassword")]
        [ResponseType(typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> ChangePassword(string userName, [FromBody] ChangePasswordInfo changePassword)
        {
            EnsureUserIsEditable(userName);

            var result = await _securityService.ChangePasswordAsync(userName, changePassword.OldPassword, changePassword.NewPassword);
            return Content(result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        [HttpPost]
        [Route("users/{userName}/resetpassword")]
        [ResponseType(typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public async Task<IHttpActionResult> ResetPassword(string userName, [FromBody] ResetPasswordInfo resetPassword)
        {
            EnsureUserIsEditable(userName);

            var result = await _securityService.ResetPasswordAsync(userName, resetPassword.NewPassword);
            return Content(result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Reset password by token
        /// </summary>
        [HttpPost]
        [Route("users/{userId}/resetpasswordconfirm")]
        [ResponseType(typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(SecurityResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPasswordByToken(string userId, [FromBody] ResetPasswordInfo resetPassword)
        {
            var result = await _securityService.ResetPasswordAsync(userId, resetPassword.Token, resetPassword.NewPassword);
            return Content(result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Validate password reset token
        /// </summary>
        [HttpPost]
        [Route("users/{userId}/validatepasswordresettoken")]
        [ResponseType(typeof(bool))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ValidatePasswordResetToken(string userId, [FromBody] ResetPasswordTokenInfo resetPasswordToken)
        {
            var result = await _securityService.ValidatePasswordResetTokenAsync(userId, resetPasswordToken.Token);
            return Ok(result);
        }

        /// <summary>
        /// Send email with instructions on how to reset user password.
        /// </summary>
        /// <remarks>
        /// Verifies provided userName and (if succeeded) sends email.
        /// </remarks>
        [HttpPost]
        [Route("users/{loginOrEmail}/requestpasswordreset")]
        [ResponseType(typeof(SecurityResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RequestPasswordReset(string loginOrEmail)
        {
            var retVal = new SecurityResult
            {
                // Return success by default for security reason
                Succeeded = true
            };

            try
            {
                var user = await _securityService.FindByNameAsync(loginOrEmail, UserDetails.Full)
                           ?? await _securityService.FindByEmailAsync(loginOrEmail, UserDetails.Full);

                // Do not permit rejected users and customers
                if (string.IsNullOrEmpty(user?.Email) || user.UserState == AccountState.Rejected || user.UserType.EqualsInvariant(AccountType.Customer.ToString()))
                {
                    if (ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:ForgotPassword:RevealAccountState", false))
                    {
                        retVal.Errors = new[] { "User with this name or email does not exist" };
                        retVal.Succeeded = false;
                    }
                }
                else
                {
                    EnsureUserIsEditable(user.UserName);

                    var uri = Request.RequestUri.AbsoluteUri;
                    uri = uri.Substring(0, uri.IndexOf("/api/platform/security/", StringComparison.OrdinalIgnoreCase));

                    var token = await _securityService.GeneratePasswordResetTokenAsync(user.Id);

                    var sender = ConfigurationHelper.GetAppSettingsValue("VirtoCommerce:Notifications:Sender:Email")
                        ?? "noreply@" + Request.RequestUri.Host;

                    var notification = _notificationManager.GetNewNotification<ResetPasswordEmailNotification>("Platform", typeof(ResetPasswordEmailNotification).Name, "en-US");
                    notification.Url = $"{uri}/#/resetpassword/{user.Id}/{token}";
                    notification.Recipient = user.Email;
                    notification.Sender = sender;

                    try
                    {
                        _notificationManager.ScheduleSendNotification(notification);
                    }
                    catch (Exception ex)
                    {
                        // Display errors only when sending notifications fails
                        retVal.Errors = new[] { ex.Message };
                        retVal.Succeeded = false;
                    }
                }
            }
            catch
            {
                // No details for security reasons
            }

            return Ok(retVal);
        }

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <param name="user">User details.</param>
        [HttpPut]
        [Route("users")]
        [ResponseType(typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public async Task<IHttpActionResult> UpdateAsync(ApplicationUserExtended user)
        {
            EnsureUserIsEditable(user.UserName);

            //ClearSecurityProperties(user);
            var result = await _securityService.UpdateAsync(user);
            return Content(result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        }

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <param name="names">An array of user names.</param>
        [HttpDelete]
        [Route("users")]
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityDelete)]
        public async Task<IHttpActionResult> DeleteAsync([FromUri] string[] names)
        {
            EnsureUserIsEditable(names);

            await _securityService.DeleteAsync(names);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Checks if user locked
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/{id}/locked")]
        [ResponseType(typeof(UserLockedResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> IsUserLockedAsync(string id)
        {
            var result = await _securityService.IsUserLockedAsync(id);
            return Ok(new UserLockedResult(result));
        }

        /// <summary>
        /// Unlock user
        /// </summary>
        /// <param name="id">>User id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("users/{id}/unlock")]
        [ResponseType(typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SecurityResult))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public async Task<IHttpActionResult> UnlockUserAsync(string id)
        {
            var result = await _securityService.UnlockUserAsync(id);
            return Content(result.Succeeded ? HttpStatusCode.OK : HttpStatusCode.BadRequest, result);
        }

        private void ApplyAuthorizationRulesForUser(ApplicationUserExtended user)
        {
            if (user != null && !_securityService.UserHasAnyPermission(User.Identity.Name, null, new[] { PredefinedPermissions.SecurityApiAccountsRead }))
            {
                user.ApiAccounts = null;
            }
        }

        private void EnsureUserIsEditable(params string[] userNames)
        {
            if (_securityOptions?.NonEditableUsers != null)
            {
                if (userNames.Any(x => _securityOptions.NonEditableUsers.Contains(x)))
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, "It is forbidden to edit this user.");
                }
            }
        }

        private static void ClearSecurityProperties(ApplicationUserExtended user)
        {
            if (user != null)
            {
                user.PasswordHash = null;
                user.SecurityStamp = null;
            }
        }
    }
}
