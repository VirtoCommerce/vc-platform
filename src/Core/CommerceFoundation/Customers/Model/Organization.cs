using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;

namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Organizations")]
    public class Organization : Member
	{
		public Organization() : base()
		{
		}


        private string _Name;
        [Required(ErrorMessage = "Field 'Name' is required.")]
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => this.Name, value);
            }
        }

        private int _OrgType;
        [DataMember]
        public int OrgType
        {
            get
            {
                return _OrgType;
            }
            set
            {
                SetValue(ref _OrgType, () => this.OrgType, value);
            }
        }

        private string _Description;
        [DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetValue(ref _Description, () => this.Description, value);
            }
        }

        private string _BusinessCategory;
        [DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string BusinessCategory
        {
            get
            {
                return _BusinessCategory;
            }
            set
            {
                SetValue(ref _BusinessCategory, () => this.BusinessCategory, value);
            }
        }

        private string _OwnerId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string OwnerId
        {
            get
            {
                return _OwnerId;
            }
            set
            {
                SetValue(ref _OwnerId, () => this.OwnerId, value);
            }
        }
	}
}
