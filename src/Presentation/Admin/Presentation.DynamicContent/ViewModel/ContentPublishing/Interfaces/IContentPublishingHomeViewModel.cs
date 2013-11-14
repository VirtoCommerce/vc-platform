using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.ManagementClient.DynamicContent.View;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces
{
	public interface IContentPublishingHomeViewModel : IViewModel
	{
		DelegateCommand<IList> ItemDuplicateCommand { get; }

		IViewModel SelectedContentPublishingItem { get; set; }
	}
}
