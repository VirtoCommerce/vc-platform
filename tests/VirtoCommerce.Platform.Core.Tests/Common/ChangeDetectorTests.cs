using VirtoCommerce.Platform.Core.ChangesUtils;
using Xunit;
using FluentAssertions;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    public class ChangeDetectorTests
    {
        private class Parent
        {
            [UseInChangesDetector("")]
            public string A { get; set; } = "A";
        }

        private class Child : Parent
        {
            [UseInChangesDetector("")]
            public string B { get; set; } = "B";
        }

        [Fact]
        public void DetectChanges_ByChilds()
        {
            var oldObj = new Child();
            var newObj = new Child() { A = "A1", B = "B1" };

            var changes = ChangesDetector.GatherByType(newObj, oldObj);
            changes.Values.Count.Should().Be(2);
        }

        [Fact]
        public void DetectChanges_ByParents()
        {
            var oldObj = new Child();
            var newObj = new Child() { A = "A1", B = "B1" };

            var changes = ChangesDetector.GatherByType<Parent>(newObj, oldObj);
            changes.Values.Count.Should().Be(1);
        }

        [Fact]
        public void DetectChanges_AsIs()
        {
            var oldObj = new Child();
            var newObj = new Child() { A = "A1", B = "B1" };

            var changes = ChangesDetector.Gather(newObj, oldObj);
            changes.Values.Count.Should().Be(2);
        }
    }
}
