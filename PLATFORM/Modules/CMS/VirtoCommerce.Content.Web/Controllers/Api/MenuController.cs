using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Web.Converters;
using VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Content.Web.Controllers.Api
{
	[RoutePrefix("api/cms/{storeId}")]
    [CheckPermission(Permission = PredefinedPermissions.Query)]
	public class MenuController : ApiController
	{
		private readonly IMenuService _menuService;

		public MenuController(IMenuService menuService)
		{
			if (menuService == null)
				throw new ArgumentNullException("menuService");

			_menuService = menuService;
		}

        /// <summary>
        /// Get menu link lists
        /// </summary>
        /// <param name="storeId">Store id</param>
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
            return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Get menu link list by id
		/// </summary>
		/// <param name="listId">List id</param>
        /// <param name="storeId">Store id</param>
		[HttpGet]
		[ResponseType(typeof(MenuLinkList))]
		[Route("menu/{listId}")]
		public IHttpActionResult GetList(string storeId, string listId)
		{
			var item = _menuService.GetListById(listId).ToWebModel();
			return Ok(item);
		}

		/// <summary>
		/// Checking name of menu link list
		/// </summary>
		/// <remarks>Checking pair of name+language of menu link list for unique, if checking result - false saving unavailable</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="name">Name of menu link list</param>
		/// <param name="language">Language of menu link list</param>
		/// <param name="id">Menu link list id</param>
		[HttpGet]
		[ResponseType(typeof(CheckNameResult))]
		[Route("menu/checkname")]
		public IHttpActionResult CheckName(string storeId, string name, string language, string id = "")
		{
			var retVal = _menuService.CheckList(storeId, name, language, id);
			var response = new CheckNameResult { Result = retVal };
			return Ok(response);
		}

		/// <summary>
		/// Update menu link list
		/// </summary>
		/// <param name="list">Menu link list</param>
		[HttpPost]
		[ResponseType(typeof(void))]
		[Route("menu")]
        [CheckPermission(Permission = PredefinedPermissions.Update)]
		public IHttpActionResult Update(MenuLinkList list)
		{
			_menuService.Update(list.ToCoreModel());
            return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Delete menu link list
		/// </summary>
		/// <param name="listId">Menu link list id</param>
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("menu")]
        [CheckPermission(Permission = PredefinedPermissions.Delete)]
		public IHttpActionResult Delete(string listId)
		{
			_menuService.DeleteList(listId);
            return StatusCode(HttpStatusCode.NoContent);
		}

	}
}
