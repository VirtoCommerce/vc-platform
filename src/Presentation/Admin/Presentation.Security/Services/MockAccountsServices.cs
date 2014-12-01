using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.ManagementClient.Security.Services
{
    public class MockAccountsServices: ISecurityRepository
    {
        #region Private property

        private List<Permission> MockPermissionList = new List<Permission>();
        private List<Role> MockRoleList = new List<Role>();
        private List<Account> MockAccountList = new List<Account>();
        private List<RoleAssignment> MockRoleAssignmentList = new List<RoleAssignment>();

        #endregion

        public MockAccountsServices()
        {
            PopulateTestData();
        }

        private void PopulateTestData()
        {
            MockAccountList.Add(new Account { AccountId = "1", MemberId = "1", StoreId = "1", AccountState = AccountState.Approved.GetHashCode(), RegisterType = RegisterType.Administrator.GetHashCode() });
            MockAccountList.Add(new Account { AccountId = "2", MemberId = "2", StoreId = "1", AccountState = AccountState.PendingApproval.GetHashCode(), RegisterType = RegisterType.GuestUser.GetHashCode() });
            MockAccountList.Add(new Account { AccountId = "3", MemberId = "3", StoreId = "2", AccountState = AccountState.Approved.GetHashCode(), RegisterType = RegisterType.RegisteredUser.GetHashCode() });
            MockAccountList.Add(new Account { AccountId = "4", MemberId = "4", StoreId = "2", AccountState = AccountState.Rejected.GetHashCode(), RegisterType = RegisterType.SiteAdministrator.GetHashCode() });
        }
        #region IDisposableMembers

        public void Dispose()
        {

        }

        #endregion

        #region IRepository
        MockUnitOfWork _MockUnitOfWorkItem = new MockUnitOfWork();
        public IUnitOfWork UnitOfWork
        {
            get { return _MockUnitOfWorkItem; }
        }

        public bool IsAttachTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }


        public void Attach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public bool IsAttachedTo<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAsQueryable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public void Refresh(IEnumerable collection)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region ISecurityRepository
        public IQueryable<Permission> Permissions
        {
            get { return MockPermissionList.AsQueryable(); }
        }
        public IQueryable<Role> Roles
        {
            get { return MockRoleList.AsQueryable(); }
        }
        public IQueryable<VirtoCommerce.Foundation.Security.Model.RolePermission> RolePermissions
        {
            get { return null; }
        }
        public IQueryable<Account> Accounts
        {
            get { return MockAccountList.AsQueryable(); }
        }
        public IQueryable<RoleAssignment> RoleAssignments
        {
            get { return MockRoleAssignmentList.AsQueryable(); }
        }
        #endregion



     
    }
    public class MockUnitOfWork : IUnitOfWork
    {
        public int Commit()
        {
            return 0;
        }

        public void CommitAndRefreshChanges()
        {
        }

        public void RollbackChanges()
        {
        }
    }
}
