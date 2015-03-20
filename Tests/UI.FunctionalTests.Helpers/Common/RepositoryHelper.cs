using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace UI.FunctionalTests.Helpers.Common
{
    public class RepositoryHelper
    {

        public static void AddItemToRepository<T>(IRepository repository, IEnumerable<T> items)
        {
            if (items != null && repository != null)
            {
                foreach (var item in items)
                {
                    AddItemToRepository(repository, (object)item);
                }
            }
        }

        public static void AddItemToRepository<T>(IRepository repository, T item)
            where T : class
        {
            if (repository != null && item != null)
            {
                repository.Add(item);
                repository.UnitOfWork.Commit();
            }
        }

    }
}
