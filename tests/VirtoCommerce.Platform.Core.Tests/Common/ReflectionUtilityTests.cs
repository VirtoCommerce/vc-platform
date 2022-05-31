using VirtoCommerce.Platform.Core.Commands;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.Common
{
    [Trait("Category", "Unit")]
    public class ReflectionUtilityTests
    {
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
    }
}
