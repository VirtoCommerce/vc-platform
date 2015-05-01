using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Web.Converters;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.MarketingModule.Web.Model;
using coreModel = VirtoCommerce.Domain.Marketing.Model;
namespace VirtoCommerce.MarketingModule.Web.Controllers.Api
{
	[RoutePrefix("api/marketing")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
    public class DynamicContentController: ApiController
    {
		private readonly IDynamicContentService _dynamicContentService;
		public DynamicContentController(IDynamicContentService dynamicContentService)
		{
			_dynamicContentService = dynamicContentService;
		}

		// GET: api/marketing/contentitems/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.DynamicContentItem))]
		[Route("contentitems/{id}")]
		public IHttpActionResult GetDynamicContentById(string id)
		{
			var retVal = _dynamicContentService.GetContentItemById(id);
			if (retVal != null)
			{
				return Ok(retVal.ToWebModel());
			}
			return NotFound();
		}


		// POST: api/marketing/contentitems
		[HttpPost]
		[ResponseType(typeof(webModel.DynamicContentItem))]
		[Route("contentitems")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreateDynamicContent(webModel.DynamicContentItem content)
		{
			var retVal = _dynamicContentService.CreateContent(content.ToCoreModel());
			return GetDynamicContentById(retVal.Id);
		}


		// PUT: api/marketing/content/tems
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("contentitems")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdateDynamicContent(webModel.DynamicContentItem content)
		{
			_dynamicContentService.UpdateContents(new coreModel.DynamicContentItem[] { content.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/contentitems?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("contentitems")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteDynamicContents([FromUri] string[] ids)
		{
			_dynamicContentService.DeleteContents(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}


		// GET: api/marketing/contentplaces/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.DynamicContentPlace))]
		[Route("contentplaces/{id}")]
		public IHttpActionResult GetDynamicContentPlaceById(string id)
		{
			var retVal = _dynamicContentService.GetPlaceById(id);
			if (retVal != null)
			{
				return Ok(retVal.ToWebModel());
			}
			return NotFound();
		}


		// POST: api/marketing/contentplaces
		[HttpPost]
		[ResponseType(typeof(webModel.DynamicContentPlace))]
		[Route("contentplaces")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreateDynamicContentPlace(webModel.DynamicContentPlace place)
		{
			var retVal = _dynamicContentService.CreatePlace(place.ToCoreModel());
			return GetDynamicContentPlaceById(retVal.Id);
		}


		// PUT: api/marketing/contentplaces
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("contentplaces")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdateDynamicContentPlace(webModel.DynamicContentPlace place)
		{
			_dynamicContentService.UpdatePlace(place.ToCoreModel());
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/contentplaces?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("contentplaces")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteDynamicContentPlaces([FromUri] string[] ids)
		{
			_dynamicContentService.DeletePlaces(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}


		// GET: api/marketing/contentpublications/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.DynamicContentPublication))]
		[Route("contentpublications/{id}")]
		public IHttpActionResult GetDynamicContentPublicationById(string id)
		{
			var retVal = _dynamicContentService.GetPublicationById(id);
			if (retVal != null)
			{
				return Ok(retVal.ToWebModel());
			}
			return NotFound();
		}


		// POST: api/marketing/contentpublications
		[HttpPost]
		[ResponseType(typeof(webModel.DynamicContentPublication))]
		[Route("contentpublications")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreateDynamicContent(webModel.DynamicContentPublication publication)
		{
			var retVal = _dynamicContentService.CreatePublication(publication.ToCoreModel());
			return GetDynamicContentPublicationById(retVal.Id);
		}


		// PUT: api/marketing/contentpublications
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("contentpublications")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdateDynamicContentPublication(webModel.DynamicContentPublication publication)
		{
			_dynamicContentService.UpdatePublications(new coreModel.DynamicContentPublication[] { publication.ToCoreModel() });
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/contentpublications?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("contentpublications")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteDynamicContentPublications([FromUri] string[] ids)
		{
			_dynamicContentService.DeletePublications(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}

		// GET: api/marketing/contentfolders/{id}
		[HttpGet]
		[ResponseType(typeof(webModel.DynamicContentFolder))]
		[Route("contentfolders/{id}")]
		public IHttpActionResult GetDynamicContentFolderById(string id)
		{
			var retVal = _dynamicContentService.GetFolderById(id);
			if (retVal != null)
			{
				return Ok(retVal.ToWebModel());
			}
			return NotFound();
		}


		// POST: api/marketing/contentfolders
		[HttpPost]
		[ResponseType(typeof(webModel.DynamicContentFolder))]
		[Route("contentfolders")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult CreateDynamicContentFolder(webModel.DynamicContentFolder folder)
		{
			var retVal = _dynamicContentService.CreateFolder(folder.ToCoreModel());
			return GetDynamicContentFolderById(retVal.Id);
		}


		// PUT: api/marketing/contentfolders
		[HttpPut]
		[ResponseType(typeof(void))]
		[Route("contentfolders")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult UpdateDynamicContentFolder(webModel.DynamicContentFolder folder)
		{
			_dynamicContentService.UpdateFolder(folder.ToCoreModel());
			return StatusCode(HttpStatusCode.NoContent);
		}

		// DELETE: api/marketing/contentfolders?ids=21
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("contentfolders")]
        [CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteDynamicContentFolders([FromUri] string[] ids)
		{
			_dynamicContentService.DeleteFolder(ids);
			return StatusCode(HttpStatusCode.NoContent);
		}

	
    }
}
