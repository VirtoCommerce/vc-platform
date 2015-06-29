using System;
using System.Linq;
using AvaTax.TaxModule.Web.Converters;
using AvaTax.TaxModule.Web.Services;
using AvaTaxCalcREST;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Common;

namespace AvaTax.TaxModule.Web.Observers
{
    public class CalculateOrderTaxesObserver : IObserver<OrderChangeEvent>
	{
        private readonly ITaxSettings _taxSettings;

        public CalculateOrderTaxesObserver(ITaxSettings taxSettings)
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
			    CalculateCustomerOrderTaxes(value);
		}

		#endregion
		private void CalculateCustomerOrderTaxes(OrderChangeEvent context)
		{
			var order = context.ModifiedOrder;

            if (order.Items.Any())
                order.Items.ForEach(x =>
                {
                    x.Tax = 0;
                    x.TaxDetails = null;
                });

		    order.Tax = 0;

            if (_taxSettings.IsEnabled && !string.IsNullOrEmpty(_taxSettings.Username) && !string.IsNullOrEmpty(_taxSettings.Password)
                && !string.IsNullOrEmpty(_taxSettings.ServiceUrl)
                && !string.IsNullOrEmpty(_taxSettings.CompanyCode))
            {
                var taxSvc = new JsonTaxSvc(_taxSettings.Username, _taxSettings.Password, _taxSettings.ServiceUrl);
                var isCommit = order.InPayments != null && order.InPayments.Any() && order.InPayments.All(pi => pi.IsApproved);
                var request = order.ToAvaTaxRequest(_taxSettings.CompanyCode, isCommit);
                if (request != null)
                {
                    var getTaxResult = taxSvc.GetTax(request);
                    if (!getTaxResult.ResultCode.Equals(SeverityLevel.Success))
                    {
                        var error = string.Join(Environment.NewLine, getTaxResult.Messages.Select(m => m.Details));
                        OnError(new Exception(error));
                    }
                    else
                    {
                        foreach (TaxLine taxLine in getTaxResult.TaxLines ?? Enumerable.Empty<TaxLine>())
                        {
                            order.Items.ToArray()[Int32.Parse(taxLine.LineNo)].Tax = taxLine.Tax;
                            //foreach (TaxDetail taxDetail in taxLine.TaxDetails ?? Enumerable.Empty<TaxDetail>())
                            //{
                            //    order.Items.ToArray()[Int32.Parse(taxLine.LineNo)].TaxDetails = new[]
                            //    {
                            //        new VirtoCommerce.Domain.Commerce.Model.TaxDetail
                            //        {
                            //            Amount = taxDetail.Tax,
                            //            Name = taxDetail.TaxName,
                            //            Rate = taxDetail.Rate
                            //        }
                            //    };
                            //}
                  
                        }
                        order.Tax = getTaxResult.TotalTax;
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