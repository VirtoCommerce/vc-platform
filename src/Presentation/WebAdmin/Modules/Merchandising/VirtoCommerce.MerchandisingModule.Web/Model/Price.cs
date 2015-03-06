namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Price
    {
        #region Public Properties

        public decimal List { get; set; }
        public int MinQuantity { get; set; }
        public string PricelistId { get; set; }
        public string ProductId { get; set; }
        public decimal? Sale { get; set; }

        #endregion
    }
}
