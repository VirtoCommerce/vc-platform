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

	public class CaseCommunicationControlViewModel : CommunicationControlViewModel
	{

		private readonly CustomersDetailViewModel _parentViewModel;
		private readonly IAuthenticationContext _authContext;
		#region Properties

		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		#endregion

		public CaseCommunicationControlViewModel(IAssetService assetService, IViewModelsFactory<IKnowledgeBaseDialogViewModel> knowledgeBaseGroupVmFactory, IAuthenticationContext authContext, string authorId, string authorName, CustomersDetailViewModel parentViewModel)
			: base(assetService, knowledgeBaseGroupVmFactory, authorId, authorName)
		{
			_parentViewModel = parentViewModel;
			_authContext = authContext;
			CommonConfirmRequest = new InteractionRequest<Confirmation>();
			IsReadOnly = !_authContext.CheckPermission(PredefinedPermissions.CustomersAddCaseComments);
			DefToolBarCommands();
		}

		private void DefToolBarCommands()
		{

			if (_authContext.CheckPermission(PredefinedPermissions.CustomersAddCaseComments))
			{
				ToolBarCommmands.Add(new CommunicationItemComands()
					{
						CommandParametr = CommunicationItemType.PublicReply,
						Header = "Public reply".Localize(),
						ToolTip = "Add public reply".Localize(),
						Command = new DelegateCommand<object>(RaisePublicReplyAddInteractionRequest, (x) => _authContext.CheckPermission(PredefinedPermissions.CustomersAddCaseComments)),
						IsActive = true,
						CommandGroupName = "CaseCommandGroup"
					});

				ToolBarCommmands.Add(new CommunicationItemComands()
					{
						CommandParametr = CommunicationItemType.Note,
						Header = "Internal note".Localize(),
						//Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/note.png",
						ToolTip = "Add note".Localize(),
						Command = new DelegateCommand<object>(RaiseNoteAddInteractionRequest, (x) => _authContext.CheckPermission(PredefinedPermissions.CustomersAddCaseComments)),
						CommandGroupName = "CaseCommandGroup"
					});

			}
			OnPropertyChanged("HasAnyToolBarCommands");
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
			//        case CaseChannel.Email:
			//            item = new CommunicationItemEmailViewModel();
			//            break;
			//        case CaseChannel.Note:
			//            item = new CommunicationItemNoteViewModel();
			//            break;
			//        case CaseChannel.InboundCall:
			//            item = new CommunicationItemInboundCallViewModel();
			//            break;
			//        case CaseChannel.OutboundCall:
			//            item = new CommunicationItemOutboundCallViewModel();
			//            break;
			//    }
			//    if (item != null)
			//    {
			//        item.State = CommunicationItemState.Appended;
			//        AppendCommunicationItem(item, true, true);
			//    }
			//}
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

		private void RaisePublicReplyAddInteractionRequest(object itemBody)
		{
			if (itemBody == null || string.IsNullOrEmpty(itemBody.ToString()))
				return;

			var item = new CommunicationItemPublicReplyViewModel();
			item.Body = itemBody.ToString();
			AppendCommunicationItem(item, true, false);
			NewBody = string.Empty;
		}

		private bool RaiseNoteEditInteractionRequest(CommunicationItemViewModel itemVM, string title)
		{
			bool result = false;

			var confirmation = new ConditionalConfirmation(() => !string.IsNullOrEmpty(itemVM.Body));
			confirmation.Title = title;
			confirmation.Content = itemVM;

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
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
			if (item is CommunicationItemNoteViewModel)
				AviableEdit(item);
			AviablePost(item);
			if (item.State == CommunicationItemState.Appended)
			{
				AviableDelete(item);
			}
			//AviableKnowledgeBase(item, "testBase");
		}

		protected override void SelectedRequest()
		{
		}

		protected override void ModifiedRequest()
		{
			if (_parentViewModel != null)
			{
				_parentViewModel.IsModified = true;
				OnPropertyChanged("HasOneValidItem");
			}
		}

	}
}
