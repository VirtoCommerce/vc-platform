using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Frameworks.Extensions;
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

            ViewTitle = new ViewTitleBase()
            {
                Title = item.Name,
                SubTitle = "REPORT"
            };
        }

        public Stream ReportDefinition
        {
            get
            {
                return _reportingService.GetReportFile(this.OriginalItem.AssetPath); 
            }
        }

        public DataSet ReportDataSet
        {
            get { return _reportingService.GetReportData(this.OriginalItem.AssetPath); }
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
    }
}
