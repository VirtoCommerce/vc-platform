using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VirtoCommerce.PowerShell.DatabaseSetup
{
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
            string text = ReadSql(fileName, modelName);
            if (!String.IsNullOrEmpty(text))
            {
                // this allows to have GO in the script
                foreach (string cmd in SplitScriptByGo(text))
                {
                    if (!String.IsNullOrEmpty(cmd))
                        context.Database.ExecuteSqlCommand(cmd);
                }
            }
            else
                throw new FileNotFoundException(fileName);
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
    }
}
