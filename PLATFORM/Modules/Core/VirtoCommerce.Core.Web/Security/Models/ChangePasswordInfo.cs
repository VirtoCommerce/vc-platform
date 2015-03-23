using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
	public class ChangePasswordInfo
	{
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
	}
}