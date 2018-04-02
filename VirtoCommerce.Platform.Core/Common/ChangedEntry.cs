namespace VirtoCommerce.Platform.Core.Common
{
    public class ChangedEntry<T> : ValueObject
    {
        public ChangedEntry(T entry, EntryState state)
            : this(entry, entry, state)
        {           
        }

        public ChangedEntry(T newEntry, T oldEntry, EntryState state)
        {
            NewEntry = newEntry;
            OldEntry = oldEntry;
            EntryState = state;
        }

        public EntryState EntryState { get; set; }
        public T NewEntry { get; set; }
        public T OldEntry { get; set; }
    }
}
