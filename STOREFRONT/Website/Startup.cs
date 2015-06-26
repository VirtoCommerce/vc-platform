using System;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using Microsoft.Owin;
using Owin;
using VirtoCommerce.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Web
{
    public partial class Startup
    {
        private static readonly string _assembliesPath = HostingEnvironment.MapPath("~/App_Data");

        public void Configuration(IAppBuilder app)
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolve;

            ConfigureAuth(app);
        }


        /// Will attempt to load missing assembly from either x86 or x64 subdir
        private static Assembly Resolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            var fileName = assemblyName.Name + ".dll";
            var assemblyPath = Path.Combine(_assembliesPath, Environment.Is64BitProcess ? "x64" : "x86", fileName);

            return File.Exists(assemblyPath)
                ? Assembly.LoadFrom(assemblyPath)
                : null;
        }
    }
}
