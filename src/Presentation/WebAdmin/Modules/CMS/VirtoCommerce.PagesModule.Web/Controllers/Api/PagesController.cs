namespace VirtoCommerce.PagesModule.Web.Controllers.Api
{
	#region

	using System;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Description;
	using VirtoCommerce.Content.Pages.Data.Services;
	using VirtoCommerce.Framework.Web.Settings;
	using VirtoCommerce.PagesModule.Web.Models;
	using VirtoCommerce.PagesModule.Web.Converters;
	using System.Collections.Generic;

	#endregion

	[RoutePrefix("api/cms/{storeId}")]
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
				"VirtoCommerce.PagesModule.MainProperties.PagesRepositoryType",
				string.Empty);

			var pagesService = serviceFactory.Invoke(chosenRepository);

			_pagesService = pagesService;
		}

		#endregion

		[HttpGet]
		[ResponseType(typeof(IEnumerable<ShortPageInfo>))]
		[Route("pages")]
		public IHttpActionResult GetPages(string storeId)
		{
			var items = _pagesService.GetPages(storeId).Select(s => s.ToWebModel());
			return Ok(items);
		}

		[HttpGet]
		[ResponseType(typeof(ShortPageInfo[]))]
		[Route("pages/{pageName}")]
		public IHttpActionResult GetPage(string storeId, string pageName)
		{
			var item = _pagesService.GetPage(storeId, pageName);
			return Ok(item.ToWebModel());
		}

		[HttpPost]
		[Route("pages")]
		public IHttpActionResult SaveItem(string storeId, Page page)
		{
			_pagesService.SavePage(storeId, page.ToCoreModel());
			return Ok();
		}

		[HttpDelete]
		[Route("pages")]
		public IHttpActionResult DeleteItem(string storeId, string[] pageNames)
		{
			_pagesService.DeletePage(storeId, pageNames);
			return Ok();
		}
	}
}