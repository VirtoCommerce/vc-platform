namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class PriceModel.
	/// </summary>
    public class PriceModel
    {
		/// <summary>
		/// Gets or sets the list price.
		/// </summary>
		/// <value>The list price.</value>
        public decimal? ListPrice { get; set; }
		/// <summary>
		/// Gets or sets the sale price.
		/// </summary>
		/// <value>The sale price.</value>
        public decimal? SalePrice { get; set; }
		/// <summary>
		/// Gets or sets the list price formatted.
		/// </summary>
		/// <value>The list price formatted.</value>
        public string ListPriceFormatted { get; set; }
		/// <summary>
		/// Gets or sets the sale price formatted.
		/// </summary>
		/// <value>The sale price formatted.</value>
        public string SalePriceFormatted { get; set; }
		/// <summary>
		/// Gets or sets the currency.
		/// </summary>
		/// <value>The currency.</value>
        public string Currency { get; set; }
		/// <summary>
		/// Gets or sets the price title.
		/// </summary>
		/// <value>The price title.</value>
        public string PriceTitle { get; set; }

	    public string ItemId { get; set; }

		/// <summary>
		/// Gets the price type.
		/// </summary>
		/// <value>The type.</value>
        public PriceType Type
        {
            get
            {
                if (!ListPrice.HasValue && !SalePrice.HasValue)
                {
                    return PriceType.None;
                }
                return SalePrice < ListPrice ? PriceType.Sale : PriceType.Regular;
            }
        }
    }
}