using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
	public enum UserState
	{
		PendingApproval,
        Approved,
        Rejected
	}
}