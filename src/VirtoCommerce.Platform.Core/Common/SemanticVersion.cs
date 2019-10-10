using System;
using System.Text.RegularExpressions;

namespace VirtoCommerce.Platform.Core.Common
{
    public class SemanticVersion : IComparable
    {
        private readonly Version _version;

        public static readonly Regex SemanticVersionStrictRegex = new Regex(@"^(?<Version>([0-9]|[1-9][0-9]*)(\.([0-9]|[1-9][0-9]*)){2,3})(?<Release>-([0]\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+(\.([0]\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+)*)?(?<Metadata>\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        public SemanticVersion(Version version)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            _version = NormalizeVersionValue(version);
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

        public bool IsCompatibleWithBySemVer(SemanticVersion other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            //MAJOR version when you make incompatible API changes,
            var retVal = Major == other.Major;
            if (retVal)
            {
                //MINOR version when you add functionality in a backwards-compatible manner
                retVal = Minor <= other.Minor;
            }
            return retVal;
        }

        public bool IsCompatibleWith(SemanticVersion other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var comparisonResult = CompareTo(other);
            return comparisonResult <= 0;
        }

        public static SemanticVersion Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var match = SemanticVersionStrictRegex.Match(value.Trim());

            Version versionValue;
            if (match.Success && Version.TryParse(match.Groups["Version"].Value, out versionValue))
            {
                var normalizedVersion = NormalizeVersionValue(versionValue);
                return new SemanticVersion(normalizedVersion);
            }

            throw new FormatException();
        }

        private static Version NormalizeVersionValue(Version version)
        {
            //Normalize version (need to use always only three properties (revision must be skipped) to prevent equality mismatches
            //e.g 1.0.0 and 1.0.0.0 aren't the same
            return new Version(version.Major,
                               version.Minor,
                               Math.Max(version.Build, 0));
        }

        public static bool operator ==(SemanticVersion a, SemanticVersion b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((object)a == null || (object)b == null)
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(SemanticVersion a, SemanticVersion b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to ModuleIdentity return false.
            var other = obj as SemanticVersion;
            if ((object)other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return _version == other._version;
        }
        public override int GetHashCode()
        {
            return _version.GetHashCode();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var other = (SemanticVersion)obj;

            var result = Major.CompareTo(other.Major);
            if (result != 0)
                return result;

            result = Minor.CompareTo(other.Minor);
            if (result != 0)
                return result;

            result = Patch.CompareTo(other.Patch);

            return result;
        }

        #endregion

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}
