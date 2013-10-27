using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	/// <summary>
	/// Interaction logic for VirtualFolderItemList.xaml
	/// </summary>
	public partial class VirtualFolderItemList : ListView
	{
		#region Private Fields

		private ScrollViewer scrollViewer;
		private double verticalOffset = 0.0;

		#endregion

		#region Constructors

		public VirtualFolderItemList()
		{
			this.Loaded += VirtualFolderItemList_Loaded;
			this.Unloaded += VirtualFolderItemList_Unloaded;

			InitializeComponent();
		}

		#endregion

		#region Handlers

		void VirtualFolderItemList_Loaded(object sender, RoutedEventArgs e)
		{
			if (scrollViewer == null)
				scrollViewer = GetScrollViewer(this);

			if (scrollViewer == null)
				return;

			scrollViewer.ScrollToVerticalOffset(verticalOffset * scrollViewer.ActualHeight);
		}

		void VirtualFolderItemList_Unloaded(object sender, RoutedEventArgs e)
		{
			if (scrollViewer == null)
				scrollViewer = GetScrollViewer(this);

			if (scrollViewer == null)
				return;

			if (!Double.IsNaN(scrollViewer.ActualHeight) && scrollViewer.ActualHeight > Double.Epsilon)
				verticalOffset = scrollViewer.VerticalOffset / scrollViewer.ActualHeight;
		}

		#endregion

		#region Help Methods

		public static ScrollViewer GetScrollViewer(DependencyObject o)
		{
			// Return the DependencyObject if it is a ScrollViewer
			if (o is ScrollViewer)
				return o as ScrollViewer;

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
			{
				var child = VisualTreeHelper.GetChild(o, i);

				var result = GetScrollViewer(child);
				if (result == null)
				{
					continue;
				}
				else
				{
					return result;
				}
			}
			return null;
		}

		#endregion
	}
}
