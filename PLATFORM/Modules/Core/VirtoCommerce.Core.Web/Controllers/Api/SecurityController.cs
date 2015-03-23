using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Omu.ValueInjecter;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Security.Models;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Security.Model;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CoreModule.Web.Controllers.Api;
using Microsoft.AspNet.Identity;
using System.Collections.ObjectModel;
using foundationModel = VirtoCommerce.Foundation.Security.Model;
using webModel = VirtoCommerce.CoreModule.Web.Security.Models;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.CoreModule.Web.Converters;
namespace VirtoCommerce.SecurityModule.Web.Controllers
{
    [RoutePrefix("api/security")]
    public class SecurityController : ApiController
    {
        private readonly Func<IFoundationSecurityRepository> _securityRepository;

        private Func<IAuthenticationManager> _authenticationManagerFactory;
        private Func<ApplicationSignInManager> _signInManagerFactory;
        private Func<ApplicationUserManager> _userManagerFactory;
        public SecurityController(Func<IFoundationSecurityRepository> securityRepository, Func<ApplicationSignInManager> signInManagerFactory,
							      Func<ApplicationUserManager> userManagerFactory, Func<IAuthenticationManager> authManagerFactory)
        {
            _securityRepository = securityRepository;
			_signInManagerFactory = signInManagerFactory;
			_userManagerFactory = userManagerFactory;
			_authenticationManagerFactory = authManagerFactory;
        }

		private ApplicationUserManager _userManager;
		private ApplicationUserManager UserManager
		{
			get
			{
				if(_userManager == null)
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
				return Ok(await GetUserExtended(model.UserName));
            }

            return StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpGet]
        [Route("usersession")]
		[ResponseType(typeof(ApplicationUserExtended))]
        public async  Task<IHttpActionResult> GetCurrentUserSession()
        {
			return Ok(await GetUserExtended(User.Identity.Name));
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
			_authenticationManagerFactory().SignOut();
            return Ok(new { status = true });
        }

        #endregion

        #region Public methods

