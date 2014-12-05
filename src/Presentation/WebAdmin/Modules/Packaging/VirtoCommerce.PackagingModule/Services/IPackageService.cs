using VirtoCommerce.PackagingModule.Model;

namespace VirtoCommerce.PackagingModule.Services
{
	public interface IPackageService
	{
		ModuleDescriptor[] GetModules();
		PackageDescriptor[] GetPackages();
		void Install(string packageId, string version);
		void Update(string packageId, string version);
		void Uninstall(string packageId);
	}
}
