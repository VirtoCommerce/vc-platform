using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Orders.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
	public class SearchHomeViewModel : CustomersCommonViewModel, ISearchHomeViewModel, IVirtualListLoader<SearchModel>, ISupportDelayInitialization
	{
		#region Dependencies
		private readonly IViewModelsFactory<ICustomersDetailViewModel> _customersDetailVmFactory;
		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
		private readonly IRepositoryFactory<IOrderRepository> _orderRepositoryFactory;
		#endregion

		#region Constructor

		public SearchHomeViewModel(IViewModelsFactory<ICustomersDetailViewModel> customersDetailVmFactory, IRepositoryFactory<ICustomerRepository> repositoryFactory, IRepositoryFactory<IOrderRepository> orderRepositoryFactory)
			: base(repositoryFactory, customersDetailVmFactory)
		{
			_customersDetailVmFactory = customersDetailVmFactory;
			_repositoryFactory = repositoryFactory;
			_orderRepositoryFactory = orderRepositoryFactory;
		}

		#endregion

		private int _overallCount;
		public int OverallCount
		{
			get { return _overallCount; }
			set
			{
				_overallCount = value;
				OnPropertyChanged();
			}
		}


		#region Properties

		//search keywords
		public string SearchPhoneNumberKeyword { get; set; }
		public string SearchEmailKeyword { get; set; }
		public string SearchCaseNumberKeyword { get; set; }
		public string SearchCustomerNameKeyword { get; set; }
		public string SearchOrderNumberKeyword { get; set; }
		public string SearchCustomerKeyword { get; set; }

		#endregion

		#region IVirtualListLoader<SearchModel> Members

		public bool CanSort
		{
			get { return true; }
		}

		public IList<SearchModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<SearchModel>();
			overallCount = 0;

			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				var isCaseFilter = !string.IsNullOrEmpty(SearchCaseNumberKeyword);
				var isMemberFilter = !string.IsNullOrEmpty(SearchCustomerKeyword)
					|| !string.IsNullOrEmpty(SearchEmailKeyword)
					|| !string.IsNullOrEmpty(SearchOrderNumberKeyword)
					|| !string.IsNullOrEmpty(SearchPhoneNumberKeyword)
					|| !string.IsNullOrEmpty(SearchCustomerNameKeyword);

				//cases
				if (isCaseFilter || !isMemberFilter)
				{
					var cases = repository.Cases.Expand(c => c.Contact);

					//  number or title
					if (!string.IsNullOrEmpty(SearchCaseNumberKeyword))
					{
						cases = cases.Where(cs => cs.Number.Contains(SearchCaseNumberKeyword) || cs.Title.Contains(SearchCaseNumberKeyword));
						isCaseFilter = true;
					}

					overallCount = cases.Count();

					var resultsCase = cases.OrderBy(c => c.Title).Skip(startIndex).Take(count);
					foreach (var result in resultsCase)
					{
						var item = new SearchModel(SearchResultType.Case, result.CaseId, this, _customersDetailVmFactory)
						{
							CaseContactId = result.ContactId,
							DisplayName = result.Title
						};

						var descriptionFields = new List<string>();
						if (!string.IsNullOrWhiteSpace(result.Description))
							descriptionFields.Add(string.Format("Description: {0}".Localize(), result.Description));
						if (!string.IsNullOrWhiteSpace(result.Status))
							descriptionFields.Add(string.Format("Status: {0}".Localize(), result.Status));
						if (!string.IsNullOrWhiteSpace(result.AgentName))
							descriptionFields.Add(string.Format("Assigned: {0}".Localize(), result.AgentName));

						if (descriptionFields.Count > 0)
							item.Description = string.Join(", ", descriptionFields);

						retVal.Add(item);
					}
				}

				// Contacts
				if (!isCaseFilter || isMemberFilter)
				{
					var query = repository.Members.OfType<Contact>()
						.Expand(c => c.Addresses).Expand(c => c.Emails).Expand(c => c.Phones)
						// .Expand(c => c.Cases)
						// .Expand(c => c.Labels).Expand(c => c.Notes)
						;

					//customer number
					if (!string.IsNullOrEmpty(SearchCustomerKeyword))
					{
						query = query.Where(cont => cont.MemberId.Contains(SearchCustomerKeyword));
					}

					//email
					if (!string.IsNullOrEmpty(SearchEmailKeyword))
					{
						query = query.Where(cont => cont.Emails.Any(x => x.Address.Contains(SearchEmailKeyword)));
					}

					//order number
					if (!string.IsNullOrEmpty(SearchOrderNumberKeyword))
					{
						using (var ordersRepository = _orderRepositoryFactory.GetRepositoryInstance())
						{
							var orderList = ordersRepository.Orders;

							var orderFromDB =
								orderList.Where(ord => ord.OrderGroupId.Contains(SearchOrderNumberKeyword))
										 .FirstOrDefault();

							if (orderFromDB != null && !string.IsNullOrEmpty(orderFromDB.CustomerId))
							{
								query = query.Where(cont => cont.MemberId.Contains(orderFromDB.CustomerId));
							}
						}
					}

					//phone number
					if (!string.IsNullOrEmpty(SearchPhoneNumberKeyword))
					{
						var phoneList = repository.Phones;

						var phone = phoneList.Where(pn => pn.Number.Contains(SearchPhoneNumberKeyword))
											 .FirstOrDefault();

						if (phone != null && !string.IsNullOrEmpty(phone.MemberId))
						{
							query = query.Where(cont => cont.MemberId.Contains(phone.MemberId));
						}
					}

					//customer name
					if (!string.IsNullOrEmpty(SearchCustomerNameKeyword))
					{
						query = query.Where(
							cont =>
							cont.FullName.Contains(SearchCustomerNameKeyword)
							);
					}


					// recalculating startIndex and count
					if (startIndex > overallCount)
						startIndex -= overallCount;
					else
						startIndex = 0;

					count -= retVal.Count;

					overallCount += query.Count();
					OverallCount = overallCount;

					var results = query.OrderBy(c => c.FullName).Skip(startIndex).Take(count);
					foreach (var result in results)
					{
						var item = new SearchModel(SearchResultType.Contact, result.MemberId, this, _customersDetailVmFactory)
						{
							DisplayName = result.FullName
						};
						var descriptionFields = new List<string>();
						if (result.Addresses.Count > 0 && !string.IsNullOrWhiteSpace(result.Addresses[0].City))
							descriptionFields.Add(string.Format("City: {0}".Localize(), result.Addresses[0].City));
						if (result.Emails.Count > 0 && !string.IsNullOrWhiteSpace(result.Emails[0].Address))
							descriptionFields.Add(string.Format("Email: {0}".Localize(), result.Emails[0].Address));
						if (result.Phones.Count > 0 && !string.IsNullOrWhiteSpace(result.Phones[0].Number))
							descriptionFields.Add(string.Format("Phone: {0}".Localize(), result.Phones[0].Number));
						if (descriptionFields.Count > 0)
							item.Description = string.Join(", ", descriptionFields);

						retVal.Add(item);
					}
				}
			}

			return retVal;
		}

		#endregion

		#region ISupportdelayInitialization

		public void InitializeForOpen()
		{
			if (ListItemsSource == null)
			{
				ListItemsSource = new VirtualList<SearchModel>(this, 20, SynchronizationContext.Current);
			}
		}

		#endregion


	}
}

