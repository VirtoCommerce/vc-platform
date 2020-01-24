using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Extensions;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.Search;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Model.Security;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/security")]
    public class SecurityController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly Core.Security.AuthorizationOptions _securityOptions;
        private readonly IPermissionsRegistrar _permissionsProvider;
        private readonly IUserSearchService _userSearchService;
        private readonly IRoleSearchService _roleSearchService;
        private readonly IPasswordCheckService _passwordCheckService;
        private readonly IEmailSender _emailSender;
        private readonly IEventPublisher _eventPublisher;

        public SecurityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager,
                IPermissionsRegistrar permissionsProvider, IUserSearchService userSearchService, IRoleSearchService roleSearchService,
                IOptions<Core.Security.AuthorizationOptions> securityOptions, IPasswordCheckService passwordCheckService, IEmailSender emailSender, IEventPublisher eventPublisher)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _securityOptions = securityOptions.Value;
            _passwordCheckService = passwordCheckService;
            _permissionsProvider = permissionsProvider;
            _roleManager = roleManager;
            _userSearchService = userSearchService;
            _roleSearchService = roleSearchService;
            _emailSender = emailSender;
            _eventPublisher = eventPublisher;
        }

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
        public async Task<ActionResult<SignInResult>> Login([FromBody]LoginRequest request)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberMe, true);
            if (loginResult.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                await _eventPublisher.Publish(new UserLoginEvent(user));
                //Do not allow login to admin customers and rejected users
                if (await _signInManager.UserManager.IsInRoleAsync(user, PlatformConstants.Security.SystemRoles.Customer))
                {
                    loginResult = SignInResult.NotAllowed;
                }
            }
            return Ok(loginResult);
        }

        /// <summary>
        /// Sign out
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("logout")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Logout()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
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
        [Route("currentuser")]
        public async Task<ActionResult<UserDetail>> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserDetail
            {
                Id = user.Id,
                isAdministrator = user.IsAdministrator,
                UserName = user.UserName,
                PasswordExpired = user.PasswordExpired
            };
            var roleNames = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (!role.Permissions.IsNullOrEmpty())
                {
                    result.Permissions.AddRange(role.Permissions.Select(x => x.Name));
                }
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
        [Route("userinfo")]
        public async Task<ActionResult<Claim[]>> Userinfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The user profile is no longer available."
                });
            }

            var claims = new JObject
            {

                //TODO: replace to PrinciplaClaims

                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [OpenIdConnectConstants.Claims.Subject] = await _userManager.GetUserIdAsync(user)
            };

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Email))
            {
                claims[OpenIdConnectConstants.Claims.Email] = await _userManager.GetEmailAsync(user);
                claims[OpenIdConnectConstants.Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIdConnectConstants.Scopes.Phone))
            {
                claims[OpenIdConnectConstants.Claims.PhoneNumber] = await _userManager.GetPhoneNumberAsync(user);
                claims[OpenIdConnectConstants.Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user);
            }

            if (User.HasClaim(OpenIdConnectConstants.Claims.Scope, OpenIddictConstants.Scopes.Roles))
            {
                claims["roles"] = JArray.FromObject(await _userManager.GetRolesAsync(user));
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
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
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
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
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
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
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
        [Authorize(PlatformConstants.Security.Permissions.SecurityDelete)]
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
        [Authorize(PlatformConstants.Security.Permissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> UpdateRole([FromBody] Role role)
        {
            IdentityResult result;
            var roleExists = string.IsNullOrEmpty(role.Id) ?
                await _roleManager.RoleExistsAsync(role.Name) :
                await _roleManager.FindByIdAsync(role.Id) != null;
            if (!roleExists)
            {
                result = await _roleManager.CreateAsync(role);
            }
            else
            {
                result = await _roleManager.UpdateAsync(role);
            }
            return Ok(result.ToSecurityResult());
        }
        /// <summary>
        /// SearchAsync users by keyword
        /// </summary>
        /// <param name="criteria">Search criteria.</param>
        [HttpPost]
        [Route("users")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<UserSearchResult>> SearchUsers([FromBody] UserSearchCriteria criteria)
        {
            var result = await _userSearchService.SearchUsersAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <param name="userName"></param>
        [HttpGet]
        [Route("users/{userName}")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserByName([FromRoute] string userName)
        {
            var retVal = await _userManager.FindByNameAsync(userName);
            return Ok(retVal);
        }

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("users/id/{id}")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserById([FromRoute] string id)
        {
            var retVal = await _userManager.FindByIdAsync(id);
            return Ok(retVal);
        }

        /// <summary>
        /// Get user details by user email
        /// </summary>
        /// <param name="email"></param>
        [HttpGet]
        [Route("users/email/{email}")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserByEmail([FromRoute] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Ok(result);
        }


        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users/login/external")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<ApplicationUser>> GetUserByLogin([FromRoute]string loginProvider, [FromRoute]string providerKey)
        {
            var result = await _userManager.FindByLoginAsync(loginProvider, providerKey);
            return Ok(result);
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="newUser"></param>
        [HttpPost]
        [Route("users/create")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityCreate)]
        public async Task<ActionResult<SecurityResult>> Create([FromBody] ApplicationUser newUser)
        {
            var result = await _userManager.CreateAsync(newUser, newUser.Password);
            return Ok(result.ToSecurityResult());
        }


        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="changePassword">Old and new passwords.</param>
        [HttpPost]
        [Route("users/{userName}/changepassword")]
        [ProducesResponseType(400)]
        [Authorize(PlatformConstants.Security.Permissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> ChangePassword([FromRoute] string userName, [FromBody] ChangePasswordRequest changePassword)
        {
            if (!IsUserEditable(userName))
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }).ToSecurityResult());
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }

            var result = await _signInManager.UserManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                await _eventPublisher.Publish(new UserPasswordChangedEvent(user.Id));

                // If the password change was required for the user, now it is not needed anymore - the password is changed.
                if (user.PasswordExpired)
                {
                    user.PasswordExpired = false;
                    await _userManager.UpdateAsync(user);
                }
            }

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Resets password for current user.
        /// </summary>
        /// <param name="resetPassword">Password reset information containing new password.</param>
        /// <returns>Result of password reset.</returns>
        [HttpPost]
        [Route("currentuser/resetpassword")]
        public async Task<ActionResult<SecurityResult>> ResetCurrentUserPassword([FromBody] ResetPasswordConfirmRequest resetPassword)
        {
            var currentUserName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(currentUserName);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }
            if (!IsUserEditable(user.UserName))
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }).ToSecurityResult());
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _signInManager.UserManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
            if (result.Succeeded)
            {
                await _eventPublisher.Publish(new UserResetPasswordEvent(user.Id));

                if (user.PasswordExpired != resetPassword.ForcePasswordChangeOnNextSignIn)
                {
                    user.PasswordExpired = resetPassword.ForcePasswordChangeOnNextSignIn;

                    var userUpdateResult = await _userManager.UpdateAsync(user);
                }
            }

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Reset password confirmation
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="resetPasswordConfirm">New password.</param>
        [HttpPost]
        [Route("users/{userName}/resetpassword")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> ResetPassword([FromRoute] string userName, [FromBody] ResetPasswordConfirmRequest resetPasswordConfirm)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }
            if (!IsUserEditable(user.UserName))
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }).ToSecurityResult());
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordConfirm.NewPassword);
            if (result.Succeeded)
            {
                await _eventPublisher.Publish(new UserResetPasswordEvent(user.Id));
                if (user.PasswordExpired != resetPasswordConfirm.ForcePasswordChangeOnNextSignIn)
                {
                    user.PasswordExpired = resetPasswordConfirm.ForcePasswordChangeOnNextSignIn;

                    var userUpdateResult = await _userManager.UpdateAsync(user);
                }
            }

            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Reset password confirmation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="resetPasswordConfirm">New password.</param>
        [HttpPost]
        [Route("users/{userId}/resetpasswordconfirm")]
        [AllowAnonymous]
        public async Task<ActionResult<SecurityResult>> ResetPasswordByToken([FromRoute] string userId, [FromBody] ResetPasswordConfirmRequest resetPasswordConfirm)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "User not found" }).ToSecurityResult());
            }
            if (!IsUserEditable(user.UserName))
            {
                return BadRequest(IdentityResult.Failed(new IdentityError { Description = "It is forbidden to edit this user." }).ToSecurityResult());
            }
            var result = await _signInManager.UserManager.ResetPasswordAsync(user, resetPasswordConfirm.Token, resetPasswordConfirm.NewPassword);
            if (result.Succeeded)
            {
                await _eventPublisher.Publish(new UserResetPasswordEvent(user.Id));

                // If the password reset was required for the user, now it is not needed anymore - the password is changed now.
                if (user.PasswordExpired)
                {
                    user.PasswordExpired = false;

                    var userUpdateResult = await _userManager.UpdateAsync(user);
                }
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
            var applicationUser = await _userManager.FindByIdAsync(userId);
            var tokenProvider = _userManager.Options.Tokens.PasswordResetTokenProvider;
            var result = await _userManager.VerifyUserTokenAsync(applicationUser, tokenProvider, "ResetPassword",
                resetPasswordToken.Token);
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
            var user = await _userManager.FindByNameAsync(loginOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginOrEmail);
            }

            //Do not permit rejected users and customers
            if (user?.Email != null && IsUserEditable(user.UserName) && !(await _userManager.IsInRoleAsync(user, PlatformConstants.Security.SystemRoles.Customer)))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = $"{Request.Scheme}://{Request.Host}/#/resetpassword/{user.Id}/{token}";

                await _emailSender.SendEmailAsync(user.Email, "Reset password", callbackUrl.ToString());

                return Ok(callbackUrl);
            }

            return Ok();
        }

        [HttpPost]
        [Route("validatepassword")]
        public async Task<ActionResult<PasswordValidationResult>> ValidatePassword([FromBody] string password)
        {
            var result = await _passwordCheckService.ValidatePasswordAsync(password);
            return Ok(result);
        }

        /// <summary>
        /// Update user details by user ID
        /// </summary>
        /// <param name="user">User details.</param>
        [HttpPut]
        [Route("users")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityUpdate)]
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
            var result = await _userManager.UpdateAsync(user);


            return Ok(result.ToSecurityResult());
        }

        /// <summary>
        /// Delete users by name
        /// </summary>
        /// <param name="names">An array of user names.</param>
        [HttpDelete]
        [Route("users")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityDelete)]
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
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
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
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<UserLockedResult>> IsUserLocked([FromRoute] string id)
        {
            var result = new UserLockedResult(false);
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                result.Locked = await _userManager.IsLockedOutAsync(user);
            }
            return Ok(result);
        }

        /// <summary>
        /// Unlock user
        /// </summary>
        /// <param name="id">>User id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("users/{id}/unlock")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityUpdate)]
        public async Task<ActionResult<SecurityResult>> UnlockUser([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
                return Ok(result.ToSecurityResult());
            }
            return Ok(IdentityResult.Failed().ToSecurityResult());
        }

        //TODO: Remove later
        #region Obsolete methods
        [Obsolete("use /roles/search instead")]
        [HttpPost]
        [Route("roles")]
        [Authorize(PlatformConstants.Security.Permissions.SecurityQuery)]
        public async Task<ActionResult<RoleSearchResult>> SearchRolesObsolete([FromBody] RoleSearchCriteria request)
        {
            var result = await _roleSearchService.SearchRolesAsync(request);
            return Ok(result);
        }

        #endregion

        private bool IsUserEditable(string userName)
        {
            return _securityOptions.NonEditableUsers?.FirstOrDefault(x => x.EqualsInvariant(userName)) == null;
        }
    }
}
