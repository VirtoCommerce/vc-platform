using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Security.Models;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Security.Model;

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
		public  IHttpActionResult GetUserSession()
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


        private AuthInfo GetUserInfo(string userName)
        {
            using (var repository = _securityRepository())
            {
                var user = repository.GetAccount(userName);

                if (user != null)
                {
                    var permissions =
                        user.RoleAssignments.Select(x => x.Role)
                            .SelectMany(x => x.RolePermissions)
                            .Select(x => x.Permission);

                    string fullname = user.UserName;

                    using (var customerRep = _customerRepository())
                    {
                        var contact = customerRep.GetContact(user.MemberId);
                        //Account should allways have associated contact info
                        if (contact != null)
                        {
                            fullname = contact.FullName;
                        }
                    }

                    return new AuthInfo
                    {
                        Login = user.UserName,
                        FullName = fullname,
                        UserType = (RegisterType) user.RegisterType,
                        Permissions = permissions.Select(x => x.PermissionId).Distinct().ToArray()
                    };
                }
            }

            return null;
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

        #endregion

        [HttpPost]
        [Route("users/create")]
        public async Task<IHttpActionResult> CreateAsync(ApplicationUserExtended user)
        {
            var result = await UserManager.CreateAsync(user);

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

                    if (string.IsNullOrEmpty(user.Icon))
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
    }
}
