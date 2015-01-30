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
using Address = VirtoCommerce.CoreModule.Web.Security.Models.Address;

namespace VirtoCommerce.SecurityModule.Web.Controllers
{
    [RoutePrefix("api/security")]
	public class SecurityController : ApiController
	{
        private readonly Func<IFoundationSecurityRepository> _securityRepository;
        private readonly Func<IFoundationCustomerRepository> _customerRepository;

        private IAuthenticationManager _authenticationManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public SecurityController(Func<IFoundationSecurityRepository> securityRepository, Func<IFoundationCustomerRepository> customerRepository)
        {
            _securityRepository = securityRepository;
            _customerRepository = customerRepository;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? (_signInManager = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return _authenticationManager ?? (_authenticationManager = HttpContext.Current.GetOwinContext().Authentication);
            }
        }

        #region Internal Web admin actions

        [HttpPost]
		[Route("login")]
       	public async Task<IHttpActionResult> Login(UserLogin model)
		{
            if (await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true) == SignInStatus.Success)
            {
                return Ok(GetUserInfo(model.UserName));
            }

             return StatusCode(HttpStatusCode.Unauthorized);
		}

		[Authorize]
		[HttpGet]
	    [Route("usersession")]
        [ResponseType(typeof(AuthInfo))]
		public IHttpActionResult GetUserSession()
		{
            return Ok(GetUserInfo(User.Identity.Name));
		}

		[HttpPost]
	    [Route("logout")]
		public IHttpActionResult Logout()
		{
            AuthenticationManager.SignOut();
			return Ok(new { status = true });
		}

        #endregion

        #region Public methods

        [HttpGet]
        [Route("usersession/{userName}")]
        [ResponseType(typeof(AuthInfoExtended))]
        public IHttpActionResult GetUserSession(string userName)
        {
            return Ok(GetUserInfo(userName));
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

        [HttpPost]
        [Route("users/delete")]
        public async Task<IHttpActionResult> DeleteAsync(string userId)
        {
            var dbUser = await UserManager.FindByIdAsync(userId);
            if (dbUser == null)
            {
                return NotFound();
            }
            var result = await UserManager.DeleteAsync(dbUser);

            if (result.Succeeded)
            {
                //TODO delete account and contact
                return Ok();
            }

            return BadRequest(String.Join(" ", result.Errors));
        }

        [HttpPost]
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
            //dbUser.UserName = user.UserName;
            dbUser.SecurityStamp = user.SecurityStamp;
            dbUser.TwoFactorEnabled = user.TwoFactorEnabled;

            var result = await UserManager.UpdateAsync(dbUser);

            if (result.Succeeded)
            {
                //TODO update account and contant
                return Ok();
            }

            return BadRequest(String.Join(" ", result.Errors));
        }

        [HttpPost]
        [Route("users/create")]
        public async Task<IHttpActionResult> CreateAsync(ApplicationUserExtended user)
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

            var result = await UserManager.CreateAsync(dbUser);

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
                        RegisterType = RegisterType.GuestUser.GetHashCode(),
                        StoreId = user.StoreId
                    };

                    repository.Add(account);
                    repository.UnitOfWork.Commit();
                }

                using (var repository = _customerRepository())
                {
                    var contact = new Contact
                    {
                        MemberId = id,
                        FullName = user.FullName
                    };

                    contact.Emails.Add(new Email { Address = user.Email, MemberId = id, Type = EmailType.Primary.ToString() });

                    if (!string.IsNullOrEmpty(user.Icon))
                    {
                        try
                        {
                            var iconBytes = Convert.FromBase64String(user.Icon);
                            contact.Photo = iconBytes;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    repository.Add(contact);
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

        private AuthInfo GetUserInfo(string userName)
        {
            AuthInfoExtended retVal = null;
            using (var repository = _securityRepository())
            {
                var user = repository.GetAccount(userName);

                if (user != null)
                {
                    var permissions =
                        user.RoleAssignments.Select(x => x.Role)
                            .SelectMany(x => x.RolePermissions)
                            .Select(x => x.Permission);


                    retVal = new AuthInfoExtended
                    {
                        Id = user.MemberId,
                        AccountId = user.AccountId,
                        Login = user.UserName,
                        FullName = user.UserName,
                        AccountState = user.AccountState,
                        UserType = (RegisterType)user.RegisterType,
                        Permissions = permissions.Select(x => x.PermissionId).Distinct().ToArray()
                    };


                    using (var customerRep = _customerRepository())
                    {
                        var contact = customerRep.GetContact(user.MemberId);
                        //Account should allways have associated contact info
                        if (contact != null)
                        {
                            retVal.FullName = contact.FullName;
                            retVal.Properties = contact.ContactPropertyValues.ToDictionary(x => x.Name, x => x.ToString());
                            if (contact.Addresses != null)
                            {
                                retVal.Addresses = contact.Addresses.Select(x => new Address().InjectFrom(x)).Cast<Address>().ToArray();
                            }

                            if (contact.Emails != null)
                            {
                                var email = contact.Emails.FirstOrDefault(x => x.Type == EmailType.Primary.ToString());

                                if (email != null)
                                {
                                    retVal.Email = email.Address;
                                }
                            }
                        }
                    }

                }
            }

            return retVal;
        }

        #endregion
    }
}
