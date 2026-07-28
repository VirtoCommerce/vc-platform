using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Modules;

namespace VirtoCommerce.Platform.MigrationScripter
{
    /// <summary>
    /// Enumerates every registered <see cref="DbContext"/> (platform + all installed modules) from a built
    /// platform host and writes the SQL migrations that startup would apply — without touching the database.
    /// </summary>
    public sealed class MigrationScriptGenerator
    {
        private const string PlatformContextName = "PlatformDbContext";
        private const string SecurityContextName = "SecurityDbContext";
        private const string CombinedFileName = "_combined.sql";

        private static readonly string _nl = Environment.NewLine;

        private readonly IServiceProvider _serviceProvider;
        private readonly ModuleBootstrapper _moduleService;
        private readonly ILogger _logger;

        public MigrationScriptGenerator(IServiceProvider serviceProvider, ModuleBootstrapper moduleService, ILogger logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _moduleService = moduleService ?? throw new ArgumentNullException(nameof(moduleService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Generate(string outputPath, bool includeEmpty = false)
        {
            Directory.CreateDirectory(outputPath);

            var contexts = DiscoverContexts();
            _logger.LogInformation("Discovered {Count} DbContext(s) to script.", contexts.Count);

            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var written = new List<(string Name, string Sql)>();

            foreach (var (name, type) in contexts)
            {
                using var scope = _serviceProvider.CreateScope();
                if (scope.ServiceProvider.GetService(type) is not DbContext context)
                {
                    _logger.LogWarning("Skipping {Type}: not resolvable from the service provider (not registered).", type.FullName);
                    continue;
                }

                var fileName = MakeUniqueName(name, type, usedNames);

                ScriptResult result;
                try
                {
                    result = ScriptContext(context, fileName);
                }
                catch (Exception ex)
                {
                    // Never let one problematic context abort the whole run — log and continue.
                    _logger.LogError(ex, "Failed to script '{Name}' ({Type}); skipping.", fileName, type.FullName);
                    continue;
                }

                if (!result.HasContent && !includeEmpty)
                {
                    _logger.LogInformation("No pending migrations for '{Name}' — skipping (use --include-empty to write it).", fileName);
                    continue;
                }

                var filePath = Path.Combine(outputPath, fileName + ".sql");
                File.WriteAllText(filePath, result.Sql);
                written.Add((fileName, result.Sql));

                _logger.LogInformation("Wrote {FilePath}", filePath);
            }

            WriteCombined(outputPath, written);
        }

        /// <summary>
        /// Scripts a single context using the chosen "pending-only delta" strategy, with an idempotent
        /// full-script fallback when the database is unreachable or has no migration history.
        /// </summary>
        private ScriptResult ScriptContext(DbContext context, string name)
        {
            var migrator = context.Database.GetService<IMigrator>();
            var header = $"-- Migration script for '{name}' ({context.GetType().FullName})" + _nl;

            IReadOnlyList<string> applied;
            try
            {
                applied = context.Database.GetAppliedMigrations().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    "Could not read applied migrations for '{Name}' ({Message}). Falling back to an idempotent full script.",
                    name, ex.Message);

                var idempotent = migrator.GenerateScript(null, null, MigrationsSqlGenerationOptions.Idempotent);
                var fallbackSql = header
                    + "-- FALLBACK: database unreachable or migration history missing." + _nl
                    + "-- This is an IDEMPOTENT full script (each migration self-guards via __EFMigrationsHistory)." + _nl
                    + idempotent;
                return new ScriptResult(fallbackSql, hasContent: true);
            }

            var all = context.Database.GetMigrations().ToList();
            var appliedSet = new HashSet<string>(applied, StringComparer.OrdinalIgnoreCase);
            var pending = all.Where(m => !appliedSet.Contains(m)).ToList();

            if (pending.Count == 0)
            {
                var emptySql = header + $"-- No pending migrations for '{name}'. Database is up to date." + _nl;
                return new ScriptResult(emptySql, hasContent: false);
            }

            // Anchor the delta on the last applied migration that ALSO exists in the assembly. The connected
            // database may have migrations applied that this build's assembly doesn't contain (DB ahead of
            // binaries); passing such an id to GenerateScript throws "migration was not found". Using the last
            // applied migration present in the assembly ("0" if none) yields the pending tail safely.
            var from = "0";
            for (var i = all.Count - 1; i >= 0; i--)
            {
                if (appliedSet.Contains(all[i]))
                {
                    from = all[i];
                    break;
                }
            }

            var script = migrator.GenerateScript(from, null, MigrationsSqlGenerationOptions.Default);

            var pendingSql = header
                + $"-- Pending migrations ({pending.Count}): {string.Join(", ", pending)}" + _nl
                + script;
            return new ScriptResult(pendingSql, hasContent: true);
        }

        private readonly struct ScriptResult
        {
            public ScriptResult(string sql, bool hasContent)
            {
                Sql = sql;
                HasContent = hasContent;
            }

            public string Sql { get; }

            public bool HasContent { get; }
        }

        /// <summary>
        /// Determines the contexts to script and their order: platform first, security second,
        /// then module contexts in module dependency (load) order, then anything else.
        /// </summary>
        private List<(string Name, Type Type)> DiscoverContexts()
        {
            var moduleByAssembly = new Dictionary<Assembly, (int Index, string Id)>();
            var modules = _moduleService.GetModules();
            for (var i = 0; i < modules.Count; i++)
            {
                var assembly = modules[i].Assembly;
                if (assembly != null && !moduleByAssembly.ContainsKey(assembly))
                {
                    moduleByAssembly[assembly] = (i, modules[i].Id);
                }
            }

            var contextTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(IsConcreteDbContext)
                .Distinct();

            var ranked = new List<(string Name, Type Type, int Sort)>();
            foreach (var type in contextTypes)
            {
                string name;
                int sort;

                if (type.Name == PlatformContextName)
                {
                    name = "Platform";
                    sort = 0;
                }
                else if (type.Name == SecurityContextName)
                {
                    name = "Security";
                    sort = 1;
                }
                else if (moduleByAssembly.TryGetValue(type.Assembly, out var module))
                {
                    name = module.Id;
                    sort = 100 + module.Index;
                }
                else
                {
                    name = type.Name;
                    sort = int.MaxValue;
                }

                ranked.Add((name, type, sort));
            }

            return ranked
                .OrderBy(x => x.Sort)
                .ThenBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .Select(x => (x.Name, x.Type))
                .ToList();
        }

        private void WriteCombined(string outputPath, IReadOnlyList<(string Name, string Sql)> written)
        {
            var combined = new System.Text.StringBuilder();
            combined.Append("-- Combined migration script (platform -> security -> modules in dependency order)").Append(_nl);
            combined.Append($"-- Generated {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC").Append(_nl);
            combined.Append(_nl);

            foreach (var (name, sql) in written)
            {
                combined.Append("-- ============================================================").Append(_nl);
                combined.Append($"-- {name}").Append(_nl);
                combined.Append("-- ============================================================").Append(_nl);
                combined.Append(sql).Append(_nl).Append(_nl);
            }

            var combinedPath = Path.Combine(outputPath, CombinedFileName);
            File.WriteAllText(combinedPath, combined.ToString());
            _logger.LogInformation("Wrote {FilePath}", combinedPath);
        }

        private static string MakeUniqueName(string name, Type type, HashSet<string> usedNames)
        {
            if (usedNames.Add(name))
            {
                return name;
            }

            // Two contexts mapped to the same base name (e.g. a module with more than one DbContext).
            var disambiguated = $"{name}.{type.Name}";
            var candidate = disambiguated;
            var counter = 1;
            while (!usedNames.Add(candidate))
            {
                candidate = $"{disambiguated}.{counter++}";
            }

            return candidate;
        }

        private static bool IsConcreteDbContext(Type type)
        {
            return type != null
                && type.IsClass
                && !type.IsAbstract
                && !type.ContainsGenericParameters
                && type != typeof(DbContext)
                && typeof(DbContext).IsAssignableFrom(type);
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }
    }
}
