using Omu.ValueInjecter;
using webModel = VirtoCommerce.Platform.Web.Model.Packaging;
using moduleModel = VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Web.Converters.Packaging
{
    public static class ModuleDecriptorConverter
    {
        public static webModel.ModuleDescriptor ToWebModel(this moduleModel.ModuleDescriptor descriptor)
        {
            var retVal = new webModel.ModuleDescriptor();
            retVal.InjectFrom(descriptor);
            return retVal;
        }

        public static moduleModel.ModuleDescriptor ToModuleModel(this webModel.ModuleDescriptor descriptor)
        {
            var retVal = new moduleModel.ModuleDescriptor();
            return retVal;
        }
    }
}
