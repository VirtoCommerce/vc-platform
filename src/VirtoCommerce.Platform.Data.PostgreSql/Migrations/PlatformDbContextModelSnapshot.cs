﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VirtoCommerce.Platform.Data.Repositories;

#nullable disable

namespace VirtoCommerce.Platform.Data.PostgreSql.Migrations
{
    [DbContext(typeof(PlatformDbContext))]
    partial class PlatformDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("PropertyId")
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId", "Name")
                        .IsUnique()
                        .HasDatabaseName("IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name");

                    b.ToTable("PlatformDynamicPropertyDictionaryItem", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemNameEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DictionaryItemId")
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Locale")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryItemId", "Locale", "Name")
                        .IsUnique()
                        .HasDatabaseName("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name");

                    b.ToTable("PlatformDynamicPropertyDictionaryItemName", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("integer");

                    b.Property<bool>("IsArray")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDictionary")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMultilingual")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("boolean");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("ObjectType")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("ObjectType", "Name")
                        .IsUnique()
                        .HasDatabaseName("IX_PlatformDynamicProperty_ObjectType_Name");

                    b.ToTable("PlatformDynamicProperty", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyNameEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Locale")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PropertyId")
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId", "Locale", "Name")
                        .IsUnique()
                        .HasDatabaseName("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name");

                    b.ToTable("PlatformDynamicPropertyName", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.OperationLogEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Detail")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectId")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ObjectType")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("OperationType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasDatabaseName("IX_OperationLog_ObjectType_ObjectId");

                    b.ToTable("PlatformOperationLog", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.RawLicenseEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RawLicense", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ObjectId")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ObjectType")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.HasKey("Id");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasDatabaseName("IX_ObjectType_ObjectId");

                    b.ToTable("PlatformSetting", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingValueEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("BooleanValue")
                        .HasColumnType("boolean");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("DateTimeValue")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("DecimalValue")
                        .HasColumnType("numeric(18,5)");

                    b.Property<int>("IntegerValue")
                        .HasColumnType("integer");

                    b.Property<string>("LongTextValue")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SettingId")
                        .HasColumnType("character varying(128)");

                    b.Property<string>("ShortTextValue")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("SettingId");

                    b.ToTable("PlatformSettingValue", (string)null);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", "Property")
                        .WithMany("DictionaryItems")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Property");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemNameEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", "DictionaryItem")
                        .WithMany("DisplayNames")
                        .HasForeignKey("DictionaryItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("DictionaryItem");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyNameEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", "Property")
                        .WithMany("DisplayNames")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Property");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingValueEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.SettingEntity", "Setting")
                        .WithMany("SettingValues")
                        .HasForeignKey("SettingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Setting");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", b =>
                {
                    b.Navigation("DisplayNames");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", b =>
                {
                    b.Navigation("DictionaryItems");

                    b.Navigation("DisplayNames");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingEntity", b =>
                {
                    b.Navigation("SettingValues");
                });
#pragma warning restore 612, 618
        }
    }
}
