using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class ItemAssetViewModel : ViewModelBase, IItemAssetViewModel
	{
		private readonly IViewModelsFactory<IPickAssetViewModel> _vmFactory;

		public ItemAssetViewModel(IViewModelsFactory<IPickAssetViewModel> vmFactory, ItemAsset item)
		{
			_vmFactory = vmFactory;
			InnerItem = item;

			AssetPickCommand = new DelegateCommand(RaiseItemPickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public DelegateCommand AssetPickCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		#region IPropertyAttributeViewModel

		public ItemAsset InnerItem { get; private set; }

		public bool Validate()
		{
			return InnerItem.Validate();
		}

		#endregion

		#region ViewModelBase overrides
		public override string DisplayName
		{
			get
			{
				return InnerItem.AssetId;
			}
		}
		#endregion


		private void RaiseItemPickInteractionRequest()
		{
			var itemVM = _vmFactory.GetViewModelInstance();
			itemVM.AssetPickMode = true;
			itemVM.RootItemId = null;

			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(() => itemVM.SelectedAsset != null) { Content = itemVM, Title = "Select an asset".Localize() },
				(x) =>
				{
					if (x.Confirmed)
					{
						var file = itemVM.SelectedAsset;
						InnerItem.AssetId = file.FolderItemId;
						// repository doesn't fill ContentType correctly
						// InnerItem.AssetType = file.ContentType;
						OnPropertyChanged("DisplayName");
					}
				});
		}
	}
}
