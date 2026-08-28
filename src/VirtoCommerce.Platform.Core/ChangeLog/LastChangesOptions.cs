using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public class LastChangesOptions
    {
        public IList<string> IgnoredEntityTypes { get; set; } = [];
    }
}
