using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Customer.Model
{
	public class Organization : Entity, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
		#endregion

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
