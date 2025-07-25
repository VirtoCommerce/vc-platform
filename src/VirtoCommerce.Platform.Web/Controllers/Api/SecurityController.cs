using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.Extensions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Security.Extensions;
using VirtoCommerce.Platform.Security.ExternalSignIn;
using VirtoCommerce.Platform.Web.Model.Security;
using VirtoCommerce.Platform.Web.Security;
using static OpenIddict.Abstractions.OpenIddictConstants;
using PlatformPermissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/security")]
    public class SecurityController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly Core.Security.AuthorizationOptions _securityOptions;
        private readonly UserOptionsExtended _userOptionsExtended;
        private readonly PasswordOptionsExtended _passwordOptions;
        private readonly PasswordLoginOptions _passwordLoginOptions;
        private readonly IdentityOptions _identityOptions;
        private readonly IPermissionsRegistrar _permissionsProvider;
        private readonly IUserSearchService _userSearchService;
        private readonly IRoleSearchService _roleSearchService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserApiKeyService _userApiKeyService;
        private readonly ILogger<SecurityController> _logger;
        private readonly IEnumerable<ExternalSignInProviderConfiguration> _externalSigninProviderConfigs;

        public SecurityController(
            SignInManager<ApplicationUser> signInManager,
            RoleManager<Role> roleManager,
            IPermissionsRegistrar permissionsProvider,
            IUserSearchService userSearchService,
            IRoleSearchService roleSearchService,
            IOptions<Core.Security.AuthorizationOptions> securityOptions,
            IOptions<UserOptionsExtended> userOptionsExtended,
            IOptions<PasswordOptionsExtended> passwordOptions,
            IOptions<PasswordLoginOptions> passwordLoginOptions,
            IOptions<IdentityOptions> identityOptions,
            IEventPublisher eventPublisher,
            IUserApiKeyService userApiKeyService,
            ILogger<SecurityController> logger,
            IEnumerable<ExternalSignInProviderConfiguration> externalSigninProviderConfigs)
        {
            _signInManager = signInManager;
            _securityOptions = securityOptions.Value;
            _userOptionsExtended = userOptionsExtended.Value;
            _passwordOptions = passwordOptions.Value;
            _passwordLoginOptions = passwordLoginOptions.Value ?? new PasswordLoginOptions();
            _identityOptions = identityOptions.Value;
            _permissionsProvider = permissionsProvider;
            _roleManager = roleManager;
            _userSearchService = userSearchService;
            _roleSearchService = roleSearchService;
            _eventPublisher = eventPublisher;
            _userApiKeyService = userApiKeyService;
            _logger = logger;
            _externalSigninProviderConfigs = externalSigninProviderConfigs;
        }

        private UserManager<ApplicationUser> UserManager => _signInManager.UserManager;

        private readonly string UserNotFound = "User not found.";
        private readonly string UserForbiddenToEdit = "It is forbidden to edit this user.";

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <remarks>
        /// Verifies provided credentials and if succeeded returns full user details, otherwise returns 401 Unauthorized.
        /// </remarks>
        /// <param name="request">Login request.</param>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<SignInResult>> Login([FromBody] LoginRequest request)
        {
            // Measure the duration of a succeeded response and delay subsequent failed responses to prevent timing attacks
            var delayedResponse = DelayedResponse.Create(nameof(SecurityController), nameof(Login));

            var user = await UserManager.FindByNameAsync(request.UserName);

            // Allows signin to back office by either username (login) or email if IdentityOptions.User.RequireUniqueEmail is True. 
            if (user == null && _identityOptions.User.RequireUniqueEmail)
            {
                user = await UserManager.FindByEmailAsync(request.UserName);
            }

            if (user == null)
            {
                await delayedResponse.FailAsync();
                return Ok(SignInResult.Failed);
            }

            await _eventPublisher.Publish(new BeforeUserLoginEvent(user));

            var loginResult = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);

            if (!loginResult.Succeeded)
            {
                await delayedResponse.FailAsync();
                return Ok(loginResult);
            }

            await SetLastLoginDate(user);
            await _eventPublisher.Publish(new UserLoginEvent(user));

            //Do not allow login to admin customers and rejected users
            if (await UserManager.IsInRoleAsync(user, PlatformConstants.Security.SystemRoles.Customer))
            {
                loginResult = SignInResult.NotAllowed;
            }

            await delayedResponse.SucceedAsync();

            return Ok(loginResult);
        }

        /// <summary>
        /// Sign out
        /// </summary>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [Route("logout")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Logout()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                await _eventPublisher.Publish(new UserLogoutEvent(user));
            }

            return NoContent();
        }

        /// <summary>
        /// Get current user details
        /// </summary>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        [Route("currentuser")]
        public async Task<ActionResult<UserDetail>> GetCurrentUser()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Ok(new { });
            }

            var result = new UserDetail
            {
                Id = user.Id,
                isAdministrator = user.IsAdministrator,
                UserName = user.UserName,
                PasswordExpired = user.PasswordExpired,
                DaysTillPasswordExpiry = PasswordExpiryHelper.ContDaysTillPasswordExpiry(user, _userOptionsExtended),
                Permissions = user.Roles.SelectMany(x => x.Permissions).Select(x => x.Name).Distinct().ToArray(),
                AuthenticationMethod = HttpContext.User.GetAuthenticationMethod(),
                IsSsoAuthenticationMethod = HttpContext.User.IsExternalSignIn(),
                MemberId = user.MemberId,
            };

            // Password never expired with SSO
            if (result.IsSsoAuthenticationMethod)
            {
                result.PasswordExpired = false;
                result.DaysTillPasswordExpiry = -1;
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [Route("userinfo")]
        public async Task<ActionResult<Claim[]>> Userinfo()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new OpenIddictResponse
                {
                    Error = Errors.InvalidGrant,
                    ErrorDescription = "The user profile is no longer available."
                });
            }

            var claims = new JObject
            {
                //TODO: replace to PrinciplaClaims

                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await UserManager.GetUserIdAsync(user)
            };

            if (User.HasClaim(Claims.Scope, Scopes.Email))
            {
                claims[Claims.Email] = await UserManager.GetEmailAsync(user);
                claims[Claims.EmailVerified] = await UserManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasClaim(Claims.Scope, Scopes.Phone))
            {
                claims[Claims.PhoneNumber] = await UserManager.GetPhoneNumberAsync(user);
                claims[Claims.PhoneNumberVerified] = await UserManager.IsPhoneNumberConfirmedAsync(user);
            }

            if (User.HasClaim(Claims.Scope, Scopes.Roles))
            {
                claims["roles"] = JArray.FromObject(await UserManager.GetRolesAsync(user));
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Json(claims);
        }

        /// <summary>
        /// Get all registered permissions
        /// </summary>
        [HttpGet]
        [Route("permissions")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public ActionResult<Permission[]> GetAllRegisteredPermissions()
        {
            var result = _permissionsProvider.GetAllPermissions().ToArray();
            return Ok(result);
        }

        /// <summary>
        /// SearchAsync roles by keyword
        /// </summary>
        /// <param name="request">SearchAsync parameters.</param>
        [HttpPost]
        [Route("roles/search")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<RoleSearchResult>> SearchRoles([FromBody] RoleSearchCriteria request)
        {
            var result = await _roleSearchService.SearchRolesAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        /// <param name="roleName"></param>
        [HttpGet]
        [Route("roles/{roleName}")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<Role>> GetRole([FromRoute] string roleName)
        {
            var result = await _roleManager.FindByNameAsync(roleName);
            return Ok(result);
        }

        /// <summary>
        /// Delete roles by ID
        /// </summary>
        /// <param name="roleIds">An array of role IDs.</param>
        [HttpDelete]
        [Route("roles")]
        [Authorize(PlatformPermissions.SecurityDelete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteRoles([FromQuery(Name = "ids")] string[] roleIds)
        {
            if (roleIds != null)
            {
                foreach (var roleId in roleIds)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    if (role != null)
                    {
                        await _roleManager.DeleteAsync(role);
                    }
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Update an existing role or create new
        /// </summary>
        /// <param name="role"></param>
        [HttpPut]
        [Route("roles")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> UpdateRole([FromBody] Role role)
        {
            var roleExists = string.IsNullOrEmpty(role.Id)
                ? await _roleManager.RoleExistsAsync(role.Name)
                : await _roleManager.FindByIdAsync(role.Id) != null;

            var identityResult = roleExists
                ? await _roleManager.UpdateAsync(role)
                : await _roleManager.CreateAsync(role);

            return Ok(identityResult.ToSecurityResult());
        }

        /// <summary>
        /// SearchAsync users by keyword
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        [HttpPost]
        [Route("users/search")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<UserSearchResult>> SearchUsers([FromBody] UserSearchCriteria criteria)
        {
            var result = await _userSearchService.SearchUsersAsync(criteria);

            result.Results = ReduceUsersDetails(result.Results);

            return Ok(result);
        }

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <param name="userName"></param>
        [HttpGet]
        [Route("users/{userName}")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserByName([FromRoute] string userName)
        {
            var result = await UserManager.FindByNameAsync(userName);

            result = ReduceUserDetails(result);

            return Ok(result);
        }

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("users/id/{id}")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserById([FromRoute] string id)
        {
            var result = await UserManager.FindByIdAsync(id);

            result = ReduceUserDetails(result);

            return Ok(result);
        }

        /// <summary>
        /// Get user details by user email
        /// </summary>
        /// <param name="email"></param>
        [HttpGet]
        [Route("users/email/{email}")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserByEmail([FromRoute] string email)
        {
            var result = await UserManager.FindByEmailAsync(email);

            result = ReduceUserDetails(result);

            return Ok(result);
        }

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        [HttpGet]
        [Route("users/login/external/{loginProvider}/{providerKey}")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserByLogin([FromRoute] string loginProvider, [FromRoute] string providerKey)
        {
            var result = await UserManager.FindByLoginAsync(loginProvider, providerKey);

            result = ReduceUserDetails(result);

            return Ok(result);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        [Route("users/create")]
        [Authorize(PlatformPermissions.SecurityCreate)]
        public async Task<ActionResult<SecurityResult>> Create([FromBody] ApplicationUser newUser)
        {
            var identityResult = string.IsNullOrEmpty(newUser.Password)
                ? await UserManager.CreateAsync(newUser)
                : await UserManager.CreateAsync(newUser, newUser.Password);

            return Ok(identityResult.ToSecurityResult());
        }

        /// <summary>
        /// Change password for current user.
        /// </summary>
        /// <param name="changePassword">Old and new passwords.</param>
        /// <returns>Result of password change</returns>
        [HttpPost]
        [Route("currentuser/changepassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<SecurityResult>> ChangeCurrentUserPassword([FromBody] ChangePasswordRequest changePassword)
        {
            if (HttpContext.User.IsExternalSignIn())
            {
                return BadRequest(new SecurityResult { Errors = [$"Could not change password for {HttpContext.User.GetAuthenticationMethod()} authentication method"] });
            }

            return await ChangePassword(User.Identity.Name, changePassword);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="changePassword">Old and new passwords.</param>
        [HttpPost]
        [Route("users/{userName}/changepassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> ChangePassword([FromRoute] string userName, [FromBody] ChangePasswordRequest changePassword)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new PlatformException("Can't find current user.");
            }

            if (currentUser.IsAdministrator && !_passwordOptions.PasswordChangeByAdminEnabled)
            {
                throw new PlatformException("Administrators are not allowed to set passwords for users in the system.");
            }

            if (!_passwordLoginOptions.Enabled)
            {
                return BadRequest(new SecurityResult { Errors = ["Password login is disabled"] });
            }

            if (changePassword.OldPassword == changePassword.NewPassword)
            {
                return BadRequest(new SecurityResult { Errors = ["You have used this password in the past. Choose another one."] });
            }

            if (!IsUserEditable(userName))
            {
                LogUserForbiddenToEdit(userName);
                return Ok(IdentityResultExtensions.CreateErrorResult(UserForbiddenToEdit));
            }

            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                LogUserNotFound(userName);
                return Ok(IdentityResultExtensions.CreateErrorResult(UserNotFound));
            }

            if (await UserManager.IsLockedOutAsync(user))
            {
                return BadRequest(new SecurityResult { Errors = ["Your account is locked."] });
            }

            var result = await UserManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (result.Succeeded && user.PasswordExpired)
            {
                user.PasswordExpired = false;
                await UserManager.UpdateAsync(user);
                await UserManager.ResetAccessFailedCountAsync(user);
            }
            else
            {
                // Register failed attempt
                await UserManager.AccessFailedAsync(user);
            }

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Reset password confirmation
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="request">Password reset information.</param>
        /// <returns>Result of password reset.</returns>
        [HttpPost]
        [Route("users/{userName}/resetpassword")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> ResetPassword([FromRoute] string userName, [FromBody] ResetPasswordRequest request)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null)
            {
                throw new PlatformException("Can't find current user.");
            }

            if (currentUser.IsAdministrator && !_passwordOptions.PasswordChangeByAdminEnabled)
            {
                throw new PlatformException("Administrators are not allowed to set passwords for users in the system.");
            }

            if (!_passwordLoginOptions.Enabled)
            {
                return BadRequest(new SecurityResult { Errors = ["Password login is disabled"] });
            }

            var user = await UserManager.FindByNameAsync(userName);
            if (user is null)
            {
                LogUserNotFound(userName);
                return Ok(IdentityResultExtensions.CreateErrorResult(UserNotFound));
            }

            if (!IsUserEditable(user.UserName))
            {
                LogUserForbiddenToEdit(userName);
                return Ok(IdentityResultExtensions.CreateErrorResult(UserForbiddenToEdit));
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);

            var result = await UserManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (result.Succeeded)
            {
                user = await UserManager.FindByNameAsync(userName);

                if (user.PasswordExpired != request.ForcePasswordChangeOnNextSignIn)
                {
                    user.PasswordExpired = request.ForcePasswordChangeOnNextSignIn;
                    await UserManager.UpdateAsync(user);
                }
            }

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Reset password confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request">Password reset information.</param>
        [HttpPost]
        [Route("users/{userId}/resetpasswordconfirm")]
        [AllowAnonymous]
        public async Task<ActionResult<SecurityResult>> ResetPasswordByToken([FromRoute] string userId, [FromBody] ResetPasswordConfirmRequest request)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                LogUserNotFound(userId);
                return Ok(IdentityResultExtensions.CreateErrorResult(UserNotFound));
            }

            if (!IsUserEditable(user.UserName))
            {
                LogUserForbiddenToEdit(user.UserName);
                return Ok(IdentityResultExtensions.CreateErrorResult(UserForbiddenToEdit));
            }

            var result = await UserManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (result.Succeeded && user.PasswordExpired)
            {
                user.PasswordExpired = false;
                await UserManager.UpdateAsync(user);
            }

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Validate password reset token
        /// </summary>
        [HttpPost]
        [Route("users/{userId}/validatepasswordresettoken")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ValidatePasswordResetToken(string userId, [FromBody] ValidatePasswordResetTokenRequest resetPasswordToken)
        {
            var applicationUser = await UserManager.FindByIdAsync(userId);
            var tokenProvider = UserManager.Options.Tokens.PasswordResetTokenProvider;
            var result = await UserManager.VerifyUserTokenAsync(applicationUser, tokenProvider, "ResetPassword", resetPasswordToken.Token);

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
        [AllowAnonymous]
        public async Task<ActionResult> RequestPasswordReset(string loginOrEmail)
        {
            // Measure the duration of a succeeded response and delay subsequent failed responses to prevent timing attacks
            var delayedResponse = DelayedResponse.Create(nameof(SecurityController), nameof(RequestPasswordReset));

            var user = await UserManager.FindByNameAsync(loginOrEmail);

            if (user == null && _identityOptions.User.RequireUniqueEmail)
            {
                user = await UserManager.FindByEmailAsync(loginOrEmail);
            }

            // Return 200 to prevent potential user name/email harvesting
            if (user == null)
            {
                await delayedResponse.FailAsync();
                return Ok();
            }

            var nextRequestDate = user.LastPasswordChangeRequestDate + _passwordOptions.RepeatedResetPasswordTimeLimit;
            if (nextRequestDate != null && nextRequestDate > DateTime.UtcNow)
            {
                await delayedResponse.FailAsync();
                return Ok(new { NextRequestAt = nextRequestDate });
            }

            //Do not permit rejected users and customers
            if (user.Email != null &&
                IsUserEditable(user.UserName) &&
                !(await UserManager.IsInRoleAsync(user, PlatformConstants.Security.SystemRoles.Customer)))
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                // ASP.NET Core has OOTB protection from Host Header Injection Attacks: AllowedHosts configuration option
                // More information https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/host-filtering
                // Also it can be additionally configured at load-balancer and firewall level. 
                var callbackUrl = $"{Request.Scheme}://{Request.Host}/#!/resetpassword/{user.Id}/{token}";

                var requestPasswordResetEvent = new UserRequestPasswordResetEvent(user, callbackUrl);

                await _eventPublisher.Publish(requestPasswordResetEvent);

                user.LastPasswordChangeRequestDate = DateTime.UtcNow;
                await UserManager.UpdateAsync(user);
            }

            await delayedResponse.SucceedAsync();

            return Ok();
        }

        [HttpPost]
        [Route("validatepassword")]
        [AllowAnonymous]
        public async Task<ActionResult<IdentityResult>> ValidatePassword([FromBody] string password)
        {
            var user = await GetCurrentUserAsync();
            var result = await ValidatePassword(user, password);

            return Ok(result);
        }

        [HttpPost]
        [Route("validateuserpassword")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<IdentityResult>> ValidateUserPassword([FromBody] ChangePasswordRequest validatePassword)
        {
            if (validatePassword == null)
            {
                throw new ArgumentNullException(nameof(validatePassword));
            }

            if (!validatePassword.UserName.IsNullOrEmpty() && !IsUserEditable(validatePassword.UserName))
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }));
            }

            ApplicationUser user = null;
            if (validatePassword.UserName != null)
            {
                user = await UserManager.FindByNameAsync(validatePassword.UserName);
            }

            var result = await ValidatePassword(user, validatePassword.NewPassword);

            return Ok(result);
        }

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <param name="user">User details.</param>
        [HttpPut]
        [Route("users")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> Update([FromBody] ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!IsUserEditable(user.UserName))
            {
                return Ok(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }).ToSecurityResult());
            }

            var applicationUser = await UserManager.FindByIdAsync(user.Id);
            if (applicationUser.EmailConfirmed != user.EmailConfirmed
                && !Request.HttpContext.User.HasGlobalPermission(PlatformPermissions.SecurityVerifyEmail))
            {
                return Forbid();
            }

            if (!applicationUser.Email.EqualsIgnoreCase(user.Email))
            {
                // SetEmailAsync also: sets EmailConfirmed to false and updates the SecurityStamp
                await UserManager.SetEmailAsync(user, user.Email);
            }

            // Restore PasswordHash and SecurityStamp, use another API for password management
            if (!_securityOptions.ReturnPasswordHash)
            {
                user.PasswordHash = applicationUser.PasswordHash;
                user.SecurityStamp = applicationUser.SecurityStamp;
            }

            user.PasswordExpired = applicationUser.PasswordExpired;
            user.LockoutEnabled = applicationUser.LockoutEnabled;

            user.LockoutEnd = applicationUser.LockoutEnd;
            user.LastLoginDate = applicationUser.LastLoginDate;
            user.LastPasswordChangedDate = applicationUser.LastPasswordChangedDate;
            user.LastPasswordChangeRequestDate = applicationUser.LastPasswordChangeRequestDate;

            var result = await UserManager.UpdateAsync(user);

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <param name="names">An array of user names.</param>
        [HttpDelete]
        [Route("users")]
        [Authorize(PlatformPermissions.SecurityDelete)]
        public async Task<ActionResult> Delete([FromQuery] string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            if (names.Any(x => !IsUserEditable(x)))
            {
                return BadRequest(new IdentityError() { Description = "It is forbidden to edit these users." });
            }

            foreach (var userName in names)
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await UserManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        return BadRequest(result);
                    }
                }
            }

            return Ok(IdentityResult.Success);
        }

        /// <summary>
        /// Checks if user locked
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/{id}/locked")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<UserLockedResult>> IsUserLocked([FromRoute] string id)
        {
            var result = new UserLockedResult(false);
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                result.Locked = await UserManager.IsLockedOutAsync(user);
            }

            return Ok(result);
        }

        /// <summary>
        /// Checks if manual password change is enabled
        /// </summary>
        [HttpGet]
        [Route("passwordchangeenabled")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<UserLockedResult>> PasswordChangeEnabled()
        {
            var result = new PasswordChangeEnabledResult(true);

            var currentUser = await GetCurrentUserAsync();
            if (currentUser?.IsAdministrator == true)
            {
                result.Enabled = _passwordOptions.PasswordChangeByAdminEnabled;
            }

            return Ok(result);
        }

        /// <summary>
        /// Lock user
        /// </summary>
        /// <param name="id">>User id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("users/{id}/lock")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> LockUser([FromRoute] string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await UserManager.SetLockoutEndDateAsync(user, DateTime.MaxValue.ToUniversalTime());
                return Ok(result.ToSecurityResult());
            }

            return Ok(IdentityResult.Failed().ToSecurityResult());
        }

        /// <summary>
        /// Unlock user
        /// </summary>
        /// <param name="id">>User id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("users/{id}/unlock")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> UnlockUser([FromRoute] string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await UserManager.ResetAccessFailedCountAsync(user);
                var result = await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue.ToUniversalTime());
                return Ok(result.ToSecurityResult());
            }

            return Ok(IdentityResult.Failed().ToSecurityResult());
        }

        [HttpGet]
        [Route("users/{id}/apikeys")]
        [Authorize(PlatformPermissions.SecurityQuery)]
        public async Task<ActionResult<UserApiKey[]>> GetUserApiKeys([FromRoute] string id)
        {
            var result = await _userApiKeyService.GetAllUserApiKeysAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("users/apikeys")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public async Task<ActionResult<UserApiKey[]>> SaveUserApiKey([FromBody] UserApiKey userApiKey)
        {
            await _userApiKeyService.SaveApiKeysAsync([userApiKey]);
            return Ok();
        }

        [HttpPut]
        [Route("users/apikeys")]
        [Authorize(PlatformPermissions.SecurityUpdate)]
        public Task<ActionResult<UserApiKey[]>> UpdateUserApiKey([FromBody] UserApiKey userApiKey)
        {
            return SaveUserApiKey(userApiKey);
        }

        [HttpDelete]
        [Route("users/apikeys")]
        [Authorize(PlatformPermissions.SecurityDelete)]
        public async Task<ActionResult<UserApiKey[]>> DeleteUserApiKeys([FromQuery] string[] ids)
        {
            await _userApiKeyService.DeleteApiKeysAsync(ids);
            return Ok();
        }

        /// <summary>
        /// Get allowed login types
        /// </summary>
        [HttpGet]
        [Route("logintypes")]
        [AllowAnonymous]
        public ActionResult<List<LoginType>> GetLoginTypes()
        {
            var options = new List<LoginType>
            {
                new LoginType
                {
                    AuthenticationType = _passwordLoginOptions.AuthenticationType,
                    Enabled = _passwordLoginOptions.Enabled,
                    Priority = _passwordLoginOptions.Priority,
                }
            };

            var externalLoginTypes = _externalSigninProviderConfigs.Select(config => new LoginType
            {
                AuthenticationType = config.AuthenticationType,
                Enabled = true,
                Priority = config.Provider?.Priority ?? 0,
                HasLoginForm = config.Provider?.HasLoginForm ?? false,
            });

            options.AddRange(externalLoginTypes);

            return Ok(options);
        }

        /// <summary>
        /// Verify user email
        /// </summary>
        /// <param name="userId"></param>
        [HttpPost]
        [Route("users/{userId}/sendVerificationEmail")]
        [Authorize(PlatformPermissions.SecurityVerifyEmail)]
        public async Task<ActionResult> SendVerificationEmail([FromRoute] string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            if (!IsUserEditable(user.UserName))
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }).ToSecurityResult());
            }

            await _eventPublisher.Publish(new UserVerificationEmailEvent(user));

            return Ok();
        }

        [HttpPost]
        [Route("users/{userId}/confirmEmail")]
        [Authorize(PlatformPermissions.SecurityConfirmEmail)]
        public async Task<ActionResult<SecurityResult>> ConfirmEmail([FromRoute] string userId, [FromBody] ConfirmEmailRequest request)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var confirmEmailResult = await _signInManager.UserManager.ConfirmEmailAsync(user, request.Token);

            return Ok(confirmEmailResult.ToSecurityResult());
        }

        [HttpGet]
        [Route("users/{userId}/generateChangeEmailToken")]
        [Authorize(PlatformPermissions.SecurityGenerateToken)]
        public async Task<ActionResult<string>> GenerateChangeEmailToken([FromRoute] string userId, [FromQuery] string newEmail)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var token = await _signInManager.UserManager.GenerateChangeEmailTokenAsync(user, newEmail);

            return Ok(token);
        }

        [HttpGet]
        [Route("users/{userId}/generateEmailConfirmationToken")]
        [Authorize(PlatformPermissions.SecurityGenerateToken)]
        public async Task<ActionResult<string>> GenerateEmailConfirmationToken([FromRoute] string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

            return Ok(token);
        }

        [HttpGet]
        [Route("users/{userId}/generatePasswordResetToken")]
        [Authorize(PlatformPermissions.SecurityGenerateToken)]
        public async Task<ActionResult<string>> GeneratePasswordResetToken([FromRoute] string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            return Ok(token);
        }

        [HttpGet]
        [Route("users/{userId}/generateToken")]
        [Authorize(PlatformPermissions.SecurityGenerateToken)]
        public async Task<ActionResult<string>> GenerateUserToken([FromRoute] string userId, [FromQuery] string tokenProvider, [FromQuery] string purpose)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var token = await _signInManager.UserManager.GenerateUserTokenAsync(user, tokenProvider, purpose);

            return Ok(token);
        }

        [HttpPost]
        [Route("users/{userId}/verifyToken")]
        [Authorize(PlatformPermissions.SecurityVerifyToken)]
        public async Task<ActionResult<bool>> VerifyUserToken([FromRoute] string userId, [FromBody] VerifyTokenRequest request)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var success = await _signInManager.UserManager.VerifyUserTokenAsync(user, request.TokenProvider, request.Purpose, request.Token);

            return Ok(success);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            if (string.IsNullOrEmpty(User.Identity?.Name) ||
                !User.Identity.IsAuthenticated)
            {
                return Task.FromResult<ApplicationUser>(null);
            }

            return UserManager.FindByNameAsync(User.Identity.Name);
        }

        private bool IsUserEditable(string userName)
        {
            return _securityOptions.NonEditableUsers?.FirstOrDefault(x => x.EqualsIgnoreCase(userName)) == null;
        }

        private void LogUserNotFound(string idOrName)
        {
            var sanitizedUserName = UserManager.SanitizeUserName(idOrName);
            _logger.LogWarning("User {user} not found.", sanitizedUserName);
        }

        private void LogUserForbiddenToEdit(string idOrName)
        {
            var sanitizedUserName = UserManager.SanitizeUserName(idOrName);
            _logger.LogWarning("User {user} is forbidden to edit.", sanitizedUserName);
        }

        private ApplicationUser ReduceUserDetails(ApplicationUser user)
        {
            if (!_securityOptions.ReturnPasswordHash)
            {
                user = user?.Clone() as ApplicationUser;

                if (user != null)
                {
                    user.PasswordHash = null;
                    user.SecurityStamp = null;
                }
            }

            return user;
        }

        private IList<ApplicationUser> ReduceUsersDetails(IList<ApplicationUser> users)
        {
            return users?.Select(ReduceUserDetails).ToList();
        }

        private async Task<IdentityResult> ValidatePassword(ApplicationUser user, string password)
        {
            var errors = new List<IdentityError>();
            var isValid = true;
            foreach (var passwordValidator in UserManager.PasswordValidators)
            {
                var identityResult = await passwordValidator.ValidateAsync(UserManager, user, password);
                if (!identityResult.Succeeded)
                {
                    errors.AddRange(identityResult.Errors);
                    isValid = false;
                }
            }

            var result = isValid ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
            return result;
        }

        private Task SetLastLoginDate(ApplicationUser user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            return _signInManager.UserManager.UpdateAsync(user);
        }
    }
}
