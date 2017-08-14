using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests.Notifications.Models
{
    public class CustomerOrder : IEntity
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string StoreName { get; set; }
        public string OrganizationName { get; set; }
        public string EmployeeName { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<LineItem> Items { get; set; }
    }
}