namespace VirtoCommerce.Web.Models
{
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