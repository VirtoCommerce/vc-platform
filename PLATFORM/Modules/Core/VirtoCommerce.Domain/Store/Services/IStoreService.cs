using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Security;
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

        /// <summary>
        /// Returns list of stores ids which passed user can signIn
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<string> GetUserAllowedStoreIds(ApplicationUserExtended user);
    }
}
