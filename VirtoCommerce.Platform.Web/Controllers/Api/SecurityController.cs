﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
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

        /// <summary>
        /// </summary>
        public SecurityController(Func<ApplicationSignInManager> signInManagerFactory, Func<IAuthenticationManager> authManagerFactory,
                                  INotificationManager notificationManager,
                                  IRoleManagementService roleService, ISecurityService securityService, ISecurityOptions securityOptions)
        {
            _signInManagerFactory = signInManagerFactory;
            _authenticationManagerFactory = authManagerFactory;
            _roleService = roleService;
            _securityService = securityService;
            _securityOptions = securityOptions;
            _notificationManager = notificationManager;
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
                var retVal = await _securityService.FindByNameAsync(model.UserName, UserDetails.Full);
                //Do not allow login to admin customers and rejected users
                if (retVal.UserState != AccountState.Rejected && !string.Equals(retVal.UserType, AccountType.Customer.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(retVal);
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
        public IHttpActionResult Logout()
        {
            _authenticationManagerFactory().SignOut();
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
            return Ok(await _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full));
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
        [ResponseType(typeof(void))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public IHttpActionResult GenerateNewApiKey(ApiAccount account)
        {
            if (account.ApiAccountType != ApiAccountType.Hmac)
            {
                return BadRequest(SecurityResources.NonHmacKeyGenerationException);
            }
            var retVal =_securityService.GenerateNewApiKey(account);
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
            var retVal = await _securityService.FindByNameAsync(userName, UserDetails.Full);
            return Ok(retVal);
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
            var retVal = await _securityService.FindByIdAsync(id, UserDetails.Full);
            return Ok(retVal);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">User details.</param>
        [HttpPost]
        [Route("users/create")]
        [ResponseType(typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityCreate)]
        public async Task<IHttpActionResult> CreateAsync(ApplicationUserExtended user)
        {
            ClearSecurityProperties(user);
            var result = await _securityService.CreateAsync(user);
            return ProcessSecurityResult(result);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        [HttpPost]
        [Route("users/{userName}/changepassword")]
        [ResponseType(typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> ChangePassword(string userName, [FromBody] ChangePasswordInfo changePassword)
        {
            EnsureThatUsersEditable(userName);

            var result = await _securityService.ChangePasswordAsync(userName, changePassword.OldPassword, changePassword.NewPassword);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resetPassword">New password.</param>
        [HttpPost]
        [Route("users/{userName}/resetpassword")]
        [ResponseType(typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public async Task<IHttpActionResult> ResetPassword(string userName, [FromBody] ResetPasswordInfo resetPassword)
        {
            EnsureThatUsersEditable(userName);

            var result = await _securityService.ResetPasswordAsync(userName, resetPassword.NewPassword);
            return ProcessSecurityResult(result);
        }

        /// <summary>
        /// Reset password by token
        /// </summary>
        [HttpPost]
        [Route("users/{userId}/resetpasswordconfirm")]
        [ResponseType(typeof(SecurityResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPasswordByToken(string userId, [FromBody] ResetPasswordInfo resetPassword)
        {
            var result = await _securityService.ResetPasswordAsync(userId, resetPassword.Token, resetPassword.NewPassword);
            return ProcessSecurityResult(result);
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
                //Return success by default for security reason
                Succeeded = true
            };

            try
            {
                var user = await _securityService.FindByNameAsync(loginOrEmail, UserDetails.Full);
                if(user == null)
                {
                    user = await _securityService.FindByEmailAsync(loginOrEmail, UserDetails.Full);
                }

                //Do not permit rejected users and customers
                if (user.Email != null && user != null && user.UserState != AccountState.Rejected && !string.Equals(user.UserType, AccountType.Customer.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    EnsureThatUsersEditable(user.UserName);                    

                    var uri = Request.RequestUri.AbsoluteUri;
                    uri = uri.Substring(0, uri.IndexOf("/api/platform/security/"));
                    var token = await _securityService.GeneratePasswordResetTokenAsync(user.Id);

                    var notification = _notificationManager.GetNewNotification<ResetPasswordEmailNotification>("Platform", typeof(ResetPasswordEmailNotification).Name, "en");
                    notification.Url = $"{uri}/#/resetpassword/{user.Id}/{token}";
                    notification.Recipient = user.Email;
                    notification.Sender = "noreply@" + Request.RequestUri.Host;
                    try
                    {
                        var result = _notificationManager.SendNotification(notification);
                        retVal.Succeeded = result.IsSuccess;
                        if (!retVal.Succeeded)
                        {
                            retVal.Errors = new string[] { result.ErrorMessage };
                        }
                    }
                    catch (Exception ex)
                    {
                        //Display errors only when sending notifications fail
                        retVal.Errors = new string[] { ex.Message };
                        retVal.Succeeded = false;
                    }
                }
            }
            catch { } // no details in here for security reasons

            return Ok(retVal);
        }

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <param name="user">User details.</param>
        [HttpPut]
        [Route("users")]
        [ResponseType(typeof(SecurityResult))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public async Task<IHttpActionResult> UpdateAsync(ApplicationUserExtended user)
        {
            EnsureThatUsersEditable(user.UserName);

            ClearSecurityProperties(user);
            var result = await _securityService.UpdateAsync(user);
            return ProcessSecurityResult(result);
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
            EnsureThatUsersEditable(names);

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
        [CheckPermission(Permission = PredefinedPermissions.SecurityUpdate)]
        public async Task<IHttpActionResult> UnlockUserAsync(string id)
        {
            var result = await _securityService.UnlockUserAsync(id);
            return ProcessSecurityResult(result);
        }


        private void EnsureThatUsersEditable(params string[] userNames)
        {
            if (_securityOptions != null && _securityOptions.NonEditableUsers != null)
            {
                if (userNames.Any(x => _securityOptions.NonEditableUsers.Contains(x)))
                {
                    throw new HttpException((int)HttpStatusCode.InternalServerError, "It is forbidden to edit this user.");
                }
            }

        }

        private void ClearSecurityProperties(ApplicationUserExtended user)
        {
            if (user != null)
            {
                user.PasswordHash = null;
                user.SecurityStamp = null;
            }
        }

        private IHttpActionResult ProcessSecurityResult(SecurityResult securityResult)
        {
            IHttpActionResult result;

            if (securityResult == null)
            {
                result = BadRequest();
            }
            else
            {
                if (!securityResult.Succeeded)
                    result = BadRequest(securityResult.Errors != null ? string.Join(" ", securityResult.Errors) : "Unknown error.");
                else
                    result = Ok(securityResult);
            }

            return result;
        }
    }
}
