using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Model
{
	public class Organization : AuditableEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string BusinessCategory { get; set; }
		public string OwnerId { get; set; }
		public string ParentId { get; set; }

		public ICollection<Address> Addresses { get; set; }
		public ICollection<string> Phones { get; set; }
		public ICollection<string> Emails { get; set; }
		public ICollection<Note> Notes { get; set; }
	}
}
