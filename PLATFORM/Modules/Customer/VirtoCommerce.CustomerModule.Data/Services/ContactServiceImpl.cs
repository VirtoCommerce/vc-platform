using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Converters;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using foundationModel = VirtoCommerce.CustomerModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Data.Services
{
	public class ContactServiceImpl : ServiceBase, IContactService, ICustomerSearchService
	{
		private readonly Func<ICustomerRepository> _repositoryFactory;
		public ContactServiceImpl(Func<ICustomerRepository> repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#region IContactService Members

		public coreModel.Contact GetById(string id)
		{
			coreModel.Contact retVal = null;
			using (var repository = _repositoryFactory())
			{
				var entity = repository.GetContactById(id);
				if (entity != null)
				{
					retVal = entity.ToCoreModel();
				}
			}

			return retVal;
		}

		public coreModel.Contact Create(coreModel.Contact contact)
		{
			var entity = contact.ToDataModel();
			coreModel.Contact retVal = null;
			using (var repository = _repositoryFactory())
			{
				repository.Add(entity);
				CommitChanges(repository);
			}
			retVal = GetById(entity.Id);
			return retVal;
		}

		public void Update(coreModel.Contact[] contacts)
		{
			using (var repository = _repositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
			{
				foreach (var contact in contacts)
				{
					var sourceEntity = contact.ToDataModel();
					var targetEntity = repository.GetContactById(contact.Id);
					if (targetEntity == null)
					{
						throw new NullReferenceException("targetEntity");
					}

					changeTracker.Attach(targetEntity);
					sourceEntity.Patch(targetEntity);
				}
				CommitChanges(repository);
			}
		}

		public void Delete(string[] ids)
		{
			using (var repository = _repositoryFactory())
			{
				foreach (var id in ids)
				{
					var entity = repository.GetContactById(id);
					repository.Remove(entity);
				}
				CommitChanges(repository);
			}
		}
		#endregion

		#region IContactSearchService Members

		public coreModel.SearchResult Search(coreModel.SearchCriteria criteria)
		{
			coreModel.SearchResult retVal = null;
			using (var repository = _repositoryFactory())
			{
				var query = repository.Members.OrderBy(x=>x.CreatedDate).OfType<foundationModel.Contact>().Select(x=>x.Id);
				retVal = new coreModel.SearchResult
				{
					TotalCount = query.Count(),
					Contacts = new List<coreModel.Contact>()
				};
				foreach(var contactId in query.Skip(criteria.Start).Take(criteria.Count).ToArray())
				{
					var contact = repository.GetContactById(contactId);
					if(contact != null)
					{
						retVal.Contacts.Add(contact.ToCoreModel());
					}
				}
			}
			return retVal;
		}

		#endregion
	}
}
