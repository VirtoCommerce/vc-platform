﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Migrations
{
    [DbContext(typeof(PlatformDbContext))]
    [Migration("20201015144414_FixFilteredBrowsingPropertyId")]
    partial class FixFilteredBrowsingPropertyId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.AssetEntryEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Group")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("MimeType")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(1024)")
                        .HasMaxLength(1024);

                    b.Property<string>("RelativeUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(2083)")
                        .HasMaxLength(2083);

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("TenantId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("TenantType")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("RelativeUrl", "Name")
                        .HasName("IX_AssetEntry_RelativeUrl_Name");

                    b.ToTable("AssetEntry");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId", "Name")
                        .IsUnique()
                        .HasName("IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name")
                        .HasFilter("[PropertyId] IS NOT NULL AND [Name] IS NOT NULL");

                    b.ToTable("PlatformDynamicPropertyDictionaryItem");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemNameEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DictionaryItemId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.HasIndex("DictionaryItemId", "Locale", "Name")
                        .IsUnique()
                        .HasName("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name")
                        .HasFilter("[DictionaryItemId] IS NOT NULL AND [Locale] IS NOT NULL AND [Name] IS NOT NULL");

                    b.ToTable("PlatformDynamicPropertyDictionaryItemName");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<bool>("IsArray")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDictionary")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMultilingual")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ObjectType")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("ObjectType", "Name")
                        .IsUnique()
                        .HasName("IX_PlatformDynamicProperty_ObjectType_Name")
                        .HasFilter("[ObjectType] IS NOT NULL AND [Name] IS NOT NULL");

                    b.ToTable("PlatformDynamicProperty");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyNameEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId", "Locale", "Name")
                        .IsUnique()
                        .HasName("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name")
                        .HasFilter("[PropertyId] IS NOT NULL AND [Locale] IS NOT NULL AND [Name] IS NOT NULL");

                    b.ToTable("PlatformDynamicPropertyName");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.OperationLogEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(2048)")
                        .HasMaxLength(2048);

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ObjectId")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("ObjectType")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("OperationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasName("IX_ObjectType_ObjectId");

                    b.ToTable("PlatformOperationLog");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ObjectId")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ObjectType")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("ObjectType", "ObjectId")
                        .HasName("IX_ObjectType_ObjectId");

                    b.ToTable("PlatformSetting");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingValueEntity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<bool>("BooleanValue")
                        .HasColumnType("bit");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTimeValue")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("DecimalValue")
                        .HasColumnType("decimal(18,5)");

                    b.Property<int>("IntegerValue")
                        .HasColumnType("int");

                    b.Property<string>("LongTextValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SettingId")
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ShortTextValue")
                        .HasColumnType("nvarchar(512)")
                        .HasMaxLength(512);

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("SettingId");

                    b.ToTable("PlatformSettingValue");
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", "Property")
                        .WithMany("DictionaryItems")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemNameEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.DynamicPropertyDictionaryItemEntity", "DictionaryItem")
                        .WithMany("DisplayNames")
                        .HasForeignKey("DictionaryItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.DynamicPropertyNameEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.DynamicPropertyEntity", "Property")
                        .WithMany("DisplayNames")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtoCommerce.Platform.Data.Model.SettingValueEntity", b =>
                {
                    b.HasOne("VirtoCommerce.Platform.Data.Model.SettingEntity", "Setting")
                        .WithMany("SettingValues")
                        .HasForeignKey("SettingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
