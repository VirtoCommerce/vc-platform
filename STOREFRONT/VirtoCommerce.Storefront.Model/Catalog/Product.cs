using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class Product : Entity, IDiscountable
    {
        public Product()
        {
            Properties = new List<CatalogProperty>();
            VariationProperties = new List<CatalogProperty>();
            Prices = new List<ProductPrice>();
            Assets = new List<Asset>();
            Variations = new List<Product>();
            Images = new List<Image>();
            Descriptions = new List<EditorialReview>();
            Discounts = new List<Discount>();
            Associations = new List<ProductAssociation>();
        }

        /// <summary>
        /// Manufacturer part number for this product
        /// </summary>
        public string ManufacturerPartNumber { get; set; }

        /// <summary>
        /// Global trade item number
        /// </summary>
        public string Gtin { get; set; }

        /// <summary>
        /// Product code
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Name of this product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product catalog id
        /// </summary>
        public string CatalogId { get; set; }

        /// <summary>
        /// Category id of this product
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// All parent categories ids concatenated with ";". E.g. (1;21;344)
        /// </summary>
        public string Outline { get; set; }

        /// <summary>
        /// Date of last indexing of product, if null - product never was indexed
        /// </summary>
        public DateTime? IndexingDate { get; set; }

        /// <summary>
        /// Titular item id for a variation
        /// </summary>
        public string TitularItemId { get; set; }

        /// <summary>
        /// Indicating whether this product is buyable
        /// </summary>
        public bool IsBuyable { get; set; }

        /// <summary>
        /// Indicating whether this product is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Indicating whether this product inventory is tracked
        /// </summary>
        public bool TrackInventory { get; set; }

        /// <summary>
        /// Maximum quantity of the product that a customer can buy
        /// </summary>
        public int MaxQuantity { get; set; }

        /// <summary>
        /// Minimum quantity of the product that a customer can buy
        /// </summary>
        public int MinQuantity { get; set; }

        /// <summary>
        /// Type of product (can be Physical, Digital or Subscription)
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Weight unit (for physical product only)
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// Weight of product (for physical product only)
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Dimensions measure unit of size (for physical product only)
        /// </summary>
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Height of product size (for physical product only)
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// Length of product size (for physical product only)
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Width of product size (for physical product only)
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// Indicating whether this product can be reviewed in storefront
        /// </summary>
        public decimal EnableReview { get; set; }

        /// <summary>
        /// Maximum number of downloads of product (for digital product only)
        /// </summary>
        public decimal MaxNumberOfDownload { get; set; }

        /// <summary>
        /// Download expiration date (for digital product only)
        /// </summary>
        public DateTime? DownloadExpiration { get; set; }

        /// <summary>
        /// Type of the download (for digital product only)
        /// </summary>
        public string DownloadType { get; set; }

        /// <summary>
        /// Indicating whether this product has user agreement (for digital product only)
        /// </summary>
        public decimal HasUserAgreement { get; set; }

        /// <summary>
        /// Type of product shipping
        /// </summary>
        public string ShippingType { get; set; }

        /// <summary>
        /// Type of product tax
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// Product's vendor
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// List og variation properties
        /// </summary>
        public ICollection<CatalogProperty> VariationProperties { get; set; }

        /// <summary>
        /// List of product assets
        /// </summary>
        public ICollection<Asset> Assets { get; set; }

        /// <summary>
        /// List of product variations
        /// </summary>
        public ICollection<Product> Variations { get; set; }

        /// <summary>
        /// Related or associated products
        /// </summary>
        public ICollection<ProductAssociation> Associations { get; set; }

        /// <summary>
        /// Product description in current language
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product editorial reviews
        /// </summary>
        public ICollection<EditorialReview> Descriptions { get; set; }

        /// <summary>
        /// Current product price
        /// </summary>
        public ProductPrice Price { get; set; }

        /// <summary>
        /// Product prices for other currencies
        /// </summary>
        public ICollection<ProductPrice> Prices { get; set; }

        /// <summary>
        /// Inventory info
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// product seo info
        /// </summary>
        public SeoInfo SeoInfo { get; set; }

        /// <summary>
        /// Product main image
        /// </summary>
        public Image PrimaryImage { get; set; }

        /// <summary>
        /// List of product images
        /// </summary>
        public ICollection<Image> Images { get; set; }

        public bool IsQuotable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Apply prices to product
        /// </summary>
        /// <param name="prices"></param>
        /// <param name="currentCurrency"></param>
        public void ApplyPrices(IEnumerable<ProductPrice> prices, Currency currentCurrency, IEnumerable<Currency> allCurrencies)
        {
            Prices.Clear();
            Price = null;

            Currency = currentCurrency;
            //group prices by currency
            var groupByCurrencyPrices = prices.GroupBy(x => x.Currency).Where(x => x.Any());
            foreach (var currencyGroup in groupByCurrencyPrices)
            {
                //For each currency need get nominal price (with min qty)
                var orderedPrices = currencyGroup.OrderBy(x => x.MinQuantity ?? 0).ThenBy(x => x.ListPrice);
                var nominalPrice = orderedPrices.FirstOrDefault();
                //and add to nominal price other prices as tier prices
                nominalPrice.TierPrices.AddRange(orderedPrices.Select(x => new TierPrice(x.SalePrice, x.MinQuantity ?? 1)));
                //Add nominal price to product prices list 
                Prices.Add(nominalPrice);
            }
            //Need add product price for all currencies (even if not returned from API need make it by currency exchange conversation)
            foreach (var currency in allCurrencies)
            {
                var price = Prices.FirstOrDefault(x => x.Currency == currency);
                if (price == null)
                {
                    price = new ProductPrice(currency);
                    //Convert exist price to new currency
                    if (Prices.Any())
                    {
                        price = Prices.First().ConvertTo(currency);
                        price.TierPrices.Add(new TierPrice(price.SalePrice, 1));
                    }
                    Prices.Add(price);
                }
            }
            //Set current product price for current currency
            Price = Prices.FirstOrDefault(x => x.Currency == currentCurrency);
        }

        #region IHasProperties Members
        public ICollection<CatalogProperty> Properties { get; set; }
        #endregion

        #region IDiscountable Members
        public ICollection<Discount> Discounts { get; private set; }

        public Currency Currency { get; set; }

        public void ApplyRewards(IEnumerable<PromotionReward> rewards)
        {
            var productRewards = rewards.Where(r => r.RewardType == PromotionRewardType.CatalogItemAmountReward && (r.ProductId.IsNullOrEmpty() || r.ProductId.EqualsInvariant(Id)));
            if (productRewards == null)
            {
                return;
            }

            Discounts.Clear();

            foreach (var reward in productRewards)
            {
                //Apply discount to main price
                var discount = reward.ToDiscountModel(Price.SalePrice);
                if (reward.IsValid)
                {
                    Discounts.Add(discount);
                    Price.ActiveDiscount = discount;
                    //apply discount to tier prices
                    foreach (var tierPrice in Price.TierPrices)
                    {
                        discount = reward.ToDiscountModel(tierPrice.Price);
                        tierPrice.ActiveDiscount = discount;
                    }
                }            
            }          
        }
        #endregion

        public string Url { get; set; }

        public override string ToString()
        {
            return String.Format("product #{0} sku: {1} name: {2}", Id ?? "undef", Sku ?? "undef", Name ?? "undef");
        }
    }
}