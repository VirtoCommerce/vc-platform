using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Frameworks.Common;
using VirtoCommerce.Foundation.Reporting.Helpers;
using VirtoCommerce.Foundation.Reporting.Services;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Reporting.ViewModel.Interfaces;
using VirtoCommerce.Foundation.Reporting.Model;

namespace VirtoCommerce.ManagementClient.Reporting.ViewModel.Implementations
{
    public class ReportViewModel : ViewModelDetailAndWizardBase<ReportInfo>, IReportViewModel
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
            : base(entityFactory, item, false)
        {
            _navManager = navManager;
            _reportingService = reportingService;
            OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
            GenerateReportCommand = new DelegateCommand(()=>ReloadReportData());
            ClearParametersCommand = new DelegateCommand(RaiseClearParameters);
            
            ViewTitle = new ViewTitleBase()
            {
                SubTitle = item.Name,
                Title = "REPORT"
            };
        }

        public event EventHandler RefreshReport;
        public event EventHandler ClearParameters;

        public void ReloadReportData(bool reloadFromDb = true)
        {
            if (reloadFromDb)
            {
                _reportDataSet = null;
            }

            if (RefreshReport != null)
            {
                ShowLoadingAnimation = true;
                PerformTasksInBackgroundWorker(() => RefreshReport(this, EventArgs.Empty), () => ShowLoadingAnimation = false);
            }
        }

        public bool UIPrepared { get; set; }

        private byte[] _buffer;
        public Stream ReportDefinition
        {
            get
            {
                if (_buffer == null)
                {
                    var stream = _reportingService.GetReportFile(this.OriginalItem.AssetPath);
                    var mem = new MemoryStream();
                    stream.CopyTo(mem);
                    _buffer = mem.ToArray();
                }

                return new MemoryStream(_buffer);
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

        private DataSet _reportDataSet;
        public DataSet GetReportDataSet(IDictionary<string, object> parameters = null)
        {
            if (_reportDataSet != null)
            {
                return _reportDataSet;
            }

            if (parameters == null)
            {
                parameters = new Dictionary<string, object>();
                foreach (var parameter in ReportType.ReportParameters)
                {
                    parameters.Add(parameter.Name, parameter.Value);
                }
            }

            _reportDataSet = _reportingService.GetReportData(this.OriginalItem.AssetPath, parameters);
            return _reportDataSet;
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

        protected override void OnLoad()
        {
            RaiseClearParameters();
            ReloadReportData();
            base.OnLoad();
        }

        protected override void OnClose()
        {
            _buffer = null;
            _reportType = null;
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
