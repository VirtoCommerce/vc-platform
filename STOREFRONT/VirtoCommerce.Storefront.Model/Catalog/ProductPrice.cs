using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class ProductPrice : ValueObject<ProductPrice>, IConvertible<ProductPrice>
    {
        public ProductPrice(Currency currency)
        {
            Currency = currency;
            ListPrice = new Money(currency);
            SalePrice = new Money(currency);
            TierPrices = new List<TierPrice>();
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
                var retVal = ListPrice - SalePrice;
                if(ActiveDiscount != null)
                {
                    retVal += ActiveDiscount.Amount;
                }
                return retVal;
            }
        }

        /// <summary>
        /// Relative benefit. 30% 
        /// </summary>
        public decimal RelativeBenefit
        {
            get
            {
                if (ListPrice.Amount > 0)
                {
                    return Math.Round(AbsoluteBenefit.Amount / ListPrice.Amount, 2);
                }
                return 0;
            }
        }

        /// <summary>
        /// Original product price (old price)
        /// </summary>
        public Money ListPrice { get; set; }
 
        /// <summary>
        /// Sale product price (new price)
        /// </summary>
        public Money SalePrice { get; set; }

        /// <summary>
        /// Actual price includes all kind of discounts
        /// </summary>
        public Money ActualPrice
        {
            get
            {
                return ListPrice - AbsoluteBenefit;
            }
        }

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

        
        /// <summary>
        /// Return tire price for passed quantity
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public TierPrice GetTierPrice(int quantity)
        {
            var retVal = TierPrices.OrderBy(x => x.Quantity).LastOrDefault(x => x.Quantity <= quantity);
            if(retVal == null)
            {
                retVal = new TierPrice(SalePrice, 1);
            }
            return retVal;
        }

        #region IConvertible<ProductPrice> Members
        /// <summary>
        /// Convert current product price to other currency using currency exchange rate
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public ProductPrice ConvertTo(Currency currency)
        {
            var retVal = new ProductPrice(currency);
            retVal.ListPrice = ListPrice.ConvertTo(currency);
            retVal.SalePrice = SalePrice.ConvertTo(currency);
            retVal.ProductId = ProductId;
            if (ActiveDiscount != null)
            {
                retVal.ActiveDiscount = ActiveDiscount.ConvertTo(currency);
            }
            if(PotentialDiscount != null)
            {
                retVal.PotentialDiscount = PotentialDiscount.ConvertTo(currency);
            }
            return retVal;
        } 
        #endregion
    }
}
