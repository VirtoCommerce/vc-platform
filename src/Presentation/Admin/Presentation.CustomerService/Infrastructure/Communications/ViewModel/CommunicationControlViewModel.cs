using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications
{
	public abstract class CommunicationControlViewModel : ViewModelBase, ICommunicationControlViewModel
	{
		#region Private Fields

		private string _authorName;
		private string _authorId;

		private readonly IViewModelsFactory<IKnowledgeBaseDialogViewModel> _knowledgeBaseGroupVmFactory;
		private readonly IAssetService _assetService;

		#endregion

		#region Public Property

		private ICollectionView _itemsCollection;
		public ICollectionView ItemsCollection
		{
			get
			{
				if (_itemsCollection == null)
				{
					_itemsCollection = CollectionViewSource.GetDefaultView(Items);
					_itemsCollection.Filter = DefineFilterItems;
					_itemsCollection.SortDescriptions.Add(new SortDescription("LastModified", ListSortDirection.Descending));
				}
				return _itemsCollection;
			}
		}

		private bool _isInitialized = false;
		public bool IsInitialized
		{
			get
			{
				return _isInitialized;
			}
			set
			{
				_isInitialized = value;
				OnPropertyChanged("ShowLoadingAnimation");
				OnPropertyChanged("IsShowEmpty");
			}
		}

		/// <summary>
		/// if case has one communication item, then this case can be saved
		/// </summary>
		private bool _hasOneItem = false;
		public bool HasOneValidItem
		{
			get
			{
				_hasOneItem = HasCommunicationControlOneItemWithFilledBody();

				return _hasOneItem;
			}
			private set { _hasOneItem = value; }
		}

		public bool HasAnyToolBarCommands
		{
			get { return ToolBarCommmands.Any(); }
		}

		public bool IsShowEmpty
		{
			get
			{
				return !ShowLoadingAnimation && ItemsCollection.IsEmpty;
			}
			set
			{
				_isInitialized = value;
				OnPropertyChanged("ShowLoadingAnimation");
			}
		}

		/// <summary>
		/// if CommunicationControl is modified, then CustomersdetailViewModel is modified too.
		/// </summary>
		private bool _isModified = false;
		public bool IsModified
		{
			get { return _isModified; }
			set
			{
				_isModified = value;
				if (value)
				{
					ModifiedRequest();
				}
			}
		}


		public bool OneItemInCommunicationIsInEditState
		{
			get
			{
				bool result = false;
				if (Items != null)
				{
					result = Items.Any(comItem => comItem.IsEditing);
				}
				return result;
			}
		}

		private string _newBody;
		public string NewBody
		{
			get { return _newBody; }
			set
			{
				_newBody = value;
				OnPropertyChanged();
				IsModified = true;
			}
		}

		private bool _isReadOnly = false;
		public bool IsReadOnly
		{
			get { return _isReadOnly; }
			set { _isReadOnly = value; OnPropertyChanged(); }
		}


		#endregion

		#region Constructors

		public CommunicationControlViewModel(
			IAssetService assetService,
			IViewModelsFactory<IKnowledgeBaseDialogViewModel> knowledgeBaseGroupVmFactory,
			string authorId,
			string authorName)
		{
			_assetService = assetService;
			_knowledgeBaseGroupVmFactory = knowledgeBaseGroupVmFactory;
			IsInitialized = false;
			SetAuthor(authorId, authorName);
			OpenKnowledgeBaseRequest = new InteractionRequest<Confirmation>();
			ConfirmRequest = new InteractionRequest<Confirmation>();


			CommandInit();

		}



		#endregion

		#region Commands

		public DelegateCommand<object> DeleteCommunicationItemWithoutConfirmationCommand { get; private set; }

		public DelegateCommand<object> OpenKnowledgeBaseCommand { get; private set; }

		public DelegateCommand<object> AddItemCommand { get; private set; }

		#endregion

		#region ICommunicationControlViewModel

		private List<CommunicationItemViewModel> _items;
		public List<CommunicationItemViewModel> Items
		{
			get
			{
				if (_items == null)
				{
					_items = new List<CommunicationItemViewModel>();
				}
				return _items;
			}
		}

		public CommunicationItemViewModel SelectedCommunicationItem { set; get; }

		private List<CommunicationItemComands> _toolBarCommmands;
		public List<CommunicationItemComands> ToolBarCommmands
		{
			get
			{
				if (_toolBarCommmands == null)
				{
					_toolBarCommmands = new List<CommunicationItemComands>();
				}
				return _toolBarCommmands;
			}
		}

		public void ClearAllCommunicationItems()
		{
			if (Items != null)
			{
				Items.Clear();
				RefreshItems();
			}
		}

		public void SetAuthor(string authorId, string authorName)
		{
			_authorId = authorId;
			_authorName = authorName;
		}

		/// <summary>
		/// Append given list of communication items to the CommunicationControl
		/// </summary>
		public void AppendCommunucationItems(List<CommunicationItemViewModel> communicationItems)
		{
			if (communicationItems != null)
			{
				foreach (CommunicationItemViewModel item in communicationItems)
				{
					AppendCommunicationItem(item, false, false);
				}
				RefreshItems();
			}
		}

		#endregion

		#region Protected Methods

		public InteractionRequest<Confirmation> ConfirmRequest { get; private set; }

		protected void SelectCommunicationItem(object param)
		{
			var selItem = param as CommunicationItemViewModel;
			if (selItem != null)
			{
				SelectedCommunicationItem = selItem;
				foreach (var item in Items)
				{
					item.IsSelected = false;
				}
				selItem.IsSelected = true;
				SelectedRequest();
			}
		}

		protected void DeleteCommunicationItem(object param)
		{
			var item = param as CommunicationItemViewModel;
			if (item != null && ConfirmRequest != null)
			{
				ConfirmRequest.Raise(
				new ConditionalConfirmation { Content = "Remove?".Localize(), Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory) },
				x =>
				{
					if (x.Confirmed)
					{
						OnUIThread(() => ChangeItemState(item, CommunicationItemState.Deleted, false));
					}
				});
			}
		}

		protected void DeleteCommunicationItemWithoutConfirmation(object param)
		{
			var item = param as CommunicationItemViewModel;
			if (item != null)
			{
				OnUIThread(() => ChangeItemState(item, CommunicationItemState.Deleted, false));
			}
		}

		protected void AppendCommunicationItem(CommunicationItemViewModel item, bool isNew, bool isEditing)
		{
			if (item != null)
			{
				if (isNew)
				{
					item.State = CommunicationItemState.Appended;
					item.IsEditing = isEditing;
					item.AuthorId = _authorId;
					item.AuthorName = _authorName;
					item.LastModified = DateTime.Now.ToUniversalTime();
				}

				DefItemCommands(item);
				item.RaiseCanExecuteChanged();

				Items.Add(item);


				if (isNew && ItemsCollection != null)
				{
					RefreshItems();
					ItemsCollection.MoveCurrentToFirst();
				}
				item.CommunicationItemPropertyChanged += CommunicationItem_OnPropertyChanged;

				OnPropertyChanged("OneItemInCommunicationIsInEditState");
				OnPropertyChanged("Items");
				OnPropertyChanged("IsShowEmpty");

				ModifiedRequest();
			}
		}

		protected void ChangeItemState(object param, CommunicationItemState state, bool isEditing)
		{
			if (param is CommunicationItemViewModel)
			{
				// if old state is  Appended we have no to change it
				if ((param as CommunicationItemViewModel).State == CommunicationItemState.Appended)
				{
					//if old state is  Appended and new state Deleted then delete item 
					if (state == CommunicationItemState.Deleted)
					{
						Items.Remove((param as CommunicationItemViewModel));

					}
				}
				else
				{
					(param as CommunicationItemViewModel).State = state;
				}

				(param as CommunicationItemViewModel).IsEditing = isEditing;
				(param as CommunicationItemViewModel).RaiseCanExecuteChanged();

				if (state == CommunicationItemState.Deleted)
				{
					RefreshItems();
				}
				if (isEditing)
				{
					ItemsCollection.MoveCurrentTo(param as CommunicationItemViewModel);
				}

				OnPropertyChanged("OneItemInCommunicationIsInEditState");
			}
		}

		protected virtual bool DefineFilterItems(object item)
		{
			return (item is CommunicationItemViewModel && (item as CommunicationItemViewModel).State != CommunicationItemState.Deleted);
		}

		protected void RefreshItems()
		{
			if (ItemsCollection != null)
			{
				ItemsCollection.Refresh();
			}
		}

		protected void AviableSelect(CommunicationItemViewModel item)
		{
			item.ItemCommands.Add(new CommunicationItemComands()
			{
				Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/star_none.png",
				ToolTip = "Select".Localize(),
				Command = new DelegateCommand<object>(SelectCommunicationItem),
				SetVisible = () => true
			});
		}

		protected void AviableDelete(CommunicationItemViewModel item)
		{
			item.ItemCommands.Add(new CommunicationItemComands()
			{
				Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/delete.png",
				ToolTip = "Delete".Localize(),
				Command = new DelegateCommand<object>(DeleteCommunicationItem),
				SetVisible = () => true
			});
		}

		protected void AviableEdit(CommunicationItemViewModel item)
		{
			item.ItemCommands.Add(new CommunicationItemComands()
			{
				Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/edit.png",
				ToolTip = "Edit".Localize(),
				Command = new DelegateCommand<object>(x => ChangeItemState(x, CommunicationItemState.Modified, true)
					, x => x is CommunicationItemNoteViewModel && (DateTime.UtcNow - ((CommunicationItemViewModel)x).Created).Hours < 1),
				SetVisible = () => !item.IsEditing
			});
		}

		protected void AviablePost(CommunicationItemViewModel item)
		{
			item.ItemCommands.Add(new CommunicationItemComands()
			{
				Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/save.png",
				ToolTip = "Post".Localize(),
				Command = new DelegateCommand<object>(x => ChangeItemState(x, CommunicationItemState.Modified, false)),
				SetVisible = () => item.IsEditing
			});
		}

		protected void AviableKnowledgeBase(CommunicationItemViewModel item, string typeKnowledgeBase)
		{
			item.ItemCommands.Add(new CommunicationItemComands()
			{
				Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/black_book.png",
				ToolTip = "Knowledge base".Localize(),
				Command = new DelegateCommand<object>(OpenKnowledgeBaseDialog, (x) => item.IsEditing)
			});
		}

		#endregion

		#region Public Methods

		public List<CommunicationItemViewModel> GetItemsByState(CommunicationItemState state)
		{
			return (Items != null) ? Items.Where((x) => x.State == state).ToList() : new List<CommunicationItemViewModel>();
		}

		private void UploadNewAttachments()
		{
			var items = GetItemsByState(CommunicationItemState.Modified);
			foreach (var item in items)
			{
				var newAttachments = item.GetAttacmentsByState(CommunicationItemState.Appended);
				foreach (var attach in newAttachments)
				{
					using (Stream stream = new FileStream(attach.Url, FileMode.Open))
					{
						//attach.Id = _assetService.Upload(stream);
					}
				}
			}
		}

		#endregion

		#region Private Methods

		private void CommandInit()
		{
			DeleteCommunicationItemWithoutConfirmationCommand =
				new DelegateCommand<object>(DeleteCommunicationItemWithoutConfirmation
					, (item) =>
						{
							var result = false;

							var commItem = item as CommunicationItemViewModel;
							if (commItem != null)
							{
								if (string.IsNullOrEmpty(commItem.Body))
								{
									result = true;
								}
							}

							return result;
						});

			OpenKnowledgeBaseCommand = new DelegateCommand<object>(OpenKnowledgeBaseDialog);

			AddItemCommand = new DelegateCommand<object>(AddItem);

		}


		public InteractionRequest<Confirmation> OpenKnowledgeBaseRequest { get; private set; }
		private void OpenKnowledgeBaseDialog(object param)
		{
			var itemViewModel = param as CommunicationItemViewModel;
			if (itemViewModel != null && OpenKnowledgeBaseRequest != null)
			{
				OpenKnowledgeBaseRequest.Raise(
					new Confirmation
					{
						Title = "Knowledge base".Localize(),
						Content = _knowledgeBaseGroupVmFactory.GetViewModelInstance()
					},
					(x) =>
					{
						if (x.Confirmed && x.Content is KnowledgeBaseDialogViewModel)
						{
							var selectedArticle = (x.Content as KnowledgeBaseDialogViewModel).KnowledgeArticleCommunicationControl.SelectedCommunicationItem;
							if (selectedArticle is CommunicationItemKnowledgeBaseArticleViewModel)
							{
								itemViewModel.Body = selectedArticle.Body;
								var attachments = (selectedArticle as CommunicationItemKnowledgeBaseArticleViewModel).Attachments;
								if (attachments != null)
								{
									foreach (var attachment in attachments)
									{
										itemViewModel.Attachments.Add(attachment);
									}
								}
							}
						}
					});
			}
		}


		private bool HasCommunicationControlOneItemWithFilledBody()
		{
			var result = false;

			if (Items != null)
			{
				result = Items.Any(comItem => string.IsNullOrEmpty(comItem.Body) == false);
			}

			return result;
		}

		private void AddItem(object item)
		{
			if (ToolBarCommmands != null)
			{
				var activeCommand = ToolBarCommmands.SingleOrDefault(c => c.IsActive);
				if (activeCommand != null)
				{
					activeCommand.Command.Execute(NewBody);
				}
			}
		}

		#endregion

		#region Handlers

		private void ItemsCollectionView_CurrentChanging(object sender, System.ComponentModel.CurrentChangingEventArgs e)
		{
			ModifiedRequest();
		}

		private void CommunicationItem_OnPropertyChanged(object sender, EventArgs e)
		{
			OnPropertyChanged("IsShowEmpty");
			ModifiedRequest();
		}

		#endregion

		#region Abstract methods
		/// <summary>
		/// Define available operations (commands) for given item
		/// </summary>
		/// <example>
		/// item.ItemCommands.Add(new CommunicationItemComands() { Icon = "Url", ToolTip = "Comment", Command = new DelegateCommand<object>(x => Func(x)) });
		/// </example>
		abstract protected void DefItemCommands(CommunicationItemViewModel item);

		/// <summary>
		/// Execute then item has been modify (delete/edit/append) in the CommunicationControl
		/// </summary>
		abstract protected void ModifiedRequest();

		/// <summary>
		/// Execute then item has been selected in the CommunicationControl
		/// </summary>
		abstract protected void SelectedRequest();

		#endregion

	}
}

