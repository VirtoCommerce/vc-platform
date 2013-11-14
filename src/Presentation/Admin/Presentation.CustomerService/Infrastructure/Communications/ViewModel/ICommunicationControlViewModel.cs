using System.Collections.Generic;


namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications
{
	public interface ICommunicationControlViewModel
	{
		List<CommunicationItemViewModel> Items { get; }
		List<CommunicationItemComands> ToolBarCommmands { get; }
		CommunicationItemViewModel SelectedCommunicationItem { get; }

        string NewBody { get; }
		bool IsInitialized { get; set; }
		bool HasOneValidItem { get; }
		bool HasAnyToolBarCommands { get; }

		void SetAuthor(string authorId, string authorName);
		void ClearAllCommunicationItems();
		void AppendCommunucationItems(List<CommunicationItemViewModel> communicationItems);
	}
}
