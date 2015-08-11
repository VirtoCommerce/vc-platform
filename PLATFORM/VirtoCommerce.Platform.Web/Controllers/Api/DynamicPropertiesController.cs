using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [RoutePrefix("api/platform/dynamic")]
    [CheckPermission(Permission = PredefinedPermissions.DynamicPropertiesManage)]
    public class DynamicPropertiesController : ApiController
    {
        private readonly IDynamicPropertyService _service;

        public DynamicPropertiesController(IDynamicPropertyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(string[]))]
        [Route("types")]
        public IHttpActionResult GetObjectTypes()
        {
            var types = _service.GetAvailableObjectTypeNames();
            return Ok(types);
        }

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(DynamicProperty[]))]
        [Route("types/{typeName}/properties")]
        public IHttpActionResult GetProperties(string typeName)
        {
            var properties = _service.GetProperties(typeName);
            return Ok(properties);
        }

        /// <summary>
        /// Add or update dynamic properties
        /// </summary>
        /// <remarks>
        /// Fill property ID to update existing property or leave it empty to create a new property.
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties")]
        public IHttpActionResult SaveProperties(string typeName, DynamicProperty[] properties)
        {
            foreach (var property in properties.Where(property => string.IsNullOrEmpty(property.ObjectType)))
            {
                property.ObjectType = typeName;
            }

            _service.SaveProperties(properties);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties/{propertyId}")]
        public IHttpActionResult DeleteProperty(string typeName, string propertyId)
        {
            _service.DeleteProperties(new[] { propertyId });
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(DynamicPropertyDictionaryItem[]))]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        public IHttpActionResult GetDictionaryItems(string typeName, string propertyId)
        {
            var items = _service.GetDictionaryItems(propertyId);
            return Ok(items);
        }

        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        public IHttpActionResult SaveDictionaryItems(string typeName, string propertyId, DynamicPropertyDictionaryItem[] items)
        {
            _service.SaveDictionaryItems(propertyId, items);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="propertyId"></param>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        public IHttpActionResult DeleteDictionaryItem(string typeName, string propertyId, [FromUri] string[] ids)
        {
            _service.DeleteDictionaryItems(ids);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
