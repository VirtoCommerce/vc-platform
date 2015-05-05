using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class Note : AuditableEntity
	{
		public string Title { get; set; }
		public string Body { get; set; }
	}
}
