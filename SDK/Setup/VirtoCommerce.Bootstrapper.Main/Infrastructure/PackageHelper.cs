using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Bootstrapper.Main.Infrastructure
{
    /*
    public class PackageHelper
    {
        const  XNamespace ManifestNamespace = ( XNamespace) "http://schemas.microsoft.com/wix/2010/BootstrapperApplicationData" ;
        public void Initialize()
        {
      
            //
            // parse the ApplicationData to find included packages and features
            //
            var bundleManifestData = this.ApplicationData;
            var bundleDisplayName = bundleManifestData 
                        .Element(ManifestNamespace + "WixBundleProperties" )
                        .Attribute( "DisplayName")
                        .Value;

            var mbaPrereqs = bundleManifestData.Descendants(ManifestNamespace + "WixMbaPrereqInformation")
                .Select(x => new MBAPrereqPackage(x)).ToList();

            //
            //exclude the MBA prereq packages, such as the .Net 4 installer
            //
            var pkgs = bundleManifestData.Descendants(ManifestNamespace + "WixPackageProperties")
                .Select(x => new BundlePackage(x))
                .Where(pkg => !mbaPrereqs.Any(preReq => preReq.PackageId == pkg.Id));

            //
            // Add the packages to a collection of BundlePackages
            //
            BundlePackages.AddRange(pkgs);

            //
            // check for features and associate them with their parent packages
            //
            var featureNodes = bundleManifestData.Descendants(ManifestNamespace + "WixPackageFeatureInfo");
            foreach ( var featureNode in featureNodes)
            {
                var feature = new PackageFeature(featureNode);
                var parentPkg = BundlePackages.First(pkg => pkg.Id == feature.PackageId);
                parentPkg.AllFeatures.Add(feature);
                feature.Package = parentPkg;
            }
        }

        /// <summary>
        /// Fetch BootstrapperApplicationData.xml and parse into XDocument.
        /// </summary>
        public XElement ApplicationData
        {
            get
            {
                var workingFolder = Path.GetDirectoryName(this.GetType().Assembly.Location);
                var bootstrapperDataFilePath = Path.Combine(workingFolder, "BootstrapperApplicationData.xml");
               
                using (var reader = new StreamReader(bootstrapperDataFilePath))
                {
                    var xml = reader.ReadToEnd();
                    var xDoc = XDocument.Parse(xml);
                    return xDoc.Element(ManifestNamespace + "BootstrapperApplicationData");                   
                }
            }
    }
     * */
}
