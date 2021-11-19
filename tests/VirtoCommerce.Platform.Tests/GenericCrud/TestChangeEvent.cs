using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Tests.GenericCrud
{
    public class TestChangeEvent : GenericChangedEntryEvent<TestModel>
    {
        public TestChangeEvent(IEnumerable<GenericChangedEntry<TestModel>> changedEntries) : base(changedEntries)
        {
        }
    }
}
