using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public abstract class Member : Entity, IAuditable
	{
		public Member(string memberType)
		{
			MemberType = memberType;
		}

		#region IAuditable Members

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }
		#endregion

		public abstract string DisplayName { get; }
		public string MemberType { get; set; }
		public ICollection<Address> Addresses { get; set; }
		public ICollection<string> Phones { get; set; }
		public ICollection<string> Emails { get; set; }
		public ICollection<Note> Notes { get; set; }

	}
}