using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Quote.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Quote.Events
{
	public class QuoteRequestChangeEvent
	{
		public QuoteRequestChangeEvent(EntryState state, QuoteRequest origQuote, QuoteRequest modifiedQuote)
		{
			ChangeState = state;
			OrigQuote = origQuote;
			ModifiedQuote = modifiedQuote;
		}

		public EntryState ChangeState { get; set; }
		public QuoteRequest OrigQuote { get; set; }
		public QuoteRequest ModifiedQuote { get; set; }
	}
}
