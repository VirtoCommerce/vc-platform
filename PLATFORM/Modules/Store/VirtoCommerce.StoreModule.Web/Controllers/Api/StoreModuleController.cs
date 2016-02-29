using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Tax.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.StoreModule.Data.Notifications;
using VirtoCommerce.StoreModule.Web.Converters;
using VirtoCommerce.StoreModule.Web.Security;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;

namespace VirtoCommerce.StoreModule.Web.Controllers.Api
{
    [RoutePrefix("api/stores")]
    public class StoreModuleController : ApiController
    {
        private readonly IStoreService _storeService;
        private readonly IShippingMethodsService _shippingService;
        private readonly IPaymentMethodsService _paymentService;
        private readonly ITaxService _taxService;
        private readonly ISecurityService _securityService;
        private readonly IPermissionScopeService _permissionScopeService;
        private readonly INotificationManager _notificationManager;

        public StoreModuleController(IStoreService storeService, IShippingMethodsService shippingService, IPaymentMethodsService paymentService, ITaxService taxService,
                                     ISecurityService securityService, IPermissionScopeService permissionScopeService, INotificationManager notificationManager)
        {
            _storeService = storeService;
            _shippingService = shippingService;
            _paymentService = paymentService;
            _taxService = taxService;
            _securityService = securityService;
            _permissionScopeService = permissionScopeService;
            _notificationManager = notificationManager;
        }

        /// <summary>
        /// Search stores
        /// </summary>
        [HttpPost]
        [Route("search")]
        [OverrideAuthorization]
        public webModel.SearchResult SearchStores(coreModel.SearchCriteria criteria)
        {
            //Filter resulting stores correspond to current user permissions
            //first check global permission
            if (!_securityService.UserHasAnyPermission(User.Identity.Name, null, StorePredefinedPermissions.Read))
            {
                //Get user 'read' permission scopes
                criteria.StoreIds = _securityService.GetUserPermissions(User.Identity.Name)
                                                      .Where(x => x.Id.StartsWith(StorePredefinedPermissions.Read))
                                                      .SelectMany(x => x.AssignedScopes)
                                                      .OfType<StoreSelectedScope>()
                                                      .Select(x => x.Scope)
                                                      .ToArray();
            }
            var result = _storeService.SearchStores(criteria);
            var retVal = new webModel.SearchResult
            {
                TotalCount = result.TotalCount,
                Stores = result.Stores.Select(x => x.ToWebModel()).ToArray()
            };
            return retVal;
        }

        /// <summary>
        /// Get all stores
        /// </summary>
        [HttpGet]
        [ResponseType(typeof(webModel.Store[]))]
        [Route("")]
        [OverrideAuthorization]
        public IHttpActionResult GetStores()
        {
            var criteria = new coreModel.SearchCriteria
            {
                Skip = 0,
                Take = int.MaxValue
            };
            var retVal = SearchStores(criteria);
            return Ok(retVal.Stores);
        }

        /// <summary>
        /// Get store by id
        /// </summary>
        /// <param name="id">Store id</param>
        /// <responce code="404">Store not found</responce>
        /// <responce code="200">Store returned successfully OK</responce>
        [HttpGet]
        [ResponseType(typeof(webModel.Store))]
        [Route("{id}")]
        public IHttpActionResult GetStoreById(string id)
        {
            var store = _storeService.GetById(id);
            if (store == null)
            {
                return NotFound();
            }
            CheckCurrentUserHasPermissionForObjects(StorePredefinedPermissions.Read, store);
            var retVal = store.ToWebModel();
            retVal.SecurityScopes = _permissionScopeService.GetObjectPermissionScopeStrings(store).ToArray();
            return Ok(retVal);
        }


