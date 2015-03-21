using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Client.Builders
{
    public class OrderBuilder
    {
        private OrderGroup _order;

        public OrderBuilder()
        {
            _order = new Order();
        }

        public OrderGroup GetOrder()
        {
            return _order;
        }

        public OrderBuilder(OrderGroup order)
        {
            _order = order;
        }

        public OrderBuilder Name(string name)
        {            
            _order.Name = name;
            return this;
        }

        public OrderBuilder Customer(string customerId, string customerName)
        {
            _order.CustomerId = customerId;
            _order.CustomerId = customerName;
            return this;
        }

        public OrderBuilder Form(Func<OrderFormBuilder, OrderFormBuilder> form)
        {
            var inputInstance = new OrderFormBuilder();
            var resultPart = form.Invoke(inputInstance);
            var root = resultPart.GetOrderForm();
            root.OrderGroupId = root.OrderGroupId;
            root.OrderGroup = _order;
            _order.OrderForms.Add(root);
            return this;
        }
    }
}