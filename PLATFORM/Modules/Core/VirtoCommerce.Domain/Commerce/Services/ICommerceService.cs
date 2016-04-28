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

        IEnumerable<Currency> GetAllCurrencies();
        void UpsertCurrencies(Currency[] currencies);
        void DeleteCurrencies(string[] codes);
        IEnumerable<SeoInfo> GetAllSeoDuplicates();
        IEnumerable<SeoInfo> GetSeoByKeyword(string keyword);
        void UpsertSeoInfos(SeoInfo[] seoinfos);
        void LoadSeoForObjects(ISeoSupport[] seoSupportObjects);
        void UpsertSeoForObjects(ISeoSupport[] seoSupportObjects);
        void DeleteSeoForObject(ISeoSupport seoSupportObject);

	}
}
