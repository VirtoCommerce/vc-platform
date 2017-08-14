using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests.Notifications.Models
{
    public class LineItem : IEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal PlacedPrice { get; set; }
        public int Quantity { get; set; }
        public virtual decimal ExtendedPrice
        {
            get
            {
                return PlacedPrice * Quantity;
            }
        }
    }
}