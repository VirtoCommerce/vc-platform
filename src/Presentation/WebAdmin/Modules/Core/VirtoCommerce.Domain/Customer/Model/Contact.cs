using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Domain.Customer.Model
{
	public class Contact : Entity, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
		#endregion

		public string FullName { get; set; }
		public string TimeZone { get; set; }
		public string DefaultLanguage { get; set; }
		public DateTime? BirthDate { get; set; }
		public string TaxpayerId { get; set; }
		public string PreferredDelivery { get; set; }
		public string PreferredCommunication { get; set; }
		public string Salutation { get; set; }

		public ICollection<string> Organizations { get; set; }
		public ICollection<Property> Properties { get; set; }
		public ICollection<Address> Addresses { get; set; }
		public ICollection<string> Phones { get; set; }
		public ICollection<string> Emails { get; set; }
		public ICollection<Note> Notes { get; set; }
		

	}
}
