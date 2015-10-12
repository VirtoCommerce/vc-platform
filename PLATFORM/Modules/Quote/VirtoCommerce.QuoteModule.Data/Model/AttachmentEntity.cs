using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.QuoteModule.Data.Model
{
	public class AttachmentEntity : AuditableEntity
	{
	
		[StringLength(2083)]
		[Required]
		public string Url { get; set; }

		[StringLength(1024)]
		public string Name { get; set; }

		[StringLength(128)]
		public string MimeType { get; set; }

		public long Size { get; set; }


		#region Navigation Properties

		public virtual QuoteRequestEntity QuoteRequest { get; set; }
		public string QuoteRequestId { get; set; }

		#endregion

	}
}
