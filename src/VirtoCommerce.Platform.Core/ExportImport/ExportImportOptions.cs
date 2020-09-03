using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class ExportImportOptions
    {
        public ModuleIdentity ModuleIdentity { get; set; }

        /// <summary>
        /// Flag means the use of  binary data in export or import operations
        /// </summary>
        public bool HandleBinaryData { get; set; } = false;

    }
}
