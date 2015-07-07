using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ImportExport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.ExportImport
{
	public class PlatformExportImportManager : IPlatformExportImportManager
	{
		 private readonly IModuleCatalog _moduleCatalog;
		 private readonly IPackageService _packageService;

		 public PlatformExportImportManager(IModuleCatalog moduleCatalog, IPackageService packageService)
		 {
			 _moduleCatalog = moduleCatalog;
			 _packageService = packageService;
		 }
		
		#region IPlatformExportImportManager Members

		public ModuleDescriptor[] GetSupportedExportModules()
		{
			return InnerGetModulesWithInterface(typeof(ISupportExportModule));
		}

		public ModuleDescriptor[] GetSupportedImportModules()
		{
			return InnerGetModulesWithInterface(typeof(ISupportImportModule));
		}

		public Stream Export(string[] moduleIds, string platformVersion, Action<ExportImportProgressInfo> progressCallback)
		{
			if(moduleIds == null)
			{
				throw new ArgumentNullException("moduleIds");
			}
			var memoryStream = new MemoryStream();
			var progressInfo = new ExportImportProgressInfo
			{
				Status = "Start exporting",
				TotalCount = moduleIds.Count(),
				ProcessedCount = 0
			};
			progressCallback(progressInfo);

			using (var package = ZipPackage.Open(memoryStream, FileMode.Create))
			{
				var exportModulesInfo = new List<ExportModuleInfo>();
				foreach (var module in _moduleCatalog.Modules.Where(x => moduleIds.Contains(x.ModuleName)))
				{
					var moduleDescriptor = _packageService.GetModules().First(x => module.ModuleName == x.Id);
					//Create part for module
					var modulePartUri = PackUriHelper.CreatePartUri(new Uri(module.ModuleName, UriKind.Relative));
					var modulePart = package.CreatePart(modulePartUri, System.Net.Mime.MediaTypeNames.Application.Octet);

				   progressInfo.Status =  String.Format("{0}: export started.", moduleDescriptor.Id);
				   progressCallback(progressInfo);

					Action<ExportImportProgressInfo> modulePorgressCallback = (x) =>
						{
							progressInfo.Status = String.Format("{0}: {1} ({2} of {3} processed)", moduleDescriptor.Id, x.Status, x.TotalCount, x.ProcessedCount);
							progressCallback(progressInfo);
						};

					((ISupportExportModule)module.ModuleInstance).DoExport(modulePart.GetStream(), modulePorgressCallback);

					//Register in manifest
					var moduleManifestPart = new ExportModuleInfo
					{
						ModuleId = moduleDescriptor.Id,
						ModuleVersion = moduleDescriptor.Version,
						PartUri = modulePartUri.ToString()
					};
					exportModulesInfo.Add(moduleManifestPart);

					progressInfo.Status = String.Format("{0}: export finished.", moduleDescriptor.Id);
					progressInfo.ProcessedCount++;
					progressCallback(progressInfo);
				}

				//Write system information about exported modules
				var partUriManifest = PackUriHelper.CreatePartUri(new Uri("Manifest.xml", UriKind.Relative));
				var manifestPart = package.CreatePart(partUriManifest, System.Net.Mime.MediaTypeNames.Text.Xml);
				var manifest = new PlatformExportManifest
				{
					Author = CurrentPrincipal.GetCurrentUserName(),
					PlatformVersion = platformVersion,
				};
				manifest.Modules = exportModulesInfo.ToArray();
				//After all modules exported need write export manifest part
				using (var streamWriter = new StreamWriter(manifestPart.GetStream()))
				{
					streamWriter.Write(manifest.SerializeXML());
				}
			}

			memoryStream.Seek(0, SeekOrigin.Begin);
			return memoryStream;
		}

		public void Import(Stream stream, Func<ExportImportProgressInfo> progressCallback)
		{
			throw new NotImplementedException();
		}

		#endregion

		private ModuleDescriptor[] InnerGetModulesWithInterface(Type interfaceType)
		{
			var moduleNames = _moduleCatalog.Modules.Where(x => x.ModuleInstance != null)
												.Where(x => x.ModuleInstance.GetType().GetInterfaces().Contains(interfaceType))
												.Select(x => x.ModuleName).ToArray();
			var retVal = _packageService.GetModules().Where(x => moduleNames.Contains(x.Id)).ToList();
			return retVal.ToArray();
		}
	}
}
