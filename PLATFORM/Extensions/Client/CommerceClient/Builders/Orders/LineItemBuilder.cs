using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.Client.Builders
{
    public class LineItemBuilder
    {
        private LineItem _lineItem;

        public LineItemBuilder()
        {
            _lineItem = new LineItem();            
        }

        public LineItem Build()
        {
            return _lineItem;
        }

        public LineItemBuilder(LineItem lineItem)
        {
            _lineItem = lineItem;
        }

        public LineItemBuilder Discount(LineItemDiscount discount)
        {
            _lineItem.Discounts.Add(discount);
            return this;
        }

        public LineItemBuilder Prop<TFunc>(Func<LineItem, TFunc> expression)
        {
            //var inputInstance = new LineItem();
            var inputInstance = _lineItem;
            var item = expression.Invoke(inputInstance);

            return this;
        }


        public LineItemBuilder Catalog(string catalog, string category)
        {
            _lineItem.Catalog = catalog;
            _lineItem.CatalogCategory = category;
            return this;
        }

        public LineItemBuilder Quantity(decimal quantity)
        {
            _lineItem.Quantity = quantity;
            return this;
        }

        public LineItemBuilder MinQuantity(decimal minquantity)
        {
            _lineItem.MinQuantity = minquantity;
            return this;
        }

        public LineItemBuilder Option(LineItemOption option)
        {
            _lineItem.Options.Add(option);
            return this;
        }
        
    }
}
