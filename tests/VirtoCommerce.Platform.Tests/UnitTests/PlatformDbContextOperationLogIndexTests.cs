using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class PlatformDbContextOperationLogIndexTests
    {
        [Fact]
        public void OperationLog_HasIndexLeadingWithObjectId_ThenCreatedDate()
        {
            // Arrange: building the model does not open a database connection.
            var options = new DbContextOptionsBuilder<PlatformDbContext>()
                .UseSqlServer("Data Source=(local);Initial Catalog=ModelOnly")
                .Options;
            using var context = new PlatformDbContext(options);

            // Act
            var entityType = context.Model.FindEntityType(typeof(OperationLogEntity));

            // Assert: a lookup by ObjectId alone (ordered by CreatedDate) must be able to seek.
            entityType.Should().NotBeNull();
            var indexColumns = entityType.GetIndexes()
                .Select(index => index.Properties.Select(property => property.Name).ToArray())
                .ToList();

            indexColumns.Should().ContainEquivalentOf(
                new[] { nameof(OperationLogEntity.ObjectId), nameof(OperationLogEntity.CreatedDate) },
                options => options.WithStrictOrdering(),
                "PlatformOperationLog lookups by ObjectId must not require a full table scan");

            // The existing composite index used by ObjectType + ObjectId lookups stays in place.
            indexColumns.Should().ContainEquivalentOf(
                new[] { nameof(OperationLogEntity.ObjectType), nameof(OperationLogEntity.ObjectId) },
                options => options.WithStrictOrdering());
        }
    }
}
