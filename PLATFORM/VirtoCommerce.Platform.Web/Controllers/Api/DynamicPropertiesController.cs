using System;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
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
        /// GET: api/platform/dynamic/types
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
        /// GET: api/platform/dynamic/types/{typeName}/properties
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
        /// POST: api/platform/dynamic/types/{typeName}/properties
        /// </summary>
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
        /// DELETE: api/platform/dynamic/types/{typeName}/properties/{propertyId}
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
        /// GET: api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems
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
        /// POST: api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems
        /// </summary>
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
        /// DELETE: api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems
        /// </summary>
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
