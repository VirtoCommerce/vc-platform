using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
	public class UserSearchResult
	{
		public UserSearchResult()
		{
			Users = new List<ApplicationUserExtended>();
		}
		public int TotalCount { get; set; }

		public List<ApplicationUserExtended> Users { get; set; }
	}
}