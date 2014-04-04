using System;
using System.Collections;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{
    public class MarketingHelper
    {
        private readonly PromotionClient _client;

        public MarketingHelper(PromotionClient client)
        {
            _client = client;
        }

	    /// <summary>
	    ///     Gets the item price model.
	    /// </summary>
	    /// <param name="item">The item.</param>
	    /// <param name="lowestPrice">The lowest price.</param>
	    /// <param name="tags">Additional tags for promotion evaluation</param>
	    /// <returns>price model</returns>
	    /// <exception cref="System.ArgumentNullException">item</exception>
	    public PriceModel GetItemPriceModel(Item item, Price lowestPrice, Hashtable tags)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (lowestPrice == null)
            {
                return new PriceModel();
            }

            var price = lowestPrice.Sale ?? lowestPrice.List;
            var discount = _client.GetItemDiscountPrice(item, lowestPrice, tags);
            var priceModel = CreatePriceModel(price, price - discount, UserHelper.CustomerSession.Currency);
	        priceModel.ItemId = item.ItemId;
            //If has any variations
            /* performance too slow with this method, need to store value on indexing instead
	        if (CatalogHelper.CatalogClient.GetItemRelations(item.ItemId).Any())
	        {
	            priceModel.PriceTitle = "Starting from:".Localize();
	        }
             * */
            return priceModel;
        }

        /// <summary>
        /// Creates the price model.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sale">The sale.</param>
        /// <param name="currency">The currency.</param>
        /// <returns>price model</returns>
        public static PriceModel CreatePriceModel(decimal list, decimal? sale, string currency)
        {
            var priceModel = new PriceModel {ListPrice = list, SalePrice = sale ?? list, Currency = currency};
            if (priceModel.ListPrice != null)
            {
                priceModel.ListPriceFormatted = StoreHelper.FormatCurrency(priceModel.ListPrice.Value, priceModel.Currency);
            }
            if (priceModel.SalePrice != null)
            {
                priceModel.SalePriceFormatted = StoreHelper.FormatCurrency(priceModel.SalePrice.Value, priceModel.Currency);
            }
            return priceModel;
        }
    }
}