using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Implementations
{
	public class OrderContactViewModel : ViewModelBase, IOrderContactViewModel
	{
		#region Constructor

		public OrderContactViewModel(string customerId, string fullName)
		{
			CustomerId = customerId;
			FullName = fullName;
		}

		#endregion

		#region IOrderContactViewModel Members

		private string _customerId;
		public string CustomerId
		{
			get { return _customerId; }
			set
			{
				_customerId = value;
				OnPropertyChanged();
			}
		}

		private string _fullName;
		public string FullName
		{
			get { return _fullName; }
			set
			{
				_fullName = value;
				OnPropertyChanged();
			}
		}

		public bool Validate()
		{
			return !String.IsNullOrEmpty(FullName);
		}

		#endregion
	}
}
