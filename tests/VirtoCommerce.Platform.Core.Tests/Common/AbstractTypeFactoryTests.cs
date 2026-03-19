using System;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    // Test type hierarchy — separate from other tests' types to avoid cross-test pollution
    // (AbstractTypeFactory is static generic, so each BaseType gets its own state)

    public class Animal
    {
        public string Name { get; set; }
    }

    public class Dog : Animal
    {
        public string Breed { get; set; }
    }

    public class Labrador : Dog
    {
    }

    public abstract class AbstractVehicle
    {
        public string Model { get; set; }
    }

    public class Car : AbstractVehicle
    {
    }

    // Separate abstract type for the "no overrides throws" test — avoids static state pollution from Car registration
    public abstract class AbstractService
    {
    }

    public class Shape
    {
        public double Area { get; set; }
    }

    public class Circle : Shape
    {
    }

    public class Square : Shape
    {
    }

    // Separate base types for tests that need isolated factory state
    public class Widget
    {
        public string Id { get; set; }
    }

    public class SuperWidget : Widget
    {
    }

    public class Gadget
    {
        public string Id { get; set; }
    }

    public class SuperGadget : Gadget
    {
    }

    public class Tool
    {
        public string Id { get; set; }
    }

    public class PowerTool : Tool
    {
    }

    public class Item
    {
        public string Id { get; set; }
    }

    public class SpecialItem : Item
    {
    }

    public class Part
    {
        public string Id { get; set; }
    }

    public class CustomPart : Part
    {
    }

    public class Record
    {
        public string Id { get; set; }
    }

    public class ParameterizedRecord
    {
        public string Id { get; }
        public string Value { get; }

        public ParameterizedRecord(string id, string value)
        {
            Id = id;
            Value = value;
        }
    }

    // Types for WithFactory + args test (Bug 1 regression test)
    public class Order
    {
        public string OperationType { get; set; }
    }

    public class CustomOrder : Order
    {
    }

    // Type for auto-compiled + args test (Bug 3 regression test)
    public class Entity
    {
        public string Id { get; set; }

        public Entity() { }

        public Entity(string id)
        {
            Id = id;
        }
    }

    // Types for WithTypeName test (Bug 2 regression test)
    public class Document
    {
        public string Id { get; set; }
    }

    public class Invoice : Document
    {
    }

    [Trait("Category", "Unit")]
    public class AbstractTypeFactoryTests
    {
        [Fact]
        public void TryCreateInstance_NoOverrides_CreatesBaseType()
        {
            // Animal has no registrations → fast path via CreateDefaultInstance
            var result = AbstractTypeFactory<Animal>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.Equal(typeof(Animal), result.GetType());
        }

        [Fact]
        public void TryCreateInstance_WithOverride_CreatesDerivedType()
        {
            AbstractTypeFactory<Widget>.RegisterType<SuperWidget>();

            var result = AbstractTypeFactory<Widget>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<SuperWidget>(result);
        }

        [Fact]
        public void TryCreateInstance_AbstractBaseType_NoOverrides_Throws()
        {
            Assert.Throws<OperationCanceledException>(() =>
                AbstractTypeFactory<AbstractService>.TryCreateInstance());
        }

        [Fact]
        public void TryCreateInstance_AbstractBaseType_WithOverride_CreatesDerivedType()
        {
            AbstractTypeFactory<AbstractVehicle>.RegisterType<Car>();

            var result = AbstractTypeFactory<AbstractVehicle>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<Car>(result);
        }

        [Fact]
        public void TryCreateInstance_RegisteredType_LookupByBaseTypeName()
        {
            // Most common pattern: register derived, lookup by base type name
            AbstractTypeFactory<Gadget>.RegisterType<SuperGadget>();

            var result = AbstractTypeFactory<Gadget>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<SuperGadget>(result);
        }

        [Fact]
        public void TryCreateInstance_RegisteredType_LookupByDerivedTypeName()
        {
            AbstractTypeFactory<Tool>.RegisterType<PowerTool>();

            var result = AbstractTypeFactory<Tool>.TryCreateInstance(nameof(PowerTool));

            Assert.NotNull(result);
            Assert.IsType<PowerTool>(result);
        }

        [Fact]
        public void OverrideType_ReplacesRegistration()
        {
            AbstractTypeFactory<Shape>.RegisterType<Circle>();

            // Override Circle with Square
            AbstractTypeFactory<Shape>.OverrideType<Circle, Square>();

            var result = AbstractTypeFactory<Shape>.TryCreateInstance();

            Assert.NotNull(result);
            Assert.IsType<Square>(result);
        }

        [Fact]
        public void OverrideType_ClearsInheritanceLookupCache()
        {
            AbstractTypeFactory<Item>.RegisterType<Item>();

            // First call — caches "Item" → ItemTypeInfo in the index
            var first = AbstractTypeFactory<Item>.TryCreateInstance();
            Assert.IsType<Item>(first);

            // Override — should invalidate cached lookup
            AbstractTypeFactory<Item>.OverrideType<Item, SpecialItem>();

            var second = AbstractTypeFactory<Item>.TryCreateInstance();
            Assert.IsType<SpecialItem>(second);
        }

        [Fact]
        public void TryCreateInstance_WithArgs_UsesActivator()
        {
            AbstractTypeFactory<ParameterizedRecord>.RegisterType<ParameterizedRecord>();

            var result = AbstractTypeFactory<ParameterizedRecord>.TryCreateInstance(
                nameof(ParameterizedRecord), "test-id", "test-value");

            Assert.NotNull(result);
            Assert.Equal("test-id", result.Id);
            Assert.Equal("test-value", result.Value);
        }

        [Fact]
        public void WithFactory_TakesPriorityOverAutoCompiledDelegate()
        {
            var customFactoryCalled = false;
            AbstractTypeFactory<Part>.RegisterType<CustomPart>()
                .WithFactory(() =>
                {
                    customFactoryCalled = true;
                    return new CustomPart { Id = "from-factory" };
                });

            var result = AbstractTypeFactory<Part>.TryCreateInstance();

            Assert.True(customFactoryCalled);
            Assert.IsType<CustomPart>(result);
            Assert.Equal("from-factory", result.Id);
        }

        [Fact]
        public void WithSetupAction_CalledAfterCreation()
        {
            AbstractTypeFactory<Record>.RegisterType<Record>()
                .WithSetupAction(x => x.Id = "setup-applied");

            var result = AbstractTypeFactory<Record>.TryCreateInstance();

            Assert.Equal("setup-applied", result.Id);
        }

        [Fact]
        public void TryCreateInstance_WithDefault_ReturnsDefault_WhenTypeNotFound()
        {
            var defaultObj = new Animal { Name = "default" };

            var result = AbstractTypeFactory<Animal>.TryCreateInstance("NonExistentType", defaultObj);

            Assert.Same(defaultObj, result);
        }

        [Fact]
        public void FindTypeInfoByName_SecondCall_UsesCache()
        {
            AbstractTypeFactory<Dog>.RegisterType<Labrador>();

            // First call — fallback to inheritance scan, caches result
            var first = AbstractTypeFactory<Dog>.FindTypeInfoByName(nameof(Dog));
            // Second call — should hit the cached entry
            var second = AbstractTypeFactory<Dog>.FindTypeInfoByName(nameof(Dog));

            Assert.NotNull(first);
            Assert.Same(first, second);
            Assert.Equal(typeof(Labrador), first.Type);
        }

        [Fact]
        public void TryCreateInstance_WithArgs_FactoryTakesPriority()
        {
            // Bug 1 regression: WithFactory must take priority even when args are passed
            AbstractTypeFactory<Order>.RegisterType<CustomOrder>()
                .WithFactory(() => new CustomOrder { OperationType = "CustomOrder" });

            // Call with args — Factory should still be used, args should be ignored
            var result = AbstractTypeFactory<Order>.TryCreateInstance(nameof(CustomOrder), "ignored-arg");

            Assert.IsType<CustomOrder>(result);
            Assert.Equal("CustomOrder", result.OperationType);
        }

        [Fact]
        public void WithTypeName_UpdatesDictionaryIndex()
        {
            // Bug 2 regression: WithTypeName must update the type name index
            AbstractTypeFactory<Document>.RegisterType<Invoice>()
                .WithTypeName("CustomInvoice");

            var result = AbstractTypeFactory<Document>.TryCreateInstance("CustomInvoice");

            Assert.NotNull(result);
            Assert.IsType<Invoice>(result);
        }

        [Fact]
        public void TryCreateInstance_WithArgs_NotPoisonedByPriorParameterlessCall()
        {
            // Bug 3 regression: auto-compiled delegate from parameterless path
            // must NOT be used when args are passed (args would be silently ignored)
            AbstractTypeFactory<Entity>.RegisterType<Entity>();

            // First call: parameterless — triggers auto-compilation of delegate
            var parameterless = AbstractTypeFactory<Entity>.TryCreateInstance();
            Assert.NotNull(parameterless);
            Assert.Null(parameterless.Id);

            // Second call: with args — must use Activator.CreateInstance(type, args), NOT the cached delegate
            var withArgs = AbstractTypeFactory<Entity>.TryCreateInstance(nameof(Entity), "my-id");
            Assert.NotNull(withArgs);
            Assert.Equal("my-id", withArgs.Id);
        }
    }
}
