using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Extensions;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model.ShippingMethod;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{
    /// <summary>
    /// Class CartHelperExtensions.
    /// </summary>
    public static class CartHelperExtensions
    {
        /// <summary>
        /// Creates the cart model.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="preloadItems">if set to <c>true</c> [preload items].</param>
        /// <returns>CartModel.</returns>
        public static CartModel CreateCartModel(this CartHelper helper, bool preloadItems)
        {
            var model = new CartModel();

            var cart = helper.Cart;

            model.SubTotalPriceFormatted = StoreHelper.FormatCurrency(cart.Subtotal, cart.BillingCurrency);
            model.TotalPriceFormatted = StoreHelper.FormatCurrency(cart.Total, cart.BillingCurrency);
            model.DiscountTotalPriceFormatted = StoreHelper.FormatCurrency(-cart.DiscountTotal, cart.BillingCurrency);
            model.ShippingPriceFormatted = StoreHelper.FormatCurrency(cart.ShippingTotal, cart.BillingCurrency);
            model.TaxTotalPriceFormatted = StoreHelper.FormatCurrency(cart.TaxTotal, cart.BillingCurrency);
            model.CouponCode = UserHelper.CustomerSession.CouponCode;

            if (preloadItems)
            {
                var ids = helper.LineItems.Select(li => li.CatalogItemId).ToList();

                var lineItems = new List<LineItemModel>();

                if (ids.Count > 0)
                {
                    var items = CartHelper.CatalogClient.GetItems(ids.ToArray());

                    lineItems.AddRange(from li in helper.LineItems
                                       let item = (from i in items
                                                   where
                                                       i.ItemId.Equals(li.CatalogItemId,
                                                                       StringComparison.OrdinalIgnoreCase)
                                                   select i).FirstOrDefault()
                                       let parentItem = CartHelper.CatalogClient.GetItem(li.ParentCatalogItemId)
                                       where item != null
                                       select new LineItemModel(li, item, parentItem, helper.Cart.BillingCurrency));
                }

                model.LineItems = lineItems.ToArray();
            }

            return model;
        }

        /// <summary>
        /// Creates the compare model.
        /// </summary>
        /// <param name="cartHelper">The cart helper.</param>
        /// <returns>CompareListModel.</returns>
        public static CompareListModel CreateCompareModel(this CartHelper cartHelper)
        {
            var lineItemModels = new LineItemModel[0];
            var items = CartHelper.CatalogClient.GetItems(cartHelper.LineItems.Select(li => li.CatalogItemId).ToArray());

            if (items != null)
            {
                lineItemModels = cartHelper.LineItems.Join(items, li => li.CatalogItemId, i => i.ItemId,
                                                               (li, item) =>
                                                               new LineItemModel(li, item,
                                                                                 CartHelper.CatalogClient.GetItem(li.ParentCatalogItemId), cartHelper.Cart.BillingCurrency)).ToArray();
            }

            return new CompareListModel(lineItemModels);
        }

        /// <summary>
        /// Gets all shipping methods.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="methodIds">The method ids.</param>
        /// <returns>shipping method model</returns>
        public static ShippingMethodModel[] GetShippingMethods(this CartHelper cart, List<string> methodIds = null)
        {
            var list = new List<ShippingMethodModel>();
            methodIds = methodIds ?? new List<string>();
            IEnumerable<ShippingMethod> shippingMethods;

            if (methodIds.Any())
            {
                shippingMethods = cart.ShippingClient.GetAllShippingMethods().Where(so => methodIds.Contains(so.ShippingMethodId));
            }
            else
            {
                var options = cart.ShippingClient.GetAllShippingOptions();
                shippingMethods = options.SelectMany(o => o.ShippingMethods.Where(m => m.IsActive));
            }

            var address = cart.FindAddressByName("Shipping") ?? cart.FindAddressByName("Billing");

            foreach (var method in shippingMethods.Where(m => m.IsActive &&
                (string.IsNullOrEmpty(m.Currency) || m.Currency.Equals(UserHelper.CustomerSession.Currency, StringComparison.OrdinalIgnoreCase))))
            {
                var model = new ShippingMethodModel(method, cart.Cart);

                //Filter by shipping method jurisdiction
                var jurisdictionRelations = method.ShippingMethodJurisdictionGroups.SelectMany(g => g.JurisdictionGroup.JurisdictionRelations).ToArray();

                if (jurisdictionRelations.Any())
                {
                    var jurisdictionFound = false;

                    if (address != null)
                    {
                        jurisdictionFound = jurisdictionRelations.Any(j => j.Jurisdiction.CheckAllFieldsMatch(
                                                                            address.CountryCode,
                                                                            address.StateProvince,
                                                                            address.PostalCode,
                                                                            address.RegionName, "", "",
                                                                            address.City));
                    }

                    if (!jurisdictionFound)
                        continue;
                }

                if (!string.IsNullOrWhiteSpace(method.RestrictJurisdictionGroups))
                {
                    var jurisdictionFound = false;
                    if (address != null)
                    {
                        var ids = method.RestrictJurisdictionGroups.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var jurisdictions = cart.OrderRepository.JurisdictionGroups.Where(x => ids.Contains(x.JurisdictionGroupId))
                            .SelectMany(x => x.JurisdictionRelations).Select(x => x.Jurisdiction).ToArray();

                        jurisdictionFound = jurisdictions.Any(j => j.CheckAllFieldsMatch(address.CountryCode, address.StateProvince, 
                            address.PostalCode, address.RegionName, "", "", address.City));
                    }

                    //If found any match in restrcitions cannot use this shipping method
                    if (jurisdictionFound)
                        continue;

                }

                methodIds.Add(method.ShippingMethodId);
                list.Add(model);
            }

            var rates = GetAllShippingRates(cart, methodIds.ToArray(), cart.LineItems);
            if (rates != null)
            {
                foreach (var sm in list)
                {
                    sm.Rate = (from r in rates where r.Id == sm.Id select r).SingleOrDefault();
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Gets all shipping rates.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <param name="shippingMethods">The shipping methods.</param>
        /// <param name="lineItems">The line items.</param>
        /// <returns>array of shipping rate model</returns>
        public static ShippingRateModel[] GetAllShippingRates(this CartHelper cart, string[] shippingMethods,
                                                              IEnumerable<LineItem> lineItems)
        {
            var rates = cart.ShippingClient.GetShippingRates(shippingMethods, lineItems.ToArray());

            if (rates == null)
            {
                return null;
            }

            var m = from r in rates select new ShippingRateModel(r);
            return m.ToArray();
        }
    }
}