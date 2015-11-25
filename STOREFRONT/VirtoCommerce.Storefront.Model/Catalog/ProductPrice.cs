using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class ProductPrice : ValueObject<ProductPrice>
    {
        public ProductPrice()
        {
            ListPrice = new Money();
            SalePrice = new Money();
        }
        /// <summary>
        /// Price list id
        /// </summary>
        public string PricelistId { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Product id
        public string ProductId { get; set; }

        /// <summary>
        /// Original product price (old price)
        /// </summary>
        public Money ListPrice { get; set; }

        /// <summary>
        /// Sale product price (new price)
        /// </summary>
        public Money SalePrice { get; set; }

        /// <summary>
        /// Current active discount
        /// </summary>
        public Discount ActiveDiscount { get; set; }

        /// <summary>
        /// Not active but potential better that active discount 
        /// </summary>
        public Discount PotentialDiscount { get; set; }

        /// <summary>
        /// It defines the minimum quantity of products
        /// </summary>
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Tier prices 
        /// </summary>
        public ICollection<TierPrice> TierPrices { get; set; }
    }
}
