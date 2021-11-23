using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestChangedEvent : GenericChangedEntryEvent<TestModel>
    {
        public static IEnumerable<GenericChangedEntry<TestModel>> testChangedEntries;
        public TestChangedEvent(IEnumerable<GenericChangedEntry<TestModel>> changedEntries) : base(changedEntries)
        {
            testChangedEntries = changedEntries;
        }
    }
}
