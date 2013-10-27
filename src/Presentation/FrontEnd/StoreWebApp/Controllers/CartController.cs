using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Controllers
{
	/// <summary>
	/// Class CartController.
	/// </summary>
	[Localize]
	public class CartController : ControllerBase
	{
		#region Private Fields

		// contains list of carts
		/// <summary>
		/// The _carts
		/// </summary>
		private readonly Dictionary<string, CartHelper> _carts = new Dictionary<string, CartHelper>();
		/// <summary>
		/// The _catalog client
		/// </summary>
		private readonly CatalogClient _catalogClient;
		/// <summary>
		/// The _country client
		/// </summary>
		private readonly CountryClient _countryClient;

		#endregion

		#region Constructors

		/// <summary>
		/// Cart controller constructor
		/// </summary>
		/// <param name="catalogClient">Catalog client used to access catalog repository methods through caching and other helpers</param>
		/// <param name="countryClient">Allows to get countries from repository through caching</param>
		public CartController(CatalogClient catalogClient, CountryClient countryClient)
		{
			_catalogClient = catalogClient;
			_countryClient = countryClient;
		}

		#endregion


		/// <summary>
		/// Cart home page
		/// </summary>
		/// <returns>ActionResult.</returns>
		public ActionResult Index()
		{
			var helper = GetCartHelper(CartHelper.CartName);
			if (!helper.IsEmpty)
			{
				helper.SaveChanges();
			}

			if (helper.IsEmpty)
			{
				return View("Empty");
			}

			var model = CreateCartModel(helper);
			return View("Index", model);
		}

		/// <summary>
		/// Configures the specified line item by given identifier.
		/// </summary>
		/// <param name="lineItemId">The line item identifier.</param>
		/// <returns>ActionResult.</returns>
		public ActionResult Configure(string lineItemId)
		{
			var cart = GetCartHelper(CartHelper.CartName);

			var lineItem = cart.LineItems.FirstOrDefault(li => li.LineItemId == lineItemId);
			if (lineItem == null)
			{
				cart = GetCartHelper(CartHelper.WishListName);
				lineItem = cart.LineItems.FirstOrDefault(li => li.LineItemId == lineItemId);
			}

			if (lineItem != null)
			{
				Response.Redirect(Url.ItemUrl(lineItem.CatalogItemId, lineItem.ParentCatalogItemId));
			}

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Removes the specified line item.
		/// </summary>
		/// <param name="lineItemId">The line item identifier.</param>
		/// <param name="sourceView">The source view.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult Remove(string lineItemId, string sourceView = "LineItems")
		{
			return RemoveFrom(lineItemId, CartHelper.CartName, false, sourceView);
		}

		/// <summary>
		/// Removes from wish list.
		/// </summary>
		/// <param name="lineItemId">The line item identifier.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult RemoveFromWishList(string lineItemId)
		{
			return RemoveFrom(lineItemId, CartHelper.WishListName);
		}

		/// <summary>
		/// Removes from compare list.
		/// </summary>
		/// <param name="lineItemId">The line item identifier.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult RemoveFromCompareList(string lineItemId)
		{
			return RemoveFrom(lineItemId, CartHelper.CompareListName);
		}

		/// <summary>
		/// Updates the specified line items.
		/// </summary>
		/// <param name="lineItems">The line items.</param>
		/// <param name="cartbutton">The cartbutton.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult Update(List<LineItemUpdateModel> lineItems, string cartbutton)
		{
			var cart = GetCartHelper(CartHelper.CartName);

			var hasUpdates = false;

			if (cartbutton == "clear")
			{
				cart.Delete();
				hasUpdates = true;
			}
			else
			{
				foreach (var ml in lineItems)
				{
					var ml1 = ml;
					foreach (var li in cart.LineItems.Where(li => li.LineItemId == ml1.LineItemId).ToArray())
					{
						if (ml.Quantity <= 0)
						{
							cart.Remove(li);
						}
						else
						{
							li.Quantity = ml.Quantity;
						}

						hasUpdates = true;
					}
				}
			}

			if (hasUpdates)
			{
				SaveChanges(cart);
			}

			return RedirectToAction("Index");
		}

		/// <summary>
		/// Removes line item from specified cart.
		/// </summary>
		/// <param name="lineItemId">The line item identifier.</param>
		/// <param name="cartName">Name of the cart.</param>
		/// <param name="clear">if set to <c>true</c> [clear] all cart.</param>
		/// <param name="sourceView">The source view. View that initiated remove action.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult RemoveFrom(string lineItemId, string cartName, bool clear = false, string sourceView = "LineItems")
		{
			var helper = GetCartHelper(cartName);

			var name = String.Empty;

			if (!helper.IsEmpty)
			{
				foreach (var item in helper.LineItems.Where(item => item.LineItemId == lineItemId || clear))
				{
					name = item.DisplayName;
					helper.Remove(item);
					break;
				}
			}

			// If cart is empty, remove it from the database
			if (helper.IsEmpty)
			{
				helper.Delete();
			}

			SaveChanges(helper);

			var renderView = sourceView.Equals("MiniCart") && !helper.LineItems.Any() ? "MiniCartEmpty" : sourceView;

			// Display the confirmation message
			var results = new CartRemoveModel
				{
					Message = Server.HtmlEncode(name) +
							  " has been removed from your shopping cart.",
					CartSubTotal = StoreHelper.FormatCurrency(helper.Cart.Subtotal, helper.Cart.BillingCurrency),
					CartTotal = StoreHelper.FormatCurrency(helper.Cart.Total, helper.Cart.BillingCurrency),
					CartCount = helper.LineItems.Count(),
					LineItemsView = RenderRazorViewToString(renderView, helper.CreateCartModel(true)),
					DeleteSource = sourceView
				};

			return Json(results);
		}

		/// <summary>
		/// Adds line item to the specified cart by name.
		/// Additionally adds related items if any are specified 
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="itemId">The item identifier.</param>
		/// <param name="parentItemId">The parent item identifier.</param>
		/// <param name="quantity">The quantity to add.</param>
		/// <param name="relatedItemId">The related items identifier.</param>
		/// <returns>ActionResult.</returns>
		public ActionResult Add(string name, string itemId, string parentItemId, decimal? quantity, string[] relatedItemId)
		{
			decimal qty = 1;
			if (quantity.HasValue)
			{
				qty = quantity.Value;
			}

			if (String.IsNullOrEmpty(name))
			{
				name = CartHelper.CartName;
			}

			var catalogItem = _catalogClient.GetItem(itemId);
			var parentItem = !string.IsNullOrEmpty(parentItemId) ? _catalogClient.GetItem(parentItemId) : null;

			var addedLineItems = new List<LineItemModel>();

			var addedLineItem = DoAddToCart(name, qty, catalogItem, parentItem);
			if (addedLineItem != null)
			{
				addedLineItems.Add(new LineItemModel(addedLineItem, catalogItem, parentItem));
			}

			if (relatedItemId != null && relatedItemId.Length > 0)
			{
				addedLineItems.AddRange(from relItemId in relatedItemId
										let relItem = _catalogClient.GetItem(relItemId)
										let relatedItem = DoAddToCart(name, 1, relItem, null)
										where relatedItem != null
										select new LineItemModel(relatedItem, relItem, null));
			}

			if (Request.UrlReferrer != null)
			{
				UserHelper.CustomerSession.LastShoppingPage = Request.UrlReferrer.AbsoluteUri;
			}

			return PartialView(addedLineItems);
		}

		/// <summary>
		/// Mini cart view
		/// </summary>
		/// <returns>Mini cart view</returns>
		public ActionResult MiniView()
		{
			var ch = GetCartHelper(CartHelper.CartName);

			return ch.IsEmpty ? PartialView("MiniCartEmpty") : PartialView("MiniCart", CreateCartModel(ch));
		}

		/// <summary>
		/// Renders add to cart button
		/// </summary>
		/// <param name="itemId">The item identifier.</param>
		/// <param name="parentItemId">The parent item identifier.</param>
		/// <param name="forcedActive">if set to <c>true</c> item is added regardless if its active.</param>
		/// <returns>ActionResult.</returns>
		public ActionResult AddToCart(string itemId, string parentItemId, bool forcedActive = true)
		{
			return PartialView(CatalogHelper.CreateCatalogModel(itemId, parentItemId, forcedActive: forcedActive));
		}

		/// <summary>
		/// Applies the discount coupon.
		/// </summary>
		/// <param name="couponCode">The coupon code.</param>
		/// <param name="renderItems">if set to <c>true</c> render items and returns with json.</param>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult ApplyCoupon(string couponCode, bool renderItems = false)
		{
			if (HttpContext.Session != null)
			{
				HttpContext.Session["CurrentCouponCode"] = couponCode;
			}

			_catalogClient.CustomerSession.CouponCode = couponCode;
			var helper = GetCartHelper(CartHelper.CartName);
			var warnings = SaveChanges(helper);
			var message = warnings != null && warnings.ContainsKey(WorkflowMessageCodes.COUPON_NOT_APPLIED)
							  ? warnings[WorkflowMessageCodes.COUPON_NOT_APPLIED]
							  : null;

			// Display the confirmation message
			var results = new CartJsonModel
			{
				Message = message,
				CartSubTotal = StoreHelper.FormatCurrency(helper.Cart.Subtotal, helper.Cart.BillingCurrency),
				CartTotal = StoreHelper.FormatCurrency(helper.Cart.Total, helper.Cart.BillingCurrency),
				CartCount = helper.LineItems.Count(),
				LineItemsView = renderItems ? RenderRazorViewToString("LineItems", helper.CreateCartModel(true)) : null
			};

			return Json(results);
		}

		/// <summary>
		/// Clears the compare list.
		/// </summary>
		/// <returns>ActionResult.</returns>
		[HttpPost]
		public ActionResult ClearCompareList()
		{
			return RemoveFrom(null, CartHelper.CompareListName, true);
		}

		/// <summary>
		/// Saves the cart changes.
		/// </summary>
		/// <param name="helper">The helper.</param>
		/// <returns>Dictionary of errors if any.</returns>
		private Dictionary<string, string> SaveChanges(CartHelper helper)
		{
			if (helper.Cart.Name != CartHelper.CartName)
			{
				_catalogClient.CustomerSession["SkipRewards"] = true;
			}

			var errors = new Dictionary<string, string>();
			if (helper.Cart.Name == CartHelper.CartName)
			{

				helper.RunWorkflow("ShoppingCartValidateWorkflow", errors);
			}

			helper.SaveChanges();
			return errors;
		}

		/// <summary>
		/// Gets the cart helper.
		/// </summary>
		/// <param name="name">The cart name.</param>
		/// <returns>CartHelper.</returns>
		private CartHelper GetCartHelper(string name)
		{
			if (!_carts.ContainsKey(name))
			{
				_carts.Add(name, new CartHelper(name));
			}

			return _carts[name];
		}

		/// <summary>
		/// Creates the cart model.
		/// </summary>
		/// <param name="cart">The cart.</param>
		/// <returns>CartModel.</returns>
		private CartModel CreateCartModel(CartHelper cart)
		{
			var model = cart.CreateCartModel(true);
			model.ShippingEstimateModel.Countries = _countryClient.GetAllCountries();
			return model;
		}

		/// <summary>
		/// Does the add to cart.
		/// </summary>
		/// <param name="cartName">Name of the cart.</param>
		/// <param name="qty">The qty.</param>
		/// <param name="catalogItem">The catalog item.</param>
		/// <param name="parentCatalogItem">The parent catalog item.</param>
		/// <returns>LineItem.</returns>
		private LineItem DoAddToCart(string cartName, decimal qty, Item catalogItem, Item parentCatalogItem)
		{
			LineItem addedLineItem = null;

			// Check if Entry Object is null.
			if (catalogItem != null)
			{
				var ch = GetCartHelper(cartName);

				if (!string.Equals(cartName, CartHelper.WishListName, StringComparison.OrdinalIgnoreCase))
				{
					// add entry to the cart. If it's in the wish list, move it from the wish list to the cart.
					var helper = GetCartHelper(CartHelper.WishListName);

					var li = helper.LineItems.FirstOrDefault(item => item.CatalogItemId == catalogItem.ItemId);

					if (li != null)
					{
						qty = li.Quantity;

						helper.Remove(li);

						// If wish list is empty, remove it from the database
						if (helper.IsEmpty)
						{
							helper.Delete();
						}

						helper.SaveChanges();
					}
				}
				// Add item to a cart.
				addedLineItem = ch.AddItem(catalogItem, parentCatalogItem, qty, false);
				SaveChanges(ch);
			}

			return addedLineItem;
		}
	}
}