using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class ValueObjectTests
    {
        public class ComplexObject : ValueObject
        {
            public string SimpleProperty { get; set; }
            public string AnotherSimpleProperty { get; set; }
            public List<string> ListProperty { get; set; }
            public List<string> AnotherListProperty { get; set; }
        }

        [Fact]
        public void ObjectsWithTheSameValuesShouldBeEqual()
        {
            var object1 = new ComplexObject
            {
                SimpleProperty = "A",
                ListProperty = new List<string> { "A", "B", "C" },
            };

            var object2 = new ComplexObject
            {
                SimpleProperty = "A",
                ListProperty = new List<string> { "A", "B", "C" },
            };

            var code1 = object1.GetHashCode();
            var code2 = object2.GetHashCode();

            Assert.Equal(code1, code2);
            Assert.Equal(object1, object2);
        }

        [Fact]
        public void ObjectsWithDifferentValuesShouldNotBeEqual()
        {
            var objectsWithDifferentValues = GetObjectsWithDifferentValues();

            // Compare each object with all other objects
            for (var i = 0; i < objectsWithDifferentValues.Count; i++)
            {
                for (var j = 0; j < objectsWithDifferentValues.Count; j++)
                {
                    if (i != j)
                    {
                        var object1 = objectsWithDifferentValues[i];
                        var object2 = objectsWithDifferentValues[j];

                        var equals = object1.Equals(object2);

                        Assert.False(equals, $"Objects #{i} and #{j} must not be equal.");
                    }
                }
            }
        }

        [Fact]
        public void ObjectsWithDifferentValuesShouldHaveDifferentCacheKeys()
        {
            var objectsWithDifferentValues = GetObjectsWithDifferentValues();

            var cacheKeys = objectsWithDifferentValues.Select(x => x.GetCacheKey()).ToList();
            var uniqueCacheKeys = cacheKeys.Distinct().ToList();

            Assert.Equal(objectsWithDifferentValues.Count, uniqueCacheKeys.Count);
        }


        private static IList<ComplexObject> GetObjectsWithDifferentValues()
        {
            return new[]
            {
                // All properties are null
                new ComplexObject(),

                new ComplexObject { SimpleProperty = "" },
                new ComplexObject { SimpleProperty = "null" },

                new ComplexObject { AnotherSimpleProperty = "" },
                new ComplexObject { AnotherSimpleProperty = "null" },

                new ComplexObject { ListProperty = new List<string>() },
                new ComplexObject { ListProperty = new List<string> { null } },
                new ComplexObject { ListProperty = new List<string> { "" } },
                new ComplexObject { ListProperty = new List<string> { "null" } },

                new ComplexObject { AnotherListProperty = new List<string>() },
                new ComplexObject { AnotherListProperty = new List<string> { null } },
                new ComplexObject { AnotherListProperty = new List<string> { "" } },
                new ComplexObject { AnotherListProperty = new List<string> { "null" } },
            };
        }

        // Mixed value-typed and reference-typed properties: exercises the boxing path of the
        // equality-component accessor (an int property must box correctly through the compiled
        // getter, exactly as it did through PropertyInfo.GetValue).
        public class MixedObject : ValueObject
        {
            public int Number { get; set; }
            public int? OptionalNumber { get; set; }
            public string Text { get; set; }
            public List<int> Numbers { get; set; }
        }

        [Fact]
        public void ValueTypedProperties_AreComparedByValue()
        {
            var object1 = new MixedObject { Number = 42, OptionalNumber = 7, Text = "x", Numbers = new List<int> { 1, 2 } };
            var object2 = new MixedObject { Number = 42, OptionalNumber = 7, Text = "x", Numbers = new List<int> { 1, 2 } };
            var object3 = new MixedObject { Number = 43, OptionalNumber = 7, Text = "x", Numbers = new List<int> { 1, 2 } };

            Assert.Equal(object1, object2);
            Assert.Equal(object1.GetHashCode(), object2.GetHashCode());
            Assert.NotEqual(object1, object3);
        }

        [Fact]
        public void NullValueTypedProperty_DoesNotThrowAndParticipatesInEquality()
        {
            var withNull = new MixedObject { Number = 1, OptionalNumber = null };
            var withValue = new MixedObject { Number = 1, OptionalNumber = 0 };

            // Must not throw on the null branch, and null must be distinguishable from a zero value.
            Assert.NotEqual(withNull, withValue);
            Assert.Equal(withNull, new MixedObject { Number = 1, OptionalNumber = null });
        }

        [Fact]
        public void GetHashCode_OnSameInstance_IsStableAcrossRepeatedCalls()
        {
            var instance = new MixedObject { Number = 5, Text = "stable", Numbers = new List<int> { 9 } };

            var first = instance.GetHashCode();
            var second = instance.GetHashCode();

            Assert.Equal(first, second);
        }

        [Fact]
        public void GetHashCode_ReflectsPropertyMutation_NotMemoized()
        {
            // Value objects are mutable; the hash must track the current property values.
            // This guards against introducing hash memoization, which would return a stale value.
            var instance = new MixedObject { Number = 1 };

            var before = instance.GetHashCode();
            instance.Number = 2;
            var after = instance.GetHashCode();

            Assert.NotEqual(before, after);
        }

        // A subtype that customizes its component set by overriding GetProperties() must have that
        // override honored. Guards the fast-path gate: without it, the property-based fast path would
        // ignore the override and compare the excluded property.
        public class FilteredPropertiesObject : ValueObject
        {
            public string Included { get; set; }
            public string Excluded { get; set; }

            protected override IEnumerable<PropertyInfo> GetProperties()
            {
                return base.GetProperties().Where(p => p.Name != nameof(Excluded));
            }
        }

        [Fact]
        public void OverriddenGetProperties_IsHonoredByEqualityAndHash()
        {
            var object1 = new FilteredPropertiesObject { Included = "same", Excluded = "x" };
            var object2 = new FilteredPropertiesObject { Included = "same", Excluded = "y" };

            // 'Excluded' differs but is filtered out of the component set, so the objects stay equal.
            Assert.Equal(object1, object2);
            Assert.Equal(object1.GetHashCode(), object2.GetHashCode());
        }
    }
}
