using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model
{
    public class ChangedEntitiesRequest
    {
        public IEnumerable<string> EntityNames { get; set; }
        public DateTime ModifiedSince { get; set; }
    }
}
