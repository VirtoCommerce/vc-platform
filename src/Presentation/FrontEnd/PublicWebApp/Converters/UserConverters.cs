using Omu.ValueInjecter;
using ApplicationUser = VirtoCommerce.Web.Models.ApplicationUser;

namespace VirtoCommerce.Web.Converters
{
    public static class UserConverters
    {
        public static Core.DataContracts.Security.ApplicationUser ToServiceModel(this ApplicationUser user)
        {
            if (user == null) return null;
            var retVal = (Core.DataContracts.Security.ApplicationUser)new Core.DataContracts.Security.ApplicationUser().InjectFrom(user);
            return retVal;
        }

        public static ApplicationUser ToWebModel(this Core.DataContracts.Security.ApplicationUser user)
        {
            if (user == null) return null;
            var retVal = (ApplicationUser)new ApplicationUser().InjectFrom(user);
            return retVal;
        }
    }
}