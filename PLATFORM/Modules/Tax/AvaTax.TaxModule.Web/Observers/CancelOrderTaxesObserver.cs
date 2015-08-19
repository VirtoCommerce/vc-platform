using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Logging;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using Common.Logging;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using domainModel = VirtoCommerce.Domain.Commerce.Model;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CancelOrderTaxesObserver : IObserver<OrderChangeEvent>
	{
        private readonly ITaxSettings _taxSettings;
        private readonly AvalaraLogger _logger;

        public CancelOrderTaxesObserver(ITaxSettings taxSettings, ILog log)
        {
            _taxSettings = taxSettings;
            _logger = new AvalaraLogger(log);
        }

		#region IObserver<CustomerOrder> Members

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(OrderChangeEvent value)
		{
            if (value.ChangeState == EntryState.Modified)
			    CancelCustomerOrderTaxes(value);
		}

		#endregion
		private void CancelCustomerOrderTaxes(OrderChangeEvent context)
		{
            if (!context.ModifiedOrder.IsCancelled)
		    {
		        return;
		    }

            LogInvoker<AvalaraLogger.TaxRequestContext>.Execute(log =>
                {
		            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
		                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
		                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
		            {
                        var order = context.ModifiedOrder;
		                var request = order.ToAvaTaxCancelRequest(_taxSettings.CompanyCode, CancelCode.DocDeleted);
		                if (request != null)
		                {
                            log.docCode = request.DocCode;
                            log.docType = request.DocType.ToString();

                            var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
		                    var getTaxResult = taxSvc.CancelTax(request);

		                    if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
		                    {
		                        var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
		                        throw new Exception(error);
		                    }
		                }
		            }
		            else
		            {
		                throw new Exception("AvaTax credentials not provided or tax calculation disabled");
		            }
                })
                .OnError(_logger, AvalaraLogger.EventCodes.TaxCalculationError)
                .OnSuccess(_logger, AvalaraLogger.EventCodes.GetTaxRequestTime);
		}
	}
}