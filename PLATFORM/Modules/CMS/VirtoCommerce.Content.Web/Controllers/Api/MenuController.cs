using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Web.Converters;
using VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Web.Controllers.Api
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
		[ClientCache(Duration = 30)]
		[Route("menu")]
		public IHttpActionResult GetLists(string storeId)
		{
		    var lists = _menuService.GetListsByStoreId(storeId);
		    if (lists.Any())
		    {
		        return this.Ok(lists.Select(s => s.ToWebModel()));
		    }
			return Ok();
		}

		[HttpGet]
		[ResponseType(typeof(MenuLinkList))]
		[Route("menu/{listId}")]
		public IHttpActionResult GetList(string listId)
		{
			var item = _menuService.GetListById(listId).ToWebModel();
			return Ok(item);
		}

		[HttpGet]
		[ResponseType(typeof(CheckNameResponse))]
		[Route("menu/checkname")]
		public IHttpActionResult CheckName(string storeId, string name, string language, string id)
		{
			var retVal = _menuService.CheckList(storeId, name, language, id);
			var response = new CheckNameResponse { Result = retVal };
			return Ok(response);
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
