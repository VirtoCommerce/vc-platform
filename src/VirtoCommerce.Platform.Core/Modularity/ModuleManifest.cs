using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Represents the type into which a module.manifest file will be deserialized.
    /// </summary>
    [XmlType("module")]
    public class ModuleManifest
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("version-tag")]
        public string VersionTag { get; set; }

        [XmlElement("platformVersion")]
        public string PlatformVersion { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlArray("authors")]
        [XmlArrayItem("author")]
        public string[] Authors { get; set; }

        [XmlArray("owners")]
        [XmlArrayItem("owner")]
        public string[] Owners { get; set; }

        [XmlElement("licenseUrl")]
        public string LicenseUrl { get; set; }

        [XmlElement("projectUrl")]
        public string ProjectUrl { get; set; }

        [XmlElement("packageUrl")]
        public string PackageUrl { get; set; }

        [XmlElement("iconUrl")]
        public string IconUrl { get; set; }

        [XmlElement("requireLicenseAcceptance")]
        public bool RequireLicenseAcceptance { get; set; }

        [XmlElement("releaseNotes")]
        public string ReleaseNotes { get; set; }

        [XmlElement("copyright")]
        public string Copyright { get; set; }

        [XmlElement("tags")]
        public string Tags { get; set; }

        [XmlElement("assemblyFile")]
        public string AssemblyFile { get; set; }

        [XmlElement("moduleType")]
        public string ModuleType { get; set; }

        [XmlArray("dependencies")]
        [XmlArrayItem("dependency")]
        public ManifestDependency[] Dependencies { get; set; }

        [XmlArray("incompatibilities")]
        [XmlArrayItem("module")]
        public ManifestDependency[] Incompatibilities { get; set; }

        [XmlArray("groups")]
        [XmlArrayItem("group")]
        public string[] Groups { get; set; }

        [XmlElement("useFullTypeNameInSwagger")]
        public bool UseFullTypeNameInSwagger { get; set; }
    }
}
