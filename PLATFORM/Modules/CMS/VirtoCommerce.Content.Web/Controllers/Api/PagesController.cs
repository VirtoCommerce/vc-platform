using System.Net;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Content.Web.Controllers.Api
{
	#region

	using System;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Description;
	using VirtoCommerce.Content.Data.Services;
	using VirtoCommerce.Content.Web.Models;
	using VirtoCommerce.Content.Web.Converters;
	using System.Collections.Generic;
	using VirtoCommerce.Web.Utilities;

	#endregion

	[RoutePrefix("api/cms/{storeId}/pages")]
	[CheckPermission(Permission = PredefinedPermissions.Query)]
	public class PagesController : ApiController
	{
		#region Fields

		private IPagesService _pagesService;

		#endregion

		#region Constructors and Destructors

		public PagesController(Func<string, IPagesService> serviceFactory, ISettingsManager settingsManager)
		{
			if (serviceFactory == null)
			{
				throw new ArgumentNullException("serviceFactory");
			}

			if (settingsManager == null)
			{
				throw new ArgumentNullException("settingsManager");
			}

			var chosenRepository = settingsManager.GetValue(
				"VirtoCommerce.Content.MainProperties.PagesRepositoryType",
				string.Empty);

			var pagesService = serviceFactory.Invoke(chosenRepository);

			_pagesService = pagesService;
		}

		#endregion

		/// <summary>
		/// Get pages
		/// </summary>
		/// <remarks>Get all pages by store. Returns array of store pages</remarks>
		/// <param name="storeId">Store Id</param>
		/// <param name="criteria">Searching pages criteria</param>
		/// <response code="500">Internal Server Error</response>
		/// <response code="200">Pages returned OK</response>
		[HttpGet]
		[ResponseType(typeof(IEnumerable<Page>))]
		[Route("")]
		public IHttpActionResult GetPages(string storeId, [FromUri]GetPagesCriteria criteria)
		{
			var items = _pagesService.GetPages(storeId, criteria.ToCoreModel()).Select(s => s.ToWebModel());
			return Ok(items);
		}

		/// <summary>
		/// Get pages folders
		/// </summary>
		/// <remarks>Get all pages folders by store. Returns array of store pages folders</remarks>
		/// <param name="storeId">Store Id</param>
		/// <response code="500">Internal Server Error</response>
		/// <response code="200">Pages folders returned OK</response>
		[HttpGet]
		[ResponseType(typeof(GetPagesResult))]
		[Route("folders")]
		public IHttpActionResult GetFolders(string storeId)
		{
			var items = _pagesService.GetPages(storeId, null);

			return Ok(items.ToWebModel());
		}

		/// <summary>
		/// Get page
		/// </summary>
		/// <remarks>Get page by store and name+language pair. Returns page</remarks>
		/// <param name="storeId">Store Id</param>
		/// <param name="language">Page language</param>
		/// <param name="pageName">Page name</param>
		/// <response code="500">Internal Server Error</response>
		/// <response code="204">No content</response>
		/// <response code="200">Page returned OK</response>
		[HttpGet]
		[ResponseType(typeof(Page))]
		[ClientCache(Duration = 30)]
		[Route("{language}/{*pageName}")]
		public IHttpActionResult GetPage(string storeId, string language, string pageName)
		{
			var item = _pagesService.GetPage(storeId, pageName, language);
			if (item == null)
			{
				return StatusCode(HttpStatusCode.NoContent);
			}

			return Ok(item.ToWebModel());
		}

		/// <summary>
		/// Check page name
		/// </summary>
		/// <remarks>Check page pair name+language for store. Returns result of checking</remarks>
		/// <param name="storeId">Store Id</param>
		/// <param name="language">Page language</param>
		/// <param name="pageName">Page name</param>
		/// <response code="500">Internal Server Error</response>
		/// <response code="200">Pages returned OK</response>
		[HttpGet]
		[ResponseType(typeof(CheckNameResult))]
		[Route("checkname")]
		public IHttpActionResult CheckName(string storeId, [FromUri]string pageName, [FromUri]string language)
		{
			var result = _pagesService.CheckList(storeId, pageName, language);
			var response = new CheckNameResult { Result = result };
			return Ok(response);
		}

		/// <summary>
		/// Save page
		/// </summary>
		/// <remarks>Save page</remarks>
		/// <param name="storeId">Store Id</param>
		/// <param name="page">Page</param>
		/// <response code="500">Internal Server Error</response>
		/// <response code="200">Page saved OK</response>
		[HttpPost]
		[Route("")]
		[CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult SaveItem(string storeId, Page page)
		{
			if (!string.IsNullOrEmpty(page.FileUrl))
			{
				using(var webClient = new WebClient())
				{
					var byteContent = webClient.DownloadData(page.FileUrl);
					page.ByteContent = byteContent;
				}
			}

			_pagesService.SavePage(storeId, page.ToCoreModel());
			return Ok();
		}

		/// <summary>
		/// Delete page
		/// </summary>
		/// <remarks>Delete page</remarks>
		/// <param name="storeId">Store Id</param>
		/// <param name="pageNamesAndLanguges">Array of pairs name+language</param>
		/// <response code="500">Internal Server Error</response>
		/// <response code="200">Pages deleted OK</response>
		[HttpDelete]
		[Route("")]
		[CheckPermission(Permission = PredefinedPermissions.Manage)]
		public IHttpActionResult DeleteItem(string storeId, [FromUri]string[] pageNamesAndLanguges)
		{
			_pagesService.DeletePage(storeId, PagesUtility.GetShortPageInfoFromString(pageNamesAndLanguges).ToArray());
			return Ok();
		}
	}
}