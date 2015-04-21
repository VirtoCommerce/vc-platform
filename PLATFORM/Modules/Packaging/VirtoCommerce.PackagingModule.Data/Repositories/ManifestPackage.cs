using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Versioning;
using NuGet;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	public class ManifestPackage : IPackage
	{
		private readonly IPackage _package;

		public bool IsRemovable { get; private set; }
		public IEnumerable<string> Dependencies { get; private set; }

		public static ManifestPackage OpenPackage(string path)
		{
			ManifestPackage result = null;

			var root = Path.GetDirectoryName(path);
			var fileName = Path.GetFileName(path);
			var fileSystem = new PhysicalFileSystem(root);
			var package = new WebsiteOptimizedZipPackage(fileSystem, fileName);
			var manifest = ReadManifest(package);

			if (manifest != null)
				result = new ManifestPackage(manifest, package);

			return result;
		}

		public ManifestPackage(ModuleManifest manifest, IPackage package)
		{
			_package = package;

			if (package != null)
			{
				IsRemovable = true;

				AssemblyReferences = package.AssemblyReferences;
				IsAbsoluteLatestVersion = package.IsAbsoluteLatestVersion;
				IsLatestVersion = package.IsLatestVersion;
				Listed = package.Listed;
				Published = package.Published;
				Authors = package.Authors;
				Copyright = package.Copyright;
				DependencySets = package.DependencySets;
				Description = package.Description;
				DevelopmentDependency = package.DevelopmentDependency;
				FrameworkAssemblies = package.FrameworkAssemblies;
				IconUrl = package.IconUrl;
				Language = package.Language;
				LicenseUrl = package.LicenseUrl;
				MinClientVersion = package.MinClientVersion;
				Owners = package.Owners;
				PackageAssemblyReferences = package.PackageAssemblyReferences;
				ProjectUrl = package.ProjectUrl;
				ReleaseNotes = package.ReleaseNotes;
				RequireLicenseAcceptance = package.RequireLicenseAcceptance;
				Summary = package.Summary;
				Tags = package.Tags;
				Title = package.Title;
				Id = package.Title;
				Version = package.Version;
				DownloadCount = package.DownloadCount;
				ReportAbuseUrl = package.ReportAbuseUrl;
			}
			else
			{
				AssemblyReferences = Enumerable.Empty<IPackageAssemblyReference>();
				Authors = Enumerable.Empty<string>();
				DependencySets = Enumerable.Empty<PackageDependencySet>();
				FrameworkAssemblies = Enumerable.Empty<FrameworkAssemblyReference>();
				Owners = Enumerable.Empty<string>();
				PackageAssemblyReferences = new List<PackageReferenceSet>();
				Version = new SemanticVersion("1.0.0.0");
			}

			if (manifest.Id != null)
				Id = manifest.Id;

			if (manifest.Version != null)
				Version = new SemanticVersion(manifest.Version);

			if (manifest.Title != null)
				Title = manifest.Title;

			if (manifest.Description != null)
				Description = manifest.Description;

			if (manifest.Authors != null)
				Authors = manifest.Authors;

			if (manifest.Owners != null)
				Owners = manifest.Owners;

			if (manifest.LicenseUrl != null)
				LicenseUrl = new Uri(manifest.LicenseUrl);

			if (manifest.ProjectUrl != null)
				ProjectUrl = new Uri(manifest.ProjectUrl);

			if (manifest.IconUrl != null)
				IconUrl = new Uri(manifest.IconUrl);

			if (manifest.RequireLicenseAcceptance)
				RequireLicenseAcceptance = manifest.RequireLicenseAcceptance;

			if (manifest.ReleaseNotes != null)
				ReleaseNotes = manifest.ReleaseNotes;

			if (manifest.Copyright != null)
				Copyright = manifest.Copyright;

			if (manifest.Tags != null)
				Tags = manifest.Tags;

			if (manifest.Dependencies != null)
				Dependencies = manifest.Dependencies;
		}

		#region IPackage Members

		public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; private set; }

		public IEnumerable<IPackageFile> GetFiles()
		{
			return _package != null ? _package.GetFiles() : Enumerable.Empty<IPackageFile>();
		}

		public Stream GetStream()
		{
			return _package != null ? _package.GetStream() : Stream.Null;
		}

		public IEnumerable<FrameworkName> GetSupportedFrameworks()
		{
			return _package != null ? _package.GetSupportedFrameworks() : Enumerable.Empty<FrameworkName>();
		}

		public bool IsAbsoluteLatestVersion { get; private set; }
		public bool IsLatestVersion { get; private set; }
		public bool Listed { get; private set; }
		public DateTimeOffset? Published { get; private set; }

		#endregion

		#region IPackageMetadata Members

		public IEnumerable<string> Authors { get; private set; }
		public string Copyright { get; private set; }
		public IEnumerable<PackageDependencySet> DependencySets { get; private set; }
		public string Description { get; set; }
		public bool DevelopmentDependency { get; set; }
		public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; private set; }
		public Uri IconUrl { get; set; }
		public string Language { get; set; }
		public Uri LicenseUrl { get; set; }
		public Version MinClientVersion { get; set; }
		public IEnumerable<string> Owners { get; private set; }
		public ICollection<PackageReferenceSet> PackageAssemblyReferences { get; private set; }
		public Uri ProjectUrl { get; set; }
		public string ReleaseNotes { get; set; }
		public bool RequireLicenseAcceptance { get; set; }
		public string Summary { get; set; }
		public string Tags { get; set; }
		public string Title { get; set; }

		#endregion

		#region IPackageName Members

		public string Id { get; private set; }
		public SemanticVersion Version { get; private set; }

		#endregion

		#region IServerPackageMetadata Members

		public int DownloadCount { get; private set; }
		public Uri ReportAbuseUrl { get; private set; }

		#endregion


		private static ModuleManifest ReadManifest(IPackage package)
		{
			ModuleManifest result = null;

			using (var stream = package.GetStream())
			using (var package2 = Package.Open(stream))
			{
				var uri = new Uri("/content/module.manifest", UriKind.Relative);

				if (package2.PartExists(uri))
				{
					var manifestPart = package2.GetPart(uri);

					using (var manifestStream = manifestPart.GetStream())
					{
						result = ManifestReader.Read(manifestStream);
					}
				}
			}

			return result;
		}
	}
}
