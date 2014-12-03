using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.PackagingModule.Model;
using VirtoCommerce.PackagingModule.Services;

namespace VirtoCommerce.PackagingModule.Data.Services
{
	public class PackageService : IPackageService
	{
		#region IPackageService Members

		public IEnumerable<Package> List()
		{
			throw new NotImplementedException();
		}

		public void Install(string packageId, string version)
		{
			throw new NotImplementedException();
		}

		public void Update(string packageId, string version)
		{
			throw new NotImplementedException();
		}

		public void Uninstall(string packageId)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
