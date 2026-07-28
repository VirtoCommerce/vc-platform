using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.Platform.MigrationScripter.Tests
{
    public class TestEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>(b =>
            {
                b.ToTable("TestEntities");
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnType("nvarchar(128)");
                b.Property(x => x.Name).HasColumnType("nvarchar(max)");
            });
        }
    }

    /// <summary>
    /// A hand-written migration so the test context has a real migration to script.
    /// Explicit column types keep SQL generation independent of a design-time model snapshot.
    /// </summary>
    [DbContextAttribute(typeof(TestDbContext))]
    [Migration("20240101000000_InitialTest")]
    public class InitialTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestEntities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TestEntities");
        }
    }
}
