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
                    quoteRequestModel.BillingAddress = billingAddress.ToViewModel();
                }

                var shippingAddress = quoteRequest.Addresses.FirstOrDefault(a => a.AddressType == DataContracts.AddressType.Shipping);
                if (shippingAddress != null)
                {
                    quoteRequestModel.ShippingAddress = shippingAddress.ToViewModel();
                }

                var firstAddressWithEmail = quoteRequest.Addresses.FirstOrDefault(a => !string.IsNullOrEmpty(a.Email));
                if (firstAddressWithEmail != null)
                {
                    quoteRequestModel.Email = firstAddressWithEmail.Email;
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

            if (quoteRequest.Items != null)
            {
                foreach (var quoteItem in quoteRequest.Items)
                {
                    var quoteItemModel = quoteItem.ToViewModel();
                    quoteRequestModel.Items.Add(quoteItemModel);
                }
            }

            quoteRequestModel.Language = quoteRequest.LanguageCode;
            quoteRequestModel.Number = quoteRequest.Number;
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

            if (quoteRequestModel.BillingAddress != null || quoteRequestModel.ShippingAddress != null)
            {
                quoteRequest.Addresses = new List<DataContracts.Address>();
            }
            if (quoteRequestModel.BillingAddress != null)
            {
                var billingAddress = quoteRequestModel.BillingAddress.ToServiceModel();
                billingAddress.AddressType = DataContracts.AddressType.Billing;
                billingAddress.Email = quoteRequestModel.Email;
                quoteRequest.Addresses.Add(billingAddress);
            }
            if (quoteRequestModel.ShippingAddress != null)
            {
                var shippingAddress = quoteRequestModel.ShippingAddress.ToServiceModel();
                shippingAddress.AddressType = DataContracts.AddressType.Shipping;
                shippingAddress.Email = quoteRequestModel.Email;
                quoteRequest.Addresses.Add(shippingAddress);
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