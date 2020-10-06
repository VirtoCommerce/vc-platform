using System.Collections.Generic;

namespace GrabMigrator
{
    /// <summary>
    /// Grab-migrator configuration
    /// </summary>
    public class Config
    {
        /// <summary>
        /// List of modules and order to apply migrations
        /// </summary>
        public List<string> ApplyingOrder { get; set; }
        /// <summary>
        /// List of directories to looking for migrations
        /// </summary>
        public List<string> MigrationDirectories { get; set; }
        /// <summary>
        /// Platform config file to gather information about modules and their connection strings
        /// </summary>
        public string PlatformConfigFile { get; set; }
        /// <summary>
        /// There grabbed migration SQL statements should reside. Default "Statements"
        /// </summary>
        public string StatementsDirectory { get; set; } = "Statements";
        /// <summary>
        /// SQL command timeout (seconds) in apply mode, default 30
        /// </summary>
        public int CommandTimeout { get; set; } = 30;
        /// <summary>
        /// Set true to grab, default true
        /// </summary>
        public bool Grab { get; set; } = true;
        /// <summary>
        /// See true to apply, default false
        /// </summary>
        public bool Apply { get; set; } = false;

        /// <summary>
        /// Grab mode, default v2->v3 migrations only
        /// </summary>
        public GrabMode GrabMode { get; set; } = GrabMode.V2V3;
        /// <summary>
        /// Set true to get output from dotnet-ef tool
        /// </summary>
        public bool VerboseEFTool { get; set; } = false;
        /// <summary>
        /// A map: module->connection string keys. Grabbed from sources and stored to config file in grab mode.
        /// </summary>
        public Dictionary<string, List<string>> ConnectionStringsRefs { get; set; } = new Dictionary<string, List<string>>();
    }
}
