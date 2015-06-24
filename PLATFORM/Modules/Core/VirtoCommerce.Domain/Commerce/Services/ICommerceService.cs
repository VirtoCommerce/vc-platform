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

		IEnumerable<SeoInfo> GetSeoByKeyword(string keyword);
		IEnumerable<SeoInfo> GetObjectsSeo(string[] ids);
		SeoInfo UpsertSeo(SeoInfo seo);
		void DeleteSeo(string[] ids);



	}
}
