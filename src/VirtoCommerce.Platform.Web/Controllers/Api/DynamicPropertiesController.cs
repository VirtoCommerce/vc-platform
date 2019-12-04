using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/platform/dynamic")]
    [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesQuery)]
    public class DynamicPropertiesController : Controller
    {
        private readonly IDynamicPropertyRegistrar _dynamicPropertyRegistrar;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly IDynamicPropertySearchService _dynamicPropertySearchService;
        private readonly IDynamicPropertyDictionaryItemsService _dynamicPropertyDictionaryItemsService;
        private readonly IDynamicPropertyDictionaryItemsSearchService _dynamicPropertyDictionaryItemsSearchService;

        public DynamicPropertiesController(IDynamicPropertyRegistrar dynamicPropertyRegistrar, IDynamicPropertyService dynamicPropertyService, IDynamicPropertySearchService dynamicPropertySearchService, IDynamicPropertyDictionaryItemsService dynamicPropertyDictionaryItemsService, IDynamicPropertyDictionaryItemsSearchService dynamicPropertyDictionaryItemsSearchService)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _dynamicPropertySearchService = dynamicPropertySearchService;
            _dynamicPropertyDictionaryItemsService = dynamicPropertyDictionaryItemsService;
            _dynamicPropertyDictionaryItemsSearchService = dynamicPropertyDictionaryItemsSearchService;
            _dynamicPropertyRegistrar = dynamicPropertyRegistrar;
        }

        /// <summary>
        /// Get object types which support dynamic properties
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]
        public ActionResult<string[]> GetObjectTypes()
        {
            return Ok(_dynamicPropertyRegistrar.AllRegisteredTypeNames);
        }

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("properties/search")]

        public async Task<ActionResult<DynamicPropertySearchResult>> SearchDynamicProperties([FromBody] DynamicPropertySearchCriteria criteria)
        {
            var result = await _dynamicPropertySearchService.SearchDynamicPropertiesAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("properties")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesCreate)]
        public async Task<ActionResult<DynamicProperty>> CreatePropertyAsync([FromBody]DynamicProperty property)
        {
            var result = await _dynamicPropertyService.SaveDynamicPropertiesAsync(new[] { property });
            return Ok(result.FirstOrDefault());
        }

        /// <summary>
        /// Does nothing. Just a way to expose DynamicObjectProperty thru Swagger.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(DynamicObjectProperty))]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public ActionResult ExposeDynamicObjectProperty()
        {
            return NoContent();
        }

        /// <summary>
        /// Update existing dynamic property
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("properties")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdatePropertyAsync([FromBody]DynamicProperty property)
        {
            await _dynamicPropertyService.SaveDynamicPropertiesAsync(new[] { property });
            return NoContent();
        }

        /// <summary>
        /// Delete dynamic property
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("properties")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesDelete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeletePropertyAsync([FromQuery] string[] propertyIds)
        {
            await _dynamicPropertyService.DeleteDynamicPropertiesAsync(propertyIds);
            return NoContent();
        }

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("dictionaryitems/search")]
        public async Task<ActionResult<DynamicPropertyDictionaryItemSearchResult>> SearchDictionaryItems([FromBody]DynamicPropertyDictionaryItemSearchCriteria criteria)
        {
            var result = await _dynamicPropertyDictionaryItemsSearchService.SearchDictionaryItemsAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Add or update dictionary items
        /// </summary>
        /// <remarks>
        /// Fill item ID to update existing item or leave it empty to create a new item.
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [Route("dictionaryitems")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveDictionaryItemsAsync([FromBody]DynamicPropertyDictionaryItem[] items)
        {
            await _dynamicPropertyDictionaryItemsService.SaveDictionaryItemsAsync(items);
            return NoContent();
        }

        /// <summary>
        /// Delete dictionary items
        /// </summary>
        /// <param name="ids">IDs of dictionary items to delete.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("dictionaryitems")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDictionaryItemAsync([FromQuery] string[] ids)
        {
            await _dynamicPropertyDictionaryItemsService.DeleteDictionaryItemsAsync(ids);
            return NoContent();
        }


        #region Legacy API methods left for backward compatibility

        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete("use POST api/platform/dynamic/properties/search instead")]
        [HttpGet]
        [Route("types/{typeName}/properties")]
        public async Task<ActionResult<DynamicProperty[]>> GetProperties([FromRoute] string typeName)
        {
            var result = await _dynamicPropertySearchService.SearchDynamicPropertiesAsync(new DynamicPropertySearchCriteria { ObjectType = typeName });
            return Ok(result.Results);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete("use POST api/platform/dynamic/properties  instead")]
        [HttpPost]
        [Route("types/{typeName}/properties")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesCreate)]
        public async Task<ActionResult<DynamicProperty>> CreateProperty([FromRoute] string typeName, [FromBody] DynamicProperty property)
        {
            property.Id = null;
            if (string.IsNullOrEmpty(property.ObjectType))
            {
                property.ObjectType = typeName;
            }
            await _dynamicPropertyService.SaveDynamicPropertiesAsync(new[] { property });
            return Ok(property);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete("use PUT api/platform/dynamic/properties  instead")]
        [HttpPut]
        [Route("types/{typeName}/properties/{propertyId}")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateProperty([FromRoute] string typeName, [FromRoute] string propertyId, [FromBody] DynamicProperty property)
        {
            property.Id = propertyId;

            if (string.IsNullOrEmpty(property.ObjectType))
            {
                property.ObjectType = typeName;
            }
            await _dynamicPropertyService.SaveDynamicPropertiesAsync(new[] { property });
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Obsolete("use DELETE api/platform/dynamic/properties?propertyIds=  instead")]
        [Route("types/{typeName}/properties/{propertyId}")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesDelete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteProperty([FromRoute] string typeName, [FromRoute] string propertyId)
        {
            await _dynamicPropertyService.DeleteDynamicPropertiesAsync(new[] { propertyId });
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete("use POST api/platform/dynamic/dictionaryitems/search instead")]
        [HttpGet]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        public async Task<ActionResult<DynamicPropertyDictionaryItem[]>> GetDictionaryItems([FromRoute] string typeName, [FromRoute] string propertyId)
        {
            var result = await _dynamicPropertyDictionaryItemsSearchService.SearchDictionaryItemsAsync(new DynamicPropertyDictionaryItemSearchCriteria { PropertyId = propertyId, ObjectType = typeName });
            return Ok(result.Results);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete("use POST api/platform/dynamic/dictionaryitems instead")]
        [HttpPost]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveDictionaryItems([FromRoute] string typeName, [FromRoute] string propertyId, [FromBody] DynamicPropertyDictionaryItem[] items)
        {
            foreach (var item in items)
            {
                item.PropertyId = propertyId;
            }
            await _dynamicPropertyDictionaryItemsService.SaveDictionaryItemsAsync(items);
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Obsolete("use DELETE api/platform/dynamic/dictionaryitems?ids= instead")]
        [HttpDelete]
        [Route("types/{typeName}/properties/{propertyId}/dictionaryitems")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesUpdate)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteDictionaryItem([FromRoute] string typeName, [FromRoute] string propertyId, [FromQuery] string[] ids)
        {
            await _dynamicPropertyDictionaryItemsService.DeleteDictionaryItemsAsync(ids);
            return NoContent();
        }
        #endregion
    }
}
