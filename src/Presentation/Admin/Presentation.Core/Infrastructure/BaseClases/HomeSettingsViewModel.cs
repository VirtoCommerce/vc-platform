using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	/// <summary>
	/// Base class for Setting HomeViewModel.
	/// Contains collection of presentation Items.
	/// Implements Refresh commands.
	/// Implement ISupportDelayInitialization (Load data when View is shown )
	/// </summary>
	/// <typeparam name="T">Class of HomeView list presentation item</typeparam>
	public abstract class HomeSettingsViewModel<T> : ViewModelBase, ISupportDelayInitialization, IHomeSettingsViewModel
	{
		protected readonly IFactory EntityFactory;

		protected HomeSettingsViewModel(IFactory entityFactory)
		{
			EntityFactory = entityFactory;
			RefreshItemListCommand = new DelegateCommand(RaiseRefreshCommand);
		}

		#region IHomeSettingsViewModel

		/// <summary>
		/// Collection of HomeView Items
		/// </summary>
		private ObservableCollection<T> _items;
		public ObservableCollection<T> Items
		{
			get
			{
				return _items;
			}
			private set
			{
				_items = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Refresh list command (Reload data from DB)
		/// </summary>
		public DelegateCommand RefreshItemListCommand { get; private set; }

		/// <summary>
		/// Refresh commands state
		/// </summary>
		public abstract void RaiseCanExecuteChanged();

		#endregion

		#region ISupportDelayInitialization

		/// <summary>
		/// Rise LoadData() each time then view showed
		/// Works in not UI thread
		/// Shows Load animation
		/// </summary>
		public void InitializeForOpen()
		{
			OnUIThread(() => { ShowLoadingAnimation = true; });
			var items = LoadData();
			OnUIThread(() =>
				{
					if (items is List<T>)
					{
						Items = new ObservableCollection<T>(items as List<T>);
					}
					ShowLoadingAnimation = false;
				});
			RaiseCanExecuteChanged();
		}

		#endregion


		private void RaiseRefreshCommand()
		{
			Task.Run(() => InitializeForOpen());
		}

		/// <summary>
		/// Load items from DB 
		/// </summary>
		/// <returns>List of loaded items</returns>
		protected abstract object LoadData();

		/// <summary>
		/// Refresh Item after it was edited in DetailView
		/// </summary>
		/// <param name="item"></param>
		public abstract void RefreshItem(object item);

	}
}
