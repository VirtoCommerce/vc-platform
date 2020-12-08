using System.Collections.Generic;

namespace VirtoCommerce.Platform.Web.Model
{
    public class ChangedEntitiesResponse
    {
        public IEnumerable<LastModifiedEntityResponse> LastModifiedDates { get; set; }
    }
}
