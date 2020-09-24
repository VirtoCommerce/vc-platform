using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using Nuke.Common;

namespace GrabMigrator
{
    /// <summary>
    /// Grab-migrator target implementation
    /// </summary>
    internal class GrabMigrator
    {
        public void Do(string configFilePath)
        {
            OutBox($@"VirtoCommerce EF-migration grabbing and applying tool.");
            if (!string.IsNullOrEmpty(configFilePath))
            {
                if (File.Exists(configFilePath))
                {
                    try
                    {
                        Out($@"Read config file...");

                        var config = (Config)JsonSerializer.Deserialize(File.ReadAllText(configFilePath), typeof(Config));

                        Dictionary<string, List<string>> sqlStatements = null;
                        if (config.Grab)
                        {
                            OutBox("Grab mode");

                            Out($@"Refresh connection strings references...");
                            config.ConnectionStringsRefs = new Dictionary<string, List<string>>();
                            foreach (var migrationDirectory in config.MigrationDirectories)
                            {
                                Out($@"Looking in {migrationDirectory}...");
                                GrabConnectionStringsRefsFromModules(config.ConnectionStringsRefs, migrationDirectory);
                            }
                            File.WriteAllText(configFilePath, JsonSerializer.Serialize(config, new JsonSerializerOptions() { WriteIndented = true }));

                            Out($@"Looking for migrations in migration directories recursively...");
                            sqlStatements = GrabSQLStatements(config);
                        }

                        if (config.Apply)
                        {
                            OutBox("Apply mode");

                            if (sqlStatements == null)
                            {
                                sqlStatements = ReadSavedStatements(config.StatementsDirectory);
                            }

                            if (sqlStatements != null)
                            {

                                Out($@"Read platform config file...");

                                var connStrings = GrabConnectionStrings(config.PlatformConfigFile);

                                foreach (var module in config.ApplyingOrder)
                                {
                                    OutBox($@"Applying scripts for module: {module}...");

                                    if (!sqlStatements.ContainsKey(module))
                                    {
                                        Out($@"Warning! There is no SQL expressions for module: {module}");
                                        continue;
                                    }

                                    var connString = string.Empty;

                                    if (config.ConnectionStringsRefs.ContainsKey(module))
                                        foreach (var moduleConnStringKey in config.ConnectionStringsRefs[module])
                                        {
                                            connString = connStrings.ContainsKey(moduleConnStringKey) ? connStrings[moduleConnStringKey] : string.Empty;
                                            if (!string.IsNullOrEmpty(connString)) break;
                                        }
                                    // Fallback connection string key is always "VirtoCommerce"
                                    connString = string.IsNullOrEmpty(connString) ? connStrings["VirtoCommerce"] : connString;

                                    using (var conn = (IDbConnection)new SqlConnection(connString))
                                    {
                                        // One connection and transaction per each module
                                        conn.Open();
                                        var tran = conn.BeginTransaction();
                                        try
                                        {
                                            foreach (var commandText in sqlStatements[module])
                                            {
                                                Out($@"Run SQL statement:{Environment.NewLine}{commandText}");
                                                var cmd = conn.CreateCommand();
                                                cmd.Transaction = tran;
                                                cmd.CommandTimeout = config.CommandTimeout;
                                                cmd.CommandText = commandText;
                                                cmd.ExecuteNonQuery();
                                            }
                                            tran.Commit();
                                            Out($@"Successfully applied for module: {module}!");
                                        }
                                        catch
                                        {
                                            tran.Rollback();
                                            Out($@"Statement not executed. Transaction for module {module} rolled back.");
                                            throw;
                                        }
                                    }
                                }
                            }
                        }
                        OutBox($@"Complete!");
                    }
                    catch (Exception exc)
                    {
                        Fail($@"An exception occured: {exc}");
                    }
                }
                else
                {
                    Fail($@"Configuration file {configFilePath} not found!");
                }
            }
            else
            {
                Out("Usage:");
                Out("vc-build GrabMigrator --grab-migrator-config <configfile>");
                Fail("Configuration file required!");
            }
        }

        private Dictionary<string, string> GrabConnectionStrings(string platformConfigFile)
        {
            var result = new Dictionary<string, string>();

            var platformConfigJson = JsonDocument.Parse(File.ReadAllText(platformConfigFile), new JsonDocumentOptions() { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });

            foreach (JsonProperty property in platformConfigJson.RootElement.GetProperty("ConnectionStrings").EnumerateObject())
            {
                result.Add(property.Name, property.Value.ToString());
            }

            return result;
        }

