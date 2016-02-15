using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model.Quote
{
    public class QuoteItemFormModel
    {
        public QuoteItemFormModel()
        {
            ProposalPrices = new List<TierPriceFormModel>();
        }

        public string Id { get; set; }

        public string Comment { get; set; }

        public TierPriceFormModel SelectedTierPrice { get; set; }

        public ICollection<TierPriceFormModel> ProposalPrices { get; set; }
    }
}