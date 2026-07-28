using System;
using System.IO;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using Xunit;

namespace VirtoCommerce.Platform.MigrationScripter.Tests
{
    public class MigrationScriptGeneratorTests
    {
        // Unreachable endpoint + tiny timeout so GetAppliedMigrations() fails fast and triggers the fallback path.
        private const string UnreachableConnectionString =
            "Server=127.0.0.1,15;Database=none;Connect Timeout=1;TrustServerCertificate=True";

        [Fact]
        public void Generate_WhenDatabaseUnreachable_WritesIdempotentFallbackAndCombined()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<TestDbContext>(options =>
                options.UseSqlServer(
                    UnreachableConnectionString,
                    sql => sql.MigrationsAssembly(typeof(TestDbContext).Assembly.GetName().Name)));

            var serviceProvider = services.BuildServiceProvider();

            var moduleService = new ModuleBootstrapper(NullLoggerFactory.Instance, new LocalStorageModuleCatalogOptions());
            var generator = new MigrationScriptGenerator(serviceProvider, moduleService, NullLogger.Instance);

            var outputPath = Path.Combine(Path.GetTempPath(), "vc-migr-" + Guid.NewGuid().ToString("N"));

            try
            {
                // Act
                generator.Generate(outputPath);

                // Assert
                var contextScript = Path.Combine(outputPath, "TestDbContext.sql");
                File.Exists(contextScript).Should().BeTrue("a script should be written for the resolvable test context");

                var content = File.ReadAllText(contextScript);
                content.Should().Contain("FALLBACK", "the database is unreachable so the idempotent fallback should be used");
                content.Should().Contain("TestEntities", "the idempotent script should include the migration's CreateTable");

                File.Exists(Path.Combine(outputPath, "_combined.sql")).Should().BeTrue("a combined master script should always be written");
            }
            finally
            {
                if (Directory.Exists(outputPath))
                {
                    Directory.Delete(outputPath, recursive: true);
                }
            }
        }
    }
}
