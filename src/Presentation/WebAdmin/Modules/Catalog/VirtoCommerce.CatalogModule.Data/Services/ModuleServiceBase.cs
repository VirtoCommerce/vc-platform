using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
namespace VirtoCommerce.CatalogModule.Data.Services
{
	public abstract class ModuleServiceBase
	{
		protected void CommitChanges(IRepository repository)
		{
			try
			{
				repository.UnitOfWork.Commit();
			}
			catch (Exception ex)
			{
				ex.ThrowFaultException();
			}
		}
		protected ObservableChangeTracker GetChangeTracker(IFoundationCatalogRepository repository)
		{
			var retVal = new ObservableChangeTracker
			{
				RemoveAction = (x) =>
				{
					repository.Remove(x);
				},
				AddAction = (x) =>
				{
					repository.Add(x);
				}
			};

			return retVal;
		}
	}
}
