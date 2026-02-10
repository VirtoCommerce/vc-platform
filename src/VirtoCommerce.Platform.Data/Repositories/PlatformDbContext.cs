using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Extensions;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Localizations;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class PlatformDbContext : DbContextBase
    {
        [Obsolete("Use Length64 or UserNameLength", DiagnosticId = "VC0010", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        protected const int _idLength64 = 64;

        [Obsolete("Use Length2048", DiagnosticId = "VC0010", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        protected const int _idLength2048 = 2048;

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {
        }

        protected PlatformDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Change logging
            modelBuilder.Entity<OperationLogEntity>().ToAuditableEntityTable("PlatformOperationLog");
            modelBuilder.Entity<OperationLogEntity>()
                        .HasIndex(x => new { x.ObjectType, x.ObjectId })
                        .IsUnique(false)
                        .HasDatabaseName("IX_OperationLog_ObjectType_ObjectId");
            #endregion

            #region Settings
            modelBuilder.Entity<SettingEntity>().ToAuditableEntityTable("PlatformSetting");
            modelBuilder.Entity<SettingEntity>()
                        .HasIndex(x => new { x.ObjectType, x.ObjectId })
                        .IsUnique(false)
                        .HasDatabaseName("IX_ObjectType_ObjectId");

            modelBuilder.Entity<SettingValueEntity>().ToAuditableEntityTable("PlatformSettingValue");
            modelBuilder.Entity<SettingValueEntity>().Property(x => x.DecimalValue).HasColumnType("decimal(18,5)");
            modelBuilder.Entity<SettingValueEntity>()
                        .HasOne(x => x.Setting)
                        .WithMany(x => x.SettingValues)
                        .HasForeignKey(x => x.SettingId)
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
            #endregion

            #region Localization
            modelBuilder.Entity<LocalizedItemEntity>().ToAuditableEntityTable("PlatformLocalizedItem");
            modelBuilder.Entity<LocalizedItemEntity>()
                        .HasIndex(x => new { x.Name, x.Alias })
                        .IsUnique(false)
                        .HasDatabaseName("IX_PlatformLocalizedItem_Name_Alias");
            #endregion

            #region Dynamic Properties
            modelBuilder.Entity<DynamicPropertyEntity>().ToAuditableEntityTable("PlatformDynamicProperty");
            modelBuilder.Entity<DynamicPropertyEntity>()
                        .HasIndex(x => new { x.ObjectType, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicProperty_ObjectType_Name")
                        .IsUnique();

            modelBuilder.Entity<DynamicPropertyNameEntity>().ToAuditableEntityTable("PlatformDynamicPropertyName");
            modelBuilder.Entity<DynamicPropertyNameEntity>()
                        .HasOne(x => x.Property)
                        .WithMany(x => x.DisplayNames)
                        .HasForeignKey(x => x.PropertyId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicPropertyNameEntity>()
                        .HasIndex(x => new { x.PropertyId, x.Locale, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name")
                        .IsUnique();

            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>().ToAuditableEntityTable("PlatformDynamicPropertyDictionaryItem");
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>()
                        .HasOne(x => x.Property)
                        .WithMany(x => x.DictionaryItems)
                        .HasForeignKey(x => x.PropertyId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>()
                        .HasIndex(x => new { x.PropertyId, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name")
                        .IsUnique();

            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>().ToAuditableEntityTable("PlatformDynamicPropertyDictionaryItemName");
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                        .HasOne(x => x.DictionaryItem)
                        .WithMany(x => x.DisplayNames)
                        .HasForeignKey(x => x.DictionaryItemId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                        .HasIndex(x => new { x.DictionaryItemId, x.Locale, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name")
                        .IsUnique();
            #endregion

            #region Raw license
            modelBuilder.Entity<RawLicenseEntity>().ToEntityTable("RawLicense");
            #endregion

            // Allows configuration for an entity type for different database types.
            // Applies configuration from all <see cref="IEntityTypeConfiguration{TEntity}" in VirtoCommerce.Platform.Data.XXX project. /> 
            switch (Database.ProviderName)
            {
                case "Pomelo.EntityFrameworkCore.MySql":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.Platform.Data.MySql"),
                        IsPlatformDBContextEntity);
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.Platform.Data.PostgreSql"),
                        IsPlatformDBContextEntity);
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("VirtoCommerce.Platform.Data.SqlServer"),
                        IsPlatformDBContextEntity);
                    break;
            }
        }

        private static bool IsPlatformDBContextEntity(Type type)
        {
            return type.Namespace.EndsWith(".EntityConfigurations.Data");
        }
    }
}
