using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PackagingModule.Model;

namespace VirtoCommerce.PackagingModule.Services
{
	public interface IPackageService
	{
		IEnumerable<Package> List();
		void Install(string packageId, string version);
		void Update(string packageId, string version);
		void Uninstall(string packageId);
	}
}
