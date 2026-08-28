using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public class LastChangesOptions
    {
        /// <summary>
        /// Full names of entity types not tracked by <see cref="ILastChangesService"/>, so that the change-log API
        /// stops reporting them as changed. An entity is excluded when the list contains its own type name or the name
        /// of any of its base types. Empty by default.
        /// </summary>
        public IList<string> IgnoredEntityTypes { get; set; } = [];
    }
}
