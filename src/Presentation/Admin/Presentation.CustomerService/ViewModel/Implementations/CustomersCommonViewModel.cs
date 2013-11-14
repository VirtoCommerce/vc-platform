using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations
{
	public class CustomersCommonViewModel : ViewModelBase, ICustomersCommonViewModel
	{
		#region Dependencies

		private readonly IViewModelsFactory<ICustomersDetailViewModel> _customersDetailVmFactory;
		private readonly IRepositoryFactory<ICustomerRepository> _customerRepositoryFactory;

		#endregion

		public CustomersCommonViewModel(IRepositoryFactory<ICustomerRepository> customerRepositoryFactory, IViewModelsFactory<ICustomersDetailViewModel> customersDetailVmFactory)
		{
			_customersDetailVmFactory = customersDetailVmFactory;
			_customerRepositoryFactory = customerRepositoryFactory;
			CreateNewEmailCaseCommand = new DelegateCommand<Contact>(CreateNewCommerceManagerCaseForCurrentContact);
			DeleteCustomerCommand = new DelegateCommand<Contact>(DeleteCustomer);
			DeleteCaseCommand = new DelegateCommand<string>(DeleteCase);
			RefreshCommand = new DelegateCommand(Refresh);


		}

		public DelegateCommand<Contact> CreateNewEmailCaseCommand { get; private set; }
		public DelegateCommand<Contact> DeleteCustomerCommand { get; private set; }
		public DelegateCommand<string> DeleteCaseCommand { get; private set; }
		public DelegateCommand RefreshCommand { get; private set; }

		public int AllUnresolvedCasesCount { get; private set; }

		public int UnassignedCasesCount { get; private set; }

		public int RecentlyUpdatedCasesCount { get; private set; }

		private int _allCasesCount;
		public int AllCasesCount
		{
			get { return _allCasesCount; }
			set
			{
				_allCasesCount = value;
				OnPropertyChanged();
				OnPropertyChanged("CaseFilterTypes");
			}
		}

		private ICollectionView _listItemsSource;
		public ICollectionView ListItemsSource
		{
			get { return _listItemsSource; }
			set
			{
				_listItemsSource = value;
				OnPropertyChanged();
			}
		}

		protected ICollection<CaseFilterTypeViewModel> _caseFilterTypes = null;
		public ICollection<CaseFilterTypeViewModel> CaseFilterTypes
		{
			get { return _caseFilterTypes; }
		}


		private async void DeleteCustomer(Contact contact)
		{
			var contactToDelete = contact;
			var deletedContactId = contactToDelete.MemberId;

			var customerRepository =
				_customerRepositoryFactory.GetRepositoryInstance();

			ShowLoadingAnimation = true;

			var cases = await Task.Run(() => customerRepository.Cases.Where(
				c => c.ContactId == deletedContactId).ToList());

			Parallel.ForEach(cases, c =>
			{
				customerRepository.Attach(c);
				c.Contact = null;
				c.ContactId = null;
			});

			await Task.Run(() =>
			{
				customerRepository.UnitOfWork.Commit();

				customerRepository.Attach(contactToDelete);
				customerRepository.Remove(contactToDelete);
				customerRepository.UnitOfWork.Commit();
			});

			ShowLoadingAnimation = false;

			Refresh();
		}

		private void CreateNewCommerceManagerCaseForCurrentContact(Contact contact)
		{
			CreateNewCommerceManagerCase(contact, ContactActionState.Open, CaseActionState.New);
		}
		
		protected void CreateNewCommerceManagerCase(Contact contact, ContactActionState contactAction,
												  CaseActionState caseAction)
		{
			var parameters = new List<KeyValuePair<string, object>>()
				{
					new KeyValuePair<string, object>("parentViewModel", this),
					new KeyValuePair<string, object>("innerCase", new Case()),
					new KeyValuePair<string, object>("innerContact", contact),
					new KeyValuePair<string, object>("caseAction", caseAction),
					new KeyValuePair<string, object>("contactAction", contactAction)
				}.ToArray();
			var itemVm = _customersDetailVmFactory.GetViewModelInstance(parameters);
			((IOpenTracking)itemVm).OpenItemCommand.Execute();
		}

		private async void DeleteCase(string item)
		{
			var caseIdToDelete = item;
			using (var repository = _customerRepositoryFactory.GetRepositoryInstance())
			{
				var caseToDelete =
					repository.Cases.Where(c => c.CaseId == caseIdToDelete).SingleOrDefault();

				if (caseToDelete != null)
				{
					await
						Task.Run(
							() => RemoveCaseDependentItems(repository, caseIdToDelete));

					repository.Remove(caseToDelete);
					repository.UnitOfWork.Commit();

					Refresh();


				}
			}
		}

		private void RemoveCaseDependentItems(ICustomerRepository repository, string caseIdToDelete)
		{
			//remove labels
			var labels = repository.Labels.Where(l => l.CaseId == caseIdToDelete).ToList();
			Parallel.ForEach(labels, l =>
			{
				l.CaseId = null;
				repository.Remove(l);
			});

			//remove notes
			var notes = repository.Notes.Where(n => n.CaseId == caseIdToDelete).ToList();
			Parallel.ForEach(notes, n =>
			{
				n.CaseId = null;
				repository.Remove(n);
			});

			//remove communication items (public reply)
			var communicationItems =
				repository.CommunicationItems.Where(c => c.CaseId == caseIdToDelete).ToList();
			Parallel.ForEach(communicationItems, c =>
			{
				c.CaseId = null;
				repository.Remove(c);
			});
		}

		public void Refresh()
		{
			OnUIThread(() =>
			{
				ListItemsSource.Refresh();
				SendEventToShell();
			});
		}

		protected void SendEventToShell()
		{
			try
			{
				UpdateCaseCount();
				var mes = new ShellMessageEvent() { Message = AllUnresolvedCasesCount.ToString() };
				EventSystem.Publish(mes);
			}
			catch
			{
			}
		}

		/// <summary>
		/// update case count for homeGridView
		/// </summary>
		/// <param name="repository"></param>
		private void UpdateCaseCount()
		{
			using (var repository = _customerRepositoryFactory.GetRepositoryInstance())
			{
				var allCases = repository.Cases.ToList();

				var resolvedStatus = CaseStatus.Resolved.ToString();
				AllUnresolvedCasesCount = allCases.Count(c => c.Status != resolvedStatus);
				AllCasesCount = allCases.Count;

				////TODO
				////сделать нормальную проверку на неподписаннные case
				UnassignedCasesCount = allCases.Count(c => string.IsNullOrEmpty(c.AgentId));

				////TODO
				////сделать проверку на case, которые подписаны не на текущего пользователя
				////this.MyGroupsUnassignedCasesCount = searchResult.CaseInfos.Count(caseDto => caseDto.AgentId != _currentUserId);

				////TODO
				////сделать проверку на недавно модифицированные case

				var lowBoundDate = DateTime.Now.AddDays(-3);
				this.RecentlyUpdatedCasesCount =
					allCases.Count(c => c.LastModified <= DateTime.Now && c.LastModified > lowBoundDate);

				if (CaseFilterTypes != null)
				{
					foreach (var filter in CaseFilterTypes)
					{
						switch (filter.Model)
						{
							case CaseFilterType.AllCases:
								filter.Count = AllCasesCount;
								break;
							case CaseFilterType.AllUnresolvedCases:
								filter.Count = AllUnresolvedCasesCount;
								break;
							case CaseFilterType.RecentlyUpdatedCases:
								filter.Count = RecentlyUpdatedCasesCount;
								break;
							case CaseFilterType.UnassignedCases:
								filter.Count = UnassignedCasesCount;
								break;
						}
					}
				}
			}
		}

	}
}
