using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class PlatformDbContext : DbContextWithTriggers
    {
        protected const int _idLength128 = 128;
        protected const int _idLength64 = 64;
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
            #region Change logging
            modelBuilder.Entity<OperationLogEntity>().ToTable("PlatformOperationLog").HasKey(x => x.Id);
            modelBuilder.Entity<OperationLogEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<OperationLogEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<OperationLogEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<OperationLogEntity>().Property(x => x.Detail).HasMaxLength(_idLength2048);
            modelBuilder.Entity<OperationLogEntity>().HasIndex(x => new { x.ObjectType, x.ObjectId })
                        .IsUnique(false)
                        .HasDatabaseName("IX_OperationLog_ObjectType_ObjectId");
            #endregion

            #region Settings
            modelBuilder.Entity<SettingEntity>().ToTable("PlatformSetting").HasKey(x => x.Id);
            modelBuilder.Entity<SettingEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<SettingEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<SettingEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<SettingEntity>().HasIndex(x => new { x.ObjectType, x.ObjectId })
                        .IsUnique(false)
                        .HasDatabaseName("IX_ObjectType_ObjectId");

            modelBuilder.Entity<SettingValueEntity>().ToTable("PlatformSettingValue").HasKey(x => x.Id);
            modelBuilder.Entity<SettingValueEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<SettingValueEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<SettingValueEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);

            modelBuilder.Entity<SettingValueEntity>().HasOne(x => x.Setting)
                        .WithMany(x => x.SettingValues)
                        .HasForeignKey(x => x.SettingId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SettingValueEntity>()
                .Property(x => x.DecimalValue)
                .HasColumnType("decimal(18,5)");

            #endregion

            #region Dynamic Properties

            modelBuilder.Entity<DynamicPropertyEntity>().ToTable("PlatformDynamicProperty").HasKey(x => x.Id);
            modelBuilder.Entity<DynamicPropertyEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicPropertyEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyEntity>().HasIndex(x => new { x.ObjectType, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicProperty_ObjectType_Name")
                        .IsUnique(true);

            modelBuilder.Entity<DynamicPropertyNameEntity>().ToTable("PlatformDynamicPropertyName").HasKey(x => x.Id);
            modelBuilder.Entity<DynamicPropertyNameEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicPropertyNameEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyNameEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyNameEntity>().HasOne(x => x.Property)
                        .WithMany(x => x.DisplayNames)
                        .HasForeignKey(x => x.PropertyId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicPropertyNameEntity>()
                        .HasIndex(x => new { x.PropertyId, x.Locale, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name")
                        .IsUnique(true);


            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>().ToTable("PlatformDynamicPropertyDictionaryItem").HasKey(x => x.Id);
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>().HasOne(x => x.Property)
                        .WithMany(x => x.DictionaryItems)
                        .HasForeignKey(x => x.PropertyId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>()
                        .HasIndex(x => new { x.PropertyId, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name")
                        .IsUnique(true);


            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>().ToTable("PlatformDynamicPropertyDictionaryItemName").HasKey(x => x.Id);
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>().Property(x => x.CreatedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>().Property(x => x.ModifiedBy).HasMaxLength(_idLength64);
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>().HasOne(x => x.DictionaryItem)
                        .WithMany(x => x.DisplayNames)
                        .HasForeignKey(x => x.DictionaryItemId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                        .HasIndex(x => new { x.DictionaryItemId, x.Locale, x.Name })
                        .HasDatabaseName("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name")
                        .IsUnique(true);


            #endregion

            #region Raw license
            modelBuilder.Entity<RawLicenseEntity>().ToTable("RawLicense").HasKey(x => x.Id);
            modelBuilder.Entity<RawLicenseEntity>().Property(x => x.Id).HasMaxLength(_idLength128).ValueGeneratedOnAdd();
            modelBuilder.Entity<RawLicenseEntity>().Property(x => x.Data);
            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }


}
