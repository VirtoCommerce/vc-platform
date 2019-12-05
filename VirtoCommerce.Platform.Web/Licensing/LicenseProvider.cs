using System.Web.Hosting;

namespace VirtoCommerce.Platform.Web.Licensing
{
    public static class LicenseProvider
    {
        public static License LoadLicense()
        {
            License license = null;
            var licenseFilePath = HostingEnvironment.MapPath(Startup.VirtualRoot + "/App_Data/VirtoCommerce.lic");
            if (System.IO.File.Exists(licenseFilePath))
            {
                var rawLicense = System.IO.File.ReadAllText(licenseFilePath);
                license = License.Parse(rawLicense);

                if (license != null)
                {
                    license.RawLicense = null;
                }
            }
            return license;
        }
    }
}
