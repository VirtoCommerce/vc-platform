using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.ServiceLocation;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Orders;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.Foundation.Orders.Services;
using VirtoCommerce.Foundation.Search.Services;
using VirtoCommerce.Web.Client.Extensions;
using VirtoCommerce.Foundation.Frameworks.Sequences;

namespace VirtoCommerce.Web.Client.Helpers
{
    /// <summary>
    /// Cart helper class used to simplify cart operations.
    /// The cart is automatically cached in the current Http Context.
    /// </summary>
    public class CartHelper
    {
        /// <summary>
        /// Default name for the cart.
        /// </summary>
        public static string WishListName = "WishList";

        /// <summary>
        /// The cart name
        /// </summary>
        public static string CartName = "ShoppingCart";
        /// <summary>
        /// The compare list name
        /// </summary>
        public static string CompareListName = "CompareList";

        #region Data Members

        /// <summary>
        /// The _order repository
        /// </summary>
        private IOrderRepository _orderRepository;

        #endregion

        /// <summary>
        /// Gets the order repository.
        /// </summary>
        /// <value>The order repository.</value>
        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ?? (_orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>()); }
        }

        /// <summary>
        /// Gets the search service.
        /// </summary>
        /// <value>The search service.</value>
        public ISearchService SearchService
        {
            get { return DependencyResolver.Current.GetService<ISearchService>(); }
        }

        /// <summary>
        /// Gets the workflow service.
        /// </summary>
        /// <value>The workflow service.</value>
        public IWorkflowService WorkflowService
        {
            get { return DependencyResolver.Current.GetService<IWorkflowService>(); }
        }

        /// <summary>
        /// Gets the price list client.
        /// </summary>
        /// <value>The price list client.</value>
        public PriceListClient PriceListClient
        {
            get { return DependencyResolver.Current.GetService<PriceListClient>(); }
        }

        /// <summary>
        /// Gets the catalog client.
        /// </summary>
        /// <value>The catalog client.</value>
        public static CatalogClient CatalogClient
        {
            get { return DependencyResolver.Current.GetService<CatalogClient>(); }
        }

        /// <summary>
        /// Gets the sequences client.
        /// </summary>
        /// <value>The sequences client.</value>
        public static ISequenceService SequencesService
        {
            get { return DependencyResolver.Current.GetService<ISequenceService>(); }
        }

        /// <summary>
        /// Gets the shipping client.
        /// </summary>
        /// <value>The shipping client.</value>
        public ShippingClient ShippingClient
        {
            get { return DependencyResolver.Current.GetService<ShippingClient>(); }
        }

        /// <summary>
        /// Gets the customer session service.
        /// </summary>
        /// <value>The customer session service.</value>
        public static ICustomerSessionService CustomerSessionService
        {
            get { return ServiceLocator.Current.GetInstance<ICustomerSessionService>(); }
        }

        public static ICatalogOutlineBuilder CatalogOutlineBuilder
        {
            get { return ServiceLocator.Current.GetInstance<ICatalogOutlineBuilder>(); }
        }

        /// <summary>
        /// Gets the customer session.
        /// </summary>
        /// <value>The customer session.</value>
        public ICustomerSession CustomerSession
        {
            get { return CustomerSessionService.CustomerSession; }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CartHelper" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public CartHelper(string name)
            : this(null, name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartHelper"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user identifier.</param>
        public CartHelper(string name, string userId)
            : this(null, name, userId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartHelper" /> class.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user id.</param>
        public CartHelper(string storeId, string name, string userId)
        {
            if (storeId == null)
            {
                storeId = CustomerSession.StoreId;
            }

            if (userId == null)
            {
                userId = CustomerSession.CustomerId;
            }

            if (storeId == null || userId == null)
                return;

            LoadCart(storeId, name, userId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CartHelper" /> class.
        /// </summary>
        /// <param name="cart">The cart.</param>
        /// <exception cref="System.ArgumentNullException">cart</exception>
        public CartHelper(ShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException("cart");
            }
            Cart = cart;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the cart.
        /// </summary>
        /// <value>The cart.</value>
        public virtual ShoppingCart Cart { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public virtual bool IsEmpty
        {
            get { return Cart == null || Cart.OrderForms.All(orderForm => orderForm.LineItems.Count <= 0); }
        }

        /// <summary>
        /// Gets the line items.
        /// </summary>
        /// <value>The line items.</value>
        public virtual IEnumerable<LineItem> LineItems
        {
            get { return Cart.OrderForms.SelectMany(orderForm => orderForm.LineItems); }
        }

        /// <summary>
        /// Gets the order form.
        /// </summary>
        /// <value>The order form.</value>
        public virtual OrderForm OrderForm
        {
            get { return GetOrderForm(Cart.Name); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is address required.
        /// </summary>
        /// <value><c>true</c> if this instance is address required; otherwise, <c>false</c>.</value>
        public virtual bool IsAddressRequired
        {
            get { return LineItems.Any(lineItem => String.IsNullOrEmpty(lineItem.ShippingAddressId)); }
        }

        /// <summary>
        /// Removes the specified line item.
        /// </summary>
        /// <param name="lineItem">The line item.</param>
        public virtual void Remove(LineItem lineItem)
        {
            //We must also remove shipmentItems that are not used when item is removed
            foreach (var shipmentItem in lineItem.OrderForm.Shipments.SelectMany(s => s.ShipmentItems)
                                                 .Where(si => si.LineItemId == lineItem.LineItemId).ToList())
            {
                shipmentItem.Shipment.ShipmentItems.Remove(shipmentItem);
                OrderRepository.Remove(shipmentItem);
            }

            lineItem.OrderForm.LineItems.Remove(lineItem);
            OrderRepository.Remove(lineItem);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Saves as order.
        /// </summary>
        /// <returns>Order.</returns>
        public virtual Order SaveAsOrder()
        {
            var order = AsOrder();
            Delete();
            OrderRepository.Add(order);
            OrderRepository.UnitOfWork.Commit();
            return order;
        }

        /// <summary>
        /// Converts shopping cart as order.
        /// </summary>
        /// <returns>Order.</returns>
        public virtual Order AsOrder()
        {
            var customerName = CustomerSession.CustomerName;

            //If user is anonymous take name from billing address
            if (string.IsNullOrEmpty(customerName))
            {
                var billingAddress = Cart.OrderAddresses.FirstOrDefault(a => a.OrderAddressId == Cart.AddressId);
                if (billingAddress != null)
                {
                    customerName = string.Format("{0} {1}", billingAddress.FirstName, billingAddress.LastName);
                }
            }

            var order = new Order();
            order.InjectFrom<CloneInjection>(Cart);
            order.CustomerName = customerName;
            order.Status = "Pending";
            order.Name = "Default";
            order.TrackingNumber = SequencesService.GetNext(typeof(Order).FullName);

            foreach (var newOf in order.OrderForms)
            {
                newOf.Name = "Default";
                newOf.OrderFormPropertyValues.Add(new OrderFormPropertyValue() { ShortTextValue = CustomerSession.Language, Name = "Language" });
                if (!string.IsNullOrEmpty(CustomerSession.CsrUsername))
                {
                    //Add order form property CSR username is saved in the order form property called "Purchased By CSR"
                    newOf.OrderFormPropertyValues.Add(new OrderFormPropertyValue() { ShortTextValue = CustomerSession.CsrUsername, Name = "Purchased By CSR" });
                }
            }

            return order;
        }

        public virtual void ToCart(OrderGroup order)
        {
            if (order == null)
            {
                throw new ArgumentException("order is required","order");
            }

            //Convert order forms to shopping cart
            order.OrderForms.ForEach(f => f.Name = CartName);

            Add(order);

            if (String.IsNullOrEmpty(Cart.BillingCurrency))
            {
                Cart.BillingCurrency = CustomerSession.Currency;
            }

            //Reset();

            // run workflows
            RunWorkflow("ShoppingCartPrepareWorkflow");
            SaveChanges();
        }

        /// <summary>
        /// Finds the address by id.
        /// </summary>
        /// <param name="addressId">The address id.</param>
        /// <returns>OrderAddress.</returns>
        public virtual OrderAddress FindAddressById(string addressId)
        {
            return OrderClient.FindAddressById(Cart, addressId);
        }

        /// <summary>
        /// Finds the name of the address by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>OrderAddress.</returns>
        public virtual OrderAddress FindAddressByName(string name)
        {
            return OrderClient.FindAddressByName(Cart, name);
        }

        /// <summary>
        /// Gets the named OrderForm.
        /// </summary>
        /// <param name="orderFormName">The name of the OrderForm object to retrieve.</param>
        /// <returns>The named OrderForm.</returns>
        public virtual OrderForm GetOrderForm(string orderFormName)
        {
            if (String.IsNullOrEmpty(orderFormName))
            {
                orderFormName = CartName;
            }

            OrderForm orderForm = null;
            foreach (var form in Cart.OrderForms
                                     .Where(form => form.Name.Equals(orderFormName, StringComparison.OrdinalIgnoreCase))
                )
            {
                orderForm = form;
            }

            if (orderForm == null)
            {
                orderForm = new OrderForm { Name = orderFormName };
                Cart.OrderForms.Add(orderForm);
            }
            return orderForm;
        }

        /// <summary>
        /// Deletes the current basket instance from the database.
        /// </summary>
        public virtual void Delete()
        {
            //TODO: Remove any reservations
            //TODO: Load existing usage promotion usage for the current order
            //TODO: Clear all old promotion usage items first
            //TODO: Save the promotion usage

            // Delete the cart
            OrderRepository.Remove(Cart);
        }

        /// <summary>
        /// Adds the entry. Line item's qty will be increased by 1.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <param name="parent">The parent.</param>
        /// <returns>LineItem.</returns>
        public virtual LineItem AddItem(Item item, Item parent)
        {
            return AddItem(item, parent, null);
        }

        /// <summary>
        /// Adds the entry. Line item's qty will be increased by 1.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <param name="parent">Parent ite,</param>
        /// <param name="helpersToRemove">The helpers to remove.</param>
        /// <returns>LineItem.</returns>
        public virtual LineItem AddItem(Item item, Item parent, params CartHelper[] helpersToRemove)
        {
            return AddItem(item, parent, 1, false, helpersToRemove);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <param name="parent">Parent item</param>
        /// <param name="fixedQuantity">If true, lineitem's qty will be set to 1
        /// value. Otherwise, 1 will be added to the current line item's qty value.</param>
        /// <returns>LineItem.</returns>
        public virtual LineItem AddItem(Item item, Item parent, bool fixedQuantity)
        {
            return AddItem(item, parent, 1, fixedQuantity);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <param name="parent">Parent item</param>
        /// <param name="fixedQuantity">If true, lineitem's qty will be set to 1 value. Otherwise, 1 will be added to the current line item's qty value.</param>
        /// <param name="helpersToRemove">The helpers to remove.</param>
        /// <returns>LineItem.</returns>
        public virtual LineItem AddItem(Item item, Item parent, bool fixedQuantity, params CartHelper[] helpersToRemove)
        {
            return AddItem(item, parent, 1, fixedQuantity, helpersToRemove);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <param name="parent">Parent item</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="fixedQuantity">If true, line item's qty will be set to <paramref name="quantity" /> value. Otherwise, <paramref name="quantity" /> will be added to the current line item's qty value.</param>
        /// <param name="helpersToRemove">CartHelper(s) from which the item needs to be removed simultaneously with adding it to the current CartHelper.</param>
        /// <returns>LineItem.</returns>
        public virtual LineItem AddItem(Item item, Item parent, decimal quantity, bool fixedQuantity, params CartHelper[] helpersToRemove)
        {
            OrderForm orderForm;
            if (Cart.OrderForms.Count == 0) // create a new one
            {
                orderForm = new OrderForm { Name = Cart.Name };
                Cart.OrderForms.Add(orderForm);
            }
            else // use first one
            {
                orderForm = Cart.OrderForms[0];
            }

            // Add line items
            var lineItem = CreateLineItem(item, parent, quantity);
            lineItem.OrderFormId = orderForm.OrderFormId;

            // check if items already exist
            var litem = orderForm.LineItems.FirstOrDefault(li => li.CatalogItemId == lineItem.CatalogItemId);
            if (litem != null)
            {
                litem.Quantity = fixedQuantity ? lineItem.Quantity : litem.Quantity + lineItem.Quantity;
                litem.ExtendedPrice = lineItem.PlacedPrice * litem.Quantity;
                lineItem = litem;
            }
            else
            {
                orderForm.LineItems.Add(lineItem);
            }

            // remove entry from other helpers if needed
            if (helpersToRemove != null && helpersToRemove.Length > 0)
            {
                foreach (var ch in helpersToRemove)
                {
                    // if entry is in the helper, remove it
                    var li = ch.LineItems.FirstOrDefault(i => i.CatalogItemId == item.ItemId);
                    if (li != null)
                    {
                        //li.Delete();
                        ch.Remove(li);

                        // If helper is empty, remove it from the database
                        if (ch.IsEmpty)
                        {
                            ch.Delete();
                        }
                    }
                }
            }

            return lineItem;
        }

        /// <summary>
        /// Creates the line item.
        /// </summary>
        /// <param name="item">The entry.</param>
        /// <param name="parent">Parent item</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns>LineItem.</returns>
        private LineItem CreateLineItem(Item item, Item parent, decimal quantity)
        {
            var lineItem = new LineItem();

            if (parent != null)
            {
                lineItem.DisplayName = item.DisplayName(String.Format("{0}: {1}", parent.Name, item.Name));
                lineItem.ParentCatalogItemId = parent.ItemId;

                //Build options
                var relations = CatalogClient.GetItemRelations(parent.ItemId);

                var relationGroups = relations.Select(rel => rel.GroupName).Distinct();
                foreach (var prop in item.ItemPropertyValues.LocalizedProperties().Where(p => relationGroups.Contains(p.Name)))
                {
                    var option = new LineItemOption
                        {
                            LineItemId = item.ItemId,
                            OptionName = prop.Name,
                            OptionValue = prop.ToString()
                        };
                    lineItem.Options.Add(option);
                }
            }
            else
            {
                lineItem.DisplayName = item.DisplayName();
                lineItem.ParentCatalogItemId = String.Empty;
            }

            lineItem.CatalogItemId = item.ItemId;
            lineItem.CatalogItemCode = item.Code;
            var price = PriceListClient.GetLowestPrice(item.ItemId, quantity, false);

            if (price != null)
            {
                lineItem.ListPrice = price.Sale ?? price.List;
                lineItem.PlacedPrice = price.Sale ?? price.List;
                lineItem.ExtendedPrice = lineItem.PlacedPrice * quantity;
            }

            lineItem.MaxQuantity = item.MaxQuantity;
            lineItem.MinQuantity = item.MinQuantity;
            lineItem.Quantity = quantity;
            lineItem.Weight = item.Weight;
            lineItem.Catalog = CustomerSession.CatalogId;
            lineItem.FulfillmentCenterId = StoreHelper.StoreClient.GetCurrentStore().FulfillmentCenterId;
            //lineItem.CatalogOutline = CatalogOutlineBuilder.BuildCategoryOutline(CatalogClient.CatalogRepository, CustomerSession.CatalogId, item);
            lineItem.CatalogOutline = CatalogOutlineBuilder.BuildCategoryOutline(CustomerSessionService.CustomerSession.CatalogId, item.ItemId).ToString();

            return lineItem;
        }

        /// <summary>
        /// Resets this instance. Will clean up line items, remove payments.
        /// The cart needs to be saved in order for changes to be persisted.
        /// </summary>
        public virtual void Reset()
        {
            // Reset shopping cart
            foreach (var lineItem in LineItems)
            {
                lineItem.ShippingAddressId = String.Empty;
                lineItem.ShippingMethodId = String.Empty;
                lineItem.ShippingMethodName = null;
            }

            foreach (var orderForm in Cart.OrderForms)
            {
                while (orderForm.Payments.Count > 0)
                {
                    OrderRepository.Remove(orderForm.Payments[0]);
                }

                while (orderForm.Shipments.Count > 0)
                {
                    OrderRepository.Remove(orderForm.Shipments[0]);
                }
            }

            //Do not delete addresses!
            //while (Cart.OrderAddresses.Count > 0)
            //    OrderRepository.Remove(Cart.OrderAddresses[0]);

            //Cart.AddressId = String.Empty;
        }

        public virtual void ClearCache(string name = "__all", string userId = null)
        {
            if (userId == null)
            {
                userId = CustomerSession.CustomerId;
            }

            foreach (var key in HttpContext.Current.Items.Keys.OfType<string>().Where(k => k.Equals(GetCacheKey(name, userId))).ToArray())
            {
                HttpContext.Current.Items.Remove(key);
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public virtual void SaveChanges()
        {
            if (!OrderRepository.IsAttachedTo(Cart))
            {
                OrderRepository.Add(Cart);
                SyncCache(); //Update cache with with new cart
            }
            OrderRepository.UnitOfWork.Commit();
        }

        /// <summary>
        /// Runs the workflow.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="orderGroup"></param>
        /// <returns>OrderWorkflowResult.</returns>
        public virtual OrderWorkflowResult RunWorkflow(string name, OrderGroup orderGroup = null)
        {
            orderGroup = orderGroup ?? Cart;
            var result = new OrderService(OrderRepository, SearchService, WorkflowService, null, null).ExecuteWorkflow(name, orderGroup);

            if (result.OrderGroup is ShoppingCart)
            {
                Cart = result.OrderGroup as ShoppingCart;
            }
            return result;
        }


        /// <summary>
        /// Adds the specified order group to the existing cart.
        /// </summary>
        /// <param name="orderGroup">The order group.</param>
        public virtual void Add(OrderGroup orderGroup)
        {
            Add(orderGroup, false);
        }

        /// <summary>
        /// Adds the specified order group to the existing cart.
        /// </summary>
        /// <param name="orderGroup">The order group.</param>
        /// <param name="lineItemRollup">if set to <c>true</c> [line item rollup].</param>
        /// <exception cref="System.ArgumentNullException">orderGroup</exception>
        public virtual void Add(OrderGroup orderGroup, bool lineItemRollup)
        {
            if (orderGroup == null)
            {
                throw new ArgumentNullException("orderGroup");
            }

            if ((orderGroup.OrderForms != null) && (orderGroup.OrderForms.Count != 0))
            {
                // need to set meta data context before cloning
                foreach (var form in orderGroup.OrderForms)
                {
                    var orderForm = Cart.OrderForms.FirstOrDefault(f => f.Name.Equals(form.Name, StringComparison.OrdinalIgnoreCase));
                    if (orderForm == null)
                    {
                        orderForm = new OrderForm { Name = form.Name };
                        Cart.OrderForms.Add(orderForm);
                    }

                    orderForm.OrderGroupId = Cart.OrderGroupId;
                    orderForm.OrderGroup = Cart;

                    var count = form.LineItems.Count;
                    for (var i = 0; i < count; i++)
                    {
                        var newLineItem = new LineItem();

                        // preserve the new id
                        var id = newLineItem.LineItemId;
                        newLineItem.InjectFrom(form.LineItems[i]);
                        newLineItem.LineItemId = id;

                        Add(orderForm, newLineItem, lineItemRollup);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="value">The value.</param>
        /// <param name="lineItemRollup">if set to <c>true</c> [line item rollup].</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public virtual int Add(OrderForm form, LineItem value, bool lineItemRollup)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            LineItem existingItem = null;

            foreach (var item in form.LineItems)
            {
                if (ReferenceEquals(value, item))
                {
                    return form.LineItems.IndexOf(value);
                }

                if (item.CatalogItemId == value.CatalogItemId)
                {
                    existingItem = item;
                    break;
                }
            }

            if (lineItemRollup && (existingItem != null))
            {
                existingItem.Quantity += value.Quantity;
                return form.LineItems.IndexOf(existingItem);
            }
            value.OrderFormId = form.OrderFormId;
            value.OrderForm = form;
            form.LineItems.Add(value);
            return form.LineItems.IndexOf(value);
        }


        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the cache key which is used to store current cart in the http context.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// name
        /// or
        /// userId
        /// </exception>
        /// <exception cref="ArgumentNullException"></exception>
        private string GetCacheKey(string name, string userId)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId");
            }

            // Cache cart in the current context
            var cartKey = CacheKey.Create("Cart-http", name, userId).ToString();

            return cartKey;
        }

        /// <summary>
        /// Synchronizes the cache.
        /// </summary>
        private void SyncCache()
        {
            if (Cart != null)
            {
                var cartKey2 = GetCacheKey("__all", Cart.CustomerId);

                CartCount[] allCarts = null;

                if (HttpContext.Current != null)
                {
                    allCarts = HttpContext.Current.Items[cartKey2] as CartCount[];
                }

                if (allCarts != null && allCarts.All(c => c.Name != Cart.Name))
                {
                    var newAllCarts = allCarts.ToList();
                    newAllCarts.Add(new[] { new CartCount { Name = Cart.Name, Count = 1 } });
                    HttpContext.Current.Items[cartKey2] = newAllCarts.ToArray();
                }
            }
        }

        /// <summary>
        /// Loads the cart. The cart is loaded from current http context if one is present.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <param name="name">The name.</param>
        /// <param name="userId">The user id.</param>
        private void LoadCart(string storeId, string name, string userId)
        {
            ShoppingCart cart = null;

            // Make sure to reinitialize cart when it was deleted

            var cartKey2 = GetCacheKey("__all", userId);

            CartCount[] allCarts = null;

            if (HttpContext.Current != null)
            {
                allCarts = HttpContext.Current.Items[cartKey2] as CartCount[];
            }

            // it is less expansive to do check if cart exists first then load all the data
            // preload all user carts
            if (allCarts == null)
            {
                var query = (OrderRepository.ShoppingCarts.Where(
                    c => c.StoreId.Equals(storeId, StringComparison.OrdinalIgnoreCase) &&
                         c.CustomerId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                                 .GroupBy(c => c.Name)
                                 .Select(cartGroup => new CartCount { Name = cartGroup.Key, Count = cartGroup.Count() }));

                allCarts = query.ToArray();
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[cartKey2] = allCarts;
                }
            }

            var count = allCarts.Count(x => (x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

            if (count > 0)
            {
                cart =
                    OrderRepository.ShoppingCarts.ExpandAll()
                        .FirstOrDefault(x => (x.Name == name) && (x.StoreId == storeId) && (x.CustomerId == userId));
            }

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    StoreId = storeId,
                    CustomerId = userId,
                    Name = name,
                    BillingCurrency = CustomerSession.Currency
                };
            }

            if (String.IsNullOrEmpty(cart.CustomerId))
            {
                cart.CustomerId = userId;
            }

            Cart = cart;

        }

        #endregion

        /// <summary>
        /// Class CartCount.
        /// </summary>
        private class CartCount
        {
            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }
            /// <summary>
            /// Gets or sets the count.
            /// </summary>
            /// <value>The count.</value>
            public int Count { get; set; }
        }
    }
}