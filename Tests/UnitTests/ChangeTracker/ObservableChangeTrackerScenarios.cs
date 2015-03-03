using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using Xunit;

namespace CommerceFoundation.UnitTests.ChangeTracker
{
    public class ObservableChangeTrackerScenarios
    {
        [Fact]
        public void Can_changetracker_manipulatecollection()
        {
            var tracker = new ObservableChangeTracker();
            var entity = new TestEntity();

            tracker.Add(entity);
            tracker.Remove(entity);
            tracker.Add(entity);
        }
    }

    public class TestEntity : StorageEntity
    {
    }
}
