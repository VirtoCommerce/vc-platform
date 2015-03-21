using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Store.Model;
namespace VirtoCommerce.Domain.Store.Services
{
	public interface IStoreService
	{
		IEnumerable<coreModel.Store> GetStoreList();
		coreModel.Store GetById(string id);
		coreModel.Store Create(coreModel.Store store);
		void Update(coreModel.Store[] stores);
		void Delete(string[] ids);
	}
}
