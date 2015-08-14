using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class ApplicationUserConverter
    {
        public static ApplicationUser ToDataModel(this ApplicationUserExtended user)
        {
            var dbUser = new ApplicationUser();
            dbUser.CopyFrom(user);
            return dbUser;
        }

        public static void CopyFrom(this ApplicationUser dbUser, ApplicationUserExtended user)
        {
            // Backup old values
            var id = dbUser.Id;
            var passwordHash = dbUser.PasswordHash;
            var securityStamp = dbUser.SecurityStamp;

            dbUser.InjectFrom(user);

            // Restore old values
            if (user.Id == null)
                dbUser.Id = id;

            if (user.PasswordHash == null)
                dbUser.PasswordHash = passwordHash;

            if (user.SecurityStamp == null)
                dbUser.SecurityStamp = securityStamp;

            // Copy logins
            if (user.Logins != null)
            {
                foreach (var login in user.Logins)
                {
                    var userLogin = dbUser.Logins.FirstOrDefault(l => l.LoginProvider == login.LoginProvider);
                    if (userLogin != null)
                    {
                        userLogin.ProviderKey = login.ProviderKey;
                    }
                    else
                    {
                        dbUser.Logins.Add(new IdentityUserLogin
                        {
                            LoginProvider = login.LoginProvider,
                            ProviderKey = login.ProviderKey,
                            UserId = dbUser.Id
                        });
                    }
                }
            }
        }
    }
}
