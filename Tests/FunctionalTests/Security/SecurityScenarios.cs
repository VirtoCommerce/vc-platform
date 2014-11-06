using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.IO;
using FunctionalTests.TestHelpers;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Security.Migrations;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.PowerShell.DatabaseSetup;
using Xunit;

namespace FunctionalTests.Security
{
    [Variant(RepositoryProvider.EntityFramework)]
	[Variant(RepositoryProvider.DataService)]
	public class SecurityScenarios : FunctionalTestBase, IDisposable
	{
		#region Infrastructure/setup

		private readonly string _databaseName;
		private readonly object _previousDataDirectory;

		public SecurityScenarios()
		{
			_previousDataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory");
			AppDomain.CurrentDomain.SetData("DataDirectory", TempPath);
			_databaseName = "SecurityTest";
		}

		public void Dispose()
		{
			try
			{
				// Ensure LocalDb databases are deleted after use so that LocalDb doesn't throw if
				// the temp location in which they are stored is later cleaned.
				using (var context = new EFSecurityRepository(_databaseName))
				{
					context.Database.Delete();
				}
			}
			finally
			{
				AppDomain.CurrentDomain.SetData("DataDirectory", _previousDataDirectory);
			}
		}

		#endregion

        #region Tests

        [Fact]
        public void Can_create_security_graph()
        {
	        var accountName = "Account create_role_graph";
	        var roleName = "Role create_role_graph";
	        var permissionName = "Permission create_role_graph";
			AddAccount(accountName);
			AddRole(roleName);
			AddPermission(permissionName);

			var client = GetRepository();
			
			var account = client.Accounts.Where(x => x.UserName == accountName).SingleOrDefault();
			Assert.NotNull(account);

	        var permission = client.Permissions.Where(x => x.Name == permissionName).SingleOrDefault();
			Assert.NotNull(permission);

			var role = client.Roles.Where(x => x.Name == roleName).SingleOrDefault();
			Assert.NotNull(role);

			AddRoleAssignment(role.RoleId, account.AccountId);
			AddRolePermission(role.RoleId, permission.PermissionId);

			role = client.Roles.Expand("RolePermissions/Permission").Where(x => x.Name == roleName).SingleOrDefault();
	        Assert.NotNull(role.RolePermissions.Where(x => x.Permission.Name == permissionName).SingleOrDefault());

			account = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
			Assert.NotNull(account.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault());
			
        }

        #endregion

        #region Real Scenarios Tests

		[Fact]
        public void Can_permission_unassign()
		{
			var roleName = "Role permission_unassign";
			var permissionName = "Permission permission_unassign";

			var roleId = AddRole(roleName);
			var permissionId = AddPermission(permissionName);
			AddRolePermission(roleId, permissionId);
			
			var client = GetRepository();

            // load to detail view
            var innerItem = client.Roles.Expand("RolePermissions/Permission").Where(x => x.Name == roleName).SingleOrDefault();
            Assert.NotNull(innerItem);

            // check RolePermission to delete
            var rolePermission = innerItem.RolePermissions.Where(x => x.Permission.Name == permissionName).SingleOrDefault();
			Assert.NotNull(rolePermission);

			innerItem.RolePermissions.Remove(rolePermission);

			// save changes to DB
			client.UnitOfWork.Commit();

			// check changes
			client = GetRepository();

			var checklItem = client.Roles.Expand("RolePermissions/Permission").Where(x => x.Name == roleName).SingleOrDefault();
            Assert.NotNull(checklItem);

            rolePermission = checklItem.RolePermissions.Where(x => x.Permission.Name == permissionName).SingleOrDefault();
			Assert.Null(rolePermission);
		}

        [Fact]
        public void Can_role_unassign()
        {
			var accountName = "Account role_unassign";
			var roleName = "Role role_unassign";
			var accountId = AddAccount(accountName);
			var roleId = AddRole(roleName);
			AddRoleAssignment(roleId, accountId);
			
            var client = GetRepository();

            // load to detail view
            var innerItem = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
            Assert.NotNull(innerItem);

            // get Role Assignment to delete
            var roleAssignment = innerItem.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault();
            Assert.NotNull(roleAssignment);

            // repository has to track it
            innerItem.RoleAssignments.Remove(roleAssignment);

            // save changes to DB
            client.UnitOfWork.Commit();

            // check changes
            client = GetRepository();

			var checklItem = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
            Assert.NotNull(checklItem);

            roleAssignment = checklItem.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault();
            Assert.Null(roleAssignment);
        }

