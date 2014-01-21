using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Reporting.Helpers;
using VirtoCommerce.Foundation.Reporting.Services;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces;
using VirtoCommerce.Foundation.Reporting.Model;

namespace VirtoCommerce.ManagementClient.Reporting.ViewModel.Implementations
{
    public class ReportViewModel : ViewModelDetailBase<ReportInfo>, IReportViewModel
    {
        private readonly INavigationManager _navManager;
        private readonly IReportingService _reportingService;
        public DelegateCommand GenerateReportCommand { get; private set; }
        public DelegateCommand ClearParametersCommand { get; private set; }

        public ReportViewModel(
            IReviewEntityFactory entityFactory,
            INavigationManager navManager,
            IReportingService reportingService,
            ReportInfo item)
            : base(entityFactory, item)
        {
            _navManager = navManager;
            _reportingService = reportingService;

            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
            GenerateReportCommand = new DelegateCommand(RaiseGenerateCommand);
            ClearParametersCommand = new DelegateCommand(RaiseClearParameters);
            
            ViewTitle = new ViewTitleBase()
            {
                Title = item.Name,
                SubTitle = "REPORT"
            };
        }

        public event EventHandler RefreshReport;
        public event EventHandler ClearParameters;

        public Stream ReportDefinition
        {
            get
            {
                return _reportingService.GetReportFile(this.OriginalItem.AssetPath); 
            }
        }

        private RdlType _reportType;
        public RdlType ReportType
        {
            get
            {
                return _reportType?? (_reportType = RdlType.Load(ReportDefinition));
            }
        }

        public DataSet GetReportDataSet(IDictionary<string, object> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
                foreach (var parameter in ReportType.ReportParameters)
                {
                    parameters.Add(parameter.Name, parameter.Value);
                }
            }
            return _reportingService.GetReportData(this.OriginalItem.AssetPath, parameters);
        }

        public IEnumerable<ReportInfo> GetReportsList()
        {
            return _reportingService.GetReportsList();
        }

        public override string DisplayName
        {
            get { return InnerItem.Name; }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.Name),
                                                            NavigationNames.HomeName, NavigationNames.MenuName, this));
            }
        }

        public override string ExceptionContextIdentity
        {
            get { return string.Format("Report ({0})", DisplayName); } 
        }

        protected override void GetRepository()
        {
        }

        protected override void LoadInnerItem()
        {

        }

        protected override bool IsValidForSave()
        {
            return true;
        }

        protected override RefusedConfirmation CancelConfirm()
        {
            return null;
        }

        private void RaiseGenerateCommand()
        {
            if (RefreshReport != null)
            {
               OnUIThread( ()=>RefreshReport(this, EventArgs.Empty) );
            }
        }

        private void RaiseClearParameters()
        {
            ReportType.SetReportParametersToDefault();
            if (ClearParameters != null)
            {
                OnUIThread(() => ClearParameters(this, EventArgs.Empty));
            }
        }

    }
}
