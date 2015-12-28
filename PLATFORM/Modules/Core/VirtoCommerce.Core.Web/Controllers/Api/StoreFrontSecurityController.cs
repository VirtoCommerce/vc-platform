﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.CoreModule.Web.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Notifications;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
    [RoutePrefix("api/storefront/security")]
    public class StorefrontSecurityController : ApiController
    {
        private readonly ISecurityService _securityService;
        private readonly Func<ApplicationSignInManager> _signInManagerFactory;
        private readonly INotificationManager _notificationManager;
        private readonly IStoreService _storeService;
        private readonly IContactService _contactService;

        public StorefrontSecurityController(ISecurityService securityService, Func<ApplicationSignInManager> signInManagerFactory, INotificationManager notificationManager, IStoreService storeService, IContactService contactService)
        {
            _securityService = securityService;
            _signInManagerFactory = signInManagerFactory;
            _notificationManager = notificationManager;
            _storeService = storeService;
            _contactService = contactService;
        }

        private ApplicationSignInManager _signInManager;
        private ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? (_signInManager = _signInManagerFactory());
            }
        }

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/id/{userId}")]
        [ResponseType(typeof(ApplicationUserExtended))]
        public async Task<IHttpActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await _securityService.FindByIdAsync(userId, UserDetails.Reduced);

            return Ok(user);
        }

        /// <summary>
        /// Get user details by user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/name/{userName}")]
        [ResponseType(typeof(ApplicationUserExtended))]
        public async Task<IHttpActionResult> GetUserByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest();
            }

            var user = await _securityService.FindByNameAsync(userName, UserDetails.Reduced);

            return Ok(user);
        }

        /// <summary>
        /// Get user details by external login provider
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/external")]
        [ResponseType(typeof(ApplicationUserExtended))]
        public async Task<IHttpActionResult> GetUserByLogin(string loginProvider, string providerKey)
        {
            if (string.IsNullOrEmpty(loginProvider) || string.IsNullOrEmpty(providerKey))
            {
                return BadRequest();
            }

            var user = await _securityService.FindByLoginAsync(loginProvider, providerKey, UserDetails.Reduced);

            return Ok(user);
        }

        /// <summary>
        /// Sign in with user name and password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/signin")]
        [ResponseType(typeof(SignInResult))]
        public async Task<IHttpActionResult> PasswordSignIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }

            var status = await SignInManager.PasswordSignInAsync(userName, password, false, shouldLockout: true);
            var result = new SignInResult { Status = status };

            return Ok(result);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user")]
        [ResponseType(typeof(SecurityResult))]
        public async Task<IHttpActionResult> Create(ApplicationUserExtended user)
        {
            if (user != null)
            {
                user.PasswordHash = null;
                user.SecurityStamp = null;
            }

            var result = await _securityService.CreateAsync(user);

            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        /// <summary>
        /// Generate a password reset token
        /// </summary>
        /// <remarks>
        /// Generates a password reset token and sends a password reset link to the user via email.
        /// </remarks>
        /// <param name="userId"></param>
        /// <param name="storeName"></param>
        /// <param name="language"></param>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/password/resettoken")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> GenerateResetPasswordToken(string userId, string storeName, string language, string callbackUrl)
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

            var notification = _notificationManager.GetNewNotification<ResetPasswordEmailNotification>(storeName, "Store", language);
            notification.Url = uriBuilder.ToString();

            var store = _storeService.GetById(storeName);
            notification.Sender = store.Email;
            notification.IsActive = true;

            var contact = _contactService.GetById(userId);
            if (contact != null)
            {
                var email = contact.Emails.FirstOrDefault();
                if (!string.IsNullOrEmpty(email))
                {
                    notification.Recipient = email;
                }
            }

            _notificationManager.ScheduleSendNotification(notification);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Reset a password for the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/password/reset")]
        [ResponseType(typeof(SecurityResult))]
        public async Task<IHttpActionResult> ResetPassword(string userId, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword))
            {
                return BadRequest();
            }

            var result = await _securityService.ResetPasswordAsync(userId, token, newPassword);

            return Ok(result);
        }
    }
}
