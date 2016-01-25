using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Cart;

namespace VirtoCommerce.Storefront.Model.Order.Events
{
    /// <summary>
    /// Event generated when user finished checkout process and placed new order
    /// </summary>
    public class OrderPlacedEvent
    {
        public OrderPlacedEvent(CustomerOrder order, ShoppingCart cart)
        {
            Cart = cart;
            Order = order;
        }

        public ShoppingCart Cart { get; set; }
        public CustomerOrder Order { get; set; }
    }
}
