using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Hangfire;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/security/frontend")]
    public class FrontendSecurityController : ApiController
    {
        private readonly ISecurityService _securityService;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;

        public FrontendSecurityController(ISecurityService securityService, Func<ApplicationSignInManager> signInManagerFactory)
        {
            _securityService = securityService;
            _signInManagerFactory = signInManagerFactory;
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? (_signInManager = _signInManagerFactory());
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

            var user = await _securityService.FindByIdAsync(userId, UserDetails.Reduced);

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

            var user = await _securityService.FindByNameAsync(userName, UserDetails.Reduced);

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

            var user = await _securityService.FindByLoginAsync(loginProvider, providerKey, UserDetails.Reduced);

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
        public async Task<IHttpActionResult> Create(ApplicationUserExtended user)
        {
            var result = await _securityService.CreateAsync(user);

            if (result == null)
                return BadRequest();

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

            string token = await _securityService.GeneratePasswordResetTokenAsync(userId);

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

            var result = await _securityService.ResetPasswordAsync(userId, token, newPassword);

            return Ok(result);
        }


        public void SendEmail(string userId, string subject, string message)
        {
            // TODO: Use notifications
        }
    }
}
