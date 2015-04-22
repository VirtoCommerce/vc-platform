using System;
using System.IO;
using System.Linq;
using NuGet;
using VirtoCommerce.Platform.Data.Packaging.Repositories;
using VirtoCommerce.Platform.Core.Packaging;

namespace VirtoCommerce.Platform.Data.Packaging
{
    public class PackageService : IPackageService
    {
        private readonly ProjectManager _projectManager;

        public PackageService(ProjectManager nugetProjectManager)
        {
            _projectManager = nugetProjectManager;
        }


        #region IPackageService Members

        public ModuleDescriptor OpenPackage(string path)
        {
            ModuleDescriptor result = null;

            var fullPath = Path.GetFullPath(path);
            var package = ManifestPackage.OpenPackage(fullPath);

            if (package != null)
                result = ConvertToModuleDescriptor(package);

            return result;
        }

        public ModuleDescriptor[] GetModules()
        {
            return _projectManager.LocalRepository.GetPackages()
                .OfType<ManifestPackage>()
                .Select(ConvertToModuleDescriptor)
                .OrderBy(m => m.Title)
                .ToArray();
        }

        public void Install(string packageId, string version, IProgress<ProgressMessage> progress)
        {
            var packageVersion = string.IsNullOrEmpty(version) ? null : new SemanticVersion(version);
            _projectManager.Logger = new LoggerProgressWrapper(progress);
            _projectManager.AddPackageReference(packageId, packageVersion, false, true);
        }

        public void Update(string packageId, string version, IProgress<ProgressMessage> progress)
        {
            var packageVersion = string.IsNullOrEmpty(version) ? null : new SemanticVersion(version);
            _projectManager.Logger = new LoggerProgressWrapper(progress);
            _projectManager.UpdatePackageReference(packageId, packageVersion, true, true);
        }

        public void Uninstall(string packageId, IProgress<ProgressMessage> progress)
        {
            _projectManager.Logger = new LoggerProgressWrapper(progress);
            _projectManager.RemovePackageReference(packageId, false, false);
        }

        #endregion

        private static ModuleDescriptor ConvertToModuleDescriptor(ManifestPackage package)
        {
            return new ModuleDescriptor
            {
                Id = package.Id,
                Version = package.Version.ToString(),
                Title = package.Title,
                Description = package.Description,
                Authors = package.Authors,
                Owners = package.Owners,
                LicenseUrl = package.LicenseUrl,
                ProjectUrl = package.ProjectUrl,
                IconUrl = package.IconUrl,
                RequireLicenseAcceptance = package.RequireLicenseAcceptance,
                ReleaseNotes = package.ReleaseNotes,
                Copyright = package.Copyright,
                Tags = package.Tags,
                Dependencies = package.Dependencies,
                IsRemovable = package.IsRemovable,
            };
        }

        private class LoggerProgressWrapper : ILogger
        {
            private readonly IProgress<ProgressMessage> _progress;
            public LoggerProgressWrapper(IProgress<ProgressMessage> progress)
            {
                _progress = progress;
            }
            #region ILogger Members

            public void Log(MessageLevel level, string message, params object[] args)
            {
                if (_progress != null)
                {
                    var reportProgress = new ProgressMessage { Level = (ProgressMessageLevel)(int)level, Message = string.Format(message, args) };
                    _progress.Report(reportProgress);
                }
            }

            #endregion

            #region IFileConflictResolver Members

            public FileConflictResolution ResolveFileConflict(string message)
            {
                return FileConflictResolution.OverwriteAll;
            }

            #endregion
        }
    }
}
