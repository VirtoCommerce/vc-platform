#region
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using VirtoCommerce.Web.Models;

#endregion

namespace VirtoCommerce.Web.Controllers
{
    [RoutePrefix("cart")]
    public class CartController : StoreControllerBase
    {
        #region Public Methods and Operators
        [Route("add")]
        public async Task<ActionResult> AddAsync(string id)
        {
            await this.Service.Cart.AddAsync(id);
            return new RedirectResult("~/cart");
        }

        [HttpPost]
        [Route("add.js")]
        public async Task<ActionResult> AddToCartAsync(string id, decimal quantity = 1)
        {
            var item = await this.Service.Cart.AddAsync(id);
            return this.Json(item);
        }

        [Route("~/cart.js")]
        public async Task<ActionResult> CartAsync()
        {
            var cart = await Task.FromResult(SiteContext.Current.Cart);
            return Json(cart, JsonRequestBehavior.AllowGet);
        }

        [Route("change")]
        // Post: Cart
        public async Task<ActionResult> ChangeAsync(int line, int quantity, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                await this.Service.Cart.ChangeAsync(id, quantity);
            }
            else
            {
                await this.Service.Cart.ChangeAsync(line - 1, quantity);
            }

            return new RedirectResult("~/cart");
        }

        [HttpGet]
        [Route("change.js")]
        public async Task<ActionResult> ChangeItemCartAsync(int line, int quantity)
        {
            var item = await this.Service.Cart.ChangeAsync(line - 1, quantity);
            return this.Json(item);
        }

        [HttpPost]
        [Route("change.js")]
        public async Task<ActionResult> ChangeItemCartAsync(string id, int quantity = 1)
        {
            var item = await this.Service.Cart.ChangeAsync(id, quantity);
            return this.Json(item);
        }

        [HttpPost]
        [Route("clear.js")]
        public async Task<ActionResult> ClearCartAsync()
        {
            var cart = await this.Service.Cart.ClearAsync();
            return this.Json(cart);
        }

        [Route("")]
        [HttpGet]
        // GET: Cart
        public async Task<ActionResult> IndexAsync()
        {
            await Task.FromResult<object>(null);
            return View("Cart");
        }

        [Route("")]
        [HttpPost]
        // Post: Cart
        public async Task<ActionResult> UpdateAsync(int[] updates, string note, string action, string update = null, string checkout = null)
        {
            var item = await this.Service.Cart.UpdateAsync(updates, note, action);

            if (update != null)
                return this.View("Cart");

            return RedirectToAction("Step1", "Checkout");
        }
        #endregion
    }
}