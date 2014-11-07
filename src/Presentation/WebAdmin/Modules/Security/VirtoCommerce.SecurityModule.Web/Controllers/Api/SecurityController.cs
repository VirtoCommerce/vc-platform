using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.SecurityModule.Web.Data;
using VirtoCommerce.SecurityModule.Web.Models;
using VirtoCommerce.Web.Client.Services.Security;

namespace VirtoCommerce.SecurityModule.Web.Controllers
{
    [RoutePrefix("api/security")]
	public class SecurityController : ApiController
	{
        private readonly Func<IFoundationSecurityRepository> _securityRepository;
        private readonly Func<IFoundationCustomerRepository> _customerRepository;
        private readonly Func<IUserIdentitySecurity> _security;
        public SecurityController(Func<IFoundationSecurityRepository> securityRepository, Func<IFoundationCustomerRepository> customerRepository, Func<IUserIdentitySecurity> security)
        {
            _securityRepository = securityRepository;
            _customerRepository = customerRepository;
            _security = security;
            //_security = new WebUserSecurity(_securityRepository, "VirtoCommerce");
        }

        [HttpPost]
		[ActionName("login")]
        [Route("login")]
		public async Task<IHttpActionResult> Login(UserLogin model)
		{
            if (await _security().LoginAsync(model.UserName, model.Password, model.RememberMe) == SignInStatus.Success)
            {
                return Ok(GetUserInfo(model.UserName));
            }

             return StatusCode(HttpStatusCode.Unauthorized);
		}

		[Authorize]
		[HttpGet]
		[ActionName("usersession")]
        [Route("usersession")]
		public  IHttpActionResult GetUserSession()
		{
            return Ok(GetUserInfo(User.Identity.Name));
		}

		[HttpPost]
		[ActionName("logout")]
        [Route("logout")]
		public IHttpActionResult Logout()
		{
            _security().Logout();
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
	}
}
