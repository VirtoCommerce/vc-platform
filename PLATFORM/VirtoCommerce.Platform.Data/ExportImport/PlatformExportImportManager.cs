using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Packaging;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Security.Identity;

namespace VirtoCommerce.Platform.Data.ExportImport
{
	public sealed class PlatformExportEntries
	{
		public PlatformExportEntries()
		{
			Users = new List<ApplicationUserExtended>();
			Settings = new List<SettingEntry>();
			DynamicPropertyDictionaryItems = new List<DynamicPropertyDictionaryItem>();
		}
		public bool IsNotEmpty
		{
			get
			{
				return Users.Any() || Settings.Any();
			}
		}
		public ICollection<ApplicationUserExtended> Users { get; set; }
		public ICollection<SettingEntry> Settings { get; set; }
		public ICollection<DynamicPropertyDictionaryItem> DynamicPropertyDictionaryItems { get; set; }
		public ICollection<DynamicProperty> DynamicProperties { get; set; } 
	}



	public class PlatformExportImportManager : IPlatformExportImportManager
	{
		private readonly Uri _manifestPartUri;
		private readonly Uri _platformEntriesPartUri;

		private readonly ISecurityService _securityService;
        private readonly IRoleManagementService _roleManagementService;
		private readonly ISettingsManager _settingsManager;
		private readonly IDynamicPropertyService _dynamicPropertyService;

		public PlatformExportImportManager(ISecurityService securityService, IRoleManagementService roleManagementService, ISettingsManager settingsManager, IDynamicPropertyService dynamicPropertyService)
		{
			_dynamicPropertyService = dynamicPropertyService;
			_securityService = securityService;
            _roleManagementService = roleManagementService;
			_settingsManager = settingsManager;

			_manifestPartUri = PackUriHelper.CreatePartUri(new Uri("Manifest.json", UriKind.Relative));
			_platformEntriesPartUri = PackUriHelper.CreatePartUri(new Uri("PlatformEntries.json", UriKind.Relative));
		}

		#region IPlatformExportImportManager Members

		public PlatformExportManifest ReadPlatformExportManifest(Stream stream)
		{
			PlatformExportManifest retVal = null;
			using (var package = ZipPackage.Open(stream, FileMode.Open))
			{
				var manifestPart = package.GetPart(_manifestPartUri);
				using (var manifestStream = manifestPart.GetStream())
				{
					retVal = manifestStream.DeserializeJson<PlatformExportManifest>();
				}
			}
			return retVal;
		}

		public void Export(Stream outStream, PlatformExportImportOptions exportOptions, Action<ExportImportProgressInfo> progressCallback)
		{
			if (exportOptions == null)
			{
				throw new ArgumentNullException("exportOptions");
			}

			using (var package = ZipPackage.Open(outStream, FileMode.Create))
			{
				//Export all selected platform entries
				ExportPlatformEntriesInternal(package, exportOptions, progressCallback);
				//Export all selected  modules
				var exportedModules = ExportModulesInternal(package, exportOptions, progressCallback);

				//Write system information about exported modules
				var manifestPart = package.CreatePart(_manifestPartUri, System.Net.Mime.MediaTypeNames.Text.Xml);
				var manifest = new PlatformExportManifest
				{
					Author = exportOptions.Author,
					Created = DateTime.UtcNow,
					PlatformVersion = exportOptions.PlatformVersion.ToString(),
					IsHasSecurity = exportOptions.HandleSecurity,
					IsHasSettings = exportOptions.HandleSettings,
					Modules = exportedModules
				};
				//After all modules exported need write export manifest part
				using (var stream = manifestPart.GetStream())
				{
					manifest.SerializeJson<PlatformExportManifest>(stream);
				}
			}
		}

		public void Import(Stream stream, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback)
		{
			var manifest = ReadPlatformExportManifest(stream);
			if (manifest == null)
			{
				throw new NullReferenceException("manifest");
			}

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = "Starting platform import...";
			progressCallback(progressInfo);
		
			using (var package = ZipPackage.Open(stream, FileMode.Open))
			{
				//Import selected platform entries
				ImportPlatformEntriesInternal(package, importOptions, progressCallback);
				//Import selected modules
				ImportModulesInternal(package, manifest, importOptions, progressCallback);
				
			}
		}

		#endregion