        /// <summary>
        /// Create store
        /// </summary>
        /// <param name="store">Store</param>
        [HttpPost]
        [ResponseType(typeof(webModel.Store))]
        [Route("")]
        [CheckPermission(Permission = StorePredefinedPermissions.Create)]
        public IHttpActionResult Create(webModel.Store store)
        {
            var coreStore = store.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods(), _taxService.GetAllTaxProviders());
            var retVal = _storeService.Create(coreStore);
            return Ok(retVal.ToWebModel());
        }

        /// <summary>
        /// Update store
        /// </summary>
        /// <param name="store">Store</param>
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Update(webModel.Store store)
        {
            var coreStore = store.ToCoreModel(_shippingService.GetAllShippingMethods(), _paymentService.GetAllPaymentMethods(), _taxService.GetAllTaxProviders());
            CheckCurrentUserHasPermissionForObjects(StorePredefinedPermissions.Update, coreStore);
            _storeService.Update(new[] { coreStore });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete stores
        /// </summary>
        /// <param name="ids">Ids of store that needed to delete</param>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Delete([FromUri] string[] ids)
        {
            var stores = ids.Select(x => _storeService.GetById(x)).ToArray();
            CheckCurrentUserHasPermissionForObjects(StorePredefinedPermissions.Delete, stores);
            _storeService.Delete(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Send dynamic notification (contains custom list of properties) an store or adminsitrator email 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("send/dynamicnotification")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SendDynamicNotificationAnStoreEmail(webModel.SendDynamicNotificationRequest request)
        {
            var store = _storeService.GetById(request.StoreId);

            if (store == null)
                throw new NullReferenceException(string.Format("no store with this id = {0}", request.StoreId));

            if (string.IsNullOrEmpty(store.Email) && string.IsNullOrEmpty(store.AdminEmail))
                throw new NullReferenceException(string.Format("set email or admin email for store with id = {0}", request.StoreId));

            var notification = _notificationManager.GetNewNotification<StoreDynamicEmailNotification>(request.StoreId, "Store", request.Language);

            notification.Recipient = !string.IsNullOrEmpty(store.Email) ? store.Email : store.AdminEmail;
            notification.IsActive = true;
            notification.FormType = request.Type;
            notification.Fields = request.Fields;

            _notificationManager.ScheduleSendNotification(notification);

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Check if given contact has login on behalf permission
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="id">Contact ID</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.LoginOnBehalfInfo))]
        [Route("{storeId}/accounts/{id}/loginonbehalf")]
        public async Task<IHttpActionResult> GetLoginOnBehalfInfo(string storeId, string id)
        {
            var result = new webModel.LoginOnBehalfInfo
            {
                UserName = id
            };

            var user = await _securityService.FindByIdAsync(id, UserDetails.Reduced);

            if (user != null)
            {
                //TODO: Check what requested user has permission to login on behalf in concrete store
                result.CanLoginOnBehalf = _securityService.UserHasAnyPermission(user.UserName, null, StorePredefinedPermissions.LoginOnBehalf);
            }

            return Ok(result);
        }

        /// <summary>
        /// Returns list of stores which user can sign in
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(webModel.Store[]))]
        [Route("allowed/{userId}")]
        public async Task<IHttpActionResult> GetUserAllowedStores(string userId)
        {
            var retVal = new List<webModel.Store>();
            var user = await _securityService.FindByIdAsync(userId, UserDetails.Reduced);
            if(user != null)
            {
                var storeIds = _storeService.GetUserAllowedStoreIds(user);
                retVal.AddRange(_storeService.GetByIds(storeIds.ToArray()).Select(x=>x.ToWebModel()));
            }
            return Ok(retVal.ToArray());
        }

        protected void CheckCurrentUserHasPermissionForObjects(string permission, params coreModel.Store[] objects)
        {
            //Scope bound security check
            var scopes = objects.SelectMany(x => _permissionScopeService.GetObjectPermissionScopeStrings(x)).Distinct().ToArray();
            if (!_securityService.UserHasAnyPermission(User.Identity.Name, scopes, permission))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }


    }
}
