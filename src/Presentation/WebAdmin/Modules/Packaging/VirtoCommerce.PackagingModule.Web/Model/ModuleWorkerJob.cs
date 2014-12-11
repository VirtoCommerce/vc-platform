using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.PackagingModule.Services;

namespace VirtoCommerce.PackagingModule.Web.Model
{
	public enum ModuleAction
	{
		Install,
		Update,
		Uninstall
	}

	public class ModuleWorkerJob
	{
		public ModuleWorkerJob(IPackageService packageService, ModuleDescriptor moduleDescriptor, ModuleAction action)
        {
			Id = Guid.NewGuid().ToString();
			CancellationToken = new CancellationToken();
			PackageService = packageService;
			ModuleDescriptor = moduleDescriptor;
        }
        public string Id;
		public string Started;
		public string Completed;
		public ModuleDescriptor ModuleDescriptor;
		public ModuleAction Action;
		[JsonIgnore]
		public IPackageService PackageService;
		[JsonIgnore]
        public CancellationToken CancellationToken;
		public List<string> Logs = new List<string>();
		public List<string> Errors = new List<string>();
	}
}
