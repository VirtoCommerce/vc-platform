using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DragDrop;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Adaptors;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
	public class KnowledgeBaseDialogViewModel : ViewModelBase, IKnowledgeBaseDialogViewModel, IDropTarget
	{
		#region Dependencies

		private readonly ICustomerRepository _customersRepository;
		private readonly string _authorId;
		private readonly string _authorName;
		private readonly IViewModelsFactory<IKnowledgeGroupViewModel> _groupVmFactory;
		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
		private readonly ICustomerEntityFactory _entityFactory;
		private readonly IViewModelsFactory<IKnowledgeGroupViewModel> _knowledgeVmFactory;
		private readonly IViewModelsFactory<IKnowledgeBaseDialogViewModel> _knowledgeBaseGroupVmFactory;

		#endregion

		#region Requests

		public InteractionRequest<Confirmation> AddKnowledgeBaseGroupDialogRequest { get; private set; }
		public InteractionRequest<Confirmation> EditKnowledgeBaseGroupDialogRequest { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#endregion

		#region Properties

		private ObservableCollection<IViewModel> _rootKnowledgeGroups;
		public ObservableCollection<IViewModel> RootKnowledgeGroups
		{
			get
			{
				if (_rootKnowledgeGroups == null)
				{
					_rootKnowledgeGroups = new ObservableCollection<IViewModel>();
				}
				return _rootKnowledgeGroups;
			}
		}

		private IViewModel _selectedKnowledgeGroup;
		public IViewModel SelectedKnowledgeGroup
		{
			get
			{
				return _selectedKnowledgeGroup;
			}
			set
			{
				if (_selectedKnowledgeGroup != value)
				{
					CommitSelectedKnowledgeGroupArticles();
					_selectedKnowledgeGroup = value;
					RefreshSelectedKnowledgeGroupArticles();
				}
			}
		}

		//public string SearchKeyword { get; set; }
		//public long TotalSearched { get; set; }

		private bool _isValid;
		public bool IsValid
		{
			get
			{
				return _isValid; //  && KnowledgeArticleCommunicationControl != null && KnowledgeArticleCommunicationControl.SelectedCommunicationItem != null;
			}
			set
			{
				_isValid = value;
				OnPropertyChanged("IsValid");
			}
		}

		#endregion

		#region Constructor

		public KnowledgeBaseDialogViewModel(
			IViewModelsFactory<IKnowledgeGroupViewModel> knowledgeVmFactory,
			IRepositoryFactory<ICustomerRepository> repositoryFactory,
			IViewModelsFactory<IKnowledgeGroupViewModel> groupVmFactory,
			ICustomerEntityFactory customerEntityFactory,
			IAuthenticationContext authContext,
			IViewModelsFactory<IKnowledgeBaseDialogViewModel> knowledgeBaseGroupVmFactory
			)
		{
			_knowledgeBaseGroupVmFactory = knowledgeBaseGroupVmFactory;
			_knowledgeVmFactory = knowledgeVmFactory;
			_entityFactory = customerEntityFactory;
			_repositoryFactory = repositoryFactory;
			_groupVmFactory = groupVmFactory;
			_customersRepository = _repositoryFactory.GetRepositoryInstance();
			InitCommands();
			RefreshRootKnowledgeGroups();
			this.ShowLoadingAnimation = false;
			AddKnowledgeBaseGroupDialogRequest = new InteractionRequest<Confirmation>();
			EditKnowledgeBaseGroupDialogRequest = new InteractionRequest<Confirmation>();
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			var _authContext = authContext;
			_authorId = _authContext.CurrentUserId;
			_authorName = _authContext.CurrentUserName;
		}

		#endregion

		#region Commands Implementation

		private void InitCommands()
		{
			CreateKnowledgeGroupCommand = new DelegateCommand<object>(RaiseCreateKnowledgeGroupCommand);
			EditKnowledgeGroupCommand = new DelegateCommand<object>(RaiseEditKnowledgeGroup);
			TreeItemDeleteCommand = new DelegateCommand<object>(RaiseTreeItemDelete);
			SearchCommand = new DelegateCommand(RiseSearchCustomer);
		}

		public DelegateCommand<object> CreateKnowledgeGroupCommand { get; private set; }
		private void RaiseCreateKnowledgeGroupCommand(object knowlegeGroupViewModel)
		{
			var parentVM = (knowlegeGroupViewModel as KnowledgeGroupViewModel) ?? SelectedKnowledgeGroup as KnowledgeGroupViewModel;
			if (AddKnowledgeBaseGroupDialogRequest != null)
			{
				AddKnowledgeBaseGroupDialogRequest.Raise(
					new Confirmation
					{
						Title = "Add group".Localize(),
						Content = new KnowledgeGroupViewModel(_repositoryFactory, _groupVmFactory, _repositoryFactory.GetRepositoryInstance(), _entityFactory, new KnowledgeBaseGroup() { ParentId = parentVM != null ? parentVM.OriginalItem.KnowledgeBaseGroupId : null })
					},
					(x) =>
					{
						if (x.Confirmed)
						{
							var currentGroup = (x.Content as KnowledgeGroupViewModel);
							if (currentGroup != null && currentGroup.InnerItem != null)
							{
								Task.Factory.StartNew(() =>
								{
									ShowLoadingAnimation = true;
									(x.Content as KnowledgeGroupViewModel).InsertToDB();
								})
								.ContinueWith(t =>
								{
									if (t.Exception == null)
									{
										OnUIThread(() =>
										{
											if (parentVM != null)
											{
												if (parentVM.IsExpanded)
												{
													parentVM.Refresh();
												}
												else
												{
													parentVM.IsExpanded = true;
												}
											}
											else
											{
												RootKnowledgeGroups.Add(x.Content as KnowledgeGroupViewModel);
											}
										});
									}
									ShowLoadingAnimation = false;
								});
							}
						}
					}
				);
			}
		}

		public DelegateCommand<object> EditKnowledgeGroupCommand { get; private set; }
		private void RaiseEditKnowledgeGroup(object knowlegeGroupViewModel)
		{
			if ((knowlegeGroupViewModel as KnowledgeGroupViewModel) != null)
			{
				SelectedKnowledgeGroup = (knowlegeGroupViewModel as KnowledgeGroupViewModel);
			}
			if (AddKnowledgeBaseGroupDialogRequest != null && (SelectedKnowledgeGroup as KnowledgeGroupViewModel) != null)
			{
				AddKnowledgeBaseGroupDialogRequest.Raise(
					new Confirmation
					{
						Title = "Edit a group".Localize(),
						Content = (SelectedKnowledgeGroup as KnowledgeGroupViewModel)
					},
					(x) =>
					{
						if (x.Confirmed)
						{
							if ((SelectedKnowledgeGroup as KnowledgeGroupViewModel).InnerItem != null)
							{
								Task.Factory.StartNew(() =>
								{
									ShowLoadingAnimation = true;
									(SelectedKnowledgeGroup as KnowledgeGroupViewModel).ApplyToDB();
								})
								.ContinueWith(t =>
								{
									ShowLoadingAnimation = false;
								});
							}
						}
					}
				);
			}
		}

		public DelegateCommand<object> TreeItemDeleteCommand { get; private set; }
		private void RaiseTreeItemDelete(object knowlegeGroupViewModel)
		{
			if ((knowlegeGroupViewModel as KnowledgeGroupViewModel) != null)
			{
				SelectedKnowledgeGroup = (knowlegeGroupViewModel as KnowledgeGroupViewModel);
			}
			bool canDelete = false;
			if (SelectedKnowledgeGroup != null)
			{
				var articles = GetKnowledgeBaseArticles(SelectedKnowledgeGroup as KnowledgeGroupViewModel);
				var children = (SelectedKnowledgeGroup as KnowledgeGroupViewModel).GetChildren((SelectedKnowledgeGroup as KnowledgeGroupViewModel).OriginalItem);
				//var subGroup = _customersRepository.KnowledgeBaseGroups.Where(x => x.Parent.KnowledgeBaseGroupId == (SelectedKnowledgeGroup as KnowledgeGroupViewModel).OriginalItem.KnowledgeBaseGroupId).ToList();
				canDelete = (articles != null && articles.Count() == 0 && children != null && children.Count() == 0);
			}

			if (CommonConfirmRequest != null && canDelete)
			{
				CommonConfirmRequest.Raise(
					new Confirmation
					{
						Title = "Remove group?".Localize(),
						Content = (SelectedKnowledgeGroup as KnowledgeGroupViewModel).InnerItem.Name
					},
					(x) =>
					{
						if (x.Confirmed)
						{
							Task.Factory.StartNew(() =>
							{
								ShowLoadingAnimation = true;
								(SelectedKnowledgeGroup as KnowledgeGroupViewModel).RemoveFromDB();
							})
						   .ContinueWith(t =>
						   {
							   if (t.Exception == null)
							   {
								   OnUIThread(() =>
								   {
									   if ((SelectedKnowledgeGroup as KnowledgeGroupViewModel).Parent != null)
									   {
										   SelectedKnowledgeGroup = (SelectedKnowledgeGroup as KnowledgeGroupViewModel).Parent;
										   (SelectedKnowledgeGroup as KnowledgeGroupViewModel).Refresh();
									   }
									   else
									   {
										   RootKnowledgeGroups.Remove(SelectedKnowledgeGroup as KnowledgeGroupViewModel);
										   SelectedKnowledgeGroup = null;
									   }
									   //TODO: update treeview
								   });
							   }
							   ShowLoadingAnimation = false;
						   });
						}

					}
				);
			}
		}

		public DelegateCommand SearchCommand { get; private set; }
		private void RiseSearchCustomer()
		{
		}

		#endregion

		#region Private methods

		private void RefreshRootKnowledgeGroups()
		{
			OnUIThread(() =>
			{
				RootKnowledgeGroups.Clear();
				KnowledgeArticleCommunicationControl.ClearAllCommunicationItems();
			});
			if (_customersRepository != null && _customersRepository.KnowledgeBaseGroups != null)
			{
				Task.Factory.StartNew(() => { return _customersRepository.KnowledgeBaseGroups.Where(x => x.Parent == null).ToList(); })
							.ContinueWith(t =>
							{
								OnUIThread(() =>
								{
									foreach (var item in t.Result)
									{
										var knowledgeGroupParameter = new KeyValuePair<string, object>("item", item);
										var knowledgeGroupViewModel = (IViewModel)_knowledgeVmFactory.GetViewModelInstance(knowledgeGroupParameter);

										RootKnowledgeGroups.Add(knowledgeGroupViewModel);
									}
								});
							});
			}
			OnUIThread(() =>
			{
				KnowledgeArticleCommunicationControl.IsInitialized = true;
				OnPropertyChanged("RootKnowledgeGroups");
			});
		}

		#endregion

		#region Communication control

		private ICommunicationControlViewModel _knowledgeArticleCommunicationControl;
		public ICommunicationControlViewModel KnowledgeArticleCommunicationControl
		{
			get
			{
				return _knowledgeArticleCommunicationControl ??
					   (_knowledgeArticleCommunicationControl =
						new KnowledgeArticleCommunicationControlViewModel(null, _knowledgeBaseGroupVmFactory, _authorId, _authorName, this));
			}
		}

		private IQueryable<KnowledgeBaseArticle> GetKnowledgeBaseArticles(KnowledgeGroupViewModel knowledgeGroup)
		{
			if (_customersRepository != null && _customersRepository.KnowledgeBaseArticles != null && knowledgeGroup != null)
			{
				var query = _customersRepository.KnowledgeBaseArticles;
				return query.Where(x => x.GroupId == knowledgeGroup.InnerItem.KnowledgeBaseGroupId);
			}
			return null;
		}

		/// <summary>
		/// refresh articles for selected KnowledgeBaseGroup
		/// </summary>
		private void RefreshSelectedKnowledgeGroupArticles()
		{
			var adaptor = new CommunicationAdaptor();

			Task.Factory.StartNew(() =>
			{
				KnowledgeArticleCommunicationControl.IsInitialized = false;
				List<CommunicationItemViewModel> result = null;
				var selectedKnowledgeGroup = SelectedKnowledgeGroup as KnowledgeGroupViewModel;
				var articles = GetKnowledgeBaseArticles(selectedKnowledgeGroup);
				if (articles != null && articles.Count() > 0)
				{
					result = new List<CommunicationItemViewModel>();
					foreach (var i in articles)
					{
						result.Add(adaptor.KnowledgeBaseArticle2KnowledgeBaseArticleCommunicationViewModel(i));
					}
					//result = articles.Select(n => (adaptor.KnowledgeBaseArticle2KnowledgeBaseArticleCommunicationViewModel(n) as CommunicationItemViewModel)).OfType<CommunicationItemViewModel>().ToList();
				}
				return result;
			}).ContinueWith(t =>
			{
				OnUIThread(() =>
				{
					if (t.Exception == null)
					{
						KnowledgeArticleCommunicationControl.ClearAllCommunicationItems();
						KnowledgeArticleCommunicationControl.AppendCommunucationItems(t.Result);
					}
					KnowledgeArticleCommunicationControl.IsInitialized = true;
				});
			});
		}

		private void CommitSelectedKnowledgeGroupArticles()
		{
			var adaptor = new CommunicationAdaptor();
			var selectedGroupId = (_selectedKnowledgeGroup as KnowledgeGroupViewModel) != null ? (_selectedKnowledgeGroup as KnowledgeGroupViewModel).OriginalItem.KnowledgeBaseGroupId : null;
			var communicationControl = KnowledgeArticleCommunicationControl as KnowledgeArticleCommunicationControlViewModel;
			var itemsForDelete = communicationControl.GetItemsByState(CommunicationItemState.Deleted);
			var itemsForAppend = communicationControl.GetItemsByState(CommunicationItemState.Appended);
			var itemsForModify = communicationControl.GetItemsByState(CommunicationItemState.Modified);

			Task.Factory.StartNew(() =>
			{
				var repository = _repositoryFactory.GetRepositoryInstance();
				ShowLoadingAnimation = true;
				// delete
				if (itemsForDelete != null && itemsForDelete.Count() > 0)
				{
					foreach (CommunicationItemViewModel item in itemsForDelete)
					{
						KnowledgeBaseArticle original = adaptor.KnowledgeBaseArticleCommunicationViewModel2KnowledgeBaseArticle(item as CommunicationItemKnowledgeBaseArticleViewModel);
						//SyncAttach(original, null);
						repository.Attach(original);
						repository.Remove(original);
					}
				}
				// append
				if (itemsForAppend != null && itemsForAppend.Count() > 0 && selectedGroupId != null)
				{
					foreach (CommunicationItemViewModel item in itemsForAppend)
					{
						KnowledgeBaseArticle article = adaptor.KnowledgeBaseArticleCommunicationViewModel2KnowledgeBaseArticle(item as CommunicationItemKnowledgeBaseArticleViewModel);
						article.GroupId = selectedGroupId;
						//SyncAttach(null, article);
						repository.Add(article);
					}
				}
				// modify
				if (itemsForModify != null && itemsForModify.Count() > 0)
				{
					foreach (CommunicationItemViewModel item in itemsForModify)
					{
						var article = adaptor.KnowledgeBaseArticleCommunicationViewModel2KnowledgeBaseArticle(item as CommunicationItemKnowledgeBaseArticleViewModel);
						article.GroupId = selectedGroupId;
						var original = repository.KnowledgeBaseArticles.Where(x => x.KnowledgeBaseArticleId == article.KnowledgeBaseArticleId).SingleOrDefault();
						//SyncAttach(original, article);
						repository.Attach(original);
						OnUIThread(() => original.InjectFrom(article));
					}
				}
				int i = repository.UnitOfWork.Commit();
			})
		   .ContinueWith(t =>
		   {
			   ShowLoadingAnimation = false;
		   });
		}

		private void SyncAttach(KnowledgeBaseArticle original, KnowledgeBaseArticle article)
		{
			var repository = _repositoryFactory.GetRepositoryInstance();
			// removing attachments
			if (original != null)
			{
				foreach (var attach in original.Attachments)
				{
					if (article != null || article.Attachments == null || !article.Attachments.Any(x => (x.AttachmentId == attach.AttachmentId)))
					{
						repository.Attach(attach);
						repository.Remove(attach);
					}
				}
			}

			// appending attachments
			if (article != null)
			{
				foreach (var attach in article.Attachments)
				{
					if (original == null || original.Attachments == null || !original.Attachments.Any(x => (x.AttachmentId == attach.AttachmentId)))
					{
						attach.CommunicationItemId = article.KnowledgeBaseArticleId;
						repository.Attach(attach);
						repository.Add(attach);
					}
				}
			}
			int i = repository.UnitOfWork.Commit();
		}

		#endregion

		#region IDropTarget Members

		public void DragOver(DropInfo dropInfo)
		{
			bool canAcceptData = dropInfo.DragInfo != null;

			if (canAcceptData)
			{
				var realSourceItem = dropInfo.DragInfo.SourceItem as HierarchyViewModelBase;
				var realTargetItem = dropInfo.TargetItem as HierarchyViewModelBase;

				canAcceptData = realSourceItem != realTargetItem
							&& realSourceItem is IKnowledgeGroupViewModel
							&& (realTargetItem is IKnowledgeGroupViewModel || realTargetItem is IKnowledgeGroupViewModel)
							&& realSourceItem.Parent != realTargetItem;
				//&& GetCatalogId(realSourceItem) == GetCatalogId(realTargetItem);

				if (canAcceptData)
				{
					dropInfo.Effects = System.Windows.DragDropEffects.Move;
					dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
				}
			}
		}

		public void Drop(DropInfo dropInfo)
		{
			var droppedVM = dropInfo.Data;
			var targetVM = dropInfo.TargetItem;

			if (droppedVM is IHierarchy)
			{
				// preparing to refresh UI
				var sourceVM = droppedVM as HierarchyViewModelBase;
				var sourceParentVM = sourceVM.Parent;

				Task.Factory.StartNew(() =>
				{
					ShowLoadingAnimation = true;
					((IHierarchy)droppedVM).SetParent(null, targetVM);
				})
			   .ContinueWith(t =>
			   {
				   if (t.Exception == null)
				   {
					   OnUIThread(() =>
					   {
						   // refresh UI
						   if (sourceParentVM != null)
						   {
							   sourceParentVM.LoadChildrens();
						   }
						   else
						   {
							   RootKnowledgeGroups.Remove(droppedVM as KnowledgeGroupViewModel);
						   }
						   ((HierarchyViewModelBase)targetVM).LoadChildrens();
					   });
				   }
				   ShowLoadingAnimation = false;
			   });
			}
		}

		#endregion

	}
}
