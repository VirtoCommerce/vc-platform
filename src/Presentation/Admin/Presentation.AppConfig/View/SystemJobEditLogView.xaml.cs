using System;
using System.Windows.Controls;
using System.Windows.Media;
using VirtoCommerce.Foundation.AppConfig.Model;
using ViewBase = VirtoCommerce.ManagementClient.Core.Infrastructure.ViewBase;

namespace VirtoCommerce.ManagementClient.AppConfig.View
{
    public partial class SystemJobEditLogView : ViewBase
    {
		public SystemJobEditLogView()
        {
            InitializeComponent();
			this.History.LoadingRow += new EventHandler<DataGridRowEventArgs>(dataGrid_LoadingRow);
        }

	    private void dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
	    {
		    // Get the DataRow corresponding to the DataGridRow that is loading.
		    DataGridRow row = e.Row;
		    var log = (SystemJobLogEntry) row.Item;
			if (log.Message != null)
		    {
			    e.Row.Background = new SolidColorBrush(Colors.LightPink);
				e.Row.ToolTip = log.Message;
		    }
	    }


    }
}
