using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Model
{
	public class Note : Entity, IAuditable
	{

		#region IAuditable Members
		[Required]
		public DateTime CreatedDate { get; set; }
		[Required]
		[StringLength(64)]
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		[StringLength(64)]
		public string ModifiedBy { get; set; }
		#endregion


		[StringLength(128)]
		public string AuthorName { get; set; }

		[StringLength(128)]
		public string ModifierName { get; set; }

		[StringLength(128)]
		public string Title { get; set; }

		public string Body { get; set; }

		public bool IsSticky { get; set; }

		#region Navigation Properties
		[StringLength(128)]
		public string MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }

		#endregion


	}
}
