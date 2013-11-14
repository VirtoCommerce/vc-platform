using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel
{
	public class EditQuantityViewModel : ViewModelBase, IEditQuantityViewModel
	{
		public int NewQuantity { get; set; }
		public List<string> AvailableReasons { get; private set; }
		public string SelectedReason { get; set; }

		public List<string> AvailableActions { get; private set; }
		public string SelectedAction { get; set; }

		public EditQuantityViewModel()
		{
			Initialize();
		}

		#region private members
		private void Initialize()
		{
			AvailableReasons = new List<string> {"Theft", "Damaged", "Transfer"};

			SelectedReason = AvailableReasons[0];

			AvailableActions = new List<string> {"Add", "Remove"};

			SelectedAction = AvailableActions[0];
		}
		#endregion

	}
}
