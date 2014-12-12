using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.PackagingModule.Data.Services;
using VirtoCommerce.PackagingModule.Services;
using VirtoCommerce.PackagingModule.Web.Controllers.Api;

namespace VirtoCommerce.PackagingModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			var packageService = new PackageService(HostingEnvironment.MapPath("~/Content/Packages"), HostingEnvironment.MapPath("~/Modules"), HostingEnvironment.MapPath("~/Packages"));
			_container.RegisterType<ModulesController>(new InjectionConstructor(packageService, HostingEnvironment.MapPath("~/Content/Packages")));
		}

		#endregion
	}
}
