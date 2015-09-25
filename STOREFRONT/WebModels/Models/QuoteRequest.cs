using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Models
{
    public class QuoteRequest : Drop
    {
        public QuoteRequest()
        {
        }

        public QuoteRequest(string storeId, string customerId)
        {
            Attachments = new List<Attachment>();
            Items = new List<QuoteItem>();
            TaxLines = new List<TaxLine>();
            Totals = new QuoteTotals();

            CustomerId = customerId;
            StoreId = storeId;
        }

        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Number { get; set; }

        public string StoreId { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool IsLocked { get; set; }

        public string Status { get; set; }

        public string Currency { get; set; }

        public string Coupon { get; set; }

        public string Language { get; set; }

        public bool IsCancelled { get; set; }

        public DateTime? CancelledAt { get; set; }

        public string CancelReason { get; set; }

        public ICollection<TaxLine> TaxLines { get; set; }

        public string Comment { get; set; }

        public string Email { get; set; }

        public CustomerAddress BillingAddress { get; set; }

        public CustomerAddress ShippingAddress { get; set; }

        public ICollection<QuoteItem> Items { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public string Tag { get; set; }

        public QuoteTotals Totals { get; set; }

        public ICollection<string> BillingAddressErrors
        {
            get
            {
                var errors = new List<string>();

                if (BillingAddress != null)
                {
                    errors = GetAddressErrors(BillingAddress);
                }

                return errors;
            }
        }

        public ICollection<string> ShippingAddressErrors
        {
            get
            {
                var errors = new List<string>();

                if (ShippingAddress != null)
                {
                    errors = GetAddressErrors(ShippingAddress);
                }

                return errors;
            }
        }

        public bool ProposalPricesUnique
        {
            get
            {
                bool isUnique = true;

                foreach (var quoteItem in Items)
                {
                    var uniqueProposalPrices = quoteItem.ProposalPrices.GroupBy(pp => pp.Quantity).Select(pp => pp.First());
                    if (quoteItem.ProposalPrices.Count != uniqueProposalPrices.Count())
                    {
                        isUnique = false;
                        break;
                    }
                }

                return isUnique;
            }
        }

        public int ItemsCount
        {
            get
            {
                return Items.Count;
            }
        }

        public bool IsTransient
        {
            get
            {
                return string.IsNullOrEmpty(Id);
            }
        }

        public QuoteRequest AddItem(QuoteItem quoteItem)
        {
            quoteItem.Id = null;
            Items.Add(quoteItem);

            return this;
        }

        public QuoteRequest RemoveItem(string id)
        {
            var quoteItem = Items.FirstOrDefault(i => i.Id == id);

            Items.Remove(quoteItem);

            return this;
        }

        public QuoteRequest MergeQuoteWith(QuoteRequest anotherQuote)
        {
            foreach (var anotherQuoteItem in anotherQuote.Items)
            {
                AddItem(anotherQuoteItem);
            }

            return this;
        }

        private List<string> GetAddressErrors(CustomerAddress address)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(address.Address1))
            {
                errors.Add("Field \"Address line 1\" is required");
            }
            if (string.IsNullOrEmpty(address.City))
            {
                errors.Add("Field \"City\" is required");
            }
            if (string.IsNullOrEmpty(address.Company))
            {
                errors.Add("Field \"Company\" is required");
            }
            if (string.IsNullOrEmpty(address.Country))
            {
                errors.Add("Field \"Country\" is required");
            }
            if (string.IsNullOrEmpty(address.FirstName))
            {
                errors.Add("Field \"First name\" is required");
            }
            if (string.IsNullOrEmpty(address.LastName))
            {
                errors.Add("Field \"Last name\" is required");
            }
            if (string.IsNullOrEmpty(address.Phone))
            {
                errors.Add("Field \"Phone\" is required");
            }
            if (string.IsNullOrEmpty(address.Zip))
            {
                errors.Add("Field \"Postal code\" is required");
            }

            return errors;
        }
    }
}