using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class TaxLine : Entity
    {
        public TaxLine(Currency currency)
        {
            Amount = new Money(currency);
            Price = new Money(currency);
        }
        /// <summary>
        /// represent  original object code (lineItem, shipment etc)
        /// </summary>
        public string Code { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Tax line total amount
        /// </summary>
        public Money Amount { get; set; }
        /// <summary>
        /// Tax line one item price
        /// </summary>
        public Money Price { get; set; }
        public string TaxType { get; set; }
    }
}
