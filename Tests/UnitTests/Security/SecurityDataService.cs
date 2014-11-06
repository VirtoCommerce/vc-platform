using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using Xunit;

namespace CommerceFoundation.UnitTests.SecurityTests
{
    public class SecurityDataService : IDisposable
    {
        private static List<Action> EndTestAction = new List<Action>();

        public void Dispose()
        {
            foreach (Action a in EndTestAction)
            {
                try
                {
                    a.Invoke();
                }
                catch { }
            }
            EndTestAction.Clear();
        }

        //[Fact]
        //public void CreatePermissions()
        //{
        //    var client = GetClient();

        //    PredefinedPermissions.GetAllPermissions().ForEach(x =>
        //        {
        //            client.Add(x);
        //        });

        //    client.UnitOfWork.Commit();
        //}

        [Fact]
        // The remote server returned an error: (500) Internal Server Error.
        public void CreateAccount()
        {
            const string userName = "testAcc";
            var client = GetClient();
            var item = new Account { UserName = userName };

            client.Add(item);
            client.UnitOfWork.Commit();
            string accountId = item.AccountId;

            //check
            client = GetClient();
            int count = client.Accounts.Where(x => x.AccountId == accountId).Count();
            Assert.Equal(count, 1);

            // clear base
            EndActionClearAccount(userName);
        }

        [Fact]
        public void CreateRole()
        {
            const string roleName = "testRole";
            var client = GetClient();
            var item = new Role { Name = roleName };

            client.Add(item);
            client.UnitOfWork.Commit();
            string roleId = item.RoleId;
            
            // check
            client = GetClient();
            int count = client.Roles.Where(x => x.RoleId == roleId).Count();
            Assert.Equal(count, 1);
            
            // clear base
            EndActionClearRole(roleName);
        }

        [Fact]
        public void RemoveUncommitedRoleTest()
        {
            ISecurityRepository client = GetClient();

            var account = new Account() { UserName = "testAcc" };
            var role = new Role() { Name = "testRole" };

            var roleAssignment = new RoleAssignment
            {
                Account = account,
                AccountId = account.AccountId,
                Role = role,
                RoleId = role.RoleId
            };

            client.Attach(account);

            // add role
            account.RoleAssignments.Add(roleAssignment);
            Assert.True(client.IsAttachedTo(roleAssignment));
 
            // remove uncommited role
            account.RoleAssignments.Remove(roleAssignment);
            client.Remove(roleAssignment);
            Assert.False(client.IsAttachedTo(roleAssignment));
        }

        [Fact]
        public void CreateAccountWithExistingRoleTest()
        {
            const string roleName = "testRole";
            const string userName = "testAcc";
            ISecurityRepository client = GetClient();

            var role = new Role() { Name = roleName };
            client.Add(role);
            client.UnitOfWork.Commit();
            EndActionClearRole(roleName);
            string roleId = role.RoleId;

            client = GetClient();
            role = client.Roles.Where(x => x.RoleId == roleId).FirstOrDefault();
            client.Attach(role);

            var account = new Account() { UserName = userName };

            var roleAssignment = new RoleAssignment
            {
                Account = account,
                AccountId = account.AccountId,
                Role = role,
                RoleId = role.RoleId
            };


            // add role to account
            account.RoleAssignments.Add(roleAssignment);

            client.Add(account);
            client.UnitOfWork.Commit();
            string accountId = account.AccountId;
            EndActionClearAccount(userName);

            client = GetClient();
            int count = client.Accounts.Where(x => x.AccountId == accountId).Count();
            Assert.Equal(count, 1);
            count = client.Roles.Where(x => x.RoleId == roleId).Count();
            Assert.Equal(count, 1);
            count = client.RoleAssignments.Where(x => x.RoleId == roleId && x.AccountId == accountId).Count();
            Assert.Equal(count, 1);

        }

        [Fact]
        public void CreateAccountWithNewRoleTest()
        {
            const string roleName = "testRole";
            const string userName = "testAcc";
            ISecurityRepository client = GetClient();

            var role = new Role() { Name = roleName };

            var account = new Account() { UserName = userName };

            var roleAssignment = new RoleAssignment
            {
                Account = account,
                AccountId = account.AccountId,
                Role = role,
                RoleId = role.RoleId
            };


            // add role to account
            account.RoleAssignments.Add(roleAssignment);

            client.Add(role);
            client.Add(account);
            client.UnitOfWork.Commit();
            string accountId = account.AccountId;
            string roleId = role.RoleId;
            EndActionClearAccount(userName);
            EndActionClearRole(roleName);

            client = GetClient();
            int count = client.Accounts.Where(x => x.AccountId == accountId).Count();
            Assert.Equal(count, 1);
            count = client.Roles.Where(x => x.RoleId == roleId).Count();
            Assert.Equal(count, 1);
            count = client.RoleAssignments.Where(x => x.RoleId == roleId && x.AccountId == accountId).Count();
            Assert.Equal(count, 1);

        }

        #region Helpers

        private ISecurityRepository GetClient()
        {
            var ServiceUri = new Uri("http://localhost/store/virto/DataServices/SecurityDataService.svc");
            var client = new DSSecurityClient(ServiceUri, new SecurityEntityFactory(), null);

            return client;
        }

        private void EndActionClearRole(string nameRole)
        {
            EndTestAction.Add((() =>
            {
                var c = GetClient();
                var r = c.Roles.Where(x => x.Name == nameRole).FirstOrDefault();
                if (r != null)
                {
                    c.Attach(r);
                    c.Remove(r);
                    c.UnitOfWork.Commit();
                }
            }));
        }

        private void EndActionClearAccount(string userName)
        {
            EndTestAction.Add((() =>
            {
                var c = GetClient();
                var a = c.Accounts.Where(x => x.UserName == userName).FirstOrDefault();
                if (a != null)
                {
                    c.Attach(a);
                    c.Remove(a);
                    c.UnitOfWork.Commit();
                }
            }));
        }

        #endregion
    }
}
