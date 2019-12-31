using System;

namespace VirtoCommerce.Platform.Core.Common
{
    public class TrackingEntry
    {
        public object Entity { get; set; }
        public EntryState EntryState { get; set; }
        internal bool IsSubscribed { get; set; }

        public override string ToString()
        {
            return String.Format("{1} {0}", Entity ?? "null", EntryState);
        }
    }
}
