#region
using Omu.ValueInjecter;
using VirtoCommerce.ApiClient.DataContracts.Security;
using WebModel = VirtoCommerce.Web.Models.Security;

#endregion

namespace VirtoCommerce.Web.Convertors
{
    public static class UserConverters
    {
        #region Public Methods and Operators
        public static ApplicationUser ToServiceModel(this WebModel.ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }
            var retVal = (ApplicationUser)new ApplicationUser().InjectFrom(user);
            return retVal;
        }

        public static WebModel.ApplicationUser ToWebModel(this ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }
            var retVal = (WebModel.ApplicationUser)new WebModel.ApplicationUser().InjectFrom(user);
            return retVal;
        }
        #endregion
    }
}