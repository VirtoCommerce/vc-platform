using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Client.Builders
{
    public class OrderFormBuilder
    {
        private OrderForm _form;

        public OrderFormBuilder()
        {
            _form = new OrderForm();
        }

        public OrderForm GetOrderForm()
        {
            return _form;
        }

        public OrderFormBuilder Name(string name)
        {
            _form.Name = name;
            return this;
        }

        public OrderFormBuilder LineItem(Func<LineItemBuilder, LineItemBuilder> expression)
        {
            var inputInstance = new LineItemBuilder();
            var item = expression.Invoke(inputInstance);
            var root = item.Build();
            _form.LineItems.Add(root);
            root.OrderFormId = _form.OrderFormId;
            root.OrderForm = _form;

            return this;
        }
    }
}
