using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IGenericCrudService<T>
	{
		T GetById(string id);
		T Create(T entity);
		void Update(T[] entities);
		void Delete(string[] ids);
	}
}
