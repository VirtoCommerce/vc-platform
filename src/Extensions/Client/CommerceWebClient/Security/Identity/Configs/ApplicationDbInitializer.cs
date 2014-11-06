using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Web.Client.Security.Identity.Data;
using VirtoCommerce.Web.Client.Security.Identity.Model;

namespace VirtoCommerce.Web.Client.Security.Identity.Configs
{

    /// <summary>
    /// This is useful if you do not want to tear down the database each time you run the application.
    /// public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    /// This example shows you how to create a new database if the Model changes
    /// </summary>
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        private static ISecurityRepository SecurityRepository
        {
            get { return ServiceLocator.Current.GetInstance<ISecurityRepository>(); }
        }
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }
  
        public static void InitializeIdentityForEf(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var defaultUsers = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "admin",
                    Email = "john_doe@gmail.com",
                    Id = "1"
                },
            };

            foreach (var account in defaultUsers)
            {
                var user = userManager.FindByName(account.UserName);
                if (user == null)
                {
                    var result = userManager.Create(account, "store");
                    if (result.Succeeded)
                    {
                        userManager.SetLockoutEnabled(account.Id, false);
                    }
                }

                //sync account
                if (SecurityRepository != null)
                {
                    var userAccount = SecurityRepository.Accounts.FirstOrDefault(x => x.UserName == account.UserName);

                    if (userAccount == null)
                    {
                        SecurityRepository.Add(new Account
                        {
                            AccountId = account.Id,
                            UserName = account.UserName,
                            AccountState = AccountState.Approved.GetHashCode(),
                            RegisterType = RegisterType.Administrator.GetHashCode(),
                            MemberId = account.Id
                        });

                        SecurityRepository.UnitOfWork.Commit();
                    }
                }
            }
        }
    }
}
