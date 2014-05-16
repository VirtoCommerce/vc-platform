using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Helpers
{

	public class KnowledgeArticleCommunicationControlViewModel : CommunicationControlViewModel
	{

		private readonly KnowledgeBaseDialogViewModel _parentViewModel;

		public KnowledgeArticleCommunicationControlViewModel(IAssetService assetService, IViewModelsFactory<IKnowledgeBaseDialogViewModel> knowledgeBaseGroupVmFactory, string authorId, string authorName, KnowledgeBaseDialogViewModel parentViewModel)
			: base(assetService, knowledgeBaseGroupVmFactory, authorId, authorName)
		{
			_parentViewModel = parentViewModel;
			DefToolBarCommands();
		}

		/// <summary>
		/// Define available ToolBar operations (commands)
		/// </summary>
		private void DefToolBarCommands()
		{
			ToolBarCommmands.Add(new CommunicationItemComands()
			{
				//CommandParametr = CaseChannel.KnowledgeBaseArticle,
				Icon = "/VirtoCommerce.ManagementClient.Customers;component/Resources/images/note.png",
				ToolTip = "Add article".Localize(),
				Header = "ARTICLE".Localize(),
				Command = new DelegateCommand<object>(AppendNewItem)
			});
			OnPropertyChanged("HasAnyToolBarCommands");
		}

		/// <summary>
		/// Create and append CommunicationItem by given item Channels 
		/// </summary>
		/// <param name="param">CaseChannel of new CommunicationItem</param>
		private void AppendNewItem(object param)
		{
			//if (param != null && param is CaseChannel)
			//{
			//    CommunicationItemViewModel item = null;

			//    switch ((CaseChannel)param)
			//    {
			//        case CaseChannel.KnowledgeBaseArticle:
			//            item = new CommunicationItemKnowledgeBaseArticleViewModel();
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
		/// Define available operations for given communication item
		/// Can add predefine such as AviableSelect, AviableEdit, AviableDelete.
		/// Can add any command (see example)
		/// </summary>
		/// <param name="item">CommunicationItem для которого надо определить доступные комманды</param>
		/// <example>
		/// item.ItemCommands.Add(new CommunicationItemComands() { Icon = "Url", ToolTip = "Comment", Command = new DelegateCommand<object>(x => Func(x)) });
		/// </example>
		protected override void DefItemCommands(CommunicationItemViewModel item)
		{
			AviableSelect(item);
			AviableEdit(item);
			AviableDelete(item);
		}

		protected override void SelectedRequest()
		{
			_parentViewModel.IsValid = true;
		}

		protected override void ModifiedRequest()
		{
			IsModified = true;
		}
	}
}
