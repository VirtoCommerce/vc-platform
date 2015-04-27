using System;
using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class DynamicContentPublication
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ICollection<DynamicContentItem> ContentItems { get; set; }

        public ICollection<DynamicContentPlace> ContentPlaces { get; set; }
    }
}