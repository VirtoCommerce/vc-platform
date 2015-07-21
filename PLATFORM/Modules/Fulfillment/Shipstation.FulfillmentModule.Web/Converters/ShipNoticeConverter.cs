using Shipstation.FulfillmentModule.Web.Models.Notice;
using VirtoCommerce.Domain.Order.Model;

namespace Shipstation.FulfillmentModule.Web.Converters
{
    public static class ShipNoticeConverter
    {
        public static void Patch(this CustomerOrder order, ShipNotice notice)
        {
            //TODO Patch order with Shipstation notify data
        }
    }
}