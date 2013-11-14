using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VirtoCommerce.Foundation.Customers.Model;

using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Helpers
{
	public class CaseFilterTypeViewModel : ViewModelBase
	{

		#region Properties

		public CaseFilterType Model { get; set; }
		public string ShortDescription { get; set; }
		public string FullDescription { get; set; }
		public bool IsActive { get; set; }

		private int _count;
		public int Count
		{
			get { return _count; }
			set
			{
				_count = value;
				OnPropertyChanged("Count");
			}
		}

		#endregion

		#region Public Methods

		//public void RefreshCount()
		//{
		//	OnPropertyChanged("Count");
		//}

		#endregion


		#region Equality Overrides

		public override bool Equals(object obj)
		{
			var caseFilterType = obj as CaseFilterTypeViewModel;

			if (caseFilterType == null)
			{
				return false;
			}

			return Model == caseFilterType.Model;
		}

		public override int GetHashCode()
		{
			return Model.GetHashCode();
		}

		#endregion
	}
}
