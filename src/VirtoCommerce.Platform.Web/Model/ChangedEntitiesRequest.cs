using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model
{
    public class ChangedEntitiesRequest
    {
        public IEnumerable<string> EntitiesNames { get; set; }
        public DateTime After { get; set; }
    }
}
