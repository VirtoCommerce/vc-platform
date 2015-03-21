using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
	public class UserSearchCriteria
	{
		public UserSearchCriteria()
		{
			Count = 20;
		}

		public string Keyword { get; set; }
	
		public int Start { get; set; }

		public int Count { get; set; }
	}
}