		private void ImportPlatformEntriesInternal(Package package, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback)
		{
			var progressInfo = new ExportImportProgressInfo();

			var platformEntriesPart = package.GetPart(_platformEntriesPartUri);
			if (platformEntriesPart != null)
			{
				PlatformExportEntries platformEntries;
				using (var stream = platformEntriesPart.GetStream())
				{
					platformEntries = stream.DeserializeJson<PlatformExportEntries>();
				}

                if (importOptions.HandleSecurity)
				{
					progressInfo.Description = String.Format("Import {0} users with roles...", platformEntries.Users.Count());
					progressCallback(progressInfo);

					//First need import roles
					var roles = platformEntries.Users.SelectMany(x => x.Roles).Distinct().ToArray();
                    foreach (var role in roles)
					{
                        _roleManagementService.AddOrUpdateRole(role);
					}
					//Next create or update users
                    foreach (var user in platformEntries.Users)
					{
                        if (_securityService.FindByIdAsync(user.Id, UserDetails.Reduced).Result != null)
						{
							_securityService.UpdateAsync(user);
						}
						else
						{
							_securityService.CreateAsync(user);
						}
					}
				}

				//Import dynamic properties
				_dynamicPropertyService.SaveProperties(platformEntries.DynamicProperties.ToArray());
				foreach (var propDicGroup in platformEntries.DynamicPropertyDictionaryItems.GroupBy(x=>x.PropertyId))
				{
					_dynamicPropertyService.SaveDictionaryItems(propDicGroup.Key, propDicGroup.ToArray());
				}
			}
		}

		private void ExportPlatformEntriesInternal(Package package, PlatformExportImportOptions exportOptions, Action<ExportImportProgressInfo> progressCallback)
		{
			var progressInfo = new ExportImportProgressInfo();
			var platformExportObj = new PlatformExportEntries();

			if (exportOptions.HandleSecurity)
			{
				//users 
				var usersResult = _securityService.SearchUsersAsync(new UserSearchRequest { TakeCount = int.MaxValue }).Result;
				progressInfo.Description = String.Format("Security: {0} users exporting...", usersResult.Users.Count());
				progressCallback(progressInfo);

				foreach (var user in usersResult.Users)
				{
					platformExportObj.Users.Add(_securityService.FindByIdAsync(user.Id, UserDetails.Full).Result);
				}
			}

			if (exportOptions.HandleSettings)
			{
				progressInfo.Description = String.Format("Settings: {0} settings exporting...", 0);
				progressCallback(progressInfo);

				//settings 
				throw new NotImplementedException();
			}

			//Dynamic properties
			var allTypes = _dynamicPropertyService.GetAvailableObjectTypeNames();

			progressInfo.Description = String.Format("Dynamic properties: load properties...");
			progressCallback(progressInfo);

			platformExportObj.DynamicProperties = allTypes.SelectMany(x => _dynamicPropertyService.GetProperties(x)).ToList();
			platformExportObj.DynamicPropertyDictionaryItems = platformExportObj.DynamicProperties.Where(x => x.IsDictionary).SelectMany(x => _dynamicPropertyService.GetDictionaryItems(x.Id)).ToList();

			//Create part for platform entries
			var platformEntiriesPart = package.CreatePart(_platformEntriesPartUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Normal);
			using (var partStream = platformEntiriesPart.GetStream())
			{
				platformExportObj.SerializeJson<PlatformExportEntries>(partStream);
			}

		}

		private void ImportModulesInternal(Package package, PlatformExportManifest manifest, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback)
		{
			var progressInfo = new ExportImportProgressInfo();
			foreach (var module in importOptions.Modules)
			{
				var moduleInfo = manifest.Modules.First(x => x.ModuleId == module.Id);
				var modulePart = package.GetPart(new Uri(moduleInfo.PartUri, UriKind.Relative));
				using (var modulePartStream = modulePart.GetStream())
				{
					Action<ExportImportProgressInfo> modulePorgressCallback = (x) =>
					{
						progressInfo.Description = String.Format("{0}: {1}", module.Id, x.Description);
						progressCallback(progressInfo);
					};
					((ISupportImportModule)module.ModuleInfo.ModuleInstance).DoImport(modulePartStream, importOptions, modulePorgressCallback);

				}
			}
		}

		private ExportModuleInfo[] ExportModulesInternal(Package package, PlatformExportImportOptions exportOptions, Action<ExportImportProgressInfo> progressCallback)
		{
			var progressInfo = new ExportImportProgressInfo();
			var result = new List<ExportModuleInfo>();
		
			foreach (var module in exportOptions.Modules)
			{
				//Create part for module
				var modulePartUri = PackUriHelper.CreatePartUri(new Uri(module.Id, UriKind.Relative));
				var modulePart = package.CreatePart(modulePartUri, System.Net.Mime.MediaTypeNames.Application.Octet, CompressionOption.Normal);

				Action<ExportImportProgressInfo> modulePorgressCallback = (x) =>
				{
					progressInfo.Description = String.Format("{0}: {1}", module.Id, x.Description);
					progressCallback(progressInfo);
				};

				progressInfo.Description = String.Format("{0}: exporting...", module.Id);
				progressCallback(progressInfo);
				((ISupportExportModule)module.ModuleInfo.ModuleInstance).DoExport(modulePart.GetStream(), exportOptions, modulePorgressCallback);

				//Register in manifest
				var moduleManifestPart = new ExportModuleInfo
				{
					ModuleId = module.Id,
					ModuleVersion = module.Version,
					PartUri = modulePartUri.ToString()
				};
				result.Add(moduleManifestPart);
			}
			return result.ToArray();
		}


	}
}
