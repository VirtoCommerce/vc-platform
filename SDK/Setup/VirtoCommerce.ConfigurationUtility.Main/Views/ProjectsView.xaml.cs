using System.Windows.Controls;

namespace VirtoCommerce.ConfigurationUtility.Main.Views
{
	public partial class ProjectsView
	{
		public ProjectsView()
		{
			InitializeComponent();
		}

		private void SelectedProjectMenuButton_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			e.Handled = true;
		}
	}
}
