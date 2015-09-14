using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.QuoteModule.Web.Converters
{
	public static class QuoteRequestConverter
    {
		public static webModel.QuoteRequest ToWebModel(this coreModel.QuoteRequest quoteRequest)
		{
			var retVal = new webModel.QuoteRequest();
			retVal.InjectFrom(quoteRequest);
            retVal.Currency = quoteRequest.Currency;

            if (quoteRequest.Addresses != null)
            {
                retVal.Addresses = quoteRequest.Addresses.Select(x => x.ToWebModel()).ToList();
            }
            if (quoteRequest.Attachments != null)
            {
                retVal.Attachments = quoteRequest.Attachments.Select(x => x.ToWebModel()).ToList();
            }
            if (quoteRequest.Items != null)
            {
                retVal.Items = quoteRequest.Items.Select(x => x.ToWebModel()).ToList();
            }
            if (quoteRequest.DynamicProperties != null)
            {
                retVal.DynamicProperties = quoteRequest.DynamicProperties;
            }
            if(quoteRequest.Totals != null)
            {
                retVal.Totals = quoteRequest.Totals.ToWebModel();
            }

            return retVal;
		}

		public static coreModel.QuoteRequest ToCoreModel(this webModel.QuoteRequest quoteRequest)
		{
			var retVal = new coreModel.QuoteRequest();
			retVal.InjectFrom(quoteRequest);
            retVal.Currency = quoteRequest.Currency;

            if (quoteRequest.Addresses != null)
            {
                retVal.Addresses = quoteRequest.Addresses.Select(x => x.ToCoreModel()).ToList();
            }
            if (quoteRequest.Attachments != null)
            {
                retVal.Attachments = quoteRequest.Attachments.Select(x => x.ToCoreModel()).ToList();
            }
            if (quoteRequest.Items != null)
            {
                retVal.Items = quoteRequest.Items.Select(x => x.ToCoreModel()).ToList();
            }
            if (quoteRequest.DynamicProperties != null)
            {
                retVal.DynamicProperties = quoteRequest.DynamicProperties;
            }
            if (quoteRequest.Totals != null)
            {
                retVal.Totals = quoteRequest.Totals.ToCoreModel();
            }
            return retVal;
		}


	}
}