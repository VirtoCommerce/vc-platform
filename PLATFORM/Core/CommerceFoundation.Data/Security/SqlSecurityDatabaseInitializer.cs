using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.Foundation.Data.Security
{
    public class SqlSecurityDatabaseInitializer : SetupDatabaseInitializer<EFSecurityRepository, Migrations.Configuration>
    {
        protected override void Seed(EFSecurityRepository context)
        {
            base.Seed(context);

            CreatePermissions(context);
            CreateRoles(context);
            CreateAccounts(context);
        }

        private static void CreateAccounts(EFSecurityRepository repository)
        {
            repository.Add(new Account
            {
                AccountId = "1",
                MemberId = "1",
                UserName = "admin",
                RegisterType = (int)RegisterType.Administrator,
                AccountState = (int)AccountState.Approved,
            });

            var frontendAccount = new Account
            {
                AccountId = "9b605a3096ba4cc8bc0b8d80c397c59f",
                UserName = "frontend",
                RegisterType = (int)RegisterType.SiteAdministrator,
                AccountState = (int)AccountState.Approved,
            };
            frontendAccount.RoleAssignments.Add(new RoleAssignment
            {
                AccountId = frontendAccount.AccountId,
                RoleId = repository.Roles.Where(r => r.Name == PredefinedPermissions.Role_ApiClient).Select(r => r.RoleId).FirstOrDefault(),
            });
            frontendAccount.ApiAccounts.Add(new ApiAccount
            {
                AccountId = frontendAccount.AccountId,
                AppId = "27e0d789f12641049bd0e939185b4fd2",
                SecretKey = "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78",
                IsActive = true,
            });
            repository.Add(frontendAccount);

            repository.UnitOfWork.Commit();
        }

        private static void CreatePermissions(EFSecurityRepository repository)
        {
            PredefinedPermissions.GetAllPermissions().ForEach(repository.Add);
            repository.UnitOfWork.Commit();
        }

        private static void CreateRoles(ISecurityRepository repository)
        {
            var allPermissions = repository.Permissions.ToArray();

            CreateRole(repository, allPermissions, PredefinedPermissions.Role_SuperAdmin, allPermissions.Select(x => x.PermissionId).ToArray());
            CreateRole(repository, allPermissions, PredefinedPermissions.Role_CustomerService, new[] { 
	                PredefinedPermissions.CustomersViewAssignedCases,
	                PredefinedPermissions.CustomersSearchCases,
	                PredefinedPermissions.CustomersCreateNewCase,
	                PredefinedPermissions.CustomersEditCaseProperties,
	                PredefinedPermissions.CustomersAddCaseComments,
	                PredefinedPermissions.CustomersAddCustomerComments,
	                PredefinedPermissions.CustomersCreateCustomer,
	                PredefinedPermissions.CustomersEditCustomer,
	                PredefinedPermissions.CustomersCreateResetPasswords,
	                PredefinedPermissions.CustomersSuspendAccounts,
	                PredefinedPermissions.CustomersCreateContactAccount,
                    PredefinedPermissions.CustomersLoginAsCustomer,
					PredefinedPermissions.OrdersAll,
					PredefinedPermissions.OrdersCreateOrderReturns,
					PredefinedPermissions.OrdersCompleteOrderReturns,
					PredefinedPermissions.OrdersCancelOrderReturns,
					PredefinedPermissions.OrdersIssueOrderReturns,
					PredefinedPermissions.OrdersCreateOrderExchange
			});
            CreateRole(repository, allPermissions, PredefinedPermissions.Role_CatalogManagement, new[] {		
                    PredefinedPermissions.CatalogItemsManage,
                    PredefinedPermissions.CatalogCatalogsManage,
                    PredefinedPermissions.CatalogCategoriesManage,
                    PredefinedPermissions.CatalogVirtual_CatalogsManage,
                    PredefinedPermissions.CatalogLinked_CategoriesManage,
                    PredefinedPermissions.CatalogCatalog_Import_JobsRun,
                    PredefinedPermissions.CatalogCatalog_Import_JobsManage,
                    PredefinedPermissions.CatalogItemAssociationsManage,
                    PredefinedPermissions.CatalogEditorialReviewsCreateEdit,
                    PredefinedPermissions.CatalogEditorialReviewsPublish,
                    PredefinedPermissions.CatalogEditorialReviewsRemove,
                    PredefinedPermissions.CatalogCustomerReviewsManage,
					PredefinedPermissions.PricingPrice_ItemPricingManage,
					PredefinedPermissions.PricingPrice_ListsImport_Jobs,
					PredefinedPermissions.PricingPrice_ListsImport_JobsRun,
					PredefinedPermissions.PricingPrice_ListsManage
			});
            CreateRole(repository, allPermissions, PredefinedPermissions.Role_Marketing, new[] {		
                    PredefinedPermissions.MarketingPromotionsManage,
                    PredefinedPermissions.MarketingDynamic_ContentManage,
                    PredefinedPermissions.MarketingContent_PublishingManage,
					PredefinedPermissions.PricingPrice_List_AssignmentsManage,
					PredefinedPermissions.SettingsStores
            });
            CreateRole(repository, allPermissions, PredefinedPermissions.Role_Fulfillment, new[] {
                    PredefinedPermissions.FulfillmentInventoryManage,
                    PredefinedPermissions.FulfillmentInventoryReceive,
                    PredefinedPermissions.FulfillmentPicklistsManage,
                    PredefinedPermissions.FulfillmentCompleteShipment,
                    PredefinedPermissions.FulfillmentReturnsManage});
            CreateRole(repository, allPermissions, PredefinedPermissions.Role_ConfigurationManagement, new[] { 		
                    PredefinedPermissions.SettingsCustomerRules,
                    PredefinedPermissions.SettingsContent_Places,
                    PredefinedPermissions.SettingsFulfillment,
                    PredefinedPermissions.SettingsStores,
                    PredefinedPermissions.SettingsPayment_Methods,
                    PredefinedPermissions.SettingsShippingOptions,
                    PredefinedPermissions.SettingsTaxes,
                    PredefinedPermissions.SettingsSearch,
                    PredefinedPermissions.SettingsAppConfigSettings,
                    PredefinedPermissions.SettingsAppConfigSystemJobs,
                    PredefinedPermissions.SettingsAppConfigEmailTemplates,
                    PredefinedPermissions.SettingsAppConfigDisplayTemplates,
                    PredefinedPermissions.SettingsCustomerInfo,
                    PredefinedPermissions.SettingsCustomerCaseTypes,
                    PredefinedPermissions.SettingsShippingMethods,
                    PredefinedPermissions.SettingsShippingPackages,
                    PredefinedPermissions.SettingsJurisdiction,
                    PredefinedPermissions.SettingsJurisdictionGroups,
                    PredefinedPermissions.SettingsTaxCategories,
                    PredefinedPermissions.SettingsTaxImport});

            CreateRole(repository, allPermissions, PredefinedPermissions.Role_PrivateShopper, new[] { PredefinedPermissions.ShopperRestrictedAccess });
            CreateRole(repository, allPermissions, PredefinedPermissions.Role_ApiClient, new[] { PredefinedPermissions.SecurityCallApi });

            repository.UnitOfWork.Commit();
        }

        private static void CreateRole(IRepository repository, IEnumerable<Permission> allPermissions, string name, ICollection<string> permissionList)
        {
            var item = new Role { Name = name };

            var rolePermissions = allPermissions.Where(x => permissionList.Contains(x.PermissionId)).ToList();
            rolePermissions.ForEach(x => item.RolePermissions.Add(new RolePermission { PermissionId = x.PermissionId, RoleId = item.RoleId }));
            repository.Add(item);
        }
    }
}
