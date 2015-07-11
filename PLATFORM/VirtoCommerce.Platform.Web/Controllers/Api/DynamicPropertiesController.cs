using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Converters.DynamicProperties;
using VirtoCommerce.Platform.Web.Model.DynamicProperties;

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
            var types = _service.GetObjectTypes();
            return Ok(types);
        }


        /// <summary>
        /// GET: api/platform/dynamic/types/{typeName}/properties
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Property[]))]
        [Route("types/{typeName}/properties")]
        public IHttpActionResult GetProperties(string typeName)
        {
            var properties = _service.GetProperties(typeName);
            var result = properties.Select(p => p.ToWebModel()).ToArray();
            return Ok(result);
        }

        /// <summary>
        /// POST: api/platform/dynamic/types/{typeName}/properties
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties")]
        public IHttpActionResult SaveProperties(string typeName, Property[] properties)
        {
            var coreProperties = properties.Select(p => p.ToCoreModel()).ToArray();
            _service.SaveProperties(coreProperties);
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
        [ResponseType(typeof(DictionaryItem[]))]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        public IHttpActionResult GetDictionaryItems(string typeName, string propertyId)
        {
            var items = _service.GetDictionaryItems(propertyId);
            var result = items.Select(p => p.ToWebModel()).ToArray();
            return Ok(result);
        }

        /// <summary>
        /// POST: api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        public IHttpActionResult SaveDictionaryItems(string typeName, string propertyId, DictionaryItem[] items)
        {
            var coreItems = items.Select(i => i.ToCoreModel()).ToArray();
            _service.SaveDictionaryItems(propertyId, coreItems);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// DELETE: api/platform/dynamic/types/{typeName}/properties/{propertyId}/dictionaryitems/{itemId}
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems/{itemId}")]
        public IHttpActionResult DeleteDictionaryItem(string typeName, string propertyId, string itemId)
        {
            _service.DeleteDictionaryItems(new[] { itemId });
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// GET: api/platform/dynamic/types/{typeName}/objects/{objectId}/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ObjectValue[]))]
        [Route("types/{typeName}/objects/{objectId}/values")]
        public IHttpActionResult GetObjectValues(string typeName, string objectId)
        {
            var values = _service.GetObjectValues(typeName, objectId);
            var result = values.Select(p => p.ToWebModel()).ToArray();
            return Ok(result);
        }
    }
}
