using System;

namespace VirtoCommerce.Platform.Core.Packaging
{
    public interface IPackageService
    {
        ModuleDescriptor OpenPackage(string path);
        ModuleDescriptor[] GetModules();
        void Install(string packageId, string version, IProgress<ProgressMessage> progress);
        void Update(string packageId, string version, IProgress<ProgressMessage> progress);
        void Uninstall(string packageId, IProgress<ProgressMessage> progress);
    }
}
