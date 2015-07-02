using System;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Inventory
    {
        public string FulfillmentCenterId { get; set; }

        public long InStockQuantity { get; set; }

        public long ReservedQuantity { get; set; }
        
        public long ReorderMinQuantity { get; set; }
        
        public long PreorderQuantity { get; set; }
        
        public long BackorderQuantity { get; set; }
        
        public bool AllowBackorder { get; set; }
        
        public bool AllowPreorder { get; set; }
        
        public long InTransit { get; set; }
        
        public DateTime? PreorderAvailabilityDate { get; set; }
        
        public DateTime? BackorderAvailabilityDate { get; set; }
        
        public int Status { get; set; }
    }
}