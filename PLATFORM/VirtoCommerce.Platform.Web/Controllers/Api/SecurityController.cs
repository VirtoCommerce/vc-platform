using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security;
using VirtoCommerce.Platform.Data.Security.Identity;
using VirtoCommerce.Platform.Web.Converters.Security;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly Func<IAuthenticationManager> _authenticationManagerFactory;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly Func<ApplicationUserManager> _userManagerFactory;
        private readonly IApiAccountProvider _apiAccountProvider;
        private readonly IPermissionService _permissionService;
        private readonly IRoleManagementService _roleService;

        public SecurityController(Func<IPlatformRepository> platformRepository, Func<ApplicationSignInManager> signInManagerFactory,
                                  Func<ApplicationUserManager> userManagerFactory, Func<IAuthenticationManager> authManagerFactory,
                                  IApiAccountProvider apiAccountProvider,
            IPermissionService permissionService, IRoleManagementService roleService)
        {
            _platformRepository = platformRepository;
            _signInManagerFactory = signInManagerFactory;
            _userManagerFactory = userManagerFactory;
            _authenticationManagerFactory = authManagerFactory;
            _apiAccountProvider = apiAccountProvider;
            _permissionService = permissionService;
            _roleService = roleService;
        }

        private ApplicationUserManager _userManager;
        private ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = _userManagerFactory();
                }
                return _userManager;
            }
        }

        #region Internal Web admin actions

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(UserLogin model)
        {
            if (await _signInManagerFactory().PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true) == SignInStatus.Success)
            {
                return Ok(await GetUserExtended(model.UserName, UserDetails.Full));
            }

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpGet]
        [Route("usersession")]
        [ResponseType(typeof(ApplicationUserExtended))]
        public async Task<IHttpActionResult> GetCurrentUserSession()
        {
            return Ok(await GetUserExtended(User.Identity.Name, UserDetails.Full));
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            _authenticationManagerFactory().SignOut();
            return Ok(new { status = true });
        }

        [HttpGet]
        [Route("permissions")]
        [ResponseType(typeof(Permission[]))]
        [CheckPermission(Permission = PredefinedPermissions.SecurityQuery)]
        public IHttpActionResult GetPermissions()
        {
            var result = _permissionService.GetAllPermissions()
                .OrderBy(p => p.Name)
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
            return await GetUserExtended(userName, UserDetails.Full);
        }

        #region Methods needed to integrate identity security with external user store that will call api methods

        #region IUserStore<ApplicationUser> Members

        [Route("users/id/{userId}")]
        [HttpGet]
        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await UserManager.FindByIdAsync(userId);
        }

        [Route("users/name/{userName}")]
        [HttpGet]
        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await UserManager.FindByNameAsync(userName);
        }

        [Route("users/email/{email}")]
        [HttpGet]
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await UserManager.FindByEmailAsync(email);
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
        public IHttpActionResult GenerateNewApiAccount()
        {
            var apiAccount = _apiAccountProvider.GenerateApiCredentials();
            apiAccount.IsActive = true;
            var result = apiAccount.ToWebModel();
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
            var retVal = await GetUserExtended(name, UserDetails.Full);
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
            var user = await UserManager.FindByNameAsync(name);
            if (user == null)
            {
                return NotFound();
            }
            var retVal = await UserManager.ChangePasswordAsync(user.Id, changePassword.OldPassword, changePassword.NewPassword);
            return Ok(retVal);
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
            request = request ?? new UserSearchRequest();
            var result = new UserSearchResponse();

            var query = UserManager.Users;

            if (request.Keyword != null)
            {
                query = query.Where(u => u.UserName.Contains(request.Keyword));
            }

            result.TotalCount = query.Count();

            var users = query.OrderBy(x => x.UserName)
                             .Skip(request.SkipCount)
                             .Take(request.TakeCount)
                             .ToArray();

            var extendedUsers = new List<ApplicationUserExtended>();

            foreach (var user in users)
            {
                var extendedUser = await GetUserExtended(user.UserName, UserDetails.Reduced);
                extendedUsers.Add(extendedUser);
            }

            result.Users = extendedUsers.ToArray();

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
            foreach (var name in names)
            {
                var dbUser = await UserManager.FindByNameAsync(name);
                if (dbUser == null)
                {
                    return NotFound();
                }

                await UserManager.DeleteAsync(dbUser);

                using (var repository = _platformRepository())
                {
                    var account = repository.GetAccountByName(name, UserDetails.Reduced);
                    if (account != null)
                    {
                        repository.Remove(account);
                        repository.UnitOfWork.Commit();
                    }
                }
            }

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
            var dbUser = await UserManager.FindByIdAsync(user.Id);

            dbUser.InjectFrom(user);

            foreach (var login in user.Logins)
            {
                var userLogin = dbUser.Logins.FirstOrDefault(l => l.LoginProvider == login.LoginProvider);
                if (userLogin != null)
                {
                    userLogin.ProviderKey = login.ProviderKey;
                }
                else
                {
                    dbUser.Logins.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin
                    {
                        LoginProvider = login.LoginProvider,
                        ProviderKey = login.ProviderKey,
                        UserId = dbUser.Id
                    });
                }
            }

            var result = await UserManager.UpdateAsync(dbUser);

            if (result.Succeeded)
            {
                using (var repository = _platformRepository())
                {
                    var acount = repository.GetAccountByName(user.UserName, UserDetails.Full);
                    if (acount == null)
                    {
                        return BadRequest("Acount not found");
                    }
                    acount.RegisterType = (RegisterType)user.UserType;
                    acount.AccountState = (AccountState)user.UserState;
                    acount.MemberId = user.MemberId;
                    acount.StoreId = user.StoreId;

                    if (user.ApiAcounts != null)
                    {
                        var source = new ObservableCollection<ApiAccountEntity>(user.ApiAcounts.Select(x => x.ToFoundation()));
                        var inventoryComparer = AnonymousComparer.Create((ApiAccountEntity x) => x.Id);
                        acount.ApiAccounts.ObserveCollection(x => repository.Add(x), x => repository.Remove(x));
                        source.Patch(acount.ApiAccounts, inventoryComparer, (sourceAccount, targetAccount) => sourceAccount.Patch(targetAccount));
                    }

                    if (user.Roles != null)
                    {
                        var sourceCollection = new ObservableCollection<RoleAssignmentEntity>(user.Roles.Select(r => new RoleAssignmentEntity { RoleId = r.Id }));
                        var comparer = AnonymousComparer.Create((RoleAssignmentEntity x) => x.RoleId);
                        acount.RoleAssignments.ObserveCollection(ra => repository.Add(ra), ra => repository.Remove(ra));
                        sourceCollection.Patch(acount.RoleAssignments, comparer, (source, target) => source.Patch(target));
                    }

                    repository.UnitOfWork.Commit();
                }

                return Ok();
            }

            return BadRequest(String.Join(" ", result.Errors));
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
            var dbUser = new ApplicationUser();

            dbUser.InjectFrom(user);

            IdentityResult result;
            if (!string.IsNullOrEmpty(user.Password))
            {
                result = await UserManager.CreateAsync(dbUser, user.Password);
            }
            else
            {
                result = await UserManager.CreateAsync(dbUser);
            }

            if (result.Succeeded)
            {
                using (var repository = _platformRepository())
                {
                    var account = new AccountEntity()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        MemberId = user.MemberId,
                        AccountState = AccountState.Approved,
                        RegisterType = (RegisterType)user.UserType,
                        StoreId = user.StoreId
                    };

                    if (user.Roles != null)
                    {
                        foreach (var role in user.Roles)
                        {
                            account.RoleAssignments.Add(new RoleAssignmentEntity { RoleId = role.Id, AccountId = account.Id });
                        }
                    }

                    repository.Add(account);
                    repository.UnitOfWork.Commit();
                }

                return Ok();
            }
            else
            {
                return BadRequest(String.Join(" ", result.Errors));
            }
        }
        #endregion

        #endregion

        #region Helpers

        private async Task<ApplicationUserExtended> GetUserExtended(string userName, UserDetails detailsLevel)
        {
            var applicationUser = await UserManager.FindByNameAsync(userName);
            return GetUserExtended(applicationUser, detailsLevel);
        }

        private ApplicationUserExtended GetUserExtended(ApplicationUser applicationUser, UserDetails detailsLevel)
        {
            ApplicationUserExtended result = null;

            if (applicationUser != null)
            {
                result = new ApplicationUserExtended();
                result.InjectFrom(applicationUser);

                using (var repository = _platformRepository())
                {
                    var user = repository.GetAccountByName(applicationUser.UserName, detailsLevel);

                    if (user != null)
                    {
                        result.InjectFrom(user);

                        result.UserState = (UserState)user.AccountState;
                        result.UserType = (UserType)user.RegisterType;

                        if (detailsLevel == UserDetails.Full)
                        {
                            var roles = user.RoleAssignments.Select(x => x.Role).ToArray();
                            result.Roles = roles.Select(r => r.ToCoreModel(false)).ToArray();

                            var permissionIds = roles
                                    .SelectMany(x => x.RolePermissions)
                                    .Select(x => x.PermissionId)
                                    .Distinct()
                                    .ToArray();

                            result.Permissions = permissionIds;
                            result.ApiAcounts = user.ApiAccounts.Select(x => x.ToWebModel()).ToList();
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
