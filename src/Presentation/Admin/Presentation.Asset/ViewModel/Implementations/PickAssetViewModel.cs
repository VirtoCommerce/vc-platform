using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Waf.Applications.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Asset.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Asset.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel.Implementations
{
	public class PickAssetViewModel : ViewModelBase, IPickAssetViewModel
	{
		#region Dependencies

		private readonly IAssetService _assetRepository;
		private IFileDialogService fileDialogService;
		private readonly IViewModelsFactory<IInputNameDialogViewModel> _inputNameVmFactory;
		public InteractionRequest<ConditionalConfirmation> InputNameDialogRequest { get; private set; }
		public InteractionRequest<Notification> CommonNotifyRequest { get; private set; }
		#endregion

		#region ctor

		public PickAssetViewModel(IAssetService assetRepository,
			IViewModelsFactory<IInputNameDialogViewModel> inputNameVmFactory)
		{
			_assetRepository = assetRepository;
			_inputNameVmFactory = inputNameVmFactory;

			AddressBarItems = new ObservableCollection<AssetEntitySearchViewModelBase>();
			SelectedFolderItems = new ObservableCollection<AssetEntitySearchViewModelBase>();

			CommonNotifyRequest = new InteractionRequest<Notification>();

			OpenItemCommand = new DelegateCommand<object>(RaiseOpenItemRequest);
			RefreshCommand = new DelegateCommand(LoadItems);
			UploadCommand = new DelegateCommand(RaiseUploadRequest, () => ParentItem.Type == AssetType.Container || ParentItem.Type == AssetType.Folder);
			CreateFolderCommand = new DelegateCommand(RaiseCreateFolderRequest);
			RenameCommand = new DelegateCommand(RaiseRenameRequest);
			DeleteCommand = new DelegateCommand(RaiseDeleteRequest);
			ParentItem = new RootSearchViewModel(null);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();

			InputNameDialogRequest = new InteractionRequest<ConditionalConfirmation>();

			AssetPickMode = true;
			RootItemId = null;
		}

		#endregion

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region Delegates

		public DelegateCommand<object> OpenItemCommand { get; private set; }
		public DelegateCommand RefreshCommand { get; private set; }
		public DelegateCommand UploadCommand { get; private set; }
		public DelegateCommand CreateFolderCommand { get; private set; }
		public DelegateCommand DeleteCommand { get; private set; }
		public DelegateCommand RenameCommand { get; private set; }

		#endregion

		#region Properties

		public string RootItemId
		{
			get { return _rootItemId; }
			set
			{
				_rootItemId = value;
				ParentItem = new FolderSearchViewModel(new Folder { FolderId = value, Name = value }, null);
			}
		}

		private AssetEntitySearchViewModelBase _parentItem;
		public AssetEntitySearchViewModelBase ParentItem
		{
			get
			{
				return _parentItem;
			}
			set
			{
				_parentItem = value;
				LoadItems();
				UploadCommand.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<AssetEntitySearchViewModelBase> SelectedFolderItems { get; private set; }
		public ObservableCollection<AssetEntitySearchViewModelBase> AddressBarItems { get; private set; }

		public bool IsItemSelected
		{
			get { return _selectedItem != null; }
		}

		public bool AssetPickMode
		{
			get { return _assetPickMode; }
			set
			{
				_assetPickMode = value;
				OnPropertyChanged();
			}
		}

		private object _selectedItem;
		public object ItemListSelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				if (value is IFileSearchViewModel)
				{
					var itemVM = value as IFileSearchViewModel;
					SelectedAsset = itemVM.InnerItem;
				}
				else
					SelectedAsset = null;

				OnPropertyChanged("IsItemSelected");
			}
		}

		public bool IsValid
		{
			get { return Validate(); }
		}

		#endregion

		#region IPickAssetViewModel

		private FolderItem _selectedAsset;
		public FolderItem SelectedAsset
		{
			get { return _selectedAsset; }
			private set
			{
				if (_selectedAsset != value)
				{
					_selectedAsset = value;
					OnPropertyChanged();
					OnPropertyChanged("IsValid");
					UpdateImagePreview();
				}
			}
		}

		object _SelectedAssetImageSource;
		public object SelectedAssetImageSource
		{
			get { return _SelectedAssetImageSource; }
			set { _SelectedAssetImageSource = value; OnPropertyChanged(); }
		}

		public bool Validate()
		{
			return !AssetPickMode || SelectedAsset != null;
		}

		#endregion

		public static BitmapImage ImageFromBuffer(Byte[] bytes)
		{
			BitmapImage result = null;

			if (bytes != null && bytes.Length > 1)
			{
				try
				{
					var stream = new MemoryStream(bytes);
					stream.Seek(0, SeekOrigin.Begin);
					var image = new BitmapImage();
					image.BeginInit();
					image.StreamSource = stream;
					image.DecodePixelWidth = 100;
					image.EndInit();
					result = image;
				}
				catch (Exception)
				{
				}
			}

			return result;
		}

		#region private members
		private void UpdateAddressBar()
		{
			AddressBarItems.Clear();
			var parent = ParentItem;
			while (parent != null)
			{
				AddressBarItems.Insert(0, parent);
				parent = parent.Parent;
			}
		}

		private void LoadItems()
		{
			ShowLoadingAnimation = true;
			var items = new List<AssetEntitySearchViewModelBase>();
			var worker = new BackgroundWorker();
			worker.DoWork += (o, ea) =>
			{
				if (!string.IsNullOrEmpty(ParentItem.InnerItemID) && ParentItem.InnerItemID != RootItemId)
				{
					items.Add(new RootSearchViewModel(ParentItem.Parent));
				}


				switch (ParentItem.Type)
				{
					case AssetType.Folder:
					case AssetType.Container:
						items.AddRange(
							_assetRepository.GetChildrenFolders(ParentItem.InnerItemID)
								.Select(x => new FolderSearchViewModel(x, ParentItem)));
						if (ParentItem.InnerItemID != null)
						{
							items.AddRange(
								_assetRepository.GetChildrenFolderItems(ParentItem.InnerItemID)
									.Select(x => new FileSearchViewModel(x)));
						}
						break;
					case AssetType.Parent:
						items.AddRange(
							_assetRepository.GetChildrenFolders(ParentItem.InnerItemID)
								.Select(x => new FolderSearchViewModel(x, ParentItem)));
						break;
				}

				OnUIThread(() =>
				{
					SelectedFolderItems.SetItems(items);

					UpdateAddressBar();
				});
			};

			worker.RunWorkerCompleted += (o, ea) =>
			{
				ShowLoadingAnimation = false;
			};

			worker.RunWorkerAsync();
		}

		private async void UpdateImagePreview()
		{
			if (_selectedAsset != null && _selectedAsset.SmallData == null)
			{
				try
				{
					_selectedAsset.SmallData = await Task.Run(() => _assetRepository.GetImagePreview(_selectedAsset.FolderItemId));
				}
				catch (Exception ex)
				{
					SelectedAssetImageSource = DependencyProperty.UnsetValue;

					if (ex.InnerException != null && ex.InnerException.Message.StartsWith("The maximum array length quota"))
					{
						throw new Exception(string.Format("The data received from IAssetRepository.GetImagePreview exceeded maximum size.\nContact your support personnel to increase the quota if needed."), ex.InnerException);
					}
					else
					{
						throw;
					}
				}
			}

			SelectedAssetImageSource = _selectedAsset == null ? DependencyProperty.UnsetValue : ImageFromBuffer(_selectedAsset.SmallData);
		}

		private void RaiseOpenItemRequest(object obj)
		{
			if (obj is FolderSearchViewModel)
			{
				ParentItem = (AssetEntitySearchViewModelBase)obj;
			}
			else if (obj is RootSearchViewModel)
			{
				ParentItem = ((RootSearchViewModel)obj).Parent ?? (AssetEntitySearchViewModelBase)obj;
			}
		}

		private void RaiseCreateFolderRequest()
		{
			var inputVm = _inputNameVmFactory.GetViewModelInstance();

			var confirmation = new ConditionalConfirmation { Title = "Enter new folder name".Localize(), Content = inputVm };

			InputNameDialogRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					var inputNameDialogViewModel = x.Content as IInputNameDialogViewModel;
					if (inputNameDialogViewModel != null)
					{
						var newFolderName = inputNameDialogViewModel.InputText;
						_assetRepository.CreateFolder(newFolderName, ParentItem.InnerItemID);
						LoadItems();
					}
				}
			});
		}

		private void RaiseRenameRequest()
		{
			if (ItemListSelectedItem is FileSearchViewModel ||
				ItemListSelectedItem is FolderSearchViewModel)
			{
				var item = (AssetEntitySearchViewModelBase)ItemListSelectedItem;
				var title = ItemListSelectedItem is FileSearchViewModel
				   ? "Enter new file name".Localize()
				   : "Enter new folder name".Localize();
				var inputVm = _inputNameVmFactory.GetViewModelInstance();
				inputVm.InputText = item.DisplayName;
				var confirmation = new ConditionalConfirmation { Title = title, Content = inputVm };

				InputNameDialogRequest.Raise(confirmation, (x) =>
				{
					if (x.Confirmed)
					{
						var inputNameDialogViewModel = x.Content as IInputNameDialogViewModel;
						if (inputNameDialogViewModel != null)
						{
							var newFolderName = inputNameDialogViewModel.InputText;
							_assetRepository.Rename(item.InnerItemID, newFolderName);
							LoadItems();
						}
					}
				});
			}
		}

		private void RaiseDeleteRequest()
		{
			if (ItemListSelectedItem is FileSearchViewModel ||
				ItemListSelectedItem is FolderSearchViewModel)
			{
				var item = (AssetEntitySearchViewModelBase)ItemListSelectedItem;
				var message = ItemListSelectedItem is FileSearchViewModel
					? "Are you sure you want to delete file '{0}'?".Localize()
					: "Are you sure you want to delete folder '{0}' and all its files and subfolders?".Localize();

				var confirmation = new ConditionalConfirmation
				{
					Content = string.Format(message, item.DisplayName),
					Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
				};

				CommonConfirmRequest.Raise(confirmation, (x) =>
				{
					if (x.Confirmed)
					{
						_assetRepository.Delete(item.InnerItemID);
						LoadItems();
					}
				});
			}
		}

		private void RaiseUploadRequest()
		{
			if (ParentItem.Parent == null)
			{
				CommonNotifyRequest.Raise(new Notification
				{
					Content = "Can not upload files to the root. Please select a folder first.".Localize(),
					Title = "Error".Localize(null, LocalizationScope.DefaultCategory)
				});
				return;
			}

			IEnumerable<FileType> fileTypes = new[] {
                new FileType("all files".Localize(), ".*"),
                new FileType("jpg image".Localize(), ".jpg"),
                new FileType("bmp image".Localize(), ".bmp"),
                new FileType("png image".Localize(), ".png"),
                new FileType("Report".Localize(), ".rld"),
                new FileType("Report".Localize(), ".rldc") 
            };

			if (fileDialogService == null)
				fileDialogService = new System.Waf.VirtoCommerce.ManagementClient.Services.FileDialogService();

			var result = fileDialogService.ShowOpenFileDialog(this, fileTypes);
			if (result.IsValid)
			{
				var delimiter = !string.IsNullOrEmpty(ParentItem.InnerItemID) && !ParentItem.InnerItemID.EndsWith(NamePathDelimiter) ? NamePathDelimiter : string.Empty;
				// construct new FolderItemId
				var fileInfo = new FileInfo(result.FileName);
				var fileName = string.Format("{0}{1}{2}", ParentItem.InnerItemID, delimiter, fileInfo.Name);

				var canUpload = true;
				var fileExists = SelectedFolderItems.OfType<IFileSearchViewModel>().Any(x => x.InnerItem.FolderItemId.EndsWith(NamePathDelimiter + fileInfo.Name, StringComparison.OrdinalIgnoreCase));
				if (fileExists)
				{
					CommonConfirmRequest.Raise(new ConditionalConfirmation
					{
						Title = "Upload file".Localize(),
						Content = string.Format("There is already a file with the same name in this location.\nDo you want to overwrite and replace the existing file '{0}'?".Localize(), fileInfo.Name)
					}, (x) =>
					{
						canUpload = x.Confirmed;
					});
				}

				if (canUpload)
				{
					ShowLoadingAnimation = true;

					var worker = new BackgroundWorker();
					worker.DoWork += (o, ea) =>
					{
						var id = o.GetHashCode().ToString();
						var item = new StatusMessage { ShortText = "File upload in progress".Localize(), StatusMessageId = id };
						EventSystem.Publish(item);

						using (var info = new UploadStreamInfo())
						using (var fileStream = new FileStream(result.FileName, FileMode.Open, FileAccess.Read))
						{
							info.FileName = fileName;
							info.FileByteStream = fileStream;
							info.Length = fileStream.Length;
							_assetRepository.Upload(info);
						}
					};

					worker.RunWorkerCompleted += (o, ea) =>
					{
						ShowLoadingAnimation = false;

						var item = new StatusMessage
						{
							StatusMessageId = o.GetHashCode().ToString()
						};

						if (ea.Cancelled)
						{
							item.ShortText = "File upload was canceled!".Localize();
							item.State = StatusMessageState.Warning;
						}
						else if (ea.Error != null)
						{
							item.ShortText = string.Format("Failed to upload file: {0}".Localize(), ea.Error.Message);
							item.Details = ea.Error.ToString();
							item.State = StatusMessageState.Error;
						}
						else
						{
							item.ShortText = "File uploaded".Localize();
							item.State = StatusMessageState.Success;

							RefreshCommand.Execute();
						}

						EventSystem.Publish(item);
					};

					worker.RunWorkerAsync();
				}
			}
		}

		private string _namePathDelimiter;
		private string _rootItemId;
		private bool _assetPickMode;

		private string NamePathDelimiter
		{
			get
			{
				if (_namePathDelimiter == null)
				{
					var assetItem = SelectedFolderItems.OfType<IFileSearchViewModel>().FirstOrDefault();
					if (assetItem != null)
					{
						var count1 = assetItem.InnerItem.FolderItemId.Count(x => x == '/');
						var count2 = assetItem.InnerItem.FolderItemId.Count(x => x == '\\');
						_namePathDelimiter = "" + (count1 > count2 ? '/' : '\\');
					}
					else
					{
						// default delimiter
						_namePathDelimiter = "/";
					}
				}

				return _namePathDelimiter;
			}
		}

		#endregion
	}
}
