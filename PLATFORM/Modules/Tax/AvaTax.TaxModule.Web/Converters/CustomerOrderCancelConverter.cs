using System.Linq;
using AvaTaxCalcREST;

namespace AvaTax.TaxModule.Web.Converters
{
    public static class CustomerOrderCancelConverter
    {
        public static CancelTaxRequest ToAvaTaxCancelRequest(this VirtoCommerce.Domain.Order.Model.CustomerOrder order, string companyCode, CancelCode cancelCode)
        {
            if (order.Addresses != null && order.Addresses.Any() && order.Items != null && order.Items.Any())
            {
                // Document Level Elements
                // Required Request Parameters
                var cancelTaxRequest = new CancelTaxRequest
                {
                    CompanyCode = companyCode,
                    DocCode = order.Number,
                    DocType = DocType.SalesInvoice,
                    CancelCode = cancelCode
                };

                return cancelTaxRequest;
            }

            return null;
        }
    }
}