using System.Linq;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Model.Modularity;

namespace VirtoCommerce.Platform.Web.Converters.Modularity
{
    public static class ModuleDescriptorConverter
    {
        public static ModuleDescriptor ToWebModel(this coreModel.ManifestModuleInfo moduleInfo)
        {
            var retVal = new ModuleDescriptor();
            retVal.InjectFrom(moduleInfo);
            retVal.Version = moduleInfo.Version.ToString();
            retVal.PlatformVersion = moduleInfo.PlatformVersion.ToString();
            retVal.Groups = moduleInfo.Groups;
            if(moduleInfo.Dependencies != null)
            {
                retVal.Dependencies = moduleInfo.Dependencies.Select(x => new ModuleIdentity { Id = x.Id, Version = x.Version.ToString() }).ToList();
            }
            retVal.ValidationErrors = moduleInfo.Errors;
            return retVal;
        }

    }
}
