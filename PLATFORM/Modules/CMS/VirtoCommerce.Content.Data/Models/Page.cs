using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Content.Data.Models
{
	public class Page
	{
		public string Id { get; set; }

		public string PageName { get; set; }

		public string Language { get; set; }

		public byte[] ByteContent { get; set; }

		public string ContentType { get; set; }

		public DateTime Updated { get; set; }
	}
}
