using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Implementations;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers
{
	public class CustomerCaseControlViewModel : CommunicationControlViewModel
	{
		private readonly CustomersDetailViewModel _parentViewModel;
		private readonly IAuthenticationContext _authContext;
		public InteractionRequest<ConditionalConfirmation> CommonConfirmRequest { get; private set; }

		public CustomerCaseControlViewModel(IAssetService assetService, IViewModelsFactory<IKnowledgeBaseDialogViewModel> knowledgeBaseGroupVmFactory, IAuthenticationContext authContext, string authorId, string authorName, CustomersDetailViewModel parentViewModel)
			: base(assetService, knowledgeBaseGroupVmFactory, authorId, authorName)
		{
			_parentViewModel = parentViewModel;
			_authContext = authContext;
			DefToolBarCommands();
			CommonConfirmRequest = new InteractionRequest<ConditionalConfirmation>();
			IsReadOnly = !_authContext.CheckPermission(PredefinedPermissions.CustomersCreateCustomer);

		}

		private void DefToolBarCommands()
		{
			if (_authContext.CheckPermission(PredefinedPermissions.CustomersCreateCustomer))
			{
				ToolBarCommmands.Add(new CommunicationItemComands()
					{
						CommandParametr = CommunicationItemType.Note,
						Header = "Internal note".Localize(),
						ToolTip = "Add note".Localize(),
						Command = new DelegateCommand<object>(RaiseNoteAddInteractionRequest, (x) => _authContext.CheckPermission(PredefinedPermissions.CustomersCreateCustomer)),
						IsActive = true,
						CommandGroupName = "ContactCommandGroup"
					});
				OnPropertyChanged("HasAnyToolBarCommands");
			}

		}


		private void RaiseNoteAddInteractionRequest(object itemBody)
		{
			if (itemBody == null || string.IsNullOrEmpty(itemBody.ToString()))
				return;

			var item = new CommunicationItemNoteViewModel();
			item.Body = itemBody.ToString();
			AppendCommunicationItem(item, true, false);
			NewBody = string.Empty;
		}

		/// <summary>
		/// Добавляет новый CommunicationItem в зависити от полученного значения в параметре.
		/// </summary>
		/// <param name="param">CaseChannel создаваемого CommunicationItem</param>
		private void AppendNewItem(object param)
		{
			//if (param != null && param is CaseChannel)
			//{
			//    CommunicationItemViewModel item = null;

			//    switch ((CaseChannel)param)
			//    {
			//        case CaseChannel.Note:
			//            item = new CommunicationItemNoteViewModel();
			//            break;
			//    }
			//    if (item != null)
			//    {
			//        item.State = CommunicationItemState.Appended;
			//        AppendCommunicationItem(item, true, true);
			//    }
			//}
		}

		/// <summary>
		/// Определяет доступные комманды для полученного в параметре Item
		/// </summary>
		/// <param name="item">CommunicationItem для которого надо определить доступные комманды</param>
		/// <example>
		/// item.ItemCommands.Add(new CommunicationItemComands() { Icon = "Url", ToolTip = "Comment", Command = new DelegateCommand<object>(x => Func(x)) });
		/// </example>
		protected override void DefItemCommands(CommunicationItemViewModel item)
		{
			AviableEdit(item);
			AviablePost(item);
			if (item.State == CommunicationItemState.Appended)
			{
				AviableDelete(item);
			}
			AviableKnowledgeBase(item, "testBase");
		}

		protected override void SelectedRequest()
		{
		}

		protected override void ModifiedRequest()
		{
			if (_parentViewModel != null)
			{
				_parentViewModel.IsModified = true;
			}
		}

	}
}
