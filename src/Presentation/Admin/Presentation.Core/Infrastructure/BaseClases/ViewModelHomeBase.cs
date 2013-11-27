using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public class ViewModelHomeBase<T> : ViewModelBase, IViewModelHomeBase
	{
		protected ViewModelHomeBase()
		{
			RefreshItemsCommand = new DelegateCommand(RaiseRefreshCommand);
			SearchItemsCommand = new DelegateCommand(RaiseSearchCommand);
		}

		#region IViewModelHomeBase

		private ICollectionView _listItemsSource;
		public ICollectionView ListItemsSource
		{
			get { return _listItemsSource; }
			set
			{
				_listItemsSource = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Refresh list command (Reload data from DB)
		/// </summary>
		public DelegateCommand RefreshItemsCommand { get; protected set; }

		public DelegateCommand SearchItemsCommand { get; private set; }

		#endregion

		#region Public Methods

		public void Refresh()
		{
			OnUIThread(() => ListItemsSource.Refresh());
		}

		#endregion

		#region Protected metods

		protected virtual void RaiseRefreshCommand()
		{
			Refresh();
		}

		protected virtual void RaiseSearchCommand()
		{
			Refresh();
		}

		protected override IEnumerable<ActionBinding> GetActionBindings()
		{
			yield return new ActionBinding { Command = RefreshItemsCommand, Name = GestureActionName.refresh };
			yield return new ActionBinding { Command = SearchItemsCommand, Name = GestureActionName.find };
		}

		#endregion
	}
}
