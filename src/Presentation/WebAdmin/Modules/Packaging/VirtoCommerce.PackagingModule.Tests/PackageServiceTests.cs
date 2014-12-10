using System.Diagnostics;
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

			var service = new PackageService("source", "target", "target\\packages") { Logger = new DebugLogger() };
			ListModules(service);

			service.Install(package2, "1.0");
			ListModules(service);

			service.Update(package2, "1.1");
			ListModules(service);

			service.Uninstall(package2);
			ListModules(service);

			service.Uninstall(package1);
			ListModules(service);
		}


		static void ListModules(IPackageService service)
		{
			var modules = service.GetModules();
			Debug.WriteLine("Modules count: {0}", modules.Length);

			foreach (var module in modules)
			{
				Debug.WriteLine("{0} {1}", module.Id, module.Version);
			}
		}
	}
}
