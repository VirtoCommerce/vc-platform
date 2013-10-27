using System;
using System.Management.Automation;
using System.Reflection;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.PowerShell.Cmdlet;
using VirtoCommerce.Foundation.Data;

namespace VirtoCommerce.PowerShell.DatabaseSetup
{
    [CLSCompliant(false)]
    public abstract class DatabaseCommand : DomainCommand
    {
        [Alias("c")]
        [Parameter(Mandatory = false, HelpMessage = "Database connection string.")]
        [ValidateNotNullOrEmpty]
        public string DbConnection { get; set; }

        [Alias("f")]
        [Parameter(HelpMessage = "Do not confirm on the creation of the new database")]
        public SwitchParameter Force { get; set; }

        private bool _setup;

        [Parameter(HelpMessage = "When set sample scripts will be ran")]
        [Alias("sample")]
        public SwitchParameter SetupSample
        {
            get { return _setup; }
            set { _setup = value; }
        }

        [Parameter(HelpMessage = "When set sample scripts will be ran")]
        [Alias("data")]
        public string DataLocation { get; set; }

        public virtual void Publish(string dbconnection, string data, bool sample)
        {
            SafeWriteVerbose("Server: " + dbconnection);
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                SafeWriteVerbose("Version: " + Assembly.GetExecutingAssembly().GetFileVersion());
                base.ProcessRecord();
                Publish(DbConnection, DataLocation, SetupSample);
                SafeWriteVerbose("Database Published!");
            }
            catch (Exception ex)
            {
                SafeWriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
            }
        }

        private const string ConnectionStringIntegratedFormat = "Server={0};Database={1};Integrated Security=True;";

        private const string ConnectionStringFormat = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=True";
    }
}