        private void GrabConnectionStringsRefsFromModules(Dictionary<string, List<string>> refs, string migrationDirectory)
        {
            var connKeyRegex = new Regex(@"\.GetConnectionString\(""(?<connkey>((?!GetConnectionString)[\w.])*)""\)", RegexOptions.Singleline);
            var moduleRegex = new Regex(@"[\\\w^\.-]*\\(?<module>.+)\.Web");
            var moduleFiles = Directory.GetFiles(migrationDirectory, @"Module.cs", SearchOption.AllDirectories);

            foreach (var moduleFile in moduleFiles)
            {
                Out($@"Parse file {moduleFile}...");
                var moduleName = moduleRegex.Match(moduleFile).Groups["module"].Value;
                var content = File.ReadAllText(moduleFile);
                var matches = connKeyRegex.Matches(content);
                var listRefs = new List<string>();
                foreach (Match match in matches)
                {
                    listRefs.Add(match.Groups["connkey"].Value);
                }
                if (listRefs.Count > 0)
                {
                    refs.Add(moduleName, listRefs);
                }
            }
        }

        private Dictionary<string, List<string>> GrabSQLStatements(Config config)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var migrationDirectory in config.MigrationDirectories)
            {
                GrabSQLStatementsWithEFTool(result, migrationDirectory, config);
            }

            return result;
        }

        private void GrabSQLStatementsWithEFTool(Dictionary<string, List<string>> sqlStatements, string migrationDirectory, Config config)
        {
            Directory.CreateDirectory(config.StatementsDirectory);
            var moduleRegex = new Regex(@"[\\\w^\.-]*\\(?<module>.+)\\Migrations");
            var migrationNameRegex = new Regex(@"\[Migration\(""(?<migration>.+)""\)\]");
            string[] migrationFiles;
            if (config.GrabMode == GrabMode.V2V3)
            {
                // Look for upgrade migrations
                migrationFiles = Directory.GetFiles(migrationDirectory, @"20000*2.Designer.cs", SearchOption.AllDirectories);
            }
            else
            {
                // look for at least one migration
                migrationFiles = Directory.GetFiles(migrationDirectory, @"2*.Designer.cs", SearchOption.AllDirectories);
                migrationFiles = migrationFiles.GroupBy(x => new FileInfo(x).Directory.FullName).Select(x => x.FirstOrDefault()).ToArray();
            }

            Out($@"Found {migrationFiles.Count()} migrations in directory {migrationDirectory}");
            foreach (var migrationFile in migrationFiles)
            {
                var moduleName = moduleRegex.Match(migrationFile).Groups["module"].Value;
                if (moduleName.EndsWith(@".Data"))
                {
                    var moduleRegexData = new Regex(@"(?<module>.+)\.Data");
                    moduleName = moduleRegexData.Match(moduleName).Groups["module"].Value;
                }

                // Set migrations range to extract. Leave it empty for all migrations
                var migrationName = config.GrabMode == GrabMode.V2V3 ?
                    $@"0 {migrationNameRegex.Match(File.ReadAllText(migrationFile)).Groups["migration"].Value}" :
                    string.Empty;

                var tempfile = Path.Combine(new DirectoryInfo(config.StatementsDirectory).FullName, $@"{moduleName}.sql");

                Out($@"Extract migrations for module {moduleName}...");

                // Run dotnet-ef to extract migrations in idempotent manner
                var fi = new FileInfo(migrationFile);
                var efTool = Process.Start(new ProcessStartInfo() { WorkingDirectory = fi.Directory.Parent.FullName, FileName = "dotnet", Arguments = $@"ef migrations script {migrationName} -o {tempfile} -i {(config.VerboseEFTool ? "-v" : "")}" });
                efTool.WaitForExit();

                sqlStatements.Add(moduleName, SplitStatements(File.ReadAllText(tempfile)));

                Out($@"OK.");
            }
        }

        private Dictionary<string, List<string>> ReadSavedStatements(string statementsDirectory)
        {
            var result = new Dictionary<string, List<string>>();
            var migrationFiles = Directory.GetFiles(statementsDirectory, @"*.sql");
            foreach (var migrationFile in migrationFiles)
            {
                var migrationFileInfo = new FileInfo(migrationFile);
                var moduleName = migrationFileInfo.Name.Replace(migrationFileInfo.Extension, string.Empty);
                result.Add(moduleName, SplitStatements(File.ReadAllText(migrationFile)));
            }

            return result;
        }

        private List<string> SplitStatements(string statements)
        {
            var statementsSplitRegex = new Regex(@"(?<statement>((?!\s*GO\s*).)+)\s*GO\s*", RegexOptions.Singleline);

            var statementsMatches = statementsSplitRegex.Matches(statements);

            var result = new List<string>();
            foreach (Match statement in statementsMatches)
            {
                result.Add(statement.Groups["statement"].Value);
            }

            return result;
        }

        private void Fail(string text)
        {
            ControlFlow.Fail($@"{DateTime.Now}: {text}");
        }

        private void Out(string text)
        {
            Logger.Normal($@"{DateTime.Now}: {text}");
        }

        private void OutBox(string text)
        {
            Out(new string('=', text.Length));
            Out(text);
            Out(new string('=', text.Length));
        }
    }
}
