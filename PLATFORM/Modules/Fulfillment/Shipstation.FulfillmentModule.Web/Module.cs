using System;
using Microsoft.Practices.Unity;
using Shipstation.FulfillmentModule.Web.Controllers;
using Shipstation.FulfillmentModule.Web.ExportImport;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;

namespace Shipstation.FulfillmentModule.Web
{
    public class Module : ModuleBase, ISupportExportModule, ISupportImportModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members
        
        public override void Initialize()
        {
            _container.RegisterType<ShipstationController>();
        }
        
        #endregion

        #region ISupportExportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<FulfillmentExportImport>();
            job.DoExport(outStream, progressCallback);
        }

        #endregion

        #region ISupportImportModule Members

        public void DoImport(System.IO.Stream inputStream, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<FulfillmentExportImport>();
            job.DoImport(inputStream, progressCallback);
        }

        #endregion


    }
}