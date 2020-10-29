using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Core
{
    public class PlatformOptions
    {
        public string DemoCredentials { get; set; }

        public string DemoResetTime { get; set; }

        [Required]
        public string LocalUploadFolderPath { get; set; } = "app_data/uploads";

        //The public url for license activation
        [Url]
        public string LicenseActivationUrl { get; set; } = "https://virtocommerce.com/admin/api/licenses/activate/";

        //Local path for license file
        public string LicenseFilePath { get; set; } = "app_data/VirtoCommerce.lic";

        //Local path to public key for license 
        public string LicensePublicKeyPath { get; set; } = "app_data/VirtoCommerce_rsa.pub";

        //Local path to private key for signing license
        public string LicensePrivateKeyPath { get; set; }

        //Url for discovery sample data for initial installation
        //e.g. http://virtocommerce.blob.core.windows.net/sample-data
        //[Url]
        public string SampleDataUrl { get; set; }

        //Default path to store export files 
        public string DefaultExportFolder { get; set; } = "app_data/export/";

        public string DefaultExportFileName { get; set; } = "exported_{0:yyyyMMddHHmmss}.zip";

        //Local path to running process like WkhtmlToPdf
        public string ProcessesPath { get; set; }

        //This options controls how the OpenID Connect
        //server (ASOS) handles the incoming requests to arriving on non-HTTPS endpoints should be rejected or not. By default, this property is set to false to help
        //mitigate man-in-the-middle attacks.
        public bool AllowInsecureHttp { get; set; }
    }

}
