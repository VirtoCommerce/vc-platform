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
        public ProductPrice(Currency currency)
        {
            Currency = currency;
            ListPrice = new Money(currency.Code);
            SalePrice = new Money(currency.Code);
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
        /// Absilute price benefit. You save 40.00 USD
        /// </summary>
        public Money AbsoluteBenefit
        {
            get
            {
                return ListPrice - SalePrice;
            }
        }

        /// <summary>
        /// Relative benefit. 30% 
        /// </summary>
        public decimal RelativeBenefit { get; set; }

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