        [HttpGet]
        [Route("usersession/{userName}")]
		[ResponseType(typeof(ApplicationUserExtended))]
        public IHttpActionResult GetUserSession(string userName)
        {
			return Ok(GetUserExtended(userName));
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
		/// <param name="criteria"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(webModel.ApiAccount))]
		[Route("apiaccounts/new")]
		public IHttpActionResult GenerateNewApiAccount()
		{
			webModel.ApiAccount retVal = null;
			//TODO: Artyom
			return Ok(retVal);
		}


		/// <summary>
		///  GET: api/security/users/jo@domain.com
		/// </summary>
		/// <param name="criteria"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(ApplicationUserExtended))]
		[Route("users/{name}")]
		public async Task<IHttpActionResult> GetUserByName(string name)
		{
			var retVal = await GetUserExtended(name);
			return Ok(retVal);
		}


		/// <summary>
		///  GET: api/security/users?q=ddd&start=0&count=20
		/// </summary>
		/// <param name="criteria"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseType(typeof(UserSearchResult))]
		[Route("users")]
		public IHttpActionResult SearchUsers([ModelBinder(typeof(UserSearchCriteriaBinder))] UserSearchCriteria criteria)
		{
			var query = UserManager.Users;
			var retVal = new UserSearchResult
			{
				TotalCount = query.Count(),
				Users = query.OrderBy(x => x.UserName)
							 .Skip(criteria.Start)
							 .Take(criteria.Count)
							 .Select(x => new webModel.ApplicationUserExtended
							{
								Id = x.Id,
								FullName = x.UserName,
								UserName = x.UserName
							}).ToList()
			};
			

			return Ok(retVal);
		}

		/// <summary>
		/// DELETE: api/security/users?names=21
		/// </summary>
		/// <param name="ids"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("users")]
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
				using (var repository = _securityRepository())
				{
					var account = repository.GetAccountByName(name);
					if(account != null)
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
        [Route("users/update")]
        public async Task<IHttpActionResult> UpdateAsync(ApplicationUserExtended user)
        {
			var dbUser = await UserManager.FindByIdAsync(user.Id);

            dbUser.AccessFailedCount = user.AccessFailedCount;
            dbUser.Email = user.Email;
            dbUser.EmailConfirmed = user.EmailConfirmed;
            dbUser.LockoutEnabled = user.LockoutEnabled;
            dbUser.LockoutEndDateUtc = user.LockoutEndDateUtc;
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            dbUser.PhoneNumber = user.PhoneNumber;
            dbUser.SecurityStamp = user.SecurityStamp;
            dbUser.TwoFactorEnabled = user.TwoFactorEnabled;

			var result = await UserManager.UpdateAsync(dbUser);

            if (result.Succeeded)
            {
				using (var repository = _securityRepository())
				{
					var acount = repository.GetAccountByName(user.UserName);
					if(acount == null)
					{
						return BadRequest("Acount not found");
					}
					acount.RegisterType = user.UserType.GetHashCode();
					acount.AccountState = (int)user.UserState;
					if (user.ApiAcounts != null)
					{
						var source = new ObservableCollection<foundationModel.ApiAccount>(user.ApiAcounts.Select(x=>x.ToFoundation()));
						var inventoryComparer = AnonymousComparer.Create((foundationModel.ApiAccount x) => x.AccountId);
						acount.ApiAccounts.ObserveCollection(x => repository.Add(x), x => repository.Remove(x));
						acount.ApiAccounts.Patch(source, inventoryComparer, (sourceAccount, targetAccount) => sourceAccount.Patch(targetAccount));
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
		/// <param name="password"></param>
		/// <returns></returns>
        [HttpPost]
        [Route("users/create")]
        public async Task<IHttpActionResult> CreateAsync(ApplicationUserExtended user, string password)
        {
            var dbUser = new ApplicationUser
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                UserName = user.UserName,
                LockoutEnabled = user.LockoutEnabled,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumber = user.PhoneNumber
            };

			IdentityResult result = null;
			if (!string.IsNullOrEmpty(password))
			{
				result = await UserManager.CreateAsync(dbUser, password);
			}
			else
			{
				result = await UserManager.CreateAsync(dbUser);
			}

            if (result.Succeeded)
            {
                var id = Guid.NewGuid().ToString();

                using (var repository = _securityRepository())
                {
                    var account = new Account
                    {
                        UserName = user.Email,
                        AccountId = user.Id,
                        AccountState = AccountState.Approved.GetHashCode(),
                        MemberId = id,
                        RegisterType = user.UserType.GetHashCode(),
                        StoreId = user.StoreId
                    };

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

		private async Task<ApplicationUserExtended> GetUserExtended(string userName)
        {
			ApplicationUserExtended retVal = null;
			var applicationUser = await UserManager.FindByNameAsync(userName);
			if(applicationUser != null)
			{
				retVal = new ApplicationUserExtended
				{
					Email = applicationUser.Email,
					EmailConfirmed = applicationUser.EmailConfirmed,
					Id = applicationUser.Id,
					LockoutEnabled = applicationUser.LockoutEnabled,
					PhoneNumber = applicationUser.PhoneNumber,
					LockoutEndDateUtc = applicationUser.LockoutEndDateUtc,
					PhoneNumberConfirmed = applicationUser.PhoneNumberConfirmed,
					SecurityStamp = applicationUser.SecurityStamp,
					UserName = applicationUser.UserName,
				};

				using (var repository = _securityRepository())
				{
					var user = repository.GetAccountByName(userName);

					if (user != null)
					{
						var permissions =
							user.RoleAssignments.Select(x => x.Role)
								.SelectMany(x => x.RolePermissions)
								.Select(x => x.Permission);

						retVal.FullName = user.UserName;
						retVal.UserState = (UserState)user.AccountState;
						retVal.UserType = (UserType)user.RegisterType;
						retVal.Permissions = permissions.Select(x => x.PermissionId).Distinct().ToArray();
						retVal.ApiAcounts = user.ApiAccounts.Select(x => x.ToWebModel()).ToList();
					}
				}
			}

        

            return retVal;
        }

        #endregion
    }
}
