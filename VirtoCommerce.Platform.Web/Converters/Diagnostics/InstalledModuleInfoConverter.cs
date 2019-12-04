using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Web.Model.Diagnostics;
using VirtoCommerce.Platform.Web.Model.Modularity;
using coreModel = VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Converters.Diagnostics
{
    public static class InstalledModuleInfoConverter
    {
        public static InstalledModuleInfo ToWebModel(this coreModel.ManifestModuleInfo moduleInfo)
        {
            var retVal = new InstalledModuleInfo();
            retVal.InjectFrom(moduleInfo);
            retVal.Version = moduleInfo.Version.ToString();
            retVal.PlatformVersion = moduleInfo.PlatformVersion.ToString();
            retVal.Groups = moduleInfo.Groups;
            if (moduleInfo.Dependencies != null)
            {
                retVal.Dependencies = moduleInfo.Dependencies.Select(x => new ModuleIdentity { Id = x.Id, Version = x.Version.ToString() }).ToList();
            }
            retVal.ValidationErrors = moduleInfo.Errors;
            return retVal;
        }
    }
}
