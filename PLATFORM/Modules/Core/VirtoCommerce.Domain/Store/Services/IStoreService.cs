using System.Collections.Generic;
using coreModel = VirtoCommerce.Domain.Store.Model;
namespace VirtoCommerce.Domain.Store.Services
{
	public interface IStoreService
	{
        coreModel.SearchResult SearchStores(coreModel.SearchCriteria criteria);
        coreModel.Store GetById(string id);
        coreModel.Store[] GetByIds(string[] ids);
        coreModel.Store Create(coreModel.Store store);
		void Update(coreModel.Store[] stores);
		void Delete(string[] ids);
	}
}
