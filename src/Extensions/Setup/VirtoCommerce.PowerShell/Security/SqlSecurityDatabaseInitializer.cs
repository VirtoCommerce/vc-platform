using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.PowerShell.DatabaseSetup;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Security.Migrations;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.PowerShell.Security
{
    public class SqlSecurityDatabaseInitializer : SetupDatabaseInitializer<EFSecurityRepository, Configuration>
    {
        protected override void Seed(EFSecurityRepository context)
        {
            base.Seed(context);
            CreateAspNetSecurity(context);

            CreatePermissions(context);
            CreateRoles(context);
        }

        private void CreateAspNetSecurity(EFSecurityRepository context)
        {
            RunCommand(context, "AspNetIdentity.sql", "Security");
        }

        private void CreatePermissions(EFSecurityRepository client)
        {
            PredefinedPermissions.GetAllPermissions().ForEach(client.Add);
            client.UnitOfWork.Commit();
        }

        private void CreateRoles(EFSecurityRepository client)
        {
            var allPermissions = client.Permissions.ToArray();

            CreateRole(PredefinedPermissions.Role_SuperAdmin, allPermissions, new List<string>(allPermissions.Select(x => x.PermissionId)), client);
            CreateRole(PredefinedPermissions.Role_CustomerService, allPermissions, new List<string> { 
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
			}, client);
            CreateRole(PredefinedPermissions.Role_CatalogManagement, allPermissions, new List<string> {		
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
			}, client);
            CreateRole(PredefinedPermissions.Role_Marketing, allPermissions, new List<string> {		
                    PredefinedPermissions.MarketingPromotionsManage,
                    PredefinedPermissions.MarketingDynamic_ContentManage,
                    PredefinedPermissions.MarketingContent_PublishingManage,
					PredefinedPermissions.PricingPrice_List_AssignmentsManage,
					PredefinedPermissions.SettingsStores,
			}, client);
            CreateRole(PredefinedPermissions.Role_Fulfillment, allPermissions, new List<string> {
                    PredefinedPermissions.FulfillmentInventoryManage,
                    PredefinedPermissions.FulfillmentInventoryReceive,
                    PredefinedPermissions.FulfillmentPicklistsManage,
                    PredefinedPermissions.FulfillmentCompleteShipment,
                    PredefinedPermissions.FulfillmentReturnsManage}, client);
            CreateRole(PredefinedPermissions.Role_ConfigurationManagement, allPermissions, new List<string> { 		
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
                    PredefinedPermissions.SettingsTaxImport}, client);
            CreateRole(PredefinedPermissions.Role_PrivateShopper, allPermissions, new List<string> { 
                    PredefinedPermissions.ShopperRestrictedAccess,}, client);

            client.UnitOfWork.Commit();
        }

        private void CreateRole(string name, IEnumerable<Permission> allPermissions, ICollection<string> permissionList, ISecurityRepository client)
        {
            var item = new Role { Name = name };

            var rolePermissions = allPermissions.Where(x => permissionList.Contains(x.PermissionId)).ToList();
            rolePermissions.ForEach(x => item.RolePermissions.Add(new RolePermission { PermissionId = x.PermissionId, RoleId = item.RoleId }));
            client.Add(item);
        }
    }
}
