using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Waf.Applications.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications.Model;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications
{

	public abstract class CommunicationItemViewModel : ViewModelBase
	{

		private readonly IFileDialogService fileDialogService;
		//private string FileSystemPath;


		#region Commands

		public InteractionRequest<Confirmation> ConfirmRequest { get; private set; }

		private void InitCommands()
		{
			DelAttachmentCommand = new DelegateCommand<object>(x => DelAttachmentRequest(x));
			AddAttachmentCommand = new DelegateCommand(AddAttachmentRequest);
			OpenAttachmentCommand = new DelegateCommand(OpenAttachmentRequest);
			SaveAttachmentCommand = new DelegateCommand(SaveAttachmentRequest);
			ConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public DelegateCommand<object> DelAttachmentCommand { set; get; }
		private void DelAttachmentRequest(object param)
		{
			CommunicationAttachment attachment = param as CommunicationAttachment;
			if (attachment != null && ConfirmRequest != null)
			{
				ConfirmRequest.Raise(
				new ConditionalConfirmation { Content = "Remove?".Localize(), Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory) },
				x =>
				{
					if (x.Confirmed)
					{
						OnUIThread(() =>
						{
							attachment.State = CommunicationItemState.Deleted;
							ModifiedParentViewModel();
							AttacmentsCollection.Refresh();
							State = CommunicationItemState.Modified;
						});
					}
				});
			}
		}

		public DelegateCommand AddAttachmentCommand { set; get; }
		private void AddAttachmentRequest()
		{
			CommunicationAttachment attachment = null;
			IEnumerable<System.Waf.Applications.Services.FileType> fileTypes = new System.Waf.Applications.Services.FileType[] {
                new System.Waf.Applications.Services.FileType("all files", ".*"),
                new System.Waf.Applications.Services.FileType("jpg image", ".jpg"),
                new System.Waf.Applications.Services.FileType("bmp image", ".bmp"),
                new System.Waf.Applications.Services.FileType("png image", ".png")
            };

			FileDialogResult result = fileDialogService.ShowOpenFileDialog(this, fileTypes);
			if (result.IsValid)
			{
				try
				{
					/*					ShowLoadingAnimation = true;

										Action addFolderItemAction = () =>
										{
											ReadFileContent(item, item.FileSystemPath);
											ShowLoadingAnimation = false;
										};
										addFolderItemAction.BeginInvoke(null, null);*/
					FileInfo fInfo = new FileInfo(result.FileName);
					if (fInfo != null)
					{
						attachment = new CommunicationAttachment() { FileUrl = fInfo.Name, Url = result.FileName, State = CommunicationItemState.Appended };
					}
				}
				catch
				{
					ShowLoadingAnimation = false;
					throw;
				}
				if (attachment != null)
				{
					OnUIThread(() =>
					{
						Attachments.Add(attachment);
						AttacmentsCollection.Refresh();
						State = CommunicationItemState.Modified;
					});
				}
			}
		}

		public DelegateCommand OpenAttachmentCommand { set; get; }
		private void OpenAttachmentRequest()
		{
		}

		public DelegateCommand SaveAttachmentCommand { set; get; }
		private void SaveAttachmentRequest()
		{
			IEnumerable<System.Waf.Applications.Services.FileType> fileTypes = new System.Waf.Applications.Services.FileType[] {
                new System.Waf.Applications.Services.FileType("all files", ".*"),
                new System.Waf.Applications.Services.FileType("jpg image", ".jpg"),
                new System.Waf.Applications.Services.FileType("bmp image", ".bmp"),
                new System.Waf.Applications.Services.FileType("png image", ".png")
            };
			FileDialogResult result = fileDialogService.ShowSaveFileDialog(fileTypes);
		}


		#endregion

		public CommunicationItemViewModel()
		{
			Created = DateTime.UtcNow;
			//LastModified = Created;
			ItemCommands = new List<CommunicationItemComands>();
			Attachments = new ObservableCollection<CommunicationAttachment>();
			Attachments.CollectionChanged += (o, e) => ModifiedParentViewModel();
			fileDialogService = new System.Waf.VirtoCommerce.ManagementClient.Services.FileDialogService();

			InitCommands();
		}

		#region Public properties

		private string _id;
		public string Id
		{
			get { return _id; }
			set
			{
				_id = value;
				OnPropertyChanged();
			}
		}


		public CommunicationItemType Type { set; get; }

		private DateTime _created;
		public DateTime Created
		{
			get
			{
				return _created;
			}
			set
			{
				if (_created != value)
				{
					ModifiedParentViewModel();
				}
				_created = value;
				OnPropertyChanged();
			}
		}

		private DateTime? _lastModified;
		public DateTime? LastModified
		{
			get
			{
				return _lastModified;
			}
			set
			{
				if (_lastModified != value)
				{
					ModifiedParentViewModel();
				}

				_lastModified = value;
				OnPropertyChanged();
			}
		}

		private string _icon;
		public string Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				if (_icon != value)
				{
					ModifiedParentViewModel();
				}

				_icon = value;
				OnPropertyChanged();
			}
		}

		private string _title;
		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (_title != value)
				{
					ModifiedParentViewModel();
				}

				_title = value;
				OnPropertyChanged();

			}
		}

		private string _body;
		public string Body
		{
			get
			{
				return _body;
			}
			set
			{
				if (_body != value)
				{
					_body = value;
					ModifiedParentViewModel();
				}


				OnPropertyChanged();
			}
		}

		private string _authorName;
		public string AuthorName
		{
			get
			{
				return _authorName;
			}
			set
			{
				if (_authorName != value)
				{
					ModifiedParentViewModel();
				}

				_authorName = value;
				OnPropertyChanged();
			}
		}

		private string _authorId;
		public string AuthorId
		{
			get
			{
				return _authorId;
			}
			set
			{
				if (_authorId != value)
				{
					ModifiedParentViewModel();
				}

				_authorId = value;
				OnPropertyChanged();
			}
		}


		private string _modifierId;
		public string ModifierId
		{
			get { return _modifierId; }
			set
			{
				if (_modifierId != value)
				{
					ModifiedParentViewModel();
				}
				_modifierId = value;
				OnPropertyChanged();
			}
		}


		private string _modifierName;
		public string ModifierName
		{
			get
			{
				return _modifierName;
			}
			set
			{
				if (_modifierName != value)
				{
					ModifiedParentViewModel();
				}
				_modifierName = value;
				OnPropertyChanged();
			}
		}


		private CommunicationItemState _state;
		public CommunicationItemState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != CommunicationItemState.Appended || value != CommunicationItemState.Modified)
				{
					_state = value;
					OnPropertyChanged();
					RaiseCanExecuteChanged();
				}
			}
		}

		public List<CommunicationItemComands> ItemCommands { set; get; }

		private bool _isEditing;
		public bool IsEditing
		{
			get
			{

				return _isEditing;
			}
			set
			{
				_isEditing = value;
				OnPropertyChanged();
				RaiseCanExecuteChanged();
			}
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get
			{

				return _isSelected;
			}
			set
			{
				_isSelected = value;
				OnPropertyChanged();
				RaiseCanExecuteChanged();
			}
		}

		private ICollectionView _attacmentsCollection;
		public ICollectionView AttacmentsCollection
		{
			get
			{
				if (_attacmentsCollection == null)
				{
					_attacmentsCollection = System.Windows.Data.CollectionViewSource.GetDefaultView(Attachments);
					_attacmentsCollection.Filter = new Predicate<object>((x) => { return (x is CommunicationAttachment) && (x as CommunicationAttachment).State != CommunicationItemState.Deleted; });
				}
				return _attacmentsCollection;
			}
		}

		#endregion

		#region Protected Methods

		protected void ModifiedParentViewModel()
		{
			if (CommunicationItemPropertyChanged != null)
				CommunicationItemPropertyChanged(this, EventArgs.Empty);
		}

		#endregion

		public ObservableCollection<CommunicationAttachment> Attachments { set; get; }

		public List<CommunicationAttachment> GetAttacmentsByState(CommunicationItemState state)
		{
			return Attachments != null ? Attachments.Where(x => x.State == state).ToList() : new List<CommunicationAttachment>();
		}

		// Refresh CanExecute for each  CommunicationItem's command.
		public void RaiseCanExecuteChanged()
		{
			foreach (CommunicationItemComands cmd in ItemCommands)
			{
				cmd.Command.RaiseCanExecuteChanged();
				cmd.IsVisible = cmd.SetVisible != null ? cmd.SetVisible() : true;
			}
		}

		public event EventHandler CommunicationItemPropertyChanged;

	}

}
