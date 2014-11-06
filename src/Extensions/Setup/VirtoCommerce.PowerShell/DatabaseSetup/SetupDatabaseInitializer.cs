using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace VirtoCommerce.PowerShell.DatabaseSetup
{
    using System.Data.SqlClient;
    using System.Linq;

    public abstract class SetupDatabaseInitializer<TContext, TMigrationsConfiguration> : SetupMigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private const string SplitByGoRegex = @"(?m)^\s*GO\s*\d*\s*$";

        protected string[] SplitScriptByGo(string script)
        {
            return Regex.Split(script + Environment.NewLine, SplitByGoRegex, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

	    protected SetupDatabaseInitializer()
        {
            
        }

	    protected SetupDatabaseInitializer(string connectionString)
            : base(connectionString)
        {
        }

        protected virtual void RunCommand(DbContext context, string fileName, string modelName)
        {
            var text = ReadSql(fileName, modelName);
            if (!String.IsNullOrEmpty(text))
            {
                // this allows to have GO in the script
                foreach (string cmd in this.SplitScriptByGo(text).Where(cmd => !String.IsNullOrEmpty(cmd)))
                {
                    try
                    {
                        context.Database.ExecuteSqlCommand(cmd);
                    }
                    catch (SqlException ex)
                    {
                        throw new ApplicationException(
                            String.Format(
                                "Failed to process \"{0}\" in \"{1}\" model. Message: {2}",
                                fileName,
                                modelName,
                                ex.Message),
                            ex);
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format("File '{0}' is missing.", fileName));
            }
        }

        protected virtual bool ExecuteCommand(string filename, string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = filename,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = arguments
            };

            try
            {
                using (var exeProcess = Process.Start(startInfo))
                {
                    if (exeProcess != null)
                    {
                        exeProcess.WaitForExit();
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        protected virtual string ReadSql(string fileName, string modelName)
        {
            var asm = Assembly.GetExecutingAssembly();
	        var asmName = asm.FullName.Substring(0, asm.FullName.IndexOf(','));
	        var name = String.Format("{0}.{1}.Data.{2}",
				asmName, 
				modelName,
	            fileName);
            var stream = asm.GetManifestResourceStream(name);
	        Debug.Assert(stream != null);
            return new StreamReader(stream).ReadToEnd();
        }

        public static string GetFrameworkDirectory()
        {
            // This is the location of the .Net Framework Registry Key
            const string framworkRegPath = @"Software\Microsoft\.NetFramework";

            // Get a non-writable key from the registry
            RegistryKey netFramework = Registry.LocalMachine.OpenSubKey(framworkRegPath, false);

            // Retrieve the install root path for the framework
            if (netFramework != null)
            {
                string installRoot = netFramework.GetValue("InstallRoot").ToString();

                // Retrieve the version of the framework executing this program
                string version = string.Format(@"v{0}.{1}.{2}\",
                    Environment.Version.Major,
                    Environment.Version.Minor,
                    Environment.Version.Build);

                // Return the path of the framework
                return Path.Combine(installRoot, version);
            }

            return string.Empty;
        }
    }
}
