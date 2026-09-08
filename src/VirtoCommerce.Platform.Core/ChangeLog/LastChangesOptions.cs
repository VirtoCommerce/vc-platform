using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.ChangeLog
{
    public class LastChangesOptions
    {
        /// <summary>
        /// Full type names excluded from last-modified invalidation. Matched case-insensitively.
        /// </summary>
        /// <remarks>
        /// Matching is chain-wide: if any type name in a saved entity's inheritance chain (the entity's
        /// own type or one of its base types, excluding the shared <c>Entity</c> base) is listed here,
        /// NONE of the chain's type names are announced for that entity, including base types not
        /// themselves listed. Use this to silence a noisy leaf type (e.g. an audit-log row-per-field
        /// entity); it also suppresses invalidation of that leaf's base types for the rows it produces,
        /// so do not list a shared base type unless its invalidation may be skipped for every derived
        /// type as well. Listing <c>Entity</c> itself has no effect — it is never part of the chain.
        /// </remarks>
        public IList<string> IgnoredEntityTypes { get; set; } = [];
    }
}
