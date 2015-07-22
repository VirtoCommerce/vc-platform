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
            var id = dbUser.Id;
            dbUser.InjectFrom(user);
            dbUser.Id = id;

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

            return dbUser;
        }
    }
}
