using System;

namespace VirtoCommerce.Platform.MigrationScripter
{
    /// <summary>
    /// Command-line options for the migration scripter tool.
    /// </summary>
    public sealed class MigrationScriptOptions
    {
        /// <summary>
        /// Path to the deployed platform folder (containing appsettings.json, ./modules and ./app_data).
        /// Defaults to the current working directory.
        /// </summary>
        public string PlatformPath { get; private set; }

        /// <summary>
        /// Folder to write the generated .sql files to. Defaults to &lt;platform-path&gt;/migration-scripts.
        /// </summary>
        public string OutputPath { get; private set; }

        /// <summary>
        /// When false (default), contexts with no pending migrations do not produce a file and are excluded
        /// from the combined script. Set via <c>--include-empty</c> to write "up to date" placeholders too.
        /// </summary>
        public bool IncludeEmpty { get; private set; }

        public bool ShowHelp { get; private set; }

        public static MigrationScriptOptions Parse(string[] args)
        {
            var options = new MigrationScriptOptions();

            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--platform-path":
                    case "-p":
                        options.PlatformPath = NextValue(args, ref i);
                        break;
                    case "--output":
                    case "-o":
                        options.OutputPath = NextValue(args, ref i);
                        break;
                    case "--include-empty":
                        options.IncludeEmpty = true;
                        break;
                    case "--help":
                    case "-h":
                    case "-?":
                        options.ShowHelp = true;
                        break;
                    default:
                        // Unknown argument — ignore so callers can pass through extras harmlessly.
                        break;
                }
            }

            return options;
        }

        private static string NextValue(string[] args, ref int i)
        {
            if (i + 1 >= args.Length)
            {
                throw new ArgumentException($"Missing value for option '{args[i]}'.");
            }

            i++;
            return args[i];
        }
    }
}
