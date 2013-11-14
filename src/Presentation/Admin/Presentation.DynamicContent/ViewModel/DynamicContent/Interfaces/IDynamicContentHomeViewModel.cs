using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.ManagementClient.DynamicContent.View;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using System.Collections;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Interfaces
{
	public interface IDynamicContentHomeViewModel : IViewModel
	{
		InteractionRequest<Confirmation> CommonWizardDialogRequest { get; }
		InteractionRequest<Confirmation> CommonConfirmRequest { get; }

		DelegateCommand AddItemCommand { get; }
		DelegateCommand<IList> DeleteItemCommand { get; }
		DelegateCommand<IList> DuplicateItemCommand { get; }

		ICollectionView SelectedDynamicContentItems { get; }
		IViewModel SelectedDynamicContentItem { get; set; }
	}
}
