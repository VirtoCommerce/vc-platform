using System;
using System.Text.RegularExpressions;

namespace VirtoCommerce.Platform.Data.Packaging
{
    public class SemanticVersion
    {
        private readonly Version _version;

        public static readonly Regex SemanticVersionStrictRegex = new Regex(@"^(?<Version>([0-9]|[1-9][0-9]*)(\.([0-9]|[1-9][0-9]*)){2})(?<Release>-([0]\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+(\.([0]\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+)*)?(?<Metadata>\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        public SemanticVersion(Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException("version");
            }

            _version = version;
        }

        /// <summary>
        /// Major version X (X.y.z)
        /// </summary>
        public int Major { get { return _version.Major; } }

        /// <summary>
        /// Minor version Y (x.Y.z)
        /// </summary>
        public int Minor { get { return _version.Minor; } }

        /// <summary>
        /// Patch version Z (x.y.Z)
        /// </summary>
        public int Patch { get { return _version.Build; } }

        public static bool TryParse(string value, out SemanticVersion version)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var match = SemanticVersionStrictRegex.Match(value.Trim());

                Version versionValue;
                if (match.Success && Version.TryParse(match.Groups["Version"].Value, out versionValue))
                {
                    var normalizedVersion = NormalizeVersionValue(versionValue);
                    version = new SemanticVersion(normalizedVersion);
                    return true;
                }
            }

            version = null;
            return false;
        }

        public static int Compare(SemanticVersion x, SemanticVersion y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
                return 0;

            if (ReferenceEquals(y, null))
                return 1;

            if (ReferenceEquals(x, null))
                return -1;

            var result = x.Major.CompareTo(y.Major);
            if (result != 0)
                return result;

            result = x.Minor.CompareTo(y.Minor);
            if (result != 0)
                return result;

            result = x.Patch.CompareTo(y.Patch);

            return result;
        }


        private static Version NormalizeVersionValue(Version version)
        {
            return new Version(version.Major,
                               version.Minor,
                               Math.Max(version.Build, 0),
                               Math.Max(version.Revision, 0));
        }
    }
}
