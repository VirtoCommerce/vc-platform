using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.ExportImport
{
	public class PlatformExportImportManager : IPlatformExportImportManager
	{
		 private readonly Uri _manifestPartUri;

		 public PlatformExportImportManager()
		 {
			 _manifestPartUri = PackUriHelper.CreatePartUri(new Uri("Manifest.xml", UriKind.Relative));
		 }
		
		#region IPlatformExportImportManager Members

		public PlatformExportManifest ReadPlatformExportManifest(Stream stream)
		{
			PlatformExportManifest retVal = null;
			using (var package = ZipPackage.Open(stream, FileMode.Open))
			{
				var manifestPart = package.GetPart(_manifestPartUri);
				using (var streamReader = new StreamReader(manifestPart.GetStream()))
				{
					retVal = streamReader.ReadToEnd().DeserializeXML<PlatformExportManifest>();
				}
			}
			return retVal;
		}

		public void Export(Stream outStream, ModuleDescriptor[] modules, SemanticVersion platformVersion, Action<ExportImportProgressInfo> progressCallback)
		{
			if (modules == null)
			{
				throw new ArgumentNullException("moduleIds");
			}
			var progressInfo = new ExportImportProgressInfo
			{
				Description = "Start exporting",
				TotalCount = modules.Count(),
				ProcessedCount = 0
			};
			progressCallback(progressInfo);

			using (var package = ZipPackage.Open(outStream, FileMode.Create))
			{
				var exportModulesInfo = new List<ExportModuleInfo>();
				foreach (var module in modules)
				{
					//Create part for module
					var modulePartUri = PackUriHelper.CreatePartUri(new Uri(module.Id, UriKind.Relative));
					var modulePart = package.CreatePart(modulePartUri, System.Net.Mime.MediaTypeNames.Application.Octet);

					progressInfo.Description = String.Format("{0}: export started.", module.Id);
				    progressCallback(progressInfo);

					Action<ExportImportProgressInfo> modulePorgressCallback = (x) =>
						{
							progressInfo.Description = String.Format("{0}: {1} ({2} of {3} processed)", module.Id, x.Description, x.TotalCount, x.ProcessedCount);
							progressCallback(progressInfo);
						};

					((ISupportExportModule)module.ModuleInfo.ModuleInstance).DoExport(modulePart.GetStream(), modulePorgressCallback);

					//Register in manifest
					var moduleManifestPart = new ExportModuleInfo
					{
						ModuleId = module.Id,
						ModuleVersion = module.Version,
						PartUri = modulePartUri.ToString()
					};
					exportModulesInfo.Add(moduleManifestPart);

					progressInfo.Description = String.Format("{0}: export finished.", module.Id);
					progressInfo.ProcessedCount++;
					progressCallback(progressInfo);
				}

				//Write system information about exported modules
				var manifestPart = package.CreatePart(_manifestPartUri, System.Net.Mime.MediaTypeNames.Text.Xml);
				var manifest = new PlatformExportManifest
				{
				    Author = CurrentPrincipal.GetCurrentUserName(),
				    PlatformVersion = platformVersion.ToString(),
				    Modules = exportModulesInfo.ToArray(),
				};
			    //After all modules exported need write export manifest part
				using (var streamWriter = new StreamWriter(manifestPart.GetStream()))
				{
					streamWriter.Write(manifest.SerializeXML());
				}
			}
		}

		public void Import(Stream stream, ModuleDescriptor[] modules, SemanticVersion platformVersion, Func<ExportImportProgressInfo> progressCallback)
		{
			throw new NotImplementedException();
		}

		#endregion

	
	}
}
