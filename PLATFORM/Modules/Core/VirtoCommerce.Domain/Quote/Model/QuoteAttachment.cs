using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Quote.Model
{
	public class QuoteAttachment : AuditableEntity
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string MimeType { get; set; }
		public long Size { get; set; }
	}
}
