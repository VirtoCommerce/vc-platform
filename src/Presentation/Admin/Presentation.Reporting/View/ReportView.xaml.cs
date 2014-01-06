using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Reporting.WinForms;
using VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Reporting.View
{
	public partial class ReportView : UserControl
	{
	    private IReportViewModel viewModel;

	    public ReportView()
	    {
	        this.InitializeComponent();
            this.Loaded += ReportView_Loaded;
	    }

        async void ReportView_Loaded(object sender, RoutedEventArgs e)
        {
            await Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                viewModel = DataContext as IReportViewModel;
                
                if (viewModel != null)
                {
                    _reportViewer.ProcessingMode = ProcessingMode.Local;
                    _reportViewer.LocalReport.LoadReportDefinition(viewModel.ReportDefinition);

                    foreach (DataTable table in viewModel.ReportDataSet.Tables)
                    {
                        var ds = new ReportDataSource(table.TableName, table);
                        _reportViewer.LocalReport.DataSources.Add(ds);
                    }

                    _reportViewer.RefreshReport();
                    
                    _reportViewer.Visible = true;
                }
            }));
        }
	}
}