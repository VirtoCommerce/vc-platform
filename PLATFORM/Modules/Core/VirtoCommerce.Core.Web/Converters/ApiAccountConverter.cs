using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Fulfillment.Model;
using foundationModel = VirtoCommerce.Foundation.Security.Model;
using webModel = VirtoCommerce.CoreModule.Web.Security.Models;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CoreModule.Web.Converters
{
	public static class ApiAccountConverter
	{
		
		public static foundationModel.ApiAccount ToFoundation(this webModel.ApiAccount account)
		{
			var retVal = new foundationModel.ApiAccount();
			retVal.InjectFrom(account);
			if(account.Id != null)
			{
				retVal.AccountId = account.Id;
			}
			return retVal;
		}

		public static webModel.ApiAccount ToWebModel(this foundationModel.ApiAccount center)
		{
			var retVal = new webModel.ApiAccount();
			retVal.InjectFrom(center);
			retVal.Id = center.AccountId;
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.ApiAccount source, foundationModel.ApiAccount target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<foundationModel.ApiAccount>(x => x.IsActive, x => x.SecretKey,
																			   x => x.AppId);
			target.InjectFrom(patchInjection, source);
		}


	}
}