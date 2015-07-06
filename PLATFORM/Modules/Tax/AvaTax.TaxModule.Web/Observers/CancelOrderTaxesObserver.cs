using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web.Http.Results;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;
using domainModel = VirtoCommerce.Domain.Commerce.Model;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CancelOrderTaxesObserver : IObserver<OrderChangeEvent>
	{
        private readonly ITaxSettings _taxSettings;

        public CancelOrderTaxesObserver(ITaxSettings taxSettings)
        {
            _taxSettings = taxSettings;
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
			var order = context.ModifiedOrder;
            if (order.Status == "Cancelled")
            
            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                
                var request = order.ToAvaTaxCancelRequest(_taxSettings.CompanyCode, CancelCode.DocVoided);
                if (request != null)
                {
                    var getTaxResult = taxSvc.CancelTax(request);
                    if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                    {
                        var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Summary));
                        OnError(new Exception(error));
                    }
                }
            }
            else
            {
                OnError(new Exception("AvaTax credentials not provided"));
            }
		}

		
	}
}