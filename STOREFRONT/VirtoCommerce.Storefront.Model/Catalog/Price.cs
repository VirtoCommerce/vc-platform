using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Price : Entity
    {
        /// <summary>
        /// Price list id
        /// </summary>
        public string PricelistId { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Product id
        public string ProductId { get; set; }

        /// <summary>
        /// Sale price of a product. It can be null, then Sale price will be equal List price
        /// </summary>
        public double? Sale { get; set; }

        /// <summary>
        /// Price of a product. It can be catalog price or purchase price
        /// </summary>
        public double? List { get; set; }

        /// <summary>
        /// It defines the minimum quantity of products
        /// </summary>
        public int? MinQuantity { get; set; }
    }
}
