namespace VirtoCommerce.Platform.Core.Common
{
    public class ChangedEntry<T> : ValueObject
    {
        public ChangedEntry(T entry, EntryState state)
        {
            Entry = entry;
            EntryState = state;
        }
        EntryState EntryState { get; set; }
        T Entry { get; set; }
    }
}
