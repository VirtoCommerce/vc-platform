using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Omu.ValueInjecter;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Security.Data;
using VirtoCommerce.CoreModule.Web.Security.Models;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [RoutePrefix("api/security/frontend")]
    public class FrontendSecurityController : ApiController
    {
        private readonly Func<IFoundationSecurityRepository> _securityRepository;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly Func<ApplicationUserManager> _userManagerFactory;
        private readonly IApiAccountProvider _apiAccountProvider;

        public FrontendSecurityController(
            Func<IFoundationSecurityRepository> securityRepository, Func<ApplicationSignInManager> signInManagerFactory,
            Func<ApplicationUserManager> userManagerFactory, Func<IAuthenticationManager> authManagerFactory,
            IApiAccountProvider apiAccountProvider)
        {
            _securityRepository = securityRepository;
            _signInManagerFactory = signInManagerFactory;
            _userManagerFactory = userManagerFactory;
            _apiAccountProvider = apiAccountProvider;
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? _signInManagerFactory();
            }
        }

        private ApplicationUserManager _userManager;
        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? _userManagerFactory();
            }
        }

        //
        // GET: /api/security/frontend/user/id
        [HttpGet]
        [Route("user/id/{userId}")]
        public async Task<IHttpActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await UserManager.FindByIdAsync(userId);

            return Ok(user);
        }

        //
        // GET: /api/security/frontend/user/name
        [HttpGet]
        [Route("user/name/{userName}")]
        public async Task<IHttpActionResult> GetUserByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest();
            }

            var user = await UserManager.FindByNameAsync(userName);

            return Ok(user);
        }

        //
        // GET: /api/security/frontend/user/login
        [HttpGet]
        [Route("user/login")]
        public async Task<IHttpActionResult> GetUserByLogin(string loginProvider, string providerKey)
        {
            if (string.IsNullOrEmpty(loginProvider) || string.IsNullOrEmpty(providerKey))
            {
                return BadRequest();
            }

            var loginInfo = new UserLoginInfo(loginProvider, providerKey);

            var user = await UserManager.FindAsync(loginInfo);

            return Ok(user);
        }

        //
        // POST: /api/security/frontend/user/signin
        [HttpPost]
        [Route("user/signin")]
        public async Task<IHttpActionResult> PasswordSignIn(string userName, string password, bool isPersistent)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }

            var result = await SignInManager.PasswordSignInAsync(userName, password, isPersistent, shouldLockout: true);

            return Ok(result);
        }

        //
        // POST: /api/security/frontend/user
        [HttpPost]
        [Route("user")]
        public async Task<IHttpActionResult> Register(ApplicationUserExtended user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var dbUser = new ApplicationUser();
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

            IdentityResult result = null;

            if (string.IsNullOrEmpty(user.Password))
            {
                result = await UserManager.CreateAsync(dbUser);
            }
            else
            {
                result = await UserManager.CreateAsync(dbUser, user.Password);
            }

            if (result.Succeeded)
            {
                using (var repository = _securityRepository())
                {
                    var account = new Account
                    {
                        AccountId = user.Id,
                        AccountState = (int)AccountState.Approved,
                        MemberId = user.MemberId,
                        RegisterType = (int)user.UserType,
                        StoreId = user.StoreId,
                        UserName = user.UserName
                    };

                    repository.Add(account);
                    repository.UnitOfWork.Commit();
                }
            }

            return Ok(result);
        }

        //
        // POST: /api/security/frontend/user/password/resettoken
        [HttpPost]
        [Route("user/password/resettoken")]
        public async Task<IHttpActionResult> GenerateResetPasswordToken(string userId, string storeName, string callbackUrl)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(storeName) || string.IsNullOrEmpty(callbackUrl))
            {
                return BadRequest();
            }

            string token = await UserManager.GeneratePasswordResetTokenAsync(userId);

            var uriBuilder = new UriBuilder(callbackUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["code"] = token;
            uriBuilder.Query = query.ToString();

            string message = string.Format(
                "Please reset your password by clicking <strong><a href=\"{0}\">here</a></strong>",
                HttpUtility.HtmlEncode(uriBuilder.ToString()));
            string subject = string.Format("\"{0}\" reset password link", storeName);

            BackgroundJob.Enqueue(() => SendEmail(userId, subject, message));

            return Ok();
        }

        //
        // POST: /api/security/frontend/user/password/reset
        [HttpPost]
        [Route("user/password/reset")]
        public async Task<IHttpActionResult> ResetPassword(string userId, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                return BadRequest();
            }

            var result = await UserManager.ResetPasswordAsync(userId, token, newPassword);

            return Ok(result);
        }


        public void SendEmail(string userId, string subject, string message)
        {
            UserManager.SendEmail(userId, subject, message);
        }
    }
}