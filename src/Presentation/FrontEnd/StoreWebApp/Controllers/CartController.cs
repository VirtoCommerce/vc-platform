using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Virto.Helpers;

namespace VirtoCommerce.Web.Controllers
{
    /// <summary>
    /// Class CartController.
    /// </summary>
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
        /// <param name="sourceView"></param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult RemoveFromWishList(string lineItemId, string sourceView = "WishList")
        {
            return RemoveFrom(lineItemId, CartHelper.WishListName, false, sourceView, false);
        }

        /// <summary>
        /// Removes from compare list.
        /// </summary>
        /// <param name="lineItemId">The line item identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult RemoveFromCompareList(string lineItemId)
        {
            return RemoveFrom(lineItemId, CartHelper.CompareListName, false, "MiniCompareList");
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
        /// <param name="renderView">True if partial view should be rendered</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult RemoveFrom(string lineItemId, string cartName, bool clear = false, string sourceView = "LineItems", bool renderView = true)
        {
            var helper = GetCartHelper(cartName);

            var name = String.Empty;

            if (!helper.IsEmpty)
            {
                foreach (var item in helper.LineItems.Where(item => item.LineItemId == lineItemId || clear).ToArray())
                {
                    name = item.DisplayName;
                    helper.Remove(item);
                }
            }

            // If cart is empty, remove it from the database
            if (helper.IsEmpty)
            {
                helper.Delete();
            }

            SaveChanges(helper);

            // Display the confirmation message
            var results = new CartRemoveModel
                {
                    CartSubTotal = StoreHelper.FormatCurrency(helper.Cart.Subtotal, helper.Cart.BillingCurrency),
                    CartTotal = StoreHelper.FormatCurrency(helper.Cart.Total, helper.Cart.BillingCurrency),
                    CartCount = helper.LineItems.Count(),
                    LineItemsView = renderView && !string.IsNullOrEmpty(sourceView) ? RenderRazorViewToString(sourceView, cartName == CartHelper.CompareListName ?
                    (object)helper.CreateCompareModel() : helper.CreateCartModel(true)) : "",
                    Source = sourceView,
                    DeleteId = lineItemId,
                    CartName = cartName
                };

            results.Messages.Add(new MessageModel(string.Format("{0} have been removed from your {1}.".Localize(), clear ? "Items " : Server.HtmlEncode(name), cartName)));

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
        [HttpPost]
        public ActionResult Add(string name, string itemId, string parentItemId, decimal? quantity, string[] relatedItemId = null)
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

            var helper = GetCartHelper(name);

            var catalogItem = _catalogClient.GetItem(itemId);
            var parentItem = !string.IsNullOrEmpty(parentItemId) ? _catalogClient.GetItem(parentItemId) : null;

            var addedLineItems = new List<LineItemModel>();

            var addedLineItem = DoAddToCart(name, qty, catalogItem, parentItem);
            if (addedLineItem != null)
            {
                addedLineItems.Add(new LineItemModel(addedLineItem, catalogItem, parentItem, helper.Cart.BillingCurrency));
            }

            if (relatedItemId != null && relatedItemId.Length > 0)
            {
                addedLineItems.AddRange(from relItemId in relatedItemId
                                        let relItem = _catalogClient.GetItem(relItemId)
                                        let relatedItem = DoAddToCart(name, 1, relItem, null)
                                        where relatedItem != null
                                        select new LineItemModel(relatedItem, relItem, null, helper.Cart.BillingCurrency));
            }

            if (Request.UrlReferrer != null)
            {
                UserHelper.CustomerSession.LastShoppingPage = Request.UrlReferrer.AbsoluteUri;
            }

            //helper.ClearCache();
            //helper = GetCartHelper(name);

            var results = new CartJsonModel
            {
                CartSubTotal = StoreHelper.FormatCurrency(helper.Cart.Subtotal, helper.Cart.BillingCurrency),
                CartTotal = StoreHelper.FormatCurrency(helper.Cart.Total, helper.Cart.BillingCurrency),
                CartCount = helper.LineItems.Count(),
                LineItemsView = null,
                CartName = name
            };

            results.Messages.Add(new MessageModel(string.Format("{0} items added to your {1}".Localize(), addedLineItems.Sum(li =>li.Quantity), name.Localize())));

            return Json(results);
        }

        /// <summary>
        /// Mini cart view
        /// </summary>
        /// <returns>Mini cart view</returns>
        public ActionResult MiniView()
        {
            var ch = GetCartHelper(CartHelper.CartName);

            return PartialView("MiniCart", CreateCartModel(ch));
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
            _catalogClient.CustomerSession.CouponCode = couponCode;
            var helper = GetCartHelper(CartHelper.CartName);
            var warnings = SaveChanges(helper);

            // Display the confirmation message
            var results = new CartJsonModel
            {
                CartSubTotal = StoreHelper.FormatCurrency(helper.Cart.Subtotal, helper.Cart.BillingCurrency),
                CartTotal = StoreHelper.FormatCurrency(helper.Cart.Total, helper.Cart.BillingCurrency),
                CartCount = helper.LineItems.Count(),
                LineItemsView = renderItems ? RenderRazorViewToString("LineItems", helper.CreateCartModel(true)) : null,
                CartName = CartHelper.CartName,
                Source = "LineItems"
            };

            if (warnings != null)
            {
                foreach (var warning in warnings.Values)
                {
                    results.Messages.Add(new MessageModel(warning));
                }
            }

            if (warnings == null || !warnings.ContainsKey(WorkflowMessageCodes.COUPON_NOT_APPLIED))
            {
                results.Messages.Add(string.IsNullOrEmpty(couponCode)
                    ? new MessageModel("Coupon has been cleared".Localize())
                    : new MessageModel("Coupon has been applied".Localize()));
            }


            return Json(results);
        }

        /// <summary>
        /// Clears the compare list.
        /// </summary>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult ClearCompareList()
        {
            return RemoveFrom(null, CartHelper.CompareListName, true, "MiniCompareList");
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
                //Need to prepare cart because shipment rates could chaange after items added/removed
                //var result = helper.RunWorkflow("ShoppingCartPrepareWorkflow");
                var result = helper.RunWorkflow("ShoppingCartValidateWorkflow");

                if (result.WorkflowResult.Warnings != null)
                {
                    foreach (var warning in result.WorkflowResult.Warnings.Distinct())
                    {
                        errors.Add(warning.Code, warning.Parameters["Message"]);
                    }
                }
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
                addedLineItem = ch.AddItem(catalogItem, parentCatalogItem, qty, string.Equals(cartName, CartHelper.CompareListName, StringComparison.OrdinalIgnoreCase));
                SaveChanges(ch);
            }

            return addedLineItem;
        }
    }
}