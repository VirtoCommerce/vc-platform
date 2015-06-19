using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.Domain.Commerce.Services
{
	public interface ICommerceService
	{
		IEnumerable<FulfillmentCenter> GetAllFulfillmentCenters();
		FulfillmentCenter UpsertFulfillmentCenter(FulfillmentCenter fullfilmentCenter);
		void DeleteFulfillmentCenter(string[] ids);

		IEnumerable<SeoUrlKeyword> GetSeoKeywordsByKeyword(string keyword);
		IEnumerable<SeoUrlKeyword> GetSeoKeywordsForEntity(string id);
		IEnumerable<SeoUrlKeyword> GetSeoKeywordsForEntities(string[] ids);
		SeoUrlKeyword UpsertSeoKeyword(SeoUrlKeyword seoKeyword);
		void DeleteSeoKeywords(string[] ids);



	}
}
