using Omu.ValueInjecter;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.Web.Models;
using ApplicationUser = VirtoCommerce.Web.Models.ApplicationUser;

namespace VirtoCommerce.Web.Converters
{
    public static class UserConverters
    {
        public static ApiClient.DataContracts.Security.ApplicationUser ToServiceModel(this ApplicationUser user)
        {
            if (user == null) return null;
            var retVal = (ApiClient.DataContracts.Security.ApplicationUser)new ApiClient.DataContracts.Security.ApplicationUser().InjectFrom(user);
            return retVal;
        }

        public static ApplicationUser ToWebModel(this ApiClient.DataContracts.Security.ApplicationUser user)
        {
            if (user == null) return null;
            var retVal = (ApplicationUser)new ApplicationUser().InjectFrom(user);
            return retVal;
        }

        public static CustomerModel ToWebModel(this AuthInfo user)
        {
            if (user == null) return null;
            var retVal = (CustomerModel)new CustomerModel().InjectFrom(user);
            return retVal;
        }
    }
}