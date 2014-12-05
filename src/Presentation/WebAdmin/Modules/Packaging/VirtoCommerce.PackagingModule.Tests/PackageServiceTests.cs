using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.PackagingModule.Data.Services;
using VirtoCommerce.PackagingModule.Services;

namespace VirtoCommerce.PackagingModule.Tests
{
	[TestClass]
	public class PackageServiceTests
	{
		[TestMethod]
		public void CatalogPatchTest()
		{
			const string package1 = "TestModule1";
			const string package2 = "TestModule2";

			var service = new PackageService("source", "target", "target\\packages", "target\\bin");
			ListPackages(service);

			service.Install(package2, "1.0");
			ListPackages(service);

			service.Update(package2, "1.1");
			ListPackages(service);

			service.Uninstall(package2);
			ListPackages(service);

			service.Uninstall(package1);
			ListPackages(service);
		}


		static void ListPackages(IPackageService service)
		{
			var packages = service.GetPackages();
			Debug.WriteLine("Packages count: {0}", packages.Length);

			foreach (var package in packages)
			{
				Debug.WriteLine("{0} {1}", package.Id, package.Version);
			}
		}
	}
}