        [Fact]
        public void Can_double_remove_competition_Throws_DbUpdateConcurrencyException()
        {
            var accountName = "Account competition";
            var roleName = "Role competition";
            var accountId = AddAccount(accountName);
            var roleId = AddRole(roleName);
            AddRoleAssignment(roleId, accountId);

            var client1 = GetRepository();
            var client2 = GetRepository();

            // load to detail first view
            var innerItem1 = client1.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
            Assert.NotNull(innerItem1);
            
            // load to detail second view
            var innerItem2 = client2.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
            Assert.NotNull(innerItem2);

            // get Role Assignment to delete
            var roleAssignment = innerItem1.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault();
            Assert.NotNull(roleAssignment);

            // repository has to track it
            innerItem1.RoleAssignments.Remove(roleAssignment);

            // save changes to DB
            client1.UnitOfWork.Commit();

            // get Role Assignment to delete
            roleAssignment = innerItem2.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault();
            Assert.NotNull(roleAssignment);

            // repository has to track it
            innerItem2.RoleAssignments.Remove(roleAssignment);

            // save changes to DB
            var ex = Assert.Throws<DbUpdateConcurrencyException>(() => client2.UnitOfWork.Commit());

            // check changes
            var client = GetRepository();

            var checklItem = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
            Assert.NotNull(checklItem);

            roleAssignment = checklItem.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault();
            Assert.Null(roleAssignment);

        }


        [Fact]
        public void Can_add_remove_add()
        {
            var accountName = "Account add_remove_add";
            var roleName = "Role create_role_graph";
            var accountId = AddAccount(accountName);
            var roleId = AddRole(roleName);


            var client = GetRepository();
            var role = client.Roles.Where(x => x.Name == roleName).SingleOrDefault();
            Assert.NotNull(role);

            var account = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.AccountId == accountId).SingleOrDefault();
            Assert.NotNull(account);

            //var newRole = client.Roles.Where(x => x.Name == roleName).SingleOrDefault(); // need to reload role in new repository

            //var roleAssignment = new RoleAssignment { RoleId = roleId, AccountId = accountId};// worked
            var roleAssignment = new RoleAssignment { RoleId = role.RoleId, AccountId = accountId, Role = role };

            account.RoleAssignments.Add(roleAssignment);
            account.RoleAssignments.Remove(roleAssignment);
            //Deattach here
            account.RoleAssignments.Add(roleAssignment);
            
            client.UnitOfWork.Commit();

            client = GetRepository();
            account = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
            Assert.NotNull(account.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault());
        }

		[Fact]
		public void Can_remove_add_the_same_role()
		{
			var accountName = "Account remove_add_the_same_role";
			var roleName = "Role remove_add_the_same_role";
			
			// create one account with one role
			var accountId = AddAccount(accountName);
			var roleId = AddRole(roleName);
			AddRoleAssignment(roleId, accountId);


			var client = GetRepository();
			
			var aviableRole = client.Roles.Where(x => x.Name == roleName).SingleOrDefault();
			Assert.NotNull(aviableRole);

			var account = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.AccountId == accountId).SingleOrDefault();
			Assert.NotNull(account);

			var checkRole = account.RoleAssignments.SingleOrDefault(x => x.RoleId == roleId).Role;
			Assert.NotNull(checkRole);

			// remove existing role from account
            var item = account.RoleAssignments.First(x => x.RoleId == roleId);
            account.RoleAssignments.Remove(item);

			// add the same role again
			item = new RoleAssignment {AccountId = accountId, Role = aviableRole, RoleId = aviableRole.RoleId};
			account.RoleAssignments.Add(item);


			client.UnitOfWork.Commit();

			client = GetRepository();
			account = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.UserName == accountName).SingleOrDefault();
			Assert.NotNull(account.RoleAssignments.Where(x => x.Role.Name == roleName).SingleOrDefault());
		}

		#endregion

        #region Helper Methods

        private ISecurityRepository GetRepository()
		{
			EnsureDatabaseInitialized(() => new EFSecurityRepository(_databaseName), () => Database.SetInitializer(new SetupMigrateDatabaseToLatestVersion<EFSecurityRepository, Configuration>()));
			var retVal = new EFSecurityRepository(_databaseName);
			return retVal;
		}

	    private string AddAccount(string name)
	    {
			var  client = GetRepository();
			var account = new Account { UserName = name };
			client.Add(account);
			client.UnitOfWork.Commit();
			return account.AccountId;
	    }

		private string AddRole(string name)
		{
			var client = GetRepository();
			var role = new Role { Name = name };
			client.Add(role);
			client.UnitOfWork.Commit();
			return role.RoleId;
		}

		private string AddPermission(string name)
		{
			var client = GetRepository();
			var permission = new Permission { Name = name };
			client.Add(permission);
			client.UnitOfWork.Commit();
			return permission.PermissionId;
		}

		private void AddRolePermission(string roleId, string permissionId)
		{
			var client = GetRepository();
			var role = client.Roles.Expand("RolePermissions/Permission").Where(x => x.RoleId == roleId).SingleOrDefault();
			var rolePermission = new RolePermission { RoleId = role.RoleId, PermissionId = permissionId };
			role.RolePermissions.Add(rolePermission);
			client.UnitOfWork.Commit();
		}

		private void AddRoleAssignment(string roleId, string accountId)
		{
			var client = GetRepository();
			var account = client.Accounts.Expand("RoleAssignments/Role").Where(x => x.AccountId == accountId).SingleOrDefault();
			var rolePermission = new RoleAssignment { RoleId = roleId, AccountId = accountId };
			account.RoleAssignments.Add(rolePermission);
			client.UnitOfWork.Commit();
		}

		#endregion
	}
}
