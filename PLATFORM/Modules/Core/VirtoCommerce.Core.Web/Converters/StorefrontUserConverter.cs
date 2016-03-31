using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Security;
using webModel = VirtoCommerce.CoreModule.Web.Model;


namespace VirtoCommerce.CoreModule.Web.Converters
{
	public static class StorefrontUserConverter
    {
		public static webModel.StorefrontUser ToWebModel(this ApplicationUserExtended user)
		{
			var retVal = new webModel.StorefrontUser();
			retVal.InjectFrom(user);
            retVal.Password = null;
            retVal.PasswordHash = null;
            retVal.SecurityStamp = null;
            retVal.UserState = user.UserState;
            retVal.Logins = user.Logins;


            return retVal;
		}


	}
}