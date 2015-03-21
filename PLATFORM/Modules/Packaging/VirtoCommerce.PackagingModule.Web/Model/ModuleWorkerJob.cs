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
			Action = action;
			ProgressLog = new List<ProgressMessage>();
        }
        public string Id;
		public DateTime? Started;
		public DateTime? Completed;
		public ModuleDescriptor ModuleDescriptor;
		public ModuleAction Action;
		[JsonIgnore]
		public IPackageService PackageService;
		[JsonIgnore]
        public CancellationToken CancellationToken;
		public ICollection<ProgressMessage> ProgressLog;
	}
}
