using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Import.Model;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Implementations
{
	public class ImportJobRunViewModel : ViewModelBase, IImportJobRunViewModel
	{
		#region Dependencies
		private readonly IViewModelsFactory<IPickAssetViewModel> _assetVmFactory;
		#endregion

		public ImportJobRunViewModel(IViewModelsFactory<IPickAssetViewModel> vmFactory, ImportEntity jobEntity)
		{
			_assetVmFactory = vmFactory;

			InnerItem = jobEntity;
			OnPropertyChanged("InnerItem");

			FilePickCommand = new DelegateCommand(RaiseFilePickInteractionRequest);
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public ImportEntity InnerItem { get; set; }

		public DelegateCommand FilePickCommand { get; private set; }
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

		public bool Validate()
		{
			return !string.IsNullOrEmpty(InnerItem.SourceFile);
		}

		// function almost duplicated in CreateItemViewModel
		private void RaiseFilePickInteractionRequest()
		{
			var itemVM = _assetVmFactory.GetViewModelInstance();
			itemVM.AssetPickMode = true;
			itemVM.RootItemId = null;

			CommonConfirmRequest.Raise(
				new ConditionalConfirmation(itemVM.Validate) { Content = itemVM, Title = "Select file".Localize(null, LocalizationScope.DefaultCategory) },
				(x) =>
				{
					if (x.Confirmed)
					{
						InnerItem.SourceFile = itemVM.SelectedAsset.FolderItemId;
						InnerItem.Bytes = itemVM.SelectedAsset.SmallData;
						OnPropertyChanged("InnerItem");
						Validate();
					}
				});
		}
	}
}
