using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Core
{
    public class PlatformOptions
    {
        public string DemoCredentials { get; set; }

        public string DemoResetTime { get; set; }

        [Required]
        public string LocalUploadFolderPath { get; set; } = "app_data/uploads";

        // The public URL for license activation
        [Url]
        public string LicenseActivationUrl { get; set; } = "https://virtocommerce.com/admin/api/licenses/activate/";

        // Local path for license file
        public string LicenseFilePath { get; set; } = "app_data/VirtoCommerce.lic";

        // Name of the license file with blob container
        public string LicenseBlobPath { get; set; } = "license/VirtoCommerce.lic";

        // Name of the public key embedded resource
        public string LicensePublicKeyResourceName { get; set; } = "VirtoCommerce_rsa.pub";

        // Local path to private key for signing license
        public string LicensePrivateKeyPath { get; set; }

        //Local path for countries list
        public string CountriesFilePath { get; set; } = "localization/common/countries.json";
        public string CountryRegionsFilePath { get; set; } = "localization/common/countriesRegions.json";

        // URL for discovery sample data for initial installation
        // e.g. http://virtocommerce.blob.core.windows.net/sample-data
        // [Url] Remove validation attribute to allow set empty strings in the option
        public string SampleDataUrl { get; set; }

        // Default path to store export files
        public string DefaultExportFolder { get; set; } = "export";

        public string DefaultExportFileName { get; set; } = "exported_{0:yyyyMMddHHmmss}.zip";

        // Local path to running process like WkhtmlToPdf
        public string ProcessesPath { get; set; }

        // This options controls how the OpenID Connect
        // server (ASOS) handles the incoming requests to arriving on non-HTTPS endpoints should be rejected or not. By default, this property is set to false to help
        // mitigate man-in-the-middle attacks.
        public bool AllowInsecureHttp { get; set; }

        /// <summary>
        /// Extensions of the files that cannot be uploaded to the server by the platform
        /// </summary>
        public string[] FileExtensionsBlackList { get; set; } = new string[0];

        /// <summary>
        /// Extensions of the files that can be uploaded to the server by the platform
        /// </summary>
        public string[] FileExtensionsWhiteList { get; set; } = new string[0];

        /// <summary>
        /// Extend Rest API reference schemas (using the allOf construct) so that contextual metadata can be applied to all parameter and property schemas.
        /// </summary>
        public bool UseAllOfToExtendReferenceSchemas { get; set; } = true;

        /// <summary>
        /// Include null values when serializing Rest API objects.
        /// </summary>
        public bool IncludeOutputNullValues { get; set; } = true;
    }
}
