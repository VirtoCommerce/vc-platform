using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Shipstation.FulfillmentModule.Web.Models.Notice;
using VirtoCommerce.Domain.Order.Model;

namespace Shipstation.FulfillmentModule.Web.Converters
{
    public static class ShipNoticeConverter
    {
        public static void Patch(this CustomerOrder order, ShipNotice notice)
        {
            if (order.Shipments != null && order.Items.All(oi => notice.Items.Any(shi => shi.SKU == oi.ProductId && int.Parse(shi.Quantity) >= oi.Quantity)))
            {
                order.Shipments.ForEach(sh =>
                {
                    sh.Status = "Sent";
                    sh.Number = notice.TrackingNumber;
                    //sh.DeliveryAddress = new Address
                    //{
                    //    AddressType = AddressType.Shipping,
                    //    City = shipnotice.Recipient.City,
                    //    CountryCode = shipnotice.Recipient.Country,
                    //    PostalCode = shipnotice.Recipient.PostalCode,
                    //    Line1 = shipnotice.Recipient.Address1,
                    //    Line2 = shipnotice.Recipient.Address2,
                    //    RegionName = shipnotice.Recipient.State != null && !shipnotice.Recipient.State.Length.Equals(2) ? shipnotice.Recipient.State : null,
                    //    RegionId = shipnotice.Recipient.State != null && shipnotice.Recipient.State.Length.Equals(2) ? shipnotice.Recipient.State : null,
                    //    Organization = shipnotice.Recipient.Company,
                    //    FirstName = shipnotice.Recipient.Name
                    //};
                });
                order.Status = "Completed";
            }
        }
    }
}