using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Quote.Model;

namespace VirtoCommerce.Domain.Quote.Services
{
	public interface IQuoteTotalsCalculator
	{
		QuoteRequestTotals CalculateTotals(QuoteRequest quote);
	}
}
