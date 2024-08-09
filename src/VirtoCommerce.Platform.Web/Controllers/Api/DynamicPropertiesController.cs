using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Exceptions;

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
        private readonly AbstractValidator<DynamicProperty> _dynamicPropertyTypeValidator;

        public DynamicPropertiesController(
            IDynamicPropertyRegistrar dynamicPropertyRegistrar,
            IDynamicPropertyService dynamicPropertyService,
            IDynamicPropertySearchService dynamicPropertySearchService,
            IDynamicPropertyDictionaryItemsService dynamicPropertyDictionaryItemsService,
            IDynamicPropertyDictionaryItemsSearchService dynamicPropertyDictionaryItemsSearchService,
            AbstractValidator<DynamicProperty> dynamicPropertyTypeValidator)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _dynamicPropertySearchService = dynamicPropertySearchService;
            _dynamicPropertyDictionaryItemsService = dynamicPropertyDictionaryItemsService;
            _dynamicPropertyDictionaryItemsSearchService = dynamicPropertyDictionaryItemsSearchService;
            _dynamicPropertyRegistrar = dynamicPropertyRegistrar;
            _dynamicPropertyTypeValidator = dynamicPropertyTypeValidator;
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

        [HttpGet]
        [Route("properties")]
        public async Task<ActionResult<DynamicProperty[]>> GetAllDynamicProperties([FromQuery] string id)
        {
            // The argument name is 'id' for compatibility with existing modules
            if (string.IsNullOrEmpty(id))
            {
                return Ok(Array.Empty<DynamicProperty>());
            }

            var criteria = AbstractTypeFactory<DynamicPropertySearchCriteria>.TryCreateInstance();
            criteria.ObjectType = id;

            var result = await _dynamicPropertySearchService.SearchAllNoCloneAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get dynamic properties registered for object type
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("properties/search")]
        public async Task<ActionResult<DynamicPropertySearchResult>> SearchDynamicProperties([FromBody] DynamicPropertySearchCriteria criteria)
        {
            var result = await _dynamicPropertySearchService.SearchNoCloneAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Add new dynamic property
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("properties")]
        [Authorize(PlatformConstants.Security.Permissions.DynamicPropertiesCreate)]
        public async Task<ActionResult<DynamicProperty>> CreatePropertyAsync([FromBody] DynamicProperty property)
        {
            var validationResult = await _dynamicPropertyTypeValidator.ValidateAsync(property);

            if (!validationResult.IsValid)
            {
                return BadRequest($"Validation failed for property: {validationResult.Errors?.FirstOrDefault()?.ErrorMessage}");
            }

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
        public async Task<ActionResult> UpdatePropertyAsync([FromBody] DynamicProperty property)
        {
            try
            {
                await _dynamicPropertyService.SaveDynamicPropertiesAsync(new[] { property });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

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
            try
            {
                await _dynamicPropertyService.DeleteDynamicPropertiesAsync(propertyIds);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("dictionaryitems")]
        public async Task<ActionResult<DynamicPropertyDictionaryItem[]>> GetAllDictionaryItems([FromQuery] string propertyId)
        {
            if (string.IsNullOrEmpty(propertyId))
            {
                return Ok(Array.Empty<DynamicPropertyDictionaryItem>());
            }

            var criteria = AbstractTypeFactory<DynamicPropertyDictionaryItemSearchCriteria>.TryCreateInstance();
            criteria.PropertyId = propertyId;

            var result = await _dynamicPropertyDictionaryItemsSearchService.SearchAllNoCloneAsync(criteria);
            return Ok(result);
        }

        /// <summary>
        /// Get dictionary items
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("dictionaryitems/search")]
        public async Task<ActionResult<DynamicPropertyDictionaryItemSearchResult>> SearchDictionaryItems([FromBody] DynamicPropertyDictionaryItemSearchCriteria criteria)
        {
            var result = await _dynamicPropertyDictionaryItemsSearchService.SearchNoCloneAsync(criteria);
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
        public async Task<ActionResult> SaveDictionaryItemsAsync([FromBody] DynamicPropertyDictionaryItem[] items)
        {
            try
            {
                await _dynamicPropertyDictionaryItemsService.SaveDictionaryItemsAsync(items);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidCollectionItemException ex)
            {
                return BadRequest(ex.Message);
            }

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
            try
            {
                await _dynamicPropertyDictionaryItemsService.DeleteDictionaryItemsAsync(ids);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
