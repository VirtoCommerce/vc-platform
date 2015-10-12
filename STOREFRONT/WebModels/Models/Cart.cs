using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;
using System;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Cart : Drop
    {
        public Cart(string storeId, string customerId, string currency, string language)
        {
            CreatedAt = System.DateTime.UtcNow;
            CreatedBy = customerId;
            Currency = currency;
            CustomerId = customerId;
            Items = new List<LineItem>();
            Language = language;
            Name = "default";
            StoreId = storeId;
        }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Currency { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public string StoreId { get; set; }

        [DataMember]
        public string Attributes { get; set; }

        [DataMember]
        public int ItemCount
        {
            get
            {
                if (this.Items == null)
                {
                    return 0;
                }

                return this.Items.Count();
            }
        }

        [DataMember]
        public List<LineItem> Items { get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public decimal TotalPrice
        {
            get
            {
                if (this.Items == null)
                {
                    return 0;
                }
                return this.Items.Sum(x => x.Quantity * x.Price);
            }
        }

        [DataMember]
        public decimal TotalWeight
        {
            get
            {
                return 0;
            }
        }

        public bool IsTransient
        {
            get
            {
                return string.IsNullOrEmpty(Key);
            }
        }

        public Cart AddLineItem(LineItem lineItem)
        {
            var existingLineItem = Items.FirstOrDefault(i => i.ProductId == lineItem.ProductId);

            if (existingLineItem != null)
            {
                existingLineItem.Quantity = lineItem.Quantity;
            }
            else
            {
                lineItem.Id = null;
                Items.Add(lineItem);
            }

            return this;
        }

        public Cart MergeCartWith(Cart anotherCart)
        {
            foreach (var anotherLineItem in anotherCart.Items)
            {
                AddLineItem(anotherLineItem);
            }

            return this;
        }
    }
}