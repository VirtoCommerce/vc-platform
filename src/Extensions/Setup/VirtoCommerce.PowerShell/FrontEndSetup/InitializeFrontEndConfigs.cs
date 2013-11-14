using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Xml.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.PowerShell.Cmdlet;

namespace VirtoCommerce.PowerShell.FrontEndSetup
{
    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Initialize, "Virto-FrontEnd-Configs", SupportsShouldProcess = true, DefaultParameterSetName = "DbConnection")]
    public class InitializeFrontEndConfigs : DomainCommand
    {
        [Parameter(HelpMessage = "Front end project location.")]
        [ValidateNotNullOrEmpty]
        public string Location { get; set; }

        [Parameter(HelpMessage = "Database connection string.")]
        [ValidateNotNullOrEmpty]
        public string DbConnection { get; set; }

        [Alias("search")]
        [Parameter(HelpMessage = "ElasticSearch connection string.")]
        [ValidateNotNullOrEmpty]
        public string SearchConnection { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                SafeWriteVerbose("Version: " + Assembly.GetExecutingAssembly().GetFileVersion());
                base.ProcessRecord();
                Initialize(Location, DbConnection, SearchConnection);
                SafeWriteVerbose("Front end configs initialized!");
            }
            catch (Exception ex)
            {
                SafeWriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
            }
        }

        public virtual void Initialize(string location, string dbconnection, string searchConnection)
        {
            SafeWriteVerbose("Location: " + location);
            SafeWriteVerbose("Database: " + dbconnection);
            SafeWriteVerbose("ElasticSearch connection: " + searchConnection);

            var connectionStringsPath = Path.GetFullPath(Path.Combine(location, @".\App_Data\Virto\Configuration\connectionStrings.local.config"));
            XDocument connectionStrings;
            using (var connectionStringsStream = File.OpenText(connectionStringsPath))
            {
                connectionStrings = XDocument.Load(connectionStringsStream);
                var dbConnectionString = connectionStrings.Root.Elements("add").FirstOrDefault(element => element.Attribute("name").Value == "VirtoCommerce").Attribute("connectionString");
                dbConnectionString.Value = dbconnection;
                var elasticSearchConnectionString = connectionStrings.Root.Elements("add").FirstOrDefault(element => element.Attribute("name").Value == "SearchConnectionString").Attribute("connectionString");
                elasticSearchConnectionString.Value = searchConnection;
            }

            connectionStrings.Save(connectionStringsPath);
        }
    }
}