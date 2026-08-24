using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Commands;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    [Trait("Category", "Unit")]
    public class ReflectionUtilityTests
    {
        private sealed class Node : Entity
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public bool Flag { get; set; }
            public Node Single { get; set; }
            public List<Node> Many { get; set; }
        }

        private class MidNode : Entity;

        private sealed class LeafNode : MidNode;

        [AttributeUsage(AttributeTargets.Property)]
        private sealed class MarkAttribute : Attribute;

        private sealed class Marked
        {
            [Mark]
            public string Tagged { get; set; }

            public string Untagged { get; set; }
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_Self()
        {
            var container = new CommandBase();
            var result = container.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Single(result);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_Entity()
        {
            var container = new { Entity = new CommandBase() };
            var result = container.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Single(result);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_NonEntity()
        {
            var container = new { NonEntity = new { Entity = new CommandBase() } };
            var result = container.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Empty(result);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_Array()
        {
            var container = new { Entities = new[] { new CommandBase() } };
            var result = container.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Single(result);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_IgnoresValueTypedAndStringProperties()
        {
            // Number/Flag/Name must not break traversal (they are skipped, not read+boxed), and the
            // nested entity is still found via the single-object branch.
            var node = new Node { Name = "x", Number = 42, Flag = true, Single = new Node { Id = "child" } };
            var result = node.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_CollectionElementsNotMatchingT_AreIgnored()
        {
            var container = new { Items = new[] { "a", "b" } };
            var result = container.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Empty(result);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_DeduplicatesCycles()
        {
            var a = new Node { Id = "a" };
            var b = new Node { Id = "b" };
            a.Single = b;
            b.Single = a;

            var result = a.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void GetFlatObjectsListWithInterface_RecursesCollectionMembers()
        {
            var root = new Node { Id = "root", Many = [new Node { Id = "1" }, new Node { Id = "2" }] };

            var result = root.GetFlatObjectsListWithInterface<IEntity>();

            Assert.Equal(3, result.Length);
        }

        [Theory]
        [InlineData(typeof(List<string>), true)]
        [InlineData(typeof(string[]), true)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(Dictionary<string, string>), false)]
        public void IsAssignableFromGenericList_MatchesIListImplementations(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsAssignableFromGenericList());
            // Second call exercises the cache and must return the same result.
            Assert.Equal(expected, type.IsAssignableFromGenericList());
        }

        [Theory]
        [InlineData(typeof(Dictionary<string, string>), true)]
        [InlineData(typeof(List<string>), false)]
        [InlineData(typeof(string), false)]
        public void IsDictionary_MatchesDictionaryTypes(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsDictionary());
            Assert.Equal(expected, type.IsDictionary());
        }

        [Fact]
        public void GetTypeInheritanceChain_WalksToEntity_Exclusive()
        {
            var chain = typeof(LeafNode).GetTypeInheritanceChain();

            // Stops before Entity and object.
            Assert.Equal([typeof(LeafNode), typeof(MidNode)], chain);
            Assert.Same(chain, typeof(LeafNode).GetTypeInheritanceChain());
        }

        [Fact]
        public void GetTypeInheritanceChainTo_StopsAtTargetBaseType()
        {
            var toEntity = typeof(LeafNode).GetTypeInheritanceChainTo(typeof(Entity));
            Assert.Equal([typeof(LeafNode), typeof(MidNode)], toEntity);

            var toObject = typeof(LeafNode).GetTypeInheritanceChainTo(typeof(object));
            Assert.Equal([typeof(LeafNode), typeof(MidNode), typeof(Entity)], toObject);
        }

        [Fact]
        public void IsDerivativeOf_MatchesBaseClassChainOnly()
        {
            Assert.True(typeof(LeafNode).IsDerivativeOf(typeof(MidNode)));
            Assert.True(typeof(LeafNode).IsDerivativeOf(typeof(Entity)));
            // Self is not a derivative; an unrelated type is not; a null comparand yields false (not throw).
            Assert.False(typeof(LeafNode).IsDerivativeOf(typeof(LeafNode)));
            Assert.False(typeof(MidNode).IsDerivativeOf(typeof(LeafNode)));
            Assert.False(typeof(LeafNode).IsDerivativeOf(null));
        }

        [Fact]
        public void FindPropertiesWithAttribute_ReturnsOnlyTaggedProperties()
        {
            var properties = typeof(Marked).FindPropertiesWithAttribute(typeof(MarkAttribute));

            Assert.Single(properties);
            Assert.Equal(nameof(Marked.Tagged), properties[0].Name);
            // Cached call returns the same instance.
            Assert.Same(properties, typeof(Marked).FindPropertiesWithAttribute(typeof(MarkAttribute)));
        }
    }
}
