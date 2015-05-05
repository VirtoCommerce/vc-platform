using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Models
{
	public class ContentPage : AuditableEntity
	{
		[Required]
		public string Name { get; set; }

		public byte[] ByteContent { get; set; }

		[Required]
		public string ContentType { get; set; }

		[Required]
		public string Path { get; set; }
		[Required]
		public string Language { get; set; }

	}
}
