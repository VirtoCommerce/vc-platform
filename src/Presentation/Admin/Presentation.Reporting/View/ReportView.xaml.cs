using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using Microsoft.Reporting.WinForms;
using VirtoCommerce.Foundation.Reporting.Helpers;
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
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                viewModel = DataContext as IReportViewModel;
                
                if (viewModel != null)
                {
                    viewModel.RefreshReport += (o, args) => RefreshReport();
                    if (viewModel.ReportType.HasReportParameters)
                    {
                        PrepareFilters();
                    }
                    else
                    {
                        RefreshReport();
                    }
                }
            }));
        }

	    private void PrepareFilters()
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
                Control paramControl;
	            if (parameter.HasValidValues)
	            {
	                paramControl = new ComboBox()
	                {
	                    ItemsSource = viewModel.ReportType.GetReportParameterValidValues(parameter.Name),
                        SelectedValuePath = "Value",
                        DisplayMemberPath = "Key"
	                };
                    
                    paramControl.SetBinding(ComboBox.SelectedValueProperty, paramValueBind);
	            }
	            else
	            {
	                paramControl= GetControlByType(parameter, paramValueBind);
	            }
	       
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

            switch (parameter.DataType)
            {
                case RdlType.ReportParameterType.DataTypeEnum.DateTime:
                    paramControl = new DatePicker();
                    paramControl.SetBinding(DatePicker.SelectedDateProperty, paramValueBind);
                    break;

                case RdlType.ReportParameterType.DataTypeEnum.Boolean:
                    paramControl = new CheckBox();
                    paramControl.SetBinding(CheckBox.IsCheckedProperty, paramValueBind);
                    break;

                case RdlType.ReportParameterType.DataTypeEnum.Integer:
                    paramControl = new IntegerUpDown();
                    paramControl.SetBinding(IntegerUpDown.ValueProperty, paramValueBind);
                    break;

                case RdlType.ReportParameterType.DataTypeEnum.Float:
                    paramControl = new DecimalUpDown();
                    paramControl.SetBinding(DecimalUpDown.ValueProperty, paramValueBind);
                    break;

                default:
                    paramControl.SetBinding(TextBox.TextProperty, paramValueBind);
                    break;
            }

	        return paramControl;
	    }

	    private void RefreshReport()
	    {
	        Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
	        {
	            _reportViewer.ProcessingMode = ProcessingMode.Local;
	            _reportViewer.LocalReport.LoadReportDefinition(viewModel.ReportDefinition);
	            _reportViewer.LocalReport.DataSources.Clear();
	            _reportViewer.LocalReport.SetParameters(
	                viewModel.ReportType.ReportParameters.Select(r => new ReportParameter(r.Name, r.Value == null?null:r.Value.ToString()))
	                );
	            foreach (DataTable table in viewModel.GetReportDataSet().Tables)
	            {
	                var ds = new ReportDataSource(table.TableName, table);

	                _reportViewer.LocalReport.DataSources.Add(ds);
	            }

	            _reportViewer.RefreshReport();
	        }));
	    }
	}
}