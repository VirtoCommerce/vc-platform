using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Models
{
    public class QuoteRequest : Drop
    {
        public QuoteRequest(string storeId, string customerId)
        {
            Attachments = new List<Attachment>();
            Items = new List<QuoteItem>();
            TaxLines = new List<TaxLine>();

            CustomerId = customerId;
            StoreId = storeId;
        }

        public string Id { get; set; }

        public string Number { get; set; }

        public string StoreId { get; set; }

        public string CustomerId { get; set; }

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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool RequestShippingQuote { get; set; }

        public ICollection<QuoteItem> Items { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public string Tag { get; set; }

        public bool IsSubmitted { get; set; }

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
            Items.Add(quoteItem);

            return this;
        }

        public QuoteRequest RemoveItem(string id)
        {
            var quoteItem = Items.FirstOrDefault(i => i.Id == id);

            Items.Remove(quoteItem);

            return this;
        }
    }
}