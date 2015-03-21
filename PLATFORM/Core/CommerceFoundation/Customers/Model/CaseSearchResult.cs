using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Customers.Model
{
	public class CaseSearchResult
	{

		public Case[] CaseInfos { get; set; }

		public long TotalResults { get; set; }

	}
}
