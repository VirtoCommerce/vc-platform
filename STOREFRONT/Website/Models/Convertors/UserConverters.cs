#region
using Omu.ValueInjecter;
using VirtoCommerce.ApiClient.DataContracts.Security;

#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class UserConverters
    {
        #region Public Methods and Operators
        public static ApplicationUser ToServiceModel(this Security.ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }
            var retVal = (ApplicationUser)new ApplicationUser().InjectFrom(user);
            return retVal;
        }

        public static Security.ApplicationUser ToWebModel(this ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }
            var retVal = (Security.ApplicationUser)new Security.ApplicationUser().InjectFrom(user);
            return retVal;
        }
        #endregion
    }
}