using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Quote.Model
{
	public class QuoteRequestSearchResult
	{
		public QuoteRequestSearchResult()
		{
			QuoteRequests = new List<QuoteRequest>();
		}

		public int TotalCount { get; set; }

		public ICollection<QuoteRequest> QuoteRequests { get; set; }

	}
}
