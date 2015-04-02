using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.Domain.Marketing.Services
{
	public interface IMarketingSearchService
	{
		SearchResult SearchPromotions(SearchCriteria criteria);
	}

}
