using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Menu.Data.Services;
using VirtoCommerce.MenuModule.Web.Converters;
using VirtoCommerce.MenuModule.Web.Models;

namespace VirtoCommerce.MenuModule.Web.Controllers.Api
{
	[RoutePrefix("api/cms/{storeId}")]
	public class MenuController : ApiController
	{
		private readonly IMenuService _menuService;

		public MenuController(IMenuService menuService)
		{
			if (menuService == null)
				throw new ArgumentNullException("menuService");

			_menuService = menuService;
		}

		[HttpGet]
		[ResponseType(typeof(IEnumerable<MenuLinkList>))]
		[Route("menu")]
		public IHttpActionResult GetLists(string storeId)
		{
			var items = _menuService.GetListsByStoreId(storeId).Select(s => s.ToWebModel());
			return Ok(items);
		}

		[HttpGet]
		[ResponseType(typeof(MenuLinkList))]
		[Route("menu/{listId}")]
		public IHttpActionResult GetList(string listId)
		{
			var item = _menuService.GetListById(listId).ToWebModel();
			return Ok(item);
		}

		[HttpPost]
		[ResponseType(typeof(void))]
		[Route("menu")]
		public IHttpActionResult Update(string storeId, MenuLinkList list)
		{
			_menuService.UpdateList(list.ToCoreModel());
			return Ok();
		}

		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("menu")]
		public IHttpActionResult Delete(string listId)
		{
			_menuService.DeleteList(listId);
			return Ok();
		}

	}
}
