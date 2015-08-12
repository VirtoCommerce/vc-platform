using System.Linq;
using Omu.ValueInjecter;
using webModel = VirtoCommerce.Platform.Web.Model.Packaging;
using moduleModel = VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Web.Converters.Packaging
{
    public static class ModuleDescriptorConverter
    {
        public static webModel.ModuleDescriptor ToWebModel(this moduleModel.ModuleDescriptor descriptor)
        {
            var retVal = new webModel.ModuleDescriptor();
            retVal.InjectFrom(descriptor);

            if (descriptor.Dependencies != null)
            {
                retVal.Dependencies = descriptor.Dependencies.Select(d => d.ToWebModel()).ToList();
            }

            return retVal;
        }

        public static webModel.ModuleIdentity ToWebModel(this moduleModel.ModuleIdentity source)
        {
            var result = new webModel.ModuleIdentity();
            result.InjectFrom(source);
            return result;
        }
    }
}
