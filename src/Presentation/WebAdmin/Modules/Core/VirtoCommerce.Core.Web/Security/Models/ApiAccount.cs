using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.CoreModule.Web.Security.Models
{
	public class ApiAccount : Entity
	{
		public bool IsActive { get; set; }
		public string AppId { get; set; }
		public string SecretKey { get; set; }
	}
}