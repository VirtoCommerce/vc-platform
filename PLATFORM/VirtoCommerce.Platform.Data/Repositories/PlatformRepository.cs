using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class PlatformRepository : EFRepositoryBase, IPlatformRepository
    {
        public PlatformRepository()
        {
            Database.SetInitializer<PlatformRepository>(null);
            Configuration.LazyLoadingEnabled = false;
        }

        public PlatformRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, null, interceptors)
        {
            Database.SetInitializer<PlatformRepository>(null);
            Configuration.LazyLoadingEnabled = false;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            #region Change logging
            modelBuilder.Entity<OperationLogEntity>().HasKey(x => x.Id)
                        .Property(x => x.Id);
            modelBuilder.Entity<OperationLogEntity>().ToTable("PlatformOperationLog");
            #endregion

            #region Settings

            modelBuilder.Entity<SettingEntity>("PlatformSetting", "Id");
            modelBuilder.Entity<SettingValueEntity>("PlatformSettingValue", "Id");

            modelBuilder.Entity<SettingValueEntity>()
                .HasRequired(x => x.Setting)
                .WithMany(x => x.SettingValues)
                .HasForeignKey(x => x.SettingId);

            #endregion

            #region Dynamic Properties

            modelBuilder.Entity<DynamicPropertyEntity>("PlatformDynamicProperty", "Id");
            modelBuilder.Entity<DynamicPropertyNameEntity>("PlatformDynamicPropertyName", "Id");
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>("PlatformDynamicPropertyDictionaryItem", "Id");
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>("PlatformDynamicPropertyDictionaryItemName", "Id");
            modelBuilder.Entity<DynamicPropertyObjectValueEntity>("PlatformDynamicPropertyObjectValue", "Id");

            modelBuilder.Entity<DynamicPropertyNameEntity>()
                .HasRequired(x => x.Property)
                .WithMany(x => x.DisplayNames)
                .HasForeignKey(x => x.PropertyId);

            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>()
                .HasRequired(x => x.Property)
                .WithMany(x => x.DictionaryItems)
                .HasForeignKey(x => x.PropertyId);

            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                .HasRequired(x => x.DictionaryItem)
                .WithMany(x => x.DisplayNames)
                .HasForeignKey(x => x.DictionaryItemId);

            modelBuilder.Entity<DynamicPropertyObjectValueEntity>()
                .HasRequired(x => x.Property)
                .WithMany(x => x.ObjectValues)
                .HasForeignKey(x => x.PropertyId);
            modelBuilder.Entity<DynamicPropertyObjectValueEntity>()
                .HasOptional(x => x.DictionaryItem)
                .WithMany(x => x.ObjectValues)
                .HasForeignKey(x => x.DictionaryItemId);

            modelBuilder.Entity<DynamicPropertyEntity>()
                .Property(x => x.ObjectType)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicProperty_ObjectType_Name", 1) { IsUnique = true }));
            modelBuilder.Entity<DynamicPropertyEntity>()
                .Property(x => x.Name)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicProperty_ObjectType_Name", 2) { IsUnique = true }));

            modelBuilder.Entity<DynamicPropertyNameEntity>()
                .Property(x => x.PropertyId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name", 1) { IsUnique = true }));
            modelBuilder.Entity<DynamicPropertyNameEntity>()
                .Property(x => x.Locale)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name", 2) { IsUnique = true }));
            modelBuilder.Entity<DynamicPropertyNameEntity>()
                .Property(x => x.Name)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyName_PropertyId_Locale_Name", 3) { IsUnique = true }));

            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>()
                .Property(x => x.PropertyId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name", 1) { IsUnique = true }));
            modelBuilder.Entity<DynamicPropertyDictionaryItemEntity>()
                .Property(x => x.Name)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyDictionaryItem_PropertyId_Name", 2) { IsUnique = true }));

            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                .Property(x => x.DictionaryItemId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name", 1) { IsUnique = true }));
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                .Property(x => x.Locale)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name", 2) { IsUnique = true }));
            modelBuilder.Entity<DynamicPropertyDictionaryItemNameEntity>()
                .Property(x => x.Name)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformDynamicPropertyDictionaryItemName_DictionaryItemId_Locale_Name", 3) { IsUnique = true }));

            #endregion

            #region Security

            // Tables
            modelBuilder.Entity<AccountEntity>("PlatformAccount", "Id");
            modelBuilder.Entity<ApiAccountEntity>("PlatformApiAccount", "Id");
            modelBuilder.Entity<RoleEntity>("PlatformRole", "Id");
            modelBuilder.Entity<PermissionEntity>("PlatformPermission", "Id");
            modelBuilder.Entity<RoleAssignmentEntity>("PlatformRoleAssignment", "Id");
            modelBuilder.Entity<RolePermissionEntity>("PlatformRolePermission", "Id");
            modelBuilder.Entity<PermissionScopeEntity>("PlatformPermissionScope", "Id");

            // Relations
            modelBuilder.Entity<ApiAccountEntity>()
                .HasRequired(x => x.Account)
                .WithMany(x => x.ApiAccounts)
                .HasForeignKey(x => x.AccountId);

            modelBuilder.Entity<RoleAssignmentEntity>()
                .HasRequired(x => x.Account)
                .WithMany(x => x.RoleAssignments)
                .HasForeignKey(x => x.AccountId).WillCascadeOnDelete(true);

            modelBuilder.Entity<RoleAssignmentEntity>()
                .HasRequired(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<RolePermissionEntity>()
                .HasRequired(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId).WillCascadeOnDelete(true);

            modelBuilder.Entity<RolePermissionEntity>()
                .HasRequired(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId).WillCascadeOnDelete(true);


            modelBuilder.Entity<PermissionScopeEntity>()
                .HasRequired(x => x.RolePermission)
                .WithMany(x => x.Scopes)
                .HasForeignKey(x => x.RolePermissionId).WillCascadeOnDelete(true);
            #endregion

            #region Notifications

            modelBuilder.Entity<NotificationEntity>().ToTable("PlatformNotification").HasKey(x => x.Id);
            modelBuilder.Entity<NotificationTemplateEntity>().ToTable("PlatformNotificationTemplate").HasKey(x => x.Id);
            modelBuilder.Entity<NotificationTemplateEntity>()
                .Property(x => x.NotificationTypeId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformNotificationTemplate_NotificationTypeId_ObjectTypeId_ObjectId_Language", 1) { IsUnique = true }));
            modelBuilder.Entity<NotificationTemplateEntity>()
                .Property(x => x.ObjectId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformNotificationTemplate_NotificationTypeId_ObjectTypeId_ObjectId_Language", 2) { IsUnique = true }));
            modelBuilder.Entity<NotificationTemplateEntity>()
                .Property(x => x.ObjectTypeId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformNotificationTemplate_NotificationTypeId_ObjectTypeId_ObjectId_Language", 3) { IsUnique = true }));
            modelBuilder.Entity<NotificationTemplateEntity>()
                .Property(x => x.Language)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PlatformNotificationTemplate_NotificationTypeId_ObjectTypeId_ObjectId_Language", 4) { IsUnique = true }));

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        #region IPlatformRepository Members

        public IQueryable<NotificationEntity> Notifications { get { return GetAsQueryable<NotificationEntity>(); } }
        public IQueryable<NotificationTemplateEntity> NotificationTemplates { get { return GetAsQueryable<NotificationTemplateEntity>(); } }

        public IQueryable<SettingEntity> Settings { get { return GetAsQueryable<SettingEntity>(); } }

        public IQueryable<DynamicPropertyEntity> DynamicProperties { get { return GetAsQueryable<DynamicPropertyEntity>(); } }
        public IQueryable<DynamicPropertyObjectValueEntity> DynamicPropertyObjectValues { get { return GetAsQueryable<DynamicPropertyObjectValueEntity>(); } }
        public IQueryable<DynamicPropertyDictionaryItemEntity> DynamicPropertyDictionaryItems { get { return GetAsQueryable<DynamicPropertyDictionaryItemEntity>(); } }

        public IQueryable<AccountEntity> Accounts { get { return GetAsQueryable<AccountEntity>(); } }
        public IQueryable<ApiAccountEntity> ApiAccounts { get { return GetAsQueryable<ApiAccountEntity>(); } }
        public IQueryable<RoleEntity> Roles { get { return GetAsQueryable<RoleEntity>(); } }
        public IQueryable<PermissionEntity> Permissions { get { return GetAsQueryable<PermissionEntity>(); } }
        public IQueryable<RoleAssignmentEntity> RoleAssignments { get { return GetAsQueryable<RoleAssignmentEntity>(); } }
        public IQueryable<RolePermissionEntity> RolePermissions { get { return GetAsQueryable<RolePermissionEntity>(); } }
        public IQueryable<OperationLogEntity> OperationLogs { get { return GetAsQueryable<OperationLogEntity>(); } }

        public RoleEntity GetRoleById(string roleId)
        {
            return Roles.Include(x => x.RolePermissions.Select(y => y.Permission))
                        .Include(x => x.RolePermissions.Select(y => y.Scopes))
                        .FirstOrDefault(x => x.Id == roleId);
        }

        public AccountEntity GetAccountByName(string userName, UserDetails detailsLevel)
        {
            var query = Accounts;

            if (detailsLevel == UserDetails.Full || detailsLevel == UserDetails.Export)
            {
                query = query
                    .Include(a => a.RoleAssignments.Select(ra => ra.Role.RolePermissions.Select(rp => rp.Permission)))
                    .Include(a => a.RoleAssignments.Select(ra => ra.Role.RolePermissions.Select(rp => rp.Scopes)))
                    .Include(a => a.ApiAccounts);
            }

            return query.FirstOrDefault(a => a.UserName == userName);
        }

        public DynamicPropertyDictionaryItemEntity[] GetDynamicPropertyDictionaryItems(string propertyId)
        {
            var retVal = DynamicPropertyDictionaryItems.Include(i => i.DisplayNames)
                                                       .Where(i => i.PropertyId == propertyId)
                                                       .ToArray();

            return retVal;
        }


        public DynamicPropertyEntity[] GetObjectDynamicProperties(string[] objectTypeNames, string[] objectIds)
        {
            var properties = DynamicProperties.Include(x => x.DisplayNames)
                                              .OrderBy(x => x.Name)
                                              .Where(x => objectTypeNames.Contains(x.ObjectType)).ToArray();

            var propertyIds = properties.Select(x => x.Id).ToArray();
            var proprValues = DynamicPropertyObjectValues.Include(x => x.DictionaryItem.DisplayNames)
                                                         .Where(x => propertyIds.Contains(x.PropertyId) && objectIds.Contains(x.ObjectId))
                                                         .ToArray();

            return properties;
        }

        public DynamicPropertyEntity[] GetDynamicPropertiesByIds(string[] ids)
        {
            var retVal = DynamicProperties.Include(x => x.DisplayNames)
                                          .Where(x => ids.Contains(x.Id))
                                          .OrderBy(x => x.Name)
                                          .ToArray();
            return retVal;
        }

        public DynamicPropertyEntity[] GetDynamicPropertiesForType(string objectType)
        {
            var retVal = DynamicProperties.Include(p => p.DisplayNames)
                                          .Where(p => p.ObjectType == objectType)
                                          .OrderBy(p => p.Name)
                                          .ToArray();
            return retVal;
        }

        #endregion

        public NotificationTemplateEntity GetNotificationTemplateByNotification(string notificationTypeId, string objectId, string objectTypeId, string language)
        {
            var query = NotificationTemplates.Where(nt => nt.NotificationTypeId.Equals(notificationTypeId) && nt.ObjectId.Equals(objectId) && nt.ObjectTypeId.Equals(objectTypeId));

            var retVal = query.FirstOrDefault(nt => nt.Language.Equals(language));
            if (retVal == null)
            {
                retVal = query.FirstOrDefault(nt => nt.IsDefault);
            }

            return retVal;
        }
    }
}
