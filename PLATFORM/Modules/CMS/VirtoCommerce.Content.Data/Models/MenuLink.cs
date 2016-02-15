using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Models
{
	public class MenuLink : AuditableEntity
	{
		[Required]
        [StringLength(1024)]
        public string Title { get; set; }
		[Required]
        [StringLength(2048)]
        public string Url { get; set; }
		[Required]
		public bool IsActive { get; set; }
		[Required]
		public int Priority { get; set; }

        [StringLength(254)]
        public string AssociatedObjectType { get; set; }

        [StringLength(254)]
        public string AssociatedObjectName { get; set; }

        [StringLength(128)]
        public string AssociatedObjectId { get; set; }

        public virtual MenuLinkList MenuLinkList { get; set; }
		public string MenuLinkListId { get; set; }
	}
}
