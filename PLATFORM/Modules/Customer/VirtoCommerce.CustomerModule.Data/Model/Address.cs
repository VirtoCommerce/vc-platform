using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public class Address : Entity
	{
		[Required]
		[StringLength(128)]
		public string Name { get; set; }
			
		[StringLength(128)]
		public string FirstName { get; set; }

		[StringLength(128)]
		public string LastName { get; set; }

		[Required]
		[StringLength(128)]
		public string Line1 { get; set; }

		[StringLength(128)]
		public string Line2 { get; set; }

		[Required]
		[StringLength(128)]
		public string City { get; set; }

		[Required]
		[StringLength(64)]
		public string CountryCode  { get; set; }

		[StringLength(128)]
		public string StateProvince { get; set; }


		[Required]
		[StringLength(128)]
		public string CountryName { get; set; }

		[Required]
		[StringLength(32)]
		public string PostalCode { get; set; }

		[StringLength(128)]
		public string RegionId { get; set; }


		[StringLength(128)]
		public string RegionName { get; set; }


		[StringLength(64)]
		public string Type { get; set; }

		[StringLength(64)]
		public string DaytimePhoneNumber { get; set; }

		[StringLength(64)]
		public string EveningPhoneNumber { get; set; }

		[StringLength(64)]
		public string FaxNumber { get; set; }

        [StringLength(256)]
		public string Email { get; set; }

		[StringLength(128)]
		public string Organization { get; set; }
		
		#region Navigation Properties

		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string MemberId { get; set; }

        [Parent]
        [ForeignKey("MemberId")]
		public virtual Member Member { get; set; }


		#endregion

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0} {1}, {2} {3}, {4}, {5} {6} {7}", 
                FirstName, LastName, Line1, Line2, City, StateProvince, PostalCode, CountryName);
            
        }
        #endregion
    }
}
