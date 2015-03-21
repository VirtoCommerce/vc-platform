using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public class SearchCaseCriteria
	{
		public CaseFilterType[] FilterTypes { get; set; }

		public string Keyword { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public Label[] CaseLabels { get; set; }

		public Label[] CustomerLabels { get; set; }

		public int StartIndex { get; set; }

		public int Count { get; set; }

	}
}
