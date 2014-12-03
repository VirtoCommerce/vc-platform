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
			List(service);

			service.Install(package2, "1.0");
			List(service);

			service.Update(package2, "1.1");
			List(service);

			service.Uninstall(package2);
			List(service);

			service.Uninstall(package1);
			List(service);
		}


		static void List(IPackageService service)
		{
			var packages = service.List().ToArray();
			Debug.WriteLine("Packages count: {0}", packages.Length);

			foreach (var package in packages)
			{
				Debug.WriteLine("{0} {1}", package.Id, package.Version);
			}
		}
	}
}
