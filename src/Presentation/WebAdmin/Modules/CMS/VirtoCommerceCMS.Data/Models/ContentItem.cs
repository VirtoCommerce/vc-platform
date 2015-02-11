using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerceCMS.Data.Models
{
	public class ContentItem : Entity
	{
		[Required]
		public DateTime CreatedDate { get; set; }
		[Required]
		[StringLength(64)]
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		[StringLength(64)]
		public string ModifiedBy { get; set; }

		[Required]
		public string Content { get; set; }

		[NotMapped]
		public ContentType ContentType 
		{
			get
			{
				return (ContentType)Enum.Parse(typeof(ContentType), this.Type);
			}
			set
			{
				this.Type = value.ToString();
			}
		}

		[Required]
		public string Type { get; set; }
		//public 

		[StringLength(128)]
		public string ParentContentItemId { get; set; }

		public virtual ContentItem ParentContentItem { get; set; }

		[Required]
		public string Path { get; set; }

		[Required]
		public string Name { get; set; }
	}
}
