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
    [RoutePrefix("api/dynamicproperties")]
    [CheckPermission(Permission = PredefinedPermissions.DynamicPropertiesManage)]
    public class DynamicPropertiesController : ApiController
    {
        private readonly IDynamicPropertyService _service;

        public DynamicPropertiesController(IDynamicPropertyService service)
        {
            _service = service;
        }

        /// <summary>
        /// GET: api/dynamicproperties/objecttypes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(string[]))]
        [Route("objecttypes")]
        public IHttpActionResult GetObjectTypes()
        {
            var types = _service.GetObjectTypes();
            return Ok(types);
        }

        /// <summary>
        /// GET: api/dynamicproperties/objecttypes/MyObjectType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Property[]))]
        [Route("objecttypes/{objectType}")]
        public IHttpActionResult GetProperties(string objectType)
        {
            var properties = _service.GetProperties(objectType);
            var result = properties.Select(p => p.ToWebModel()).ToArray();
            return Ok(result);
        }
        /// <summary>
        /// GET: api/dynamicproperties/objecttypes/MyObjectType/MyObjectId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Property[]))]
        [Route("objecttypes/{objectType}/{objectId}")]
        public IHttpActionResult GetObjectValues(string objectType, string objectId)
        {
            var properties = _service.GetObjectValues(objectType, objectId);
            var result = properties.Select(p => p.ToWebModel()).ToArray();
            return Ok(result);
        }

        /// <summary>
        /// GET: api/dynamicproperties/dictionary/MyPropertyId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(DictionaryItem[]))]
        [Route("dictionary/{propertyId}")]
        public IHttpActionResult GetDictionaryItems(string propertyId)
        {
            var items = _service.GetDictionaryItems(propertyId);
            var result = items.Select(p => p.ToWebModel()).ToArray();
            return Ok(result);
        }


        /// <summary>
        /// POST: api/dynamicproperties
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult Update(Property property)
        {
            var properties = new[] { property.ToCoreModel() };

            if (!string.IsNullOrEmpty(property.ObjectId))
                _service.SaveObjectValues(properties);
            else
                _service.SaveProperties(properties);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// DELETE: api/dynamicproperties/MyPropertyId
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(void))]
        [Route("{propertyId}")]
        public IHttpActionResult Delete(string propertyId)
        {
            _service.DeleteProperties(new[] { propertyId });
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
