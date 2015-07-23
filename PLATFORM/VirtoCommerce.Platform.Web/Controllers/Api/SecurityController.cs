using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {
        private readonly Func<IAuthenticationManager> _authenticationManagerFactory;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly IPermissionService _permissionService;
        private readonly IRoleManagementService _roleService;
        private readonly ISecurityService _securityService;

        public SecurityController(Func<ApplicationSignInManager> signInManagerFactory, Func<IAuthenticationManager> authManagerFactory,
            IPermissionService permissionService, IRoleManagementService roleService, ISecurityService securityService)
        {
            _signInManagerFactory = signInManagerFactory;
            _authenticationManagerFactory = authManagerFactory;
            _permissionService = permissionService;
            _roleService = roleService;
            _securityService = securityService;
        }

        #region Internal Web admin actions

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(UserLogin model)
        {
            if (await _signInManagerFactory().PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true) == SignInStatus.Success)
            {
                return Ok(await _securityService.FindByNameAsync(model.UserName, UserDetails.Full));
            }

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            _authenticationManagerFactory().SignOut();
            return Ok(new { status = true });
        }

        [HttpGet]
        [Route("usersession")]
        [ResponseType(typeof(ApplicationUserExtended))]
        public async Task<IHttpActionResult> GetCurrentUserSession()
        {
            return Ok(await _securityService.FindByNameAsync(User.Identity.Name, UserDetails.Full));
        }

        [HttpGet]
        [Route("permissions")]
        [ResponseType(typeof(Permission[]))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult GetPermissions()
        {
            var result = _permissionService.GetAllPermissions()
                .OrderBy(p => p.GroupName)
                .ThenBy(p => p.Name)
                .ToArray();

            return Ok(result);
        }

        [HttpGet]
        [Route("roles")]
        [ResponseType(typeof(RoleSearchResponse))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult SearchRoles([FromUri]RoleSearchRequest request)
        {
            var result = _roleService.SearchRoles(request);
            return Ok(result);
        }

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
        /// DELETE: api/security/roles?id=1&amp;id=2
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("roles")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityManage)]
        public IHttpActionResult DeleteRoles([FromUri(Name = "ids")] string[] roleIds)
        {
            if (roleIds != null)
            {
                foreach (var roleId in roleIds)
                {
                    _roleService.DeleteRole(roleId);
                }
            }

            return Ok();
        }

        [HttpPut]
        [Route("roles")]
        [ResponseType(typeof(Role))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityManage)]
        public IHttpActionResult UpdateRole(Role role)
        {
            var result = _roleService.AddOrUpdateRole(role);
            return Ok(result);
        }

        #endregion

        #region Public methods

        [HttpGet]
        [Route("usersession/{userName}")]
        public async Task<ApplicationUserExtended> GetUserSession(string userName)
        {
            return await _securityService.FindByNameAsync(userName, UserDetails.Full);
        }

        #region Methods needed to integrate identity security with external user store that will call api methods

        #region IUserStore<ApplicationUser> Members

        [Route("users/id/{userId}")]
        [HttpGet]
        public async Task<ApplicationUserExtended> FindByIdAsync(string userId)
        {
            return await _securityService.FindByIdAsync(userId, UserDetails.Reduced);
        }

        [Route("users/name/{userName}")]
        [HttpGet]
        public async Task<ApplicationUserExtended> FindByNameAsync(string userName)
        {
            return await _securityService.FindByNameAsync(userName, UserDetails.Reduced);
        }

        [Route("users/email/{email}")]
        [HttpGet]
        public async Task<ApplicationUserExtended> FindByEmailAsync(string email)
        {
            return await _securityService.FindByEmailAsync(email, UserDetails.Reduced);
        }

        #endregion

        /// <summary>
        ///  GET: api/security/apiaccounts/new
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ApiAccount))]
        [Route("apiaccounts/new")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityManage)]
        public IHttpActionResult GenerateNewApiAccount(ApiAccountType type)
        {
            var result = _securityService.GenerateNewApiAccount(type);
            result.IsActive = null;
            return Ok(result);
        }


        /// <summary>
        ///  GET: api/security/users/jo@domain.com
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ApplicationUserExtended))]
        [Route("users/{name}")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> GetUserByName(string name)
        {
            var retVal = await _securityService.FindByNameAsync(name, UserDetails.Full);
            return Ok(retVal);
        }

        /// <summary>
        /// POST: api/security/users/jo@domain.com/changepassword
        /// </summary>
        /// <param name="name"></param>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(IdentityResult))]
        [Route("users/{name}/changepassword")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> ChangePassword(string name, [FromBody] ChangePasswordInfo changePassword)
        {
            var result = await _securityService.ChangePasswordAsync(name, changePassword.OldPassword, changePassword.NewPassword);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        ///  GET: api/security/users?q=ddd&amp;start=0&amp;count=20
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(UserSearchResponse))]
        [Route("users")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public async Task<IHttpActionResult> SearchUsersAsync([FromUri] UserSearchRequest request)
        {
            var result = await _securityService.SearchUsersAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// DELETE: api/security/users?names=21
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("users")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityManage)]
        public async Task<IHttpActionResult> DeleteAsync([FromUri] string[] names)
        {
            await _securityService.DeleteAsync(names);
            return Ok();
        }

        /// <summary>
        /// PUT: api/security/users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("users")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityManage)]
        public async Task<IHttpActionResult> UpdateAsync(ApplicationUserExtended user)
        {
            var result = await _securityService.UpdateAsync(user);
            return ProcessSecurityResult(result);
        }

        /// <summary>
        /// POST: api/security/users/create
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("users/create")]
        [CheckPermission(Permission = PredefinedPermissions.SecurityManage)]
        public async Task<IHttpActionResult> CreateAsync(ApplicationUserExtended user)
        {
            var result = await _securityService.CreateAsync(user);
            return ProcessSecurityResult(result);
        }

        #endregion

        #endregion


        private IHttpActionResult ProcessSecurityResult(SecurityResult result)
        {
            if (result == null)
            {
                return BadRequest();
            }
            else
            {
                if (!result.Succeeded)
                    return BadRequest(result.Errors != null ? string.Join(" ", result.Errors) : "Unknown error.");
                else
                    return Ok();
            }
        }
    }
}
