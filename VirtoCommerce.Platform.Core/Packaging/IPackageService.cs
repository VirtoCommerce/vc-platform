using System;

namespace VirtoCommerce.Platform.Core.Packaging
{
    public interface IPackageService
    {
        ModuleDescriptor OpenPackage(string path);
        string[] GetDependencyErrors(ModuleDescriptor package);
        ModuleDescriptor[] GetModules();
        void Install(string sourcePackageFilePath, IProgress<ProgressMessage> progress);
        void Update(string packageId, string newPackageFilePath, IProgress<ProgressMessage> progress);
        void Uninstall(string packageId, IProgress<ProgressMessage> progress);
    }
}
