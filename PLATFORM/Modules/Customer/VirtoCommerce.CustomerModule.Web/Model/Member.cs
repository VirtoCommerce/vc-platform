using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public abstract class Member : AuditableEntity
	{
	    protected Member(string memberType)
		{
			MemberType = memberType;
		}

		public abstract string DisplayName { get; }

        /// <summary>
        /// String representation of member type (Organization or Contact). Used as Discriminator
        /// </summary>
        public string MemberType { get; set; }

		public ICollection<Address> Addresses { get; set; }
		public ICollection<string> Phones { get; set; }
		public ICollection<string> Emails { get; set; }
        /// <summary>
        /// All security accounts logins associated with this member (test@mail.com, test2@mail.com etc.)
        /// </summary>
        public ICollection<ApplicationUserExtended> SecurityAccounts { get; set; }
        /// <summary>
        /// Additional information about the member
        /// </summary>
		public ICollection<Note> Notes { get; set; }

        /// <summary>
        /// Not documented
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Some additional properties
        /// </summary>
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }

	}
}