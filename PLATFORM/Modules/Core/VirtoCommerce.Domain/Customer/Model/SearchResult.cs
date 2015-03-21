using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Customer.Model
{
	public class SearchResult
	{
		public SearchResult()
		{
			Contacts = new List<Contact>();
			Organizations = new List<Organization>();
		}
		public int TotalCount { get; set; }

		public List<Contact> Contacts { get; set; }
		public List<Organization> Organizations { get; set; }

	}
}
