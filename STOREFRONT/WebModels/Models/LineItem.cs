using DotLiquid;
using System.Runtime.Serialization;

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class LineItem : Drop
    {
        [DataMember]
        public Fulfillment Fulfillment { get; set; }

        [DataMember]
        public int Grams { get; set; }

        [DataMember]
        public string Handle { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public decimal LinePrice { get { return this.Quantity * this.Price; } }

        [DataMember]
        public decimal Price { get; set; }

        public Product Product { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public bool RequiresShipping { get; set; }

        [DataMember]
        public string Sku { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Url { get; set; }

        public Variant Variant { get; set; }

        [DataMember]
        public string VariantId { get; set; }

        [DataMember]
        public string Vendor { get; set; }

        [DataMember]
        public bool Taxable { get; set; }

        [DataMember]
        public decimal TaxAmount { get; set; }
    }
}