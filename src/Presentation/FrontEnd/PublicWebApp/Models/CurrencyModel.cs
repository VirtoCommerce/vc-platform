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

    /// <summary>
    /// Class CurrenciesModel.
    /// </summary>
    public class CurrenciesModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrenciesModel"/> class.
        /// </summary>
        /// <param name="selectedCurrency">The selected currency.</param>
        /// <param name="currencies">The currencies.</param>
        public CurrenciesModel(string selectedCurrency, CurrencyModel[] currencies)
        {
            SelectedCurrency = selectedCurrency;
            Currencies = currencies;
        }

        /// <summary>
        /// Gets or sets the selected currency.
        /// </summary>
        /// <value>The selected currency.</value>
        public string SelectedCurrency { get; set; }
        /// <summary>
        /// Gets or sets the currencies.
        /// </summary>
        /// <value>The currencies.</value>
        public CurrencyModel[] Currencies { get; set; }
    }


}