using System;
using VirtoCommerce.PackagingModule.Model;

namespace VirtoCommerce.PackagingModule.Services
{
	public interface IPackageService
	{
		ModuleDescriptor OpenPackage(string path);
		ModuleDescriptor[] GetModules();
		void Install(string packageId, string version, IProgress<ProgressMessage> progress);
		void Update(string packageId, string version, IProgress<ProgressMessage> progress);
		void Uninstall(string packageId, IProgress<ProgressMessage> progress);
	}
}
