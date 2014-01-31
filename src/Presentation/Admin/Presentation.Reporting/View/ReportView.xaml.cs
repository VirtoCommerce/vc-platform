using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Reporting.WinForms;
using VirtoCommerce.Foundation.Reporting.Helpers;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces;
using Xceed.Wpf.Toolkit;

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

        void ReportView_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = DataContext as IReportViewModel;

            if (viewModel != null && !viewModel.UIPrepared)
            {
                viewModel.RefreshReport += (o, args) => RefreshReport();
                _reportViewer.ReportRefresh += (o, args) =>viewModel.ReloadReportData();
                if (viewModel.ReportType.HasReportParameters)
                {
                    PrepareParameters();
                }
                else
                {
                    expandedTab.Collapse();
                }

                viewModel.ReloadReportData(false);
                viewModel.UIPrepared = true;
            }
        }

	    private void PrepareParameters()
	    {
            rdlParameters.Children.Clear();
            rdlParameters.RowDefinitions.Clear();

	        int row = 0;
	        int paramId = 0;
	        foreach (var parameter in viewModel.ReportType.ReportParameters)
	        {
	            var label = new TextBlock
	            {
                    Text = !parameter.Nullable ? "* " + parameter.Prompt : parameter.Prompt
	            };

	            var paramValueBind = new Binding("Value"){ Source = viewModel.ReportType.ReportParameters[paramId++]};

                Control paramControl= GetControlByType(parameter, paramValueBind);

                Grid.SetRow(label, row++);
                Grid.SetRow(paramControl, row++);

                this.rdlParameters.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                this.rdlParameters.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
	            this.rdlParameters.Children.Add(label);
                this.rdlParameters.Children.Add(paramControl);
	        }
	    }

	    private Control GetControlByType(RdlType.ReportParameterType parameter, Binding paramValueBind)
	    {
            Control paramControl = new TextBox();
	        BindingExpressionBase expr = null;

	        if (parameter.HasValidValues)
	        {
	            paramControl = new ComboBox()
	            {
	                ItemsSource = viewModel.ReportType.GetReportParameterValidValues(parameter.Name),
	                SelectedValuePath = "Value",
	                DisplayMemberPath = "Key"
	            };

	            expr = paramControl.SetBinding(ComboBox.SelectedValueProperty, paramValueBind);
	        }
	        else
	        {

	            switch (parameter.DataType)
	            {
	                case RdlType.ReportParameterType.DataTypeEnum.DateTime:
	                    paramControl = new DatePicker();
	                    expr = paramControl.SetBinding(DatePicker.SelectedDateProperty, paramValueBind);
	                    break;

	                case RdlType.ReportParameterType.DataTypeEnum.Boolean:
	                    paramControl = new CheckBox();
	                    expr = paramControl.SetBinding(CheckBox.IsCheckedProperty, paramValueBind);
	                    break;

	                case RdlType.ReportParameterType.DataTypeEnum.Integer:
	                    paramControl = new IntegerUpDown();
	                    expr = paramControl.SetBinding(IntegerUpDown.ValueProperty, paramValueBind);
	                    break;

	                case RdlType.ReportParameterType.DataTypeEnum.Float:
	                    paramControl = new DecimalUpDown();
	                    expr = paramControl.SetBinding(DecimalUpDown.ValueProperty, paramValueBind);
	                    break;

	                default:
	                    expr = paramControl.SetBinding(TextBox.TextProperty, paramValueBind);
	                    break;
	            }
	        }
           
	        viewModel.ClearParameters += (sender, args) => expr.UpdateTarget();
	        return paramControl;
	    }

	    private void PrepareReport()
	    {
	        Dispatcher.Invoke(() => _reportViewer.Reset());

	        if (viewModel.ReportType.ReportParametersAreValid)
	        {
	            _reportViewer.ProcessingMode = ProcessingMode.Local;
	            _reportViewer.LocalReport.LoadReportDefinition(viewModel.ReportDefinition);
	            _reportViewer.LocalReport.DataSources.Clear();
	            _reportViewer.LocalReport.SetParameters(
	                viewModel.ReportType.ReportParameters.Select(
	                    r => new ReportParameter(r.Name, r.Value == null ? null : r.Value.ToString()))
	                );
	            foreach (DataTable table in viewModel.GetReportDataSet().Tables)
	            {
	                var ds = new ReportDataSource(table.TableName, table);

	                _reportViewer.LocalReport.DataSources.Add(ds);
	            }
	        }
	    }

	    private void RefreshReport()
	    {
            Dispatcher.Invoke(delegate
            {
                ReportsHost.Visibility = Visibility.Hidden;
            });

            PrepareReport();

            Dispatcher.Invoke(delegate
	        {
	            _reportViewer.RefreshReport();
	            ReportsHost.Visibility = Visibility.Visible;
	        });
	    }
	}
}