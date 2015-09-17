using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
{
    public static class TaxLineConverter
    {
        public static TaxLine ToViewModel(this DataContracts.TaxDetail taxDetail)
        {
            var taxLineModel = new TaxLine();

            taxLineModel.Price = taxDetail.Amount;
            taxLineModel.Rate = taxDetail.Rate;
            taxLineModel.Title = taxDetail.Name;

            return taxLineModel;
        }

        public static DataContracts.TaxDetail ToServiceModel(this TaxLine taxLineModel)
        {
            var taxDetail = new DataContracts.TaxDetail();

            taxDetail.Amount = taxLineModel.Price;
            taxDetail.Name = taxLineModel.Title;
            taxDetail.Rate = taxLineModel.Rate;

            return taxDetail;
        }
    }
}