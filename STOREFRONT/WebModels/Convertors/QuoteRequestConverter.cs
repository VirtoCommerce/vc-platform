using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Web.Models;
using DataContracts = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
{
    public static class QuoteRequestConverter
    {
        public static QuoteRequest ToViewModel(this DataContracts.Quotes.QuoteRequest quoteRequest)
        {
            var quoteRequestModel = new QuoteRequest(quoteRequest.StoreId, quoteRequest.CustomerId);

            if (quoteRequest.Addresses != null)
            {
                var billingAddress = quoteRequest.Addresses.FirstOrDefault(a => a.AddressType == DataContracts.AddressType.Billing);
                if (billingAddress != null)
                {
                    quoteRequestModel.Email = billingAddress.Email;
                    quoteRequestModel.FirstName = billingAddress.FirstName;
                    quoteRequestModel.LastName = billingAddress.LastName;
                }
            }

            if (quoteRequest.Attachments != null)
            {
                foreach (var attachment in quoteRequest.Attachments)
                {
                    quoteRequestModel.Attachments.Add(attachment.ToViewModel());
                }
            }

            quoteRequestModel.CancelledAt = quoteRequest.CancelledDate;
            quoteRequestModel.CancelReason = quoteRequest.CancelReason;
            quoteRequestModel.Comment = quoteRequest.Comment;
            quoteRequestModel.Coupon = quoteRequest.Coupon;
            quoteRequestModel.CreatedAt = quoteRequest.CreatedDate;
            quoteRequestModel.Currency = quoteRequest.Currency;
            quoteRequestModel.CustomerName = quoteRequest.CustomerName;
            quoteRequestModel.ExpirationDate = quoteRequest.ExpirationDate;
            quoteRequestModel.Id = quoteRequest.Id;
            quoteRequestModel.IsCancelled = quoteRequest.IsCancelled;
            quoteRequestModel.IsLocked = quoteRequest.IsLocked;
            quoteRequestModel.IsSubmitted = quoteRequest.IsSubmitted;

            if (quoteRequest.Items != null)
            {
                foreach (var quoteItem in quoteRequest.Items)
                {
                    quoteRequestModel.Items.Add(quoteItem.ToViewModel());
                }
            }

            quoteRequestModel.Language = quoteRequest.LanguageCode;
            quoteRequestModel.Number = quoteRequest.Number;
            quoteRequestModel.RequestShippingQuote = false; // TODO
            quoteRequestModel.Status = quoteRequest.Status;
            quoteRequestModel.Tag = quoteRequest.Tag;

            if (quoteRequest.TaxDetails != null)
            {
                foreach (var taxDetail in quoteRequest.TaxDetails)
                {
                    quoteRequestModel.TaxLines.Add(taxDetail.ToViewModel());
                }
            }

            if (quoteRequest.Totals != null)
            {
                quoteRequestModel.Totals.AdjustmentQuoteExlTax = quoteRequest.Totals.AdjustmentQuoteExlTax;
                quoteRequestModel.Totals.DiscountTotal = quoteRequest.Totals.DiscountTotal;
                quoteRequestModel.Totals.GrandTotalExlTax = quoteRequest.Totals.GrandTotalExlTax;
                quoteRequestModel.Totals.GrandTotalInclTax = quoteRequest.Totals.GrandTotalInclTax;
                quoteRequestModel.Totals.OriginalSubTotalExlTax = quoteRequest.Totals.OriginalSubTotalExlTax;
                quoteRequestModel.Totals.ShippingTotal = quoteRequest.Totals.ShippingTotal;
                quoteRequestModel.Totals.SubTotalExlTax = quoteRequest.Totals.SubTotalExlTax;
                quoteRequestModel.Totals.TaxTotal = quoteRequest.Totals.TaxTotal;
            }

            return quoteRequestModel;
        }

        public static DataContracts.Quotes.QuoteRequest ToServiceModel(this QuoteRequest quoteRequestModel)
        {
            var quoteRequest = new DataContracts.Quotes.QuoteRequest();

            if (quoteRequestModel.Attachments != null)
            {
                quoteRequest.Attachments = new List<DataContracts.Attachment>();
                foreach (var attachmentModel in quoteRequestModel.Attachments)
                {
                    quoteRequest.Attachments.Add(attachmentModel.ToServiceModel());
                }
            }

            if (quoteRequestModel.RequestShippingQuote)
            {
                quoteRequest.Addresses = new List<DataContracts.Address>();
                quoteRequest.Addresses.Add(new DataContracts.Address
                {
                    AddressType = DataContracts.AddressType.Shipping,
                    Email = quoteRequestModel.Email,
                    FirstName = quoteRequestModel.FirstName,
                    LastName = quoteRequestModel.LastName
                });
            }

            quoteRequest.CancelledDate = quoteRequestModel.CancelledAt;
            quoteRequest.CancelReason = quoteRequestModel.CancelReason;
            quoteRequest.Comment = quoteRequestModel.Comment;
            quoteRequest.Coupon = quoteRequestModel.Coupon;
            quoteRequest.Currency = quoteRequestModel.Currency;
            quoteRequest.CustomerId = quoteRequestModel.CustomerId;
            quoteRequest.CustomerName = quoteRequestModel.CustomerName;
            quoteRequest.ExpirationDate = quoteRequestModel.ExpirationDate;
            quoteRequest.Id = quoteRequestModel.Id;
            quoteRequest.IsCancelled = quoteRequestModel.IsCancelled;
            quoteRequest.IsLocked = quoteRequestModel.IsLocked;
            quoteRequest.IsSubmitted = quoteRequestModel.IsSubmitted;

            quoteRequest.Items = new List<DataContracts.Quotes.QuoteItem>();
            foreach (var quoteItemModel in quoteRequestModel.Items)
            {
                quoteRequest.Items.Add(quoteItemModel.ToServiceModel());
            }

            quoteRequest.LanguageCode = quoteRequestModel.Language;
            quoteRequest.Number = quoteRequestModel.Number;
            quoteRequest.Status = quoteRequestModel.Status;
            quoteRequest.StoreId = quoteRequestModel.StoreId;
            quoteRequest.Tag = quoteRequestModel.Tag;

            quoteRequest.TaxDetails = new List<DataContracts.TaxDetail>();
            foreach (var taxLineModel in quoteRequestModel.TaxLines)
            {
                quoteRequest.TaxDetails.Add(taxLineModel.ToServiceModel());
            }

            return quoteRequest;
        }
    }
}