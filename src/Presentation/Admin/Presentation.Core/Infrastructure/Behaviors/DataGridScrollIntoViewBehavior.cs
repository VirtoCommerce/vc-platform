using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
	public class DataGridScrollIntoViewBehavior : Behavior<DataGrid>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
		}
		void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is DataGrid)
			{
				var grid = (sender as DataGrid);
				var selected = grid.SelectedItem;
				if (selected != null)
				{
					grid.Dispatcher.BeginInvoke(new Action(delegate
						{
							grid.UpdateLayout();
							grid.ScrollIntoView(selected, null);
						}));
				}
			}
		}
		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.SelectionChanged -= new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
		}
	}
}