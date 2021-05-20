using FluentAssertions;
using VirtoCommerce.Platform.Core.Utils.ChangeDetector;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    public class ChangeDetectorTests
    {
        private class Parent
        {
            [DetectChanges("")]
            public virtual string Prop1 { get; set; } = "1";

            [DetectChanges("")]
            public virtual string Prop2 { get; set; } = "2";
        }

        private class Child : Parent
        {
            [DetectChanges("")]
            public virtual string Prop3 { get; set; } = "3";

            [DetectChanges("")]
            public virtual string Prop4 { get; set; } = "4";
        }


        [Fact]
        public void DetectChanges_ByChild()
        {
            var oldObj = new Child();
            var newObj = new Child() { Prop1 = "11", Prop2 = "22", Prop3 = "33", Prop4 = "44" };

            var changes = ChangesDetector.Gather(newObj, oldObj, typeof(Child), false);
            changes.Values.Count.Should().Be(2);
            changes.Values.Contains("Changes: Prop3: 3 -> 33").Should().BeTrue();
            changes.Values.Contains("Changes: Prop4: 4 -> 44").Should().BeTrue();
        }

        [Fact]
        public void DetectChanges_ByParent()
        {
            var oldObj = new Child();
            var newObj = new Child() { Prop1 = "11", Prop2 = "22", Prop3 = "33", Prop4 = "44" };

            var changes = ChangesDetector.Gather(newObj, oldObj, typeof(Parent));
            changes.Values.Count.Should().Be(2);
            changes.Values.Contains("Changes: Prop1: 1 -> 11").Should().BeTrue();
            changes.Values.Contains("Changes: Prop2: 2 -> 22").Should().BeTrue();
        }

        [Fact]
        public void DetectChanges_AsIs()
        {
            var oldObj = new Child();
            var newObj = new Child() { Prop1 = "11", Prop2 = "22", Prop3 = "33", Prop4 = "44" };

            var changes = ChangesDetector.Gather(newObj, oldObj);
            changes.Values.Count.Should().Be(4);
        }
    }
}
