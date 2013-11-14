using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications.Model
{
	public class CommunicationAttachment
	{
		public string AttachmentId { get; set; }
		public string CreatorName { get; set; }
		public string FileUrl { get; set; }
		public string DisplayName { get; set; }
		public string FileType { get; set; }
		public DateTime? LastModified { get; set; }

		public string Url { get; set; }
		public CommunicationItemState State { get; set; }

	}
}
