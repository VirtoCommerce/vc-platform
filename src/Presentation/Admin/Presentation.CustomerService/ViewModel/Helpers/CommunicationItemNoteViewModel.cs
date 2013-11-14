using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers
{
	public class CommunicationItemNoteViewModel : CommunicationItemViewModel
	{
		public CommunicationItemNoteViewModel()
			: base()
		{
			this.Type = CommunicationItemType.Note;
		}
	}
}
