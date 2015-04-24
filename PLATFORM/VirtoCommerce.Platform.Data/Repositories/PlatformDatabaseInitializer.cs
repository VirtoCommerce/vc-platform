using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Repositories
{
    public class PlatformDatabaseInitializer : SetupDatabaseInitializer<PlatformRepository, Migrations.Configuration>
    {
        protected override void Seed(PlatformRepository context)
        {
            base.Seed(context);

            CreatePermissions(context);
            CreateRoles(context);
            CreateAccounts(context);
        }

        private static void CreateAccounts(PlatformRepository repository)
        {
            repository.Add(new AccountEntity
            {
                Id = "1eb2fa8ac6574541afdb525833dadb46",
                UserName = "admin",
                RegisterType = RegisterType.Administrator,
                AccountState = AccountState.Approved,
            });

            var frontendAccount = new AccountEntity
            {
                Id = "9b605a3096ba4cc8bc0b8d80c397c59f",
                UserName = "frontend",
                RegisterType = RegisterType.SiteAdministrator,
                AccountState = AccountState.Approved,
            };
            frontendAccount.RoleAssignments.Add(new RoleAssignmentEntity
            {
                AccountId = frontendAccount.Id,
                RoleId = repository.Roles.Where(r => r.Name == PredefinedPermissions.RoleApiClient).Select(r => r.Id).FirstOrDefault(),
            });
            frontendAccount.ApiAccounts.Add(new ApiAccountEntity
            {
                AccountId = frontendAccount.Id,
                AppId = "27e0d789f12641049bd0e939185b4fd2",
                SecretKey = "34f0a3c12c9dbb59b63b5fece955b7b2b9a3b20f84370cba1524dd5c53503a2e2cb733536ecf7ea1e77319a47084a3a2c9d94d36069a432ecc73b72aeba6ea78",
                IsActive = true,
            });
            repository.Add(frontendAccount);

            repository.UnitOfWork.Commit();
        }

        private static void CreatePermissions(IRepository repository)
        {
            repository.Add(new PermissionEntity { Id = PredefinedPermissions.SecurityCallApi, Name = PredefinedPermissions.NameSecurityCallApi });
            repository.UnitOfWork.Commit();
        }

        private static void CreateRoles(IPlatformRepository repository)
        {
            var allPermissions = repository.Permissions.ToArray();

            CreateRole(repository, allPermissions, PredefinedPermissions.RoleApiClient, new[] { PredefinedPermissions.SecurityCallApi });

            repository.UnitOfWork.Commit();
        }

        private static void CreateRole(IRepository repository, IEnumerable<PermissionEntity> allPermissions, string name, ICollection<string> permissionList)
        {
            var role = new RoleEntity { Name = name };

            var rolePermissions = allPermissions.Where(p => permissionList.Contains(p.Id)).ToList();
            rolePermissions.ForEach(p => role.RolePermissions.Add(new RolePermissionEntity { PermissionId = p.Id, Role = role }));
            repository.Add(role);
        }
    }
}
