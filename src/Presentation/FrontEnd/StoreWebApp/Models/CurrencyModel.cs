namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CurrencyModel.
	/// </summary>
    public class CurrencyModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CurrencyModel"/> class.
		/// </summary>
		/// <param name="currencyCode">The currency code.</param>
		/// <param name="currencyName">Name of the currency.</param>
        public CurrencyModel(string currencyCode, string currencyName)
        {
            CurrencyCode = currencyCode;
            CurrencyName = currencyName;
        }

		/// <summary>
		/// Gets or sets the currency code.
		/// </summary>
		/// <value>The currency code.</value>
        public string CurrencyCode { get; set; }
		/// <summary>
		/// Gets or sets the name of the currency.
		/// </summary>
		/// <value>The name of the currency.</value>
        public string CurrencyName { get; set; }
    }
}