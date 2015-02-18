using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.Foundation.Data.Security
{
	public class SqlSecurityDatabaseInitializer : SetupDatabaseInitializer<EFSecurityRepository, Migrations.Configuration>
	{
		protected override void Seed(EFSecurityRepository context)
		{
			base.Seed(context);

			CreateAccounts(context);
			CreatePermissions(context);
			CreateRoles(context);
		}

		private static void CreateAccounts(EFSecurityRepository context)
		{
			context.Add(new Account
			{
				AccountId = "1",
				MemberId = "1",
				UserName = "admin",
				RegisterType = (int)RegisterType.Administrator,
				AccountState = (int)AccountState.Approved,
			});

			context.Add(new Account
			{
				AccountId = "9b605a3096ba4cc8bc0b8d80c397c59f",
				MemberId = "060C4620-F84C-45AB-A3AE-8E3133FFDAEF",
				UserName = "frontend",
				RegisterType = (int)RegisterType.SiteAdministrator,
				AccountState = (int)AccountState.Approved,
			});

			context.Add(new ApiAccount
			{
				ApiAccountId = "eaa4c211288b49238e7cdf59c32e0661",
				AccountId = "9b605a3096ba4cc8bc0b8d80c397c59f",
				AppId = "27e0d789f12641049bd0e939185b4fd2",
				SecretKey = "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78",
				IsActive = true,
			});

			context.UnitOfWork.Commit();
